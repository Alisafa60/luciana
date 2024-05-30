public class ProductPromotion{
    public int ProductId { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public Product Product { get; set; }

    public int PromotionId { get; set; }
    public Promotion Promotion { get; set; }
}
