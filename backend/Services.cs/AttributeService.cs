public interface IAttributeService {
    Dictionary<int, string> GetColorNames(IEnumerable<int> ids);
    Dictionary<int, string> GetCategoryNames(IEnumerable<int> ids);
    Dictionary<int, string> GetFabricNames(IEnumerable<int> ids);
    Dictionary<int, string> GetTagNames(IEnumerable<int> ids);
    Dictionary<int, string> GetTexturePatternName(IEnumerable<int> ids);
}

