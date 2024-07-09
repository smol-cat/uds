using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uds.Models;

[Table("run")]
public class UdsRunModel
{
    [Required, Key]
    public int Id { get; set; }

    [Required, ForeignKey("fk_run_orderId")]
    public int OrderId { get; set; }

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }

    [Required, ForeignKey("fk_run_statusId")]
    public int? StatusId { get; set; }

    public string Status { get; set; }

    public UdsRunModel() { }

    public UdsRunModel StatusExtended(StatusModel status)
    {
        StatusId = null;
        Status = status.Description;
        return this;
    }
}
