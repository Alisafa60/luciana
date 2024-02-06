using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User{
    
    public int Id {get; set;}
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    [Required]
    [EmailAddress]
    public string Email {get; set;}

    [Required]
    [MinLength(8)]
    public string Password {get; set;}

    [Required]
    public string FirstName {get; set;}

    [Required]
    public string LastName {get; set;}

    [Required]
    public DateTime DateOfBirth { get; set; }

    public string? PhoneNumber {get; set;}
      
    public string? Address {get; set;}

    [Required]
    public string Gender {get; set;}

    public int RoleId {get; set;}

    [ForeignKey(nameof(RoleId))]
    public Role Role {get; set;}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}