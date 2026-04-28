using System.Threading.Tasks;
using Salute.Interfaces;

namespace Salute.Services;

public class MockGeminiService : IGeminiService
{
    public Task<string> GenerateClinicalSummaryAsync(string prompt)
    {
        // Retorna uma anamnese simulada
        return Task.FromResult(@"
Queixa principal: Dor de cabeça frequente.
Histórico médico: Hipertensão controlada.
Alergias: Nenhuma conhecida.
Medicamentos: Losartana.
Hábitos: Não fumante, consumo eventual de álcool.
Histórico odontológico: Limpeza a cada 6 meses.
Observações: Paciente relata bruxismo noturno.
");
    }
}
