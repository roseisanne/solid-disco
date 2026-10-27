using System.ComponentModel.DataAnnotations;
using Clinic.Data;
using Clinic.Domain;

namespace Clinic.Services;

public record RegisterPatientRequest(
    string FirstName, string LastName, DateOnly DateOfBirth, string Phone, string Email);

public class PatientService(IPatientRepository repository, IClock clock)
{
    // DONE for you. Read it: this is the style the team expects.
    public int Register(RegisterPatientRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FirstName))
            throw new ValidationException("First name is required.");
        if (string.IsNullOrWhiteSpace(request.LastName))
            throw new ValidationException("Last name is required.");
        if (request.DateOfBirth > DateOnly.FromDateTime(clock.Now))
            throw new ValidationException("Date of birth cannot be in the future.");

        var first = request.FirstName.Trim();
        var last = request.LastName.Trim();

        if (repository.Exists(first, last, request.DateOfBirth))
            throw new ConflictException("Patient already exists.");

        return repository.Add(new Patient
        {
            FirstName = first,
            LastName = last,
            DateOfBirth = request.DateOfBirth,
            Phone = request.Phone,
            Email = request.Email
        });
    }

    // TASK 1: implement me.
    public IReadOnlyList<Patient> SearchByLastName(string lastName)
    {
        var listOfLastNames = new List<Patient>();

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ValidationException("No input");

        var last = lastName.Trim();
        foreach (var item in repository.GetAll())
        {
            if (last.ToLower().Equals(item.LastName.ToLower()))
                listOfLastNames.Add(item);
        }

        return listOfLastNames.OrderBy(p => p.FirstName).ToList();;
    }
}
