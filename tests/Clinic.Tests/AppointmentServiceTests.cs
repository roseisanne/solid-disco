using Clinic.Data;
using Clinic.Domain;
using Clinic.Services;

namespace Clinic.Tests;

public class AppointmentServiceTests
{
    private readonly InMemoryAppointmentRepository _repo = new();
    private readonly AppointmentService _service;

    public AppointmentServiceTests() => _service = new AppointmentService(_repo, TestData.Clock());

    private static DateTime Jan15(int hour, int minute = 0) => new(2026, 1, 15, hour, minute, 0);
    private static readonly TimeSpan HalfHour = TimeSpan.FromMinutes(30);

    // ---------- TASK 2: Book ----------

    [Fact]
    public void Book_DefaultsTo30Minutes()
    {
        var appt = _service.Book(patientId: 1, providerId: 1, start: Jan15(9));

        Assert.Equal(HalfHour, appt.Duration);
        Assert.True(appt.Id > 0);
        Assert.Equal(AppointmentStatus.Scheduled, appt.Status);
    }

    [Fact]
    public void Book_InThePast_ThrowsValidation() =>
        Assert.Throws<ValidationException>(() => _service.Book(1, 1, Jan15(7)));   // clock says 8:00

    [Fact]
    public void Book_OverlappingSameProvider_ThrowsConflict()
    {
        _service.Book(1, 1, Jan15(9));                    // 9:00 - 9:30

        Assert.Throws<ConflictException>(() => _service.Book(2, 1, Jan15(9, 15)));
    }

    [Fact]
    public void Book_BackToBack_IsAllowed()
    {
        _service.Book(1, 1, Jan15(9));                    // ends exactly at 9:30

        var second = _service.Book(2, 1, Jan15(9, 30));

        Assert.True(second.Id > 0);
    }

    [Fact]
    public void Book_SameTimeDifferentProvider_IsAllowed()
    {
        _service.Book(1, 1, Jan15(9));

        var other = _service.Book(2, 2, Jan15(9));

        Assert.True(other.Id > 0);
    }

    // ---------- TASK 3: Cancel ----------

    [Fact]
    public void Cancel_SetsStatusToCancelled()
    {
        var id = _repo.Add(new Appointment { ProviderId = 1, Start = Jan15(9), Duration = HalfHour });

        _service.Cancel(id);

        Assert.Equal(AppointmentStatus.Cancelled, _repo.GetById(id)!.Status);
    }

    [Fact]
    public void Cancel_UnknownId_ThrowsNotFound() =>
        Assert.Throws<NotFoundException>(() => _service.Cancel(999));

    [Fact]
    public void Cancel_FreesTheSlotForRebooking()
    {
        var first = _service.Book(1, 1, Jan15(9));
        _service.Cancel(first.Id);

        var again = _service.Book(2, 1, Jan15(9));

        Assert.True(again.Id > 0);
    }

    // ---------- TASK 4: GetDailySchedule has bugs. Which tests fail, and why? ----------

    [Fact]
    public void Schedule_IsSortedByStartTime()
    {
        _repo.Add(new Appointment { ProviderId = 1, Start = Jan15(11), Duration = HalfHour });
        _repo.Add(new Appointment { ProviderId = 1, Start = Jan15(9), Duration = HalfHour });

        var schedule = _service.GetDailySchedule(1, Jan15(0));

        Assert.Equal(new[] { 9, 11 }, schedule.Select(a => a.Start.Hour));
    }

    [Fact]
    public void Schedule_ExcludesCancelled()
    {
        _repo.Add(new Appointment { ProviderId = 1, Start = Jan15(9), Duration = HalfHour,
                                    Status = AppointmentStatus.Cancelled });

        Assert.Empty(_service.GetDailySchedule(1, Jan15(0)));
    }

    [Fact]
    public void Schedule_ExcludesSameDayNumberInAnotherMonth()
    {
        _repo.Add(new Appointment { ProviderId = 1, Start = new DateTime(2026, 2, 15, 9, 0, 0),
                                    Duration = HalfHour });

        Assert.Empty(_service.GetDailySchedule(1, Jan15(0)));
    }

    [Fact]
    public void Schedule_OnlyIncludesRequestedProvider()
    {
        _repo.Add(new Appointment { ProviderId = 2, Start = Jan15(9), Duration = HalfHour });

        Assert.Empty(_service.GetDailySchedule(1, Jan15(0)));
    }
}
