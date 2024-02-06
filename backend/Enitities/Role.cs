using System.ComponentModel.DataAnnotations;

public enum UserRole{
    Admin,
    Customer
}

public class Role{
    public int Id { get; set; }

    [Required]
    public UserRole UserRole { get; set; }
}
