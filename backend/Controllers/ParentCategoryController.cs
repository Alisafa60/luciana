using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using backend.Data;

[Route("api/parentcategory")]
[ApiController]
public class ParentCategoryController : ControllerBase
{
    private readonly AppDbContext _context;

    public ParentCategoryController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public ActionResult<ParentCategory> CreateParentCategory(ParentCategory category)
    {
        _context.ParentCategories.Add(category);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetParentCategory), new { id = category.Id }, category);
    }

    [HttpGet]
    public ActionResult<IEnumerable<ParentCategory>> GetParentCategories()
    {
        return _context.ParentCategories.ToList();
    }

    [HttpGet("{id}")]
    public ActionResult<ParentCategory> GetParentCategory(int id)
    {
        var category = _context.ParentCategories.Find(id);
        if (category == null)
        {
            return NotFound();
        }

        return category;
    }
}
