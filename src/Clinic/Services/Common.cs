namespace Clinic.Services;

public class ValidationException(string message) : Exception(message);   // API would return 400
public class ConflictException(string message) : Exception(message);     // API would return 409
public class NotFoundException(string message) : Exception(message);     // API would return 404

// Lets tests control "now" so they don't depend on the real clock.
public interface IClock { DateTime Now { get; } }
public class SystemClock : IClock { public DateTime Now => DateTime.Now; }
