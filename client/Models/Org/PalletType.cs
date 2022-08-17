namespace Application.Models.Org;

public enum Material
{
    Wooden,
    Plastic,
    Metal
}

public class PalletType : Detail
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id { get; set; }

    public string Type { get; set; }

    public Material? Material { get; set; }

    public decimal? Width { get; set; }
    public decimal? Length { get; set; }
}