using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using backend.Data;

[Route("api/product")]
[ApiController]
public class ProductController : Controller{
    private readonly AppDbContext _context;

    public ProductController(AppDbContext context){
        _context = context;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public ActionResult<Product> CreateProduct(Product product){
        _context.Products.Add(product);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id, product});
    }

    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetProducts(){
        return _context.Products.ToList();
    }

    [HttpGet("{id}")]
    public ActionResult<Product> GetProduct(int id){
        var product = _context.Products.Find(id);
        if(product == null){
            return NotFound();
        }

        return product;
    }
}