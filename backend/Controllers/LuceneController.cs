using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/Lucene")]
public class SearchController : ControllerBase {
    private readonly LuceneSearchService _luceneSearchService;

    public SearchController(LuceneSearchService luceneSearchService) {
        _luceneSearchService = luceneSearchService;
    }

    [HttpGet("search")]
    public IActionResult SearchProducts(string query) {
        var results = _luceneSearchService.SearchProducts(query);
        return Ok(results);
    }
}
