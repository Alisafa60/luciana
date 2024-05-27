using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using backend.Data;

[Route("api/category")]
[ApiController]
public class CategoryController : Controller {
    private readonly AppDbContext _context;
    
    public CategoryController(AppDbContext context) {
        _context = context;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public ActionResult<CategoryModel> CreateCategory(CategoryModel categoryModel) {
        var category = new Category{
            Name = categoryModel.Name,
            ParentCategoryId = categoryModel.ParentCategoryId
        };

        _context.Categories.Add(category);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetCategory), new {id = category.Id}, categoryModel);
    }

    [HttpGet]
    public ActionResult<IEnumerable<CategoryModel>> GetCategories() {
        var categories = _context.Categories
            .Select( c => new CategoryModel {
                Id = c.Id,
                Name = c.Name,
                ParentCategoryId = c.ParentCategoryId
            })
            .ToList();

        return categories;
    }

    [HttpGet("{id}")]
    public ActionResult<CategoryModel> GetCategory(int id) {
        var category = _context.Categories.Find(id);
        if (category == null){
            return NotFound();
        }

        var categoryModel = new CategoryModel {
            Id = category.Id,
            Name = category.Name,
            ParentCategoryId = category.ParentCategoryId
        };

        return categoryModel;
    }
}