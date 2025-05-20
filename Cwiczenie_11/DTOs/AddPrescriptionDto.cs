namespace Cwiczenie_11.DTOs;

public class AddPrescriptionDto
{
    public string PatientFirstName { get; set; }
    public string PatientLastName { get; set; }
    public DateTime PatientBirthdate { get; set; }

    public int DoctorId { get; set; }

    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }

    public List<PrescribedMedicamentDto> Medicaments { get; set; }
}

public class PrescribedMedicamentDto
{
    public int MedicamentId { get; set; }
    public int Dose { get; set; }
    public string Description { get; set; }
}
