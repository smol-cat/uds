using System.ComponentModel.DataAnnotations.Schema;

[Table("bookingSite")]
public class BookingSiteModel
{
    public int Id { get; set; }

    public string Name { get; set; }
}
