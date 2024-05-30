using System.ComponentModel.DataAnnotations;

public class SizeModel {
    public int Id { get; set; }
    
    [Required]
    public int Height { get; set; }

    [Required]
    public int Width { get; set; }
}