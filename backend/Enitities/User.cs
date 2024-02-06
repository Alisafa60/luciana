using System.ComponentModel.DataAnnotations;

public class User{
    
    public int Id {get; set;}
    
    [Required]
    [EmailAddress]
    public string Email {get; set;}

    [Required]
    [MinLength(8)]
    public string Password {get; set;}

    [Required]
    public required string FirstName {get; set;}

    [Required]
    public required string LastName {get; set;}

    [Required]
    public required DateTime DateOfBirth { get; set; }

    public string? PhoneNumber {get; set;}
      
    public string? Address {get; set;}

    [Required]
    public string Gender {get; set;}

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public User(){
        Gender = "Female";
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}