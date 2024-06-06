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
public class LuceneSearchService {
    private readonly FSDirectory _indexDirectory;
    private readonly CustomAnalyzer _analyzer;

    public LuceneSearchService(string indexDirectoryPath) {
        _indexDirectory = FSDirectory.Open(new DirectoryInfo(indexDirectoryPath));
        _analyzer = new CustomAnalyzer(LuceneVersion.LUCENE_48);
    }

    public void AddOrUpdateProductToIndex(ProductModel product) {
        using (var writer = new IndexWriter(_indexDirectory, new IndexWriterConfig(LuceneVersion.LUCENE_48, _analyzer))) {
            var document = new Document {
                new StringField("Id", product.Id.ToString(), Field.Store.YES),
                new TextField("Name", product.Name, Field.Store.YES),
                new TextField("Desctiption", product.Description ?? string.Empty, Field.Store.YES),
            };

            foreach (var texturePatternId in product.ProductTexturePatternIds) {
                document.Add(new Int32Field("TexturePatternId", texturePatternId, Field.Store.YES));
            }

            foreach (var colorId in product.ProductColorIds) {
                document.Add(new Int32Field("ColorId", colorId, Field.Store.YES));
            }

            foreach(var fabricId in product.ProductFabricIds) {
                document.Add(new Int32Field("FabricId", fabricId, Field.Store.YES));
            }

            foreach(var categoryId in product.ProductCategoryIds) {
                document.Add(new Int32Field("CategoryId", categoryId, Field.Store.YES));
            }

            foreach(var tagId in product.ProductTagIds) {
                document.Add(new Int32Field("TagId", tagId, Field.Store.YES));
            }

            foreach(var promotionId in product.ProductPromotionIds) {
                document.Add(new Int32Field("PromotionId", promotionId, Field.Store.YES));
            }

            writer.UpdateDocument(new Term("Id", product.Id.ToString()), document);
            writer.Commit();
        }
    }

    public IEnumerable<ProductModel> SearchProducts(string searchTerm, bool fuzzySearch = false) {
        using (var reader = DirectoryReader.Open(_indexDirectory)) {
            var searcher = new IndexSearcher(reader);
            Query query;

            if (fuzzySearch){
                query = new FuzzyQuery(new Term("Name", searchTerm.ToLower()), 2);
            } else {
                var parser = new MultiFieldQueryParser(LuceneVersion.LUCENE_48, new[] { "Name", "Description", "TexturePatternId",
                    "FabricId", "ColorId", "TagId", "CategoryId", "PromotionId"}, _analyzer);
                query = parser.Parse(searchTerm);
            }

            var hits = searcher.Search(query, 10).ScoreDocs;

            foreach (var hit in hits) {
                var doc = searcher.Doc(hit.Doc);
                yield return new ProductModel {
                    Id = int.Parse(doc.Get("Id")),
                    Name = doc.Get("Name"),
                    Description = doc.Get("Description"),
                    ProductTexturePatternIds = doc.GetFields("TexturePatternId").Select(f => int.Parse(f.GetStringValue())).ToList(),
                    ProductColorIds = doc.GetFields("ColorId").Select(f => int.Parse(f.GetStringValue())).ToList(),
                    ProductFabricIds = doc.GetFields("FabricId").Select(f => int.Parse(f.GetStringValue())).ToList(),
                    ProductPromotionIds = doc.GetFields("PromotionId").Select(f => int.Parse(f.GetStringValue())).ToList(),
                    ProductCategoryIds = doc.GetFields("CatgoryId").Select(f => int.Parse(f.GetStringValue())).ToList(),
                    ProductTagIds = doc.GetFields("TagId").Select(f => int.Parse(f.GetStringValue())).ToList(),
                };
            }

        }
    }
}