#pragma warning disable CS8618 
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Antiforgery;

public class ParentCategory{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }
}