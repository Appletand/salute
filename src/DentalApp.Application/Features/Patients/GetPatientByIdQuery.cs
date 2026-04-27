using DentalApp.Domain.Entities;

namespace DentalApp.Application.Features.Patients;

public record GetPatientByIdQuery(int Id);
public record GetPatientByIdResponse(Patient? Patient);

public class GetPatientByIdHandler
{
    private readonly IPatientRepository _repository;
    public GetPatientByIdHandler(IPatientRepository repository) => _repository = repository;
    public async Task<GetPatientByIdResponse> Handle(GetPatientByIdQuery query)
    {
        var patient = await _repository.GetByIdAsync(query.Id);
        return new GetPatientByIdResponse(patient);
    }
}