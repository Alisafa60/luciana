#pragma warning disable CS8618 
using System.ComponentModel.DataAnnotations;

public class Color{
    public int Id {get; set; }

    [Required]
    public string HexValue {get; set; }


    [Required]
    public string Name {get; set; }

    public string? Description {get; set; }
}