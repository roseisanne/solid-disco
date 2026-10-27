# Likely Interview Questions (short answers)

**What is TDD?** Write a failing test, write the simplest code to pass it, refactor. Repeat in small steps.

**Interface vs abstract class?** An interface is a contract with no state, a class can implement many.
An abstract class can share code and state, but a class can inherit only one.

**What is dependency injection and why use it?** Passing a class its dependencies (like a repository)
instead of letting it create them. It makes code loosely coupled and easy to test with fakes.

**What does n-tier mean?** Separating presentation (API), business logic (services), and data access
(repositories) so each layer has one job and can change independently.

**`IEnumerable` vs `IQueryable`?** `IEnumerable` filters in memory. `IQueryable` builds a query the
database runs, so filtering happens in SQL.

**`async/await`?** Frees the thread while waiting on I/O (database, HTTP) instead of blocking it.

**INNER vs LEFT JOIN?** INNER keeps only matching rows. LEFT keeps all rows from the left table, with NULLs where there's no match.

**`WHERE` vs `HAVING`?** WHERE filters rows before grouping. HAVING filters groups after aggregation.

**What is an index?** A sorted lookup structure that speeds up reads on a column but slows writes slightly.

**HTTP codes?** 200 OK, 201 Created, 400 bad input, 404 not found, 409 conflict, 500 server error.

**What is Git branching / a PR?** Work on a feature branch, open a pull request so teammates review before merging to main.

**What is CI (like TeamCity)?** A server that automatically builds and tests every push so broken code is caught early.

**HIPAA in one sentence?** US law protecting patient health information: limit access, don't log or leak it, keep an audit trail.

**Tell me about a bug you fixed / something you learned.** Prepare one real story using: problem, how you investigated, fix, what you learned.
