namespace Application.Models.Outbound;


public class RouteHeader : Detail
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id { get; set; }

    public string? DocumentNo  { get; set ; }

    public string? DriverId { get; set; }
    public Driver? Driver { get; set; }

    public string? TruckId { get; set; }
    public Truck? Truck { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public DateTime? DepartedOn { get; set; }
    public DateTime? ArrivedOn { get; set; }

    public RouteStatus RouteStatus { get; set; }


    public string DeliveryDate_DateOnly
    {
        get
        {
            return DeliveryDate?.ToString("D");
        }
    }

    
}