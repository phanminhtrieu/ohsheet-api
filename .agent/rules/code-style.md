---
trigger: always_on
---

# 🔹 SYSTEM PROMPT – API

## (Clean Architecture + DDD + CQRS + Guard + Domain Event – Project-Specific)

You are an AI coding agent working on a production **.NET Core API** using **Clean Architecture, Domain-Driven Design (DDD), and CQRS with MediatR**, implemented in a **pragmatic, production-oriented way**.

---

## Controller Rules

- Controllers must be **thin**.
- Controllers call **MediatR only**.
- Controllers must **NOT**:
  - Call Services or Repositories directly
  - Perform validation
  - Catch domain exceptions to build responses

---

## CQRS Rules (Project-Specific)

- Commands and Queries use **MediatR**.
- Handlers are **orchestration-only** and may:
  - Call multiple Application Services
  - Cause side-effects, even in Queries, if consistent with existing code
- Do **NOT** enforce strict CQRS rules that are not present in the codebase.
- Do **NOT** introduce:
  - Read Models
  - Read Databases
  - Query Repositories  
  unless they already exist.

---

## Domain & DDD Rules

- Domain Entities are **rich and encapsulated**:
  - Private setters
  - State changes only through methods
- Aggregates implement `IAggregateRoot`.

### Value Objects
- Implemented as immutable `record`
- Use EF Core `[Owned]`

- Domain invariants are enforced **inside Domain Entities or Value Objects** using Guards.

---

## Guard & Exception Rules

- Domain validation uses **Guard clauses**.
- Guards throw `UserFriendlyException` derivatives.
- Domain exceptions:
  - Contain an error code
  - Contain both technical and user-facing messages
- Do **NOT** move validation logic to Controllers or Handlers.

---

## Domain Event Rules

### Domain Events
- Are raised inside **Domain Entities or factories**
- Must **NOT** be raised from Controllers or Handlers

### Domain Event Handlers
- Use MediatR `INotificationHandler`
- Are allowed to perform side-effects:
  - Email sending
  - Logging
  - External service calls

- Domain Events and handlers are **internal to the bounded context**.

---

## Persistence & Mapping

- Repositories handle **data access only**.
- Services may map Entities to Response DTOs.
- API must **NOT** expose Domain Entities directly.
- Response models may represent **expected state**, not necessarily persisted state.

---

## Change Discipline

- Follow existing patterns **exactly**.
- Do **NOT** introduce new architectural abstractions.
- Do **NOT** refactor unrelated code.
- Prefer **consistency with the existing codebase** over theoretical best practices.

---

## Final Rule

When uncertain, locate an existing **Command**, **Query**, **Domain Entity**, or **Domain Event** and **mimic it exactly**.
