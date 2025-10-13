# Seminar 2: Domain-Driven Design in Practice

## Task 1: Explore the Codebase

Familiarize yourself with the new features:

### 1. User Management & Authorization

- **3 Roles**: Admin, Librarian, Member
- **JWT Authentication** configured
- **Endpoints**: `/api/auth` (register, login, user management)
- **Test Users**:
  ```
  admin@library.com / Admin@123
  librarian@library.com / Librarian@123
  member@library.com / Member@123
  ```

### 2. Exception Handling

- `GlobalExceptionHandlingMiddleware` - centralized error handling
- No more try-catch blocks in endpoints

### 3. Domain Model (DDD)

- **Loan** (Aggregate Root) - represents book checkout
  - Contains collection of **Fine** entities
  - Uses **Money** value object
  - Status: Active, Returned, Lost, Completed
- **Authorization in Domain**: `Loan.DetermineBorrowerId()` enforces who can borrow for whom

### 4. Current Implementation

- `CheckOutBookCommand` - borrow a book
  - Members can only borrow for themselves
  - Librarians/Admins can create loans for anyone

## Task 2: Implement Domain Methods

### Add two use cases leveraging the rich domain model:

#### 1. Extend Loan Due Date

- Implement the `ExtendDueDate(int additionalDays)` method in `Loan.cs`
- New endpoint: `POST /api/loans/{loanId}/extend`
- Implement `ExtendLoanDueDateCommand` + handler that calls the domain method

**Acceptance Criteria**

- Only for **Active** loans
- `additionalDays > 0`
- Cannot extend **overdue** loans
- Total duration (`DueDate - LoanDate`) must not exceed **90 days**
- Updates `DueDate` by `additionalDays`
- Throws meaningful exceptions when rules are violated

#### 2. Report Damage

- Implement the `ReportDamage(string damageDescription, Money damageCost)` method in `Loan.cs`
- New endpoint: `POST /api/loans/{loanId}/report-damage`
- Implement `ReportDamageCommand` + handler that calls the domain method

**Acceptance Criteria**

- Not allowed when loan is **Lost** or **Completed**
- `damageDescription` must be **non-empty**
- `damageCost` is a valid `Money` (zero allowed; negativní částky odmítá `Money.Create`)
- Adds a **Fine** of type `DamageFee` with reason containing the description
- **Do NOT** auto-complete the loan here; completion happens only after all fines are settled

### Key Principles

- Business logic stays in the **domain (Loan aggregate)**
- Use **guard clauses** for validation
- Throw **meaningful exceptions**
- Aggregate must always be in a **valid state**

### Testing

Unit tests are already provided for the `Loan` aggregate.  
They cover both:

- **Implemented behavior** — e.g. creating, returning, marking as lost, fines
- **TODO methods** — `ExtendDueDate` and `ReportDamage`

> 💡 **Expected behavior:**
>
> - When you first run the tests, the ones covering the TODO methods will **fail** (since they are not yet implemented).
> - After completing both methods, **all tests should pass**.

#### How to Run Tests

```bash
dotnet test
```

#### How to Test API Endpoints

1. Run the API: `dotnet run --project Library.API`
2. Open Swagger UI
3. Login and get token
4. Click "Authorize" → Enter: `Bearer [token]`
5. Test endpoints

## Task 3 (Homework): Refactor to Clean Architecture

Current problem: Domain entities are in DataAccess layer - this violates DDD principles.

### Why Clean Architecture?

Clean Architecture is domain-centric - your business logic sits at the core with zero dependencies on infrastructure. Unlike layered architecture
where domain (buisness) depends on data access, here the dependency flow is inverted. This keeps your business rules pure and focused, prevents
infrastructure concerns from leaking into domain logic, and makes the codebase easier to understand and maintain as it grows.

### Goal

Transform from 3-layer to Clean Architecture:

```
   API    Infrastructure
    ↓        ↓
    Application
       ↓
     Domain
```

### Steps

1. **Create Library.Domain**

   - Move all domain objects (`Loan`, `Fine`, `Money`) here
   - No dependencies on anything

2. **Create Library.Application**

   - Move commands/queries from BusinessLayer
   - Define repository interfaces
   - Depends only on Domain

3. **Rename DataAccess → Library.Infrastructure**

   - Keep EF Core implementations
   - Implement repository interfaces (from the Application layer)
   - Depends on Domain & Application

4. **Update Library.API**
   - Wire up dependency injection
   - Depends on all layers

### Challenges to Solve

- Separate EF configurations from domain entities
- Handle `ClaimsPrincipal` dependency in Loan
- Move `ApplicationUser` and Identity properly
- Fix all dependency inversions
