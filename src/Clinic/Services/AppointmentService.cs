using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using Clinic.Data;
using Clinic.Domain;

namespace Clinic.Services;

public class AppointmentService(IAppointmentRepository repository, IClock clock)
{
    // TASK 2: implement me.
    public Appointment Book(int patientId, int providerId, DateTime start, TimeSpan? duration = null)
    {
        //if null do 30
        TimeSpan newDuration = duration ?? TimeSpan.FromMinutes(30);

        if (start < clock.Now)
            throw new ValidationException("Cannot book in the past");

        DateTime newEnd = start + newDuration;
        foreach (var appt in repository.GetByProvider(providerId))
        {
            if (start < appt.End && appt.Start < newEnd 
            && appt.Status != AppointmentStatus.Cancelled)
                throw new ConflictException
                ("Cannot book an appointment that overlaps with other appointments");
        }

        Appointment newAppointment = new Appointment
        {
            PatientId = patientId, 
            ProviderId = providerId, 
            Start = start, 
            Duration = newDuration,
        };
        repository.Add(newAppointment);
        
        return newAppointment;
    }

    // TASK 3: implement me.
    public void Cancel(int appointmentId)
    {
        var appt = repository.GetById(appointmentId);
        if (appt == null)
            throw new NotFoundException("Appt Id does not exist");
    
        appt.Status = AppointmentStatus.Cancelled;
    }

    // TASK 4: this "works" but has bugs. Tests will show you. Find and fix them.
    public IReadOnlyList<Appointment> GetDailySchedule(int providerId, DateTime date)
    {
        return repository.GetByProvider(providerId)
            .Where(a => a.Start.Day == date.Day)
            .ToList();
    }
}
