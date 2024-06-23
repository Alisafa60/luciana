using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Analysis.En;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis.Util;
using Lucene.Net.Documents;
using Lucene.Net.Documents.Extensions;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

public class CustomAnalyzer : Analyzer {
    private readonly LuceneVersion _matchVersion;

    public CustomAnalyzer(LuceneVersion matchVersion) {
        _matchVersion = matchVersion;
    }

    protected override TokenStreamComponents CreateComponents(string fieldName, TextReader reader) {
        var source = new StandardTokenizer(_matchVersion, reader);
        TokenStream result = new LowerCaseFilter(_matchVersion, source);
        result = new StopFilter(_matchVersion, result, StopAnalyzer.ENGLISH_STOP_WORDS_SET);
        result = new PorterStemFilter(result);
        return new TokenStreamComponents(source, result);
    }
}
public class LuceneSearchService : IDisposable {
    private readonly FSDirectory _indexDirectory;
    private readonly CustomAnalyzer _analyzer;
    private readonly IAttributeService _attributeService;
    private readonly string _connectionString;
    private IndexWriter _writer;
    private bool _disposed = false;

    public LuceneSearchService( string connectionString, string indexDirectoryPath, IAttributeService attributeService) {
        _indexDirectory = FSDirectory.Open(new DirectoryInfo(indexDirectoryPath));
        _analyzer = new CustomAnalyzer(LuceneVersion.LUCENE_48);
        _attributeService = attributeService;
        _connectionString = connectionString;
        var config = new IndexWriterConfig(LuceneVersion.LUCENE_48, _analyzer);
        _writer = new IndexWriter(_indexDirectory, config);
    }

    public async Task AddOrUpdateProductToIndexAsync(ProductDto product) {
        var colorNames = await _attributeService.GetColorNames(product.ProductColorIds);
        var fabricNames = await _attributeService.GetFabricNames(product.ProductFabricIds);
        var categoryNames = await _attributeService.GetCategoryNames(product.ProductCategoryIds);
        var texturePatternNames = await _attributeService.GetTexturePatternNames(product.ProductTexturePatternIds);
        var tagNames = await _attributeService.GetTagNames(product.ProductTagIds);

        
        var document = new Document {
            new StringField("Id", product.Id.ToString(), Field.Store.YES),
            new TextField("Name", product.Name, Field.Store.YES),
            new TextField("Description", product.Description ?? string.Empty, Field.Store.YES),
        };
            
        AddFields(document, "ColorId", "ColorName", product.ProductColorIds, colorNames);
        AddFields(document, "FabricId", "FabricName", product.ProductFabricIds, fabricNames);
        AddFields(document, "CategoryId", "CategoryName", product.ProductCategoryIds, categoryNames);
        AddFields(document, "TexturePatternId", "TexturePatternName", product.ProductTexturePatternIds, texturePatternNames);
        AddFields(document, "TagId", "TagName", product.ProductTagIds, tagNames);

        _writer.UpdateDocument(new Term("Id", product.Id.ToString()), document);
        _writer.Commit();
    }

    public IEnumerable<ProductDto> SearchProducts(string searchTerm) {
        IEnumerable<ProductDto> results = PerformExactSearch(searchTerm);

        if(!results.Any()) {
            results = PerformFuzzySearch(searchTerm);
        }

        return results;
    }

    private IEnumerable<ProductDto> PerformExactSearch(string searchTerm) {
        using (var reader = DirectoryReader.Open(_indexDirectory)) {
            var searcher = new IndexSearcher(reader);
            var parser = new MultiFieldQueryParser(LuceneVersion.LUCENE_48, new[] {
                "Name", "Description", "ColorName", "FabricName",
                "CategoryName", "TexturePatternName", "TagName"
            }, _analyzer);

            Query query = parser.Parse(searchTerm);
            var hits = searcher.Search(query, 10).ScoreDocs;
            return hits.Select(hit => CreateProductDtoFromDocument(searcher.Doc(hit.Doc))).ToList();
        }

    }
    private IEnumerable<ProductDto> PerformFuzzySearch(string searchTerm) {
        using (var reader = DirectoryReader.Open(_indexDirectory)) {
            var searcher = new IndexSearcher(reader);
            var booleanQuery = new BooleanQuery();
            var terms = searchTerm.ToLower().Split(' ');

            var fields = new[] {"Name", "Discription", "ColorName", "FabricName",
            "CategoryName", "TexturePatternName", "TagName"};
            foreach (var term in terms) {
                foreach (var field in fields) {
                    var fuzzeQuery = new FuzzyQuery(new Term(field, term), 2);
                    booleanQuery.Add(fuzzeQuery, Occur.SHOULD);
                }
            }

            var hits = searcher.Search(booleanQuery, 10).ScoreDocs;
             return hits.Select(hit => CreateProductDtoFromDocument(searcher.Doc(hit.Doc))).ToList();
        }
    }

