namespace Clinic.Domain;

public class Patient
{
    public int Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public DateOnly DateOfBirth { get; set; }
    public string Phone { get; set; } = "";
    public string Email { get; set; } = "";
}

public enum AppointmentStatus { Scheduled, Cancelled }

public class Appointment
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int ProviderId { get; set; }
    public DateTime Start { get; set; }
    public TimeSpan Duration { get; set; }
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;

    public DateTime End => Start + Duration;
}
