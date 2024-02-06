using System.ComponentModel.DataAnnotations;

public class UserRegisterationModel
{
    [EmailAddress]
    public required string Email { get; set; }

    [MinLength(8)]
    public required string Password { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required DateTime DateOfBirth { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string Gender { get; set; }

    public UserRegisterationModel()
    {
        Gender = "Female";
    }
}
