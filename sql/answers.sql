-- Try the questions first!

-- Q1
SELECT FirstName, LastName, DateOfBirth
FROM Patients
WHERE LastName LIKE 'S%'
ORDER BY DateOfBirth ASC;

-- Q2: put the status filter in the JOIN condition so providers with zero still appear
SELECT pr.Name, COUNT(a.AppointmentId) AS ActiveAppointments
FROM Providers pr
LEFT JOIN Appointments a
       ON a.ProviderId = pr.ProviderId AND a.Status <> 'Cancelled'
GROUP BY pr.ProviderId, pr.Name;

-- Q3
SELECT p.FirstName, p.LastName, SUM(c.Amount) AS TotalOwed
FROM Patients p
JOIN Charges c ON c.PatientId = p.PatientId
WHERE c.IsPaid = 0
  AND c.ChargeDate < DATEADD(day, -30, CAST(GETDATE() AS DATE))
GROUP BY p.PatientId, p.FirstName, p.LastName
HAVING SUM(c.Amount) > 100
ORDER BY TotalOwed DESC;

-- Q4
SELECT p.FirstName, p.LastName
FROM Patients p
LEFT JOIN Appointments a ON a.PatientId = p.PatientId
WHERE a.AppointmentId IS NULL;

-- Q5: An index on Charges (IsPaid, ChargeDate) INCLUDE (PatientId, Amount) lets the
-- database find old unpaid charges without scanning the whole table.
-- Also be able to explain: WHERE filters rows before grouping, HAVING filters groups after.
