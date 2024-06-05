using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Analysis.En;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis.Util;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using System;
using System.Collections.Generic;
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
            };
        }
    }
}