using Lucene.Net.Analysis.Hunspell;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Internal;

public interface IAttributeService {
    Task<Dictionary<int, string>> GetColorNames(IEnumerable<int> ids);
    Task<Dictionary<int, string>> GetCategoryNames(IEnumerable<int> ids);
    Task<Dictionary<int, string>> GetFabricNames(IEnumerable<int> ids);
    Task<Dictionary<int, string>> GetTagNames(IEnumerable<int> ids);
    Task<Dictionary<int, string>> GetTexturePatternNames(IEnumerable<int> ids);
}
public class AttributesService : IAttributeService {
    private readonly IColorRepository _colorRepository;
    private readonly IFabricRepository _fabricRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITagRepository _tagRepository;
    private readonly ITexturePatternRepository _texturePatternRepository;

    public AttributesService(
        IColorRepository colorRepository, 
        IFabricRepository fabricRepository, 
        ICategoryRepository categoryRepository, 
        ITagRepository tagRepository,
        ITexturePatternRepository texturePatternRepository) {
    
        _colorRepository = colorRepository;
        _fabricRepository = fabricRepository;
        _categoryRepository = categoryRepository;
        _tagRepository = tagRepository;
        _texturePatternRepository = texturePatternRepository;
    }
    
    public async Task<Dictionary<int, string>> GetColorNames(IEnumerable<int> ids) {
        var colorNames = new Dictionary<int, string>();
        var colors = await _colorRepository.GetByIdsAsync(ids);

        foreach(var color in colors) {
            if (color != null) {
                string fullName = color.Name;
                if (color.ParentColor != null){
                    fullName = $"{color.ParentColor.Name} {color.Name}";
                }

                colorNames[color.Id] = fullName;
            }
        }

        return colorNames;
    }

    public async Task<Dictionary<int, string>> GetFabricNames(IEnumerable<int> ids) {
        var fabricNames = new Dictionary<int, string>();
        var fabrics = await _fabricRepository.GetByIdsAsync(ids);

        foreach(var fabric in fabrics) {
            if (fabric != null) {
                string fullName = fabric.Name;
                if(fabric.ParentFabric != null){
                    fullName = $"{fabric.ParentFabric.Name} {fabric.Name}";
                }

                fabricNames[fabric.Id] = fullName;
            }
        }

        return fabricNames;
    }

    public async Task<Dictionary<int, string>> GetCategoryNames(IEnumerable<int> ids) {
        var categoryNames = new Dictionary<int, string>();
        var categories = await _categoryRepository.GetByIdsAsync(ids);

        foreach(var category in categories) {
            if (category != null) {
                string fullName = category.Name;
                if(category.ParentCategory != null) {
                    fullName = $"{category.ParentCategory.Name} {category.Name}";
                }

                categoryNames[category.Id] = fullName;
            }
        }

        return categoryNames;
    }

    public async Task<Dictionary<int, string>> GetTagNames(IEnumerable<int> ids) {
        var tagNames = new Dictionary<int, string>();
        var tags = await _tagRepository.GetByIdsAsync(ids);

        foreach (var tag in tags) {
            if (tag != null) {
                tagNames[tag.Id] = tag.Name;
            }
        }

        return tagNames;
    }

    public async Task<Dictionary<int, string>> GetTexturePatternNames(IEnumerable<int> ids) {
        var texturePatternNames = new Dictionary<int, string>();
        var texturePatterns = await _texturePatternRepository.GetByIdsAsync(ids);

        foreach (var texturePattern in texturePatterns) {
            if (texturePattern != null){
                texturePatternNames[texturePattern.Id] = texturePattern.Name;
            }
        }

        return texturePatternNames;
    }

}