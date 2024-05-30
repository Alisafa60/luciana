public class Tag {
    public int Id { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public string Name { get; set; }
    public string Description { get; set; }
    public TagType Type { get; set; }
    public enum TagType {
        Style,
        Occasion,
        Pattern,
    }
}