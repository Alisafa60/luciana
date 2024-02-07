using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Npgsql.PostgresTypes;
#pragma warning disable CS8618 

public class SkinToneColorCompatibility{

    [Required]
    public int ColorId {get; set;}

    [ForeignKey(nameof(ColorId))]

    public Color Color {get; set;}


    [Required]
    public int SkinToneId {get; set;}

    [ForeignKey(nameof(SkinToneId))]
    public SkinTone SkinTone {get; set;}

    [Required]
    public int CompatibilityScore {get; set;}

}
#pragma warning restore CS8618 