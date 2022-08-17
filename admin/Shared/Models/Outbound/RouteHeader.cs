namespace Application.Shared.Models.Outbound;



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


    public string ConvertRouteStatusToString(RouteStatus status) {
        if(RouteStatus == RouteStatus.Pending) {
            return "Pending";
        } else if(RouteStatus == RouteStatus.Departed) {
            return "Departed";
        } else if(RouteStatus == RouteStatus.Arrived) {
            return "Arrived";
        } else if(RouteStatus == RouteStatus.Canceled) {
            return "Canceled";
        } else {
            return "";
        }
    }

    public string DeliveryDate_DateOnly
    {
        get
        {
            return DeliveryDate?.ToString("yyyy-MM-dd");
        }
    }

    
}