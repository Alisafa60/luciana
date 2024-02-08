#pragma warning disable CS8618 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public class Category{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }

    public int? ParentCategotyId { get; set; }

    [ForeignKey(nameof(ParentCategotyId))]
    public Category ParentCategory { get; set;}
}