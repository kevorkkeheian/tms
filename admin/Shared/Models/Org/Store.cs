namespace Application.Shared.Models.Org;


public class Store : Detail
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id { get; set; }

    public string? EntityId { get; set; }
    public Entity? Entity { get; set; }

    [Required]
    [MaxLength(6)]
    public string Code { get; set; }

    [Required]
    [MaxLength(20)]
    public string Name { get; set; }

    [Required]
    [MaxLength(100)]
    public string? Description { get; set; }

    [MaxLength(21, ErrorMessage = "Longitude must be 21 characters or less")]
    public string? Longitude { get; set; }

    [MaxLength(21, ErrorMessage = "Latitude must be 21 characters or less")]
    public string? Latitude { get; set; }
}