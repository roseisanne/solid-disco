using Clinic.Data;
using Clinic.Services;

namespace Clinic.Tests;

public class PatientServiceTests
{
    private readonly InMemoryPatientRepository _repo = new();
    private readonly PatientService _service;

    public PatientServiceTests() => _service = new PatientService(_repo, TestData.Clock());

    private static RegisterPatientRequest Req(string first = "Pat", string last = "Smith", int year = 1990) =>
        new(first, last, new DateOnly(year, 1, 1), "555-0100", "pat@example.com");

    // ---------- Already passing (example of the style) ----------

    [Fact]
    public void Register_EmptyFirstName_Throws() =>
        Assert.Throws<ValidationException>(() => _service.Register(Req(first: "")));

    [Fact]
    public void Register_FutureDob_Throws() =>
        Assert.Throws<ValidationException>(() => _service.Register(Req(year: 2027)));

    [Fact]
    public void Register_Valid_ReturnsId()
    {
        var id = _service.Register(Req());
        Assert.True(id > 0);
    }

    [Fact]
    public void Register_Duplicate_ThrowsConflict()
    {
        _service.Register(Req());
        Assert.Throws<ConflictException>(() => _service.Register(Req()));
    }

    // ---------- TASK 1: these fail until you implement SearchByLastName ----------

    [Fact]
    public void Search_IsCaseInsensitive()
    {
        _service.Register(Req("Pat", "Smith"));
        _service.Register(Req("Ana", "Jones"));

        var results = _service.SearchByLastName("smith");

        Assert.Single(results);
        Assert.Equal("Smith", results[0].LastName);
    }

    [Fact]
    public void Search_IgnoresSurroundingSpaces()
    {
        _service.Register(Req("Pat", "Smith"));

        Assert.Single(_service.SearchByLastName("  SMITH "));
    }

    [Fact]
    public void Search_NoMatch_ReturnsEmptyList()
    {
        _service.Register(Req("Pat", "Smith"));

        Assert.Empty(_service.SearchByLastName("Nguyen"));
    }

    [Fact]
    public void Search_MultipleMatches_AreOrderedByFirstName()
    {
        _service.Register(Req("Zed", "Smith", 1980));
        _service.Register(Req("Amy", "Smith", 1985));

        var results = _service.SearchByLastName("Smith");

        Assert.Equal(new[] { "Amy", "Zed" }, results.Select(p => p.FirstName));
    }

    [Fact]
    public void Search_BlankInput_ThrowsValidation() =>
        Assert.Throws<ValidationException>(() => _service.SearchByLastName("  "));
}
