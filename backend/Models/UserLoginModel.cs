using System.ComponentModel.DataAnnotations;

public class UserLoginModel{
    
    [Required]
    [EmailAddress]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public string Email {get; set;}

    [Required]
    public string Password {get; set;}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}