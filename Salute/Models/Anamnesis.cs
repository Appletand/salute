using System.ComponentModel.DataAnnotations;

namespace Salute.Models;

public class Anamnesis
{
    [Key]
    public int Id { get; set; }

    // Relacionamento
    public int PatientId { get; set; }
    public Patient? Patient { get; set; }

    // Datas
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }

    // ========== DADOS PESSOAIS ==========
    public string? Profession { get; set; }
    public string? MaritalStatus { get; set; }

    // ========== HISTÓRICO DE SAÚDE ==========
    public string? MedicalConditions { get; set; }      // Doenças pré-existentes
    public string? Allergies { get; set; }              // Alergias
    public string? CurrentMedications { get; set; }     // Medicamentos em uso
    public string? PreviousSurgeries { get; set; }      // Cirurgias anteriores
    public string? Hospitalizations { get; set; }       // Internações

    // ========== HISTÓRICO ODONTOLÓGICO ==========
    public string? PreviousTreatments { get; set; }     // Tratamentos anteriores
    public string? LastDentalVisit { get; set; }        // Última visita ao dentista
    public string? DentalHygiene { get; set; }          // Hábitos de higiene
    public bool? UsesDentalProsthesis { get; set; }     // Usa prótese?
    public bool? HasBleedingGums { get; set; }          // Sangramento gengival?
    public bool? HasSensitiveTeeth { get; set; }        // Dentes sensíveis?

    // ========== HÁBITOS ==========
    public string? Smoking { get; set; }                 // Tabagismo
    public string? Alcohol { get; set; }                 // Consumo de álcool
    public string? Diet { get; set; }                    // Hábitos alimentares
    public string? PhysicalActivity { get; set; }       // Atividade física

    // ========== QUEIXA PRINCIPAL ==========
    [Required(ErrorMessage = "Queixa principal é obrigatória")]
    public string MainComplaint { get; set; } = string.Empty;
    public string? Symptoms { get; set; }
    public string? SymptomsDuration { get; set; }
    public string? PainLevel { get; set; }              // Nível de dor (1-10)

    // ========== SINAIS VITAIS ==========
    public int? BloodPressureSystolic { get; set; }     // Pressão sistólica
    public int? BloodPressureDiastolic { get; set; }    // Pressão diastólica
    public int? HeartRate { get; set; }                 // Frequência cardíaca
    public decimal? Temperature { get; set; }           // Temperatura
    public decimal? RespiratoryRate { get; set; }       // Frequência respiratória

    // ========== RESPONSÁVEL (para menores/dependentes) ==========
    public string? ResponsibleName { get; set; }
    public string? ResponsibleContact { get; set; }
    public string? ResponsibleRelation { get; set; }

    // ========== OBSERVAÇÕES ==========
    public string? Observations { get; set; }
    public string? Attachments { get; set; }            // Caminho para arquivos/imagens
}