using Cwiczenie_11.DTOs;

namespace Cwiczenie_11.Services;

public interface IPrescriptionService
{
    Task AddPrescriptionAsync(AddPrescriptionDto dto);
    Task<GetPatientDto> GetPatientAsync(int idPatient);
}