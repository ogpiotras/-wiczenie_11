using Cwiczenie_11.Data;
using Cwiczenie_11.DTOs;
using Cwiczenie_11.Models;
using Cwiczenie_11.Services;

namespace Tutorial5.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly AddDbContext _context;

        public PrescriptionService(AddDbContext context)
        {
            _context = context;
        }

        public async Task AddPrescriptionAsync(AddPrescriptionDto dto)
        {
            if (dto.Medicaments == null || dto.Medicaments.Count == 0)
                throw new ArgumentException("Recepta musi zawierać leki.");

            if (dto.Medicaments.Count > 10)
                throw new ArgumentException("Recepta może zawierać maksymalnie 10 leków.");

            if (dto.DueDate < dto.Date)
                throw new ArgumentException("DueDate nie może być wcześniejszy niż Date.");

            var doctor = await _context.Doctors.FindAsync(dto.DoctorId);
            if (doctor == null)
                throw new ArgumentException("Doktor nie istnieje.");

            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.FirstName == dto.PatientFirstName &&
                                          p.LastName == dto.PatientLastName &&
                                          p.Birthdate == dto.PatientBirthdate);

            if (patient == null)
            {
                patient = new Patient
                {
                    FirstName = dto.PatientFirstName,
                    LastName = dto.PatientLastName,
                    Birthdate = dto.PatientBirthdate
                };
                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();
            }

            var prescription = new Prescription
            {
                Date = dto.Date,
                DueDate = dto.DueDate,
                IdDoctor = doctor.IdDoctor,
                IdPatient = patient.IdPatient,
                PrescriptionMedicaments = new List<PrescriptionMedicament>()
            };

            foreach (var med in dto.Medicaments)
            {
                var medicament = await _context.Medicaments.FindAsync(med.MedicamentId);
                if (medicament == null)
                    throw new ArgumentException($"Lek o ID {med.MedicamentId} nie istnieje.");

                prescription.PrescriptionMedicaments.Add(new PrescriptionMedicament
                {
                    IdMedicament = medicament.IdMedicament,
                    Dose = med.Dose,
                    Description = med.Description
                });
            }

            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();
        }

        public async Task<GetPatientDto> GetPatientAsync(int idPatient)
        {
            var patient = await _context.Patients
                .Include(p => p.Prescriptions)
                .ThenInclude(r => r.Doctor)
                .Include(p => p.Prescriptions)
                .ThenInclude(r => r.PrescriptionMedicaments)
                .ThenInclude(pm => pm.Medicament)
                .FirstOrDefaultAsync(p => p.IdPatient == idPatient);

            if (patient == null)
                return null;

            return new GetPatientDto
            {
                IdPatient = patient.IdPatient,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Birthdate = patient.Birthdate,
                Prescriptions = patient.Prescriptions
                    .OrderBy(r => r.DueDate)
                    .Select(r => new PrescriptionDto
                    {
                        IdPrescription = r.IdPrescription,
                        Date = r.Date,
                        DueDate = r.DueDate,
                        Doctor = new DoctorDto
                        {
                            IdDoctor = r.Doctor.IdDoctor,
                            FirstName = r.Doctor.FirstName,
                            LastName = r.Doctor.LastName
                        },
                        Medicaments = r.PrescriptionMedicaments.Select(pm => new MedicamentDto
                        {
                            IdMedicament = pm.Medicament.IdMedicament,
                            Name = pm.Medicament.Name,
                            Dose = pm.Dose,
                            Description = pm.Description
                        }).ToList()
                    }).ToList()
            };
        }
    }
}
