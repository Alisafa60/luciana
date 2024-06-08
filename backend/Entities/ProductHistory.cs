using System.ComponentModel.DataAnnotations;

public class ProductHistory {
    public int Id { get; set; }
    [Required]
    public int ProductId { get; set; }
    [Required]
    public string ProductName { get; set; }
    public string? ProductDescription { get; set; }
    [Required]
    public DateTime ChangeDate { get; set; }

}