    private ProductDto CreateProductDtoFromDocument(Document doc) {
        return new ProductDto{
            Id = int.Parse(doc.Get("Id")),
            Name = doc.Get("Name"),
            Description = doc.Get("Description"),
            ProductTexturePatternIds = doc.GetFields("TexturePatternId").Select(f => int.Parse(f.GetStringValue())).ToList(),
            ProductColorIds = doc.GetFields("ColorId").Select(f => int.Parse(f.GetStringValue())).ToList(),
            ProductFabricIds = doc.GetFields("FabricId").Select(f => int.Parse(f.GetStringValue())).ToList(),
            ProductCategoryIds = doc.GetFields("CategoryId").Select(f => int.Parse(f.GetStringValue())).ToList(),
            ProductTagIds = doc.GetFields("TagId").Select(f => int.Parse(f.GetStringValue())).ToList(),
        };
    }

    public async Task RemoveProductFromIndex(int productId) {
        await Task.Run(() => {
            _writer.DeleteDocuments(new Term("Id", productId.ToString()));
            _writer.Commit();
        });
    }

    private void AddFields(Document document, string idFieldName, string nameFieldName, ICollection<int> ids, Dictionary<int, string> names) {
        foreach (var id in ids) {
            document.Add(new Int32Field(idFieldName, id, Field.Store.YES));
            if (names.TryGetValue(id, out var name)) {
                document.Add(new TextField(nameFieldName, name, Field.Store.YES));
            }
        }
    }

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) {
        if (!_disposed) {
            if (disposing) {
                _writer?.Dispose();
                _indexDirectory?.Dispose();
            }
        }
        _disposed = true;
    }

    ~LuceneSearchService() {
        Dispose(false);
    }
}

    
    //**************************************************************
    // public IEnumerable<ProductDto> SearchProducts(string searchTerm, bool fuzzySearch = false) {
    //     using (var reader = DirectoryReader.Open(_indexDirectory)) {
    //         var searcher = new IndexSearcher(reader);
    //         Query query;

    //         if (fuzzySearch) {
    //             var booleanQuery = new BooleanQuery();
    //             var terms = searchTerm.ToLower().Split(' ');
                
                // var fields = new[] {"Name", "Discription", "ColorName", "FabricName",
                //     "CategoryName", "TexturePatternName", "TagName"};
                //     foreach (var term in terms) {
                //         foreach (var field in fields) {
                //             var fuzzeQuery = new FuzzyQuery(new Term(field, term), 2);
                //             booleanQuery.Add(fuzzeQuery, Occur.SHOULD);
                //         }
                //     }

                // query = booleanQuery;
    //         } else {
    //             var parser = new MultiFieldQueryParser(LuceneVersion.LUCENE_48, [
    //                 "Name", "Description", "ColorName", "FabricName",
    //                 "CategoryName", "TexturePatternName", "TagName"
    //             ], _analyzer);
    //             query = parser.Parse(searchTerm);
    //         }

    //         var hits = searcher.Search(query, 10).ScoreDocs;

    //         foreach (var hit in hits) {
    //             var doc = searcher.Doc(hit.Doc);
    //             yield return new ProductDto {
    //                 Id = int.Parse(doc.Get("Id")),
    //                 Name = doc.Get("Name"),
    //                 Description = doc.Get("Description"),
    //                 ProductTexturePatternIds = doc.GetFields("TexturePatternId").Select(f => int.Parse(f.GetStringValue())).ToList(),
    //                 ProductColorIds = doc.GetFields("ColorId").Select(f => int.Parse(f.GetStringValue())).ToList(),
    //                 ProductFabricIds = doc.GetFields("FabricId").Select(f => int.Parse(f.GetStringValue())).ToList(),
    //                 ProductCategoryIds = doc.GetFields("CategoryId").Select(f => int.Parse(f.GetStringValue())).ToList(),
    //                 ProductTagIds = doc.GetFields("TagId").Select(f => int.Parse(f.GetStringValue())).ToList(),
    //             };
    //         }
    //     }
    // }

