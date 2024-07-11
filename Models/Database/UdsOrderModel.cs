using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Uds.Models.Request;

namespace Uds.Models;

[Table("order")]
public class UdsOrderModel
{
    [Required, Key]
    public int Id { get; set; }

    [Required]
    [JsonIgnore]
    public int BookingSiteId { get; set; }

    [Required]
    public string ProfileId { get; set; }

    [Required]
    [JsonIgnore]
    public int ScheduleId { get; set; }

    [Required, MinLength(3), MaxLength(3)]
    public string Origin { get; set; }

    [Required, MinLength(3), MaxLength(3)]
    public string Destination { get; set; }

    [Required]
    [JsonIgnore]
    public bool Deleted { get; set; }

    public BookingSiteModel BookingSite { get; set; }
    public ScheduleModel Schedule { get; set; }

    public UdsOrderModel() { }

    public UdsOrderModel(UdsOrderCreateModel createModel)
    {
        ProfileId = createModel.ProfileId;
        BookingSiteId = createModel.BookingSiteId;
        ScheduleId = createModel.ScheduleId;
        Origin = createModel.Origin;
        Destination = createModel.Destination;
    }
}
