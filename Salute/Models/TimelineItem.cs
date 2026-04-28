namespace Salute.Models;

public enum TimelineItemType
{
    Anamnesis,
    Appointment,
    ClinicalNote
}

public class TimelineItem
{
    public DateTime Date { get; set; }
    public TimelineItemType Type { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string TypeLabel => Type switch
    {
        TimelineItemType.Anamnesis => "Anamnese",
        TimelineItemType.Appointment => "Consulta",
        TimelineItemType.ClinicalNote => "Nota Clínica",
        _ => "Evento"
    };
    public string TypeColor => Type switch
    {
        TimelineItemType.Anamnesis => "#5B4FCF",
        TimelineItemType.Appointment => "#2E7D32",
        TimelineItemType.ClinicalNote => "#C0392B",
        _ => "#555555"
    };
}