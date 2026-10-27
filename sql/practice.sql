-- SQL Server syntax. Run in SSMS / Azure Data Studio, or adapt for any SQL database.
-- Step 1: run the CREATE + INSERT section. Step 2: answer the questions below.

CREATE TABLE Patients (
    PatientId INT IDENTITY PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName  NVARCHAR(50) NOT NULL,
    DateOfBirth DATE NOT NULL
);
CREATE TABLE Providers (
    ProviderId INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Specialty NVARCHAR(50) NOT NULL
);
CREATE TABLE Appointments (
    AppointmentId INT IDENTITY PRIMARY KEY,
    PatientId  INT NOT NULL REFERENCES Patients(PatientId),
    ProviderId INT NOT NULL REFERENCES Providers(ProviderId),
    StartTime DATETIME2 NOT NULL,
    Status NVARCHAR(20) NOT NULL   -- 'Scheduled' or 'Cancelled'
);
CREATE TABLE Charges (
    ChargeId INT IDENTITY PRIMARY KEY,
    PatientId INT NOT NULL REFERENCES Patients(PatientId),
    Amount DECIMAL(10,2) NOT NULL,
    ChargeDate DATE NOT NULL,
    IsPaid BIT NOT NULL DEFAULT 0
);

INSERT INTO Patients (FirstName, LastName, DateOfBirth) VALUES
 ('Pat','Smith','1990-01-01'), ('Ana','Jones','1985-06-12'),
 ('Sam','Stone','1978-03-30'), ('Lee','Nguyen','2001-11-05');
INSERT INTO Providers (Name, Specialty) VALUES
 ('Dr. Rivera','Cardiology'), ('Dr. Chen','Pediatrics'), ('Dr. Okafor','Dermatology');
INSERT INTO Appointments (PatientId, ProviderId, StartTime, Status) VALUES
 (1,1,'2026-01-20 09:00','Scheduled'), (1,1,'2026-01-21 10:00','Cancelled'),
 (2,1,'2026-01-20 09:30','Scheduled'), (3,2,'2026-01-22 14:00','Scheduled');
INSERT INTO Charges (PatientId, Amount, ChargeDate, IsPaid) VALUES
 (1, 150.00, DATEADD(day,-60,CAST(GETDATE() AS DATE)), 0),
 (1,  80.00, DATEADD(day,-45,CAST(GETDATE() AS DATE)), 0),
 (2,  40.00, DATEADD(day,-50,CAST(GETDATE() AS DATE)), 0),
 (2, 200.00, DATEADD(day,-5, CAST(GETDATE() AS DATE)), 0),
 (3, 120.00, DATEADD(day,-90,CAST(GETDATE() AS DATE)), 1);

-- ==================== QUESTIONS ====================
-- Q1 (warm-up): List patients whose last name starts with 'S', oldest first.
-- Q2 (JOIN + GROUP BY): For each provider, show the provider name and how many
--     NON-cancelled appointments they have. Include providers with zero.
-- Q3 (the classic): Patients with unpaid charges older than 30 days, showing patient
--     name and total owed, only where total owed is over 100. Biggest first.
--     (Uses JOIN, WHERE, GROUP BY, HAVING, ORDER BY.)
-- Q4 (LEFT JOIN): Patients who have never had an appointment.
-- Q5 (talk it through): What index would you add to speed up Q3, and why?
