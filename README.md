# Seminar 3: Aggregates and Domain Events in Clean Architecture

## Task 1: Explore the Codebase

What Changed from Seminar 2? Familiarize yourself with the architectural changes and new features:

### 1. Clean Architecture Implementation

The project has been refactored from **3-layer architecture** to **Clean Architecture** with separate projects:

```
   Library.API    Library.Infrastructure
        ↓                ↓
         Library.Application
                ↓
           Library.Domain
```

**Key Changes:**

- **Library.Domain** - Pure domain layer with zero dependencies

  - Contains domain entities, aggregates, value objects
  - No references to infrastructure or frameworks

- **Library.Application** - Application layer

  - Commands, queries, and handlers (CQRS pattern)
  - Repository interfaces defined here
  - Depends only on Domain

- **Library.Infrastructure** - Infrastructure layer

  - EF Core implementation
  - Repository implementations
  - Depends on Domain & Application (dependency inversion)

- **Library.API** - Presentation layer
  - Controllers, middleware, configuration
  - Depends on all layers

**Benefits:**

- Clean dependency flow (dependencies point inward toward Domain)
- Domain logic is isolated and testable
- Infrastructure can be easily swapped without affecting business logic

### 2. Base Classes for Entities and Aggregates

- **Entity** ([Domain/Common/Entity.cs](Domain/Common/Entity.cs)) - Base class providing `Id` property
- **AggregateRoot** ([Domain/Common/AggregateRoot.cs](Domain/Common/AggregateRoot.cs)) - Extends `Entity`, enables publishing domain events via `RaiseDomainEvent()`

### 3. Domain Events Publishing

- Domain events are **automatically published** in overridden `SaveChangesAsync()` in [ApplicationDbContext](Infrastructure/Data/ApplicationDbContext.cs)
- Flow: Aggregate raises event → SaveChanges collects and publishes via MediatR → Event handlers respond

### 4. Refactored EF Core Configurations

- Separated EF Core configurations from domain entities into dedicated classes ([Infrastructure/Data/Configurations/](Infrastructure/Data/Configurations/))
- Each entity has its own `IEntityTypeConfiguration<T>` implementation
- Applied automatically via `ApplyConfigurationsFromAssembly()` in `ApplicationDbContext`

### Test Users:

```
admin@library.com / Admin@123
librarian@library.com / Librarian@123
member@library.com / Member@123
```
