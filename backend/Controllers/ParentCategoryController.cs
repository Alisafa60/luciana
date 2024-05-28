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
    public ActionResult<ParentCategory> CreateParentCategory(ParentCategoryModel categoryModel)
    {
        var category = new ParentCategory{Name = categoryModel.Name};
        _context.ParentCategories.Add(category);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetParentCategory), new { id = category.Id }, category);
    }

    [HttpGet]
    public ActionResult<IEnumerable<ParentCategory>> GetParentCategories() {
        var categories = _context.ParentCategories
            .Select(pc => new ParentCategory{
                Id = pc.Id,
                Name = pc.Name,
            })
            .ToList();

        return categories;
    }

    [HttpGet("{id}")]
    public ActionResult<ParentCategoryModel> GetParentCategory(int id)
    {
        var category = _context.ParentCategories.Find(id);
        if (category == null)
        {
            return NotFound();
        }

        var categoryModel = new ParentCategoryModel{
            Id = category.Id,
            Name = category.Name,
        };

        return categoryModel;
    }
}
