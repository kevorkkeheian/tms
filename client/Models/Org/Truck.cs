namespace Application.Models.Org;

public class Truck : Detail
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id { get; set; }
    public string PlateNo { get; set; }
}