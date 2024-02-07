using System.ComponentModel.DataAnnotations;

public class SkinTone {

    public int Id {get; set; }

    [Required]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public string HexValue {get; set; }
#pragma warning restore CS8618 

    public string? Name {get; set;}
    
}