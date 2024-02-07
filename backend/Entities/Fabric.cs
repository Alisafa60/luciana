#pragma warning disable CS8618 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public class Fabric{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }

    public int? ParentFabricId { get; set; }

    [ForeignKey(nameof(ParentFabricId))]
    public Fabric ParentFabric { get; set;}
}