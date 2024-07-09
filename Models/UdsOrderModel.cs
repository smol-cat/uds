using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uds.Models;

[Table("order")]
public class UdsOrderModel
{
    [Required, Key]
    public int Id { get; set; }

    [Required]
    public int BookingSiteId { get; set; }

    [Required]
    public string ProfileId { get; set; }

    [Required]
    public int ScheduleId { get; set; }

    [Required, StringLength(3)]
    public string Origin { get; set; }

    [Required, StringLength(3)]
    public string Destination { get; set; }

    public bool Deleted { get; set; }

    public string ScheduleDescription { get; set; }
}
