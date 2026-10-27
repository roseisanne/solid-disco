using Clinic.Domain;

namespace Clinic.Data;

public interface IPatientRepository
{
    int Add(Patient patient);
    bool Exists(string firstName, string lastName, DateOnly dateOfBirth);
    IReadOnlyList<Patient> GetAll();
}

public interface IAppointmentRepository
{
    int Add(Appointment appointment);
    Appointment? GetById(int id);
    IReadOnlyList<Appointment> GetByProvider(int providerId);
}

// In-memory versions so you never need a database for this exercise.
public class InMemoryPatientRepository : IPatientRepository
{
    private readonly List<Patient> _items = new();
    private int _nextId = 1;

    public int Add(Patient patient)
    {
        patient.Id = _nextId++;
        _items.Add(patient);
        return patient.Id;
    }

    public bool Exists(string firstName, string lastName, DateOnly dateOfBirth) =>
        _items.Any(p =>
            string.Equals(p.FirstName, firstName, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(p.LastName, lastName, StringComparison.OrdinalIgnoreCase) &&
            p.DateOfBirth == dateOfBirth);

    public IReadOnlyList<Patient> GetAll() => _items.ToList();
}

public class InMemoryAppointmentRepository : IAppointmentRepository
{
    private readonly List<Appointment> _items = new();
    private int _nextId = 1;

    public int Add(Appointment appointment)
    {
        appointment.Id = _nextId++;
        _items.Add(appointment);
        return appointment.Id;
    }

    public Appointment? GetById(int id) => _items.FirstOrDefault(a => a.Id == id);

    public IReadOnlyList<Appointment> GetByProvider(int providerId) =>
        _items.Where(a => a.ProviderId == providerId).ToList();
}
