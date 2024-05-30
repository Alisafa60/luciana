using System.ComponentModel.DataAnnotations;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
public class ColorModel {
    public int Id { get; set; }

    public string? HexValue { get; set; }

    [Required]
    public string Name { get; set; }

    public string? Description { get; set; }

    public int ParentColorId { get; set; }
}