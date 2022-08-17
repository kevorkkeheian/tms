namespace Application.Models.Org;

public class Employee : Detail
{
    public string? PunchId { get; set; }
    public string? ApplicationUserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public string? StoreId { get; set; }
    public Store? Store { get; set; }

    public string FullName { 
        get {
            return $"{FirstName} {LastName}";
        }
    }
}