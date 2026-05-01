namespace SaluteWeb.Models;

public class Appointment
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public DateTime DateTime { get; set; }
    public string Status { get; set; } = "Scheduled"; // Scheduled, Completed, Cancelled
    public string? Notes { get; set; }
    public Patient? Patient { get; set; }
}
