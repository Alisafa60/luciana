public class ProductHistoryDto {
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string? ProductDescription { get; set; }
    public DateTime ChangeDate { get; set; }
}