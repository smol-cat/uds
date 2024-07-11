using System.ComponentModel.DataAnnotations;

namespace Uds.Models.Request;

public class UdsOrderCreateModel
{
    [Required]
    public string ProfileId { get; set; }

    [Required]
    public int ScheduleId { get; set; }

    [Required]
    public int BookingSiteId { get; set; }

    [Required, MinLength(3), MaxLength(3)]
    public string Origin { get; set; }

    [Required, MinLength(3), MaxLength(3)]
    public string Destination { get; set; }
}
