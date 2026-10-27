using Clinic.Data;
using Clinic.Domain;

namespace Clinic.Services;

public class AppointmentService(IAppointmentRepository repository, IClock clock)
{
    // TASK 2: implement me.
    public Appointment Book(int patientId, int providerId, DateTime start, TimeSpan? duration = null)
    {
        throw new NotImplementedException();
    }

    // TASK 3: implement me.
    public void Cancel(int appointmentId)
    {
        throw new NotImplementedException();
    }

    // TASK 4: this "works" but has bugs. Tests will show you. Find and fix them.
    public IReadOnlyList<Appointment> GetDailySchedule(int providerId, DateTime date)
    {
        return repository.GetByProvider(providerId)
            .Where(a => a.Start.Day == date.Day)
            .ToList();
    }
}
