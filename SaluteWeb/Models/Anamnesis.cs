using System.ComponentModel.DataAnnotations;

namespace SaluteWeb.Models;

public class Anamnesis
{
    [Key]
    public int Id { get; set; }

    // Relacionamento
    public int PatientId { get; set; }
    public Patient? Patient { get; set; }

    // Datas e Controle (Soft Delete)
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;

    // ========== DADOS PESSOAIS ==========
    public string? Profession { get; set; }
    public string? MaritalStatus { get; set; }

    // ========== HISTÓRICO DE SAÚDE ==========
    public string? MedicalConditions { get; set; }
    public string? Allergies { get; set; }

    // Mapeamento para suportar nomes curtos usados nas views
    public string History { get => MedicalConditions ?? string.Empty; set => MedicalConditions = value; }
    public string Medications { get => CurrentMedications ?? string.Empty; set => CurrentMedications = value; }

    public string? CurrentMedications { get; set; }
    public string? PreviousSurgeries { get; set; }
    public string? Hospitalizations { get; set; }

    // ========== HISTÓRICO ODONTOLÓGICO ==========
    public string? PreviousTreatments { get; set; }
    public string? LastDentalVisit { get; set; }
    public string? DentalHygiene { get; set; }
    public bool? UsesDentalProsthesis { get; set; }
    public bool? HasBleedingGums { get; set; }
    public bool? HasSensitiveTeeth { get; set; }

    // ========== HÁBITOS ==========
    public string? Smoking { get; set; }
    public string? Alcohol { get; set; }
    public string? Diet { get; set; }
    public string? PhysicalActivity { get; set; }

    // ========== QUEIXA PRINCIPAL ==========
    [Required(ErrorMessage = "Queixa principal é obrigatória")]
    public string MainComplaint { get; set; } = string.Empty;
    public string? Symptoms { get; set; }
    public string? SymptomsDuration { get; set; }
    public string? PainLevel { get; set; }

    // ========== SINAIS VITAIS ==========
    public int? BloodPressureSystolic { get; set; }
    public int? BloodPressureDiastolic { get; set; }
    public int? HeartRate { get; set; }
    public decimal? Temperature { get; set; }
    public decimal? RespiratoryRate { get; set; }

    // ========== RESPONSÁVEL ==========
    public string? ResponsibleName { get; set; }
    public string? ResponsibleContact { get; set; }
    public string? ResponsibleRelation { get; set; }

    // ========== OBSERVAÇÕES ==========
    public string? Observations { get; set; }
    public string? Attachments { get; set; }
}