using System.ComponentModel.DataAnnotations;

namespace SaluteWeb.Models;

public class Patient
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Sobrenome é obrigatório")]
    public string LastName { get; set; } = string.Empty;

    public string? Email { get; set; }
    public string? Phone { get; set; }

    public DateTime DateOfBirth { get; set; } = DateTime.Today.AddYears(-30);

    public string Initials => $"{(FirstName?.FirstOrDefault())}{(LastName?.FirstOrDefault())}".ToUpper();

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}