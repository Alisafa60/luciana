#pragma warning disable CS8618 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Color{
    public int Id {get; set; }

    [Required]
    public string HexValue {get; set; }


    [Required]
    public string Name {get; set; }

    public string? Description {get; set; }

    public int ParentColorId { get; set; } 

    [ForeignKey(nameof(ParentColorId))]
    public ParentColor ParentColor { get; set; }
}