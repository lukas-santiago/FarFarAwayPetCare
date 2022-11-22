namespace SimuladorDoFirmware.Models;
public class BaseModel
{
    public int Id { get; set; }
    public string? User { get; set; }
    public DateTime UpdatedDate { get; set; }
    public DateTime CreatedOn { get; set; }
}
