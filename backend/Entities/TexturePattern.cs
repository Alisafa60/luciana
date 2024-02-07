#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
 
public class TexturePattern{
    public int Id {get; set; }

    [Required]
    public string Name {get; set;}
} 