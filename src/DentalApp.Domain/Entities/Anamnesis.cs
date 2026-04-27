// 1. Modelo específico de anamnese (criar novo arquivo)
// DentalApp.Domain/Entities/Anamnesis.cs

namespace DentalApp.Domain.Entities;

public class Anamnesis
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public DateTime CreatedAt { get; set; }

    // Histórico de saúde
    public string MedicalConditions { get; set; } = string.Empty;  // Doenças pré-existentes
    public string Allergies { get; set; } = string.Empty;         // Alergias
    public string CurrentMedications { get; set; } = string.Empty; // Medicamentos em uso
    public string Surgeries { get; set; } = string.Empty;         // Cirurgias anteriores

    // Histórico odontológico
    public string PreviousTreatments { get; set; } = string.Empty; // Tratamentos anteriores
    public string DentalHygiene { get; set; } = string.Empty;      // Hábitos de higiene
    public string LastDentalVisit { get; set; } = string.Empty;    // Última visita ao dentista

    // Hábitos
    public string Smoking { get; set; } = string.Empty;            // Tabagismo
    public string Alcohol { get; set; } = string.Empty;            // Consumo de álcool
    public string Diet { get; set; } = string.Empty;               // Hábitos alimentares

    // Queixa principal
    public string MainComplaint { get; set; } = string.Empty;      // Motivo da consulta
    public string Symptoms { get; set; } = string.Empty;           // Sintomas atuais
    public string SymptomsDuration { get; set; } = string.Empty;   // Há quanto tempo

    // Sinais vitais
    public decimal BloodPressure { get; set; }                     // Pressão arterial
    public int HeartRate { get; set; }                             // Frequência cardíaca
    public decimal Temperature { get; set; }                       // Temperatura

    // Responsáveis (para menores ou dependentes)
    public string ResponsibleName { get; set; } = string.Empty;
    public string ResponsibleContact { get; set; } = string.Empty;

    public Patient? Patient { get; set; }
}

// 2. ViewModel específico (criar)
// DentalApp.Presentation/ViewModels/AnamnesisViewModel.cs

// 3. Página de anamnese (criar)
// DentalApp.Presentation/Views/Anamnesis/AnamnesisPage.xaml

// 4. Serviço de IA para gerar anamnese
// DentalApp.Application/Features/AIAssistant/GenerateAnamnesisCommand.cs