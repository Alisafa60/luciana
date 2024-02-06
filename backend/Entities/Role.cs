using System.ComponentModel.DataAnnotations;

public enum UserRole{
    [Display(Name = "Admin")]
    Admin,
    
    [Display(Name = "Customer")]
    Customer
}

public class Role{
    public int Id { get; set; }

    [Required]
    public UserRole UserRole { get; set; }
}
