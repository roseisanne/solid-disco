# ClinicPractice: Pair-Programming Interview Practice

A small C# project that imitates what a pair-programming session for a healthcare
.NET job looks like. The app mostly works. Some tests fail on purpose. Your job is to
make them pass while talking out loud, like you would with an interviewer.

## Setup (2 minutes)

```bash
cd ClinicPractice
dotnet test
```

Expected first result: some tests pass, some fail. That is correct. The failing ones are your tasks.
Tip: `dotnet test --filter "Search"` runs only tests with "Search" in the name.

## Layout

```
src/Clinic/Domain      Patient, Appointment
src/Clinic/Data        Repository interfaces + in-memory versions (no database needed)
src/Clinic/Services    The business logic you will edit
tests/Clinic.Tests     xUnit tests (do not edit these unless a task says to)
sql/                   SQL practice (separate from the C# work)
```

## The Tasks (do them in order, ~10 min each)

Each task is a "real story" like the ones an interviewer would hand you.

**Task 1: Search patients by last name** (`PatientService.SearchByLastName`)
> As a front-desk user, I can search patients by last name so I can find a chart quickly.
- Case-insensitive, ignores surrounding spaces, sorted by first name, blank input is a validation error.
- Good habit: run just the Search tests, pick ONE failing test, make it pass, repeat.

**Task 2: Book an appointment** (`AppointmentService.Book`)
> As a front-desk user, I can book a patient with a provider.
- Default 30 minutes. Cannot be in the past (use `clock.Now`, not `DateTime.Now`).
- No overlap for the same provider. Back-to-back is fine. Cancelled appointments don't block.
- Hint for overlap: two ranges overlap when `newStart < existing.End && existing.Start < newEnd`.

**Task 3: Cancel an appointment** (`AppointmentService.Cancel`)
- Sets status to Cancelled. Unknown id throws `NotFoundException`.

**Task 4: Find the bug** (`AppointmentService.GetDailySchedule`)
> "The doctor says the schedule screen shows the wrong appointments."
- Run the Schedule tests. Read each failure message. There are 3 separate bugs. Fix them one at a time.
- Interview skill being tested: debugging out loud ("this test says X, so I suspect line Y").

**Task 5: Refactor, then add a feature** (`CopayCalculator`)
- Tests already pass. Make the code cleaner WITHOUT breaking them (for example a dictionary
  of rules or a small Strategy per plan type). Then make `Medicaid_IsFlatFiveDollars` pass.
- Interview skill being tested: refactoring safely, design patterns, extensibility.

## SQL practice
Open `sql/practice.sql`. Try each question before looking at `sql/answers.sql`.

## How to practice like it's the real interview
1. Set a 10-minute timer per task.
2. Talk out loud the whole time, even alone. Say what you're about to do and why.
3. Before writing code, ask yourself the clarifying question you'd ask the interviewer
   (for example "should the search match partial names?").
4. If you get stuck, say what you've tried and what you'd try next. Don't go silent.
5. Finish by running ALL tests, then commit with a clear message.
