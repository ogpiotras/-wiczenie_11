using Cwiczenie_11.DTOs;
using Cwiczenie_11.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cwiczenie_11.Controller;

[ApiController]
[Route("api/[controller]")]
public class PrescriptionsController : ControllerBase
{
    private readonly IPrescriptionService _service;

    public PrescriptionsController(IPrescriptionService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> AddPrescription([FromBody] AddPrescriptionDto dto)
    {
        try
        {
            await _service.AddPrescriptionAsync(dto);
            return Ok("Recepta została dodana.");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{idPatient}")]
    public async Task<IActionResult> GetPatient(int idPatient)
    {
        var result = await _service.GetPatientAsync(idPatient);
        if (result == null)
            return NotFound("Pacjent nie został znaleziony.");

        return Ok(result);
    }
}
