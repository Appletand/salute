namespace SaluteWeb.Models;

public class ClinicalNote
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? Diagnosis { get; set; }
    public Patient? Patient { get; set; }
}
