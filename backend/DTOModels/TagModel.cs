using System.ComponentModel.DataAnnotations;

public class TagModel {
    public int Id { get; set; }

    [Required]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public string Name { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public string? Description { get; set; }

    [Required]
    public TagType Type { get; set; }

    public enum TagType {
        Style,
        Occasion,
        Pattern,
    }
}