public class CategoryDto {
    public int Id { get; set; }

    public required string Name { get; set; }

    public int? ParentCategoryId { get; set; }
}