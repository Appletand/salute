using System.Threading.Tasks;

namespace Salute.Interfaces;

public interface IGeminiService
{
    Task<string> GenerateClinicalSummaryAsync(string prompt);
}
