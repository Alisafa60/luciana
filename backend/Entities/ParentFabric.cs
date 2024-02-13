#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

public class ParentFabric{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }
}