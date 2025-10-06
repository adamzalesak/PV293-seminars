# Seminar 1: Library Management System - MediatR Refactoring


## Background

You have a working Library Management API that uses traditional service layer pattern. Your task is to refactor it to use MediatR library to implement the CQRS (Command Query Responsibility Segregation) pattern and improve code organization. 

## Tasks

### 1. Setup MediatR

- Install MediatR NuGet package in both `Library.BusinessLayer` and `Library.API` projects
- Register MediatR in the DI container in `Program.cs` (see the MediatR docs)
- Create marker interfaces: `ICommand`, `IQuery`, and `IDomainEvent` that inherit from appropriate MediatR interfaces. These provide semantic meaning to your handlers and enable specific behaviors (e.g., transactions only for commands, caching for queries) - CQRS

### 2. Refactor GetAllBooks Endpoint

- Create `GetAllBooksQuery` and its handler
- Look at the current implementation in `BookService.GetAllBooksAsync()` - what performance issue do you notice?
- **Hint:** Count how many database queries are executed when you have 10 books
- Fix the performance issue in your query handler
- Update the endpoint to use MediatR instead of `IBookService`

### 3. Refactor CreateBook Endpoint

- Create `CreateBookCommand` and its handler
- Move the book creation logic from `BookService.CreateBookAsync()` to the command handler
- Notice how the method does multiple things (validation, creation, statistics update)
- Update the endpoint to use MediatR

### 4. Refactor GetAuthors Endpoint

- Create `GetAuthorsQuery` and its handler
- Move the logic from `AuthorService` to the query handler
- Update the endpoint to use MediatR

## Optional Advanced Task

Move the author statistics update logic (in CreateBook) to a separate domain event handler:

- Create `BookCreatedEvent` that implements `IDomainEvent`
- Create `BookCreatedEventHandler` that updates author statistics
- Publish the event after creating a book
- This demonstrates proper separation of concerns

## Tips

- Test your endpoints using Swagger UI
- The N+1 problem occurs when you load a list of entities and then load related data for each one individually
- Entity Framework's `Include()` method can help with eager loading
- Using `Select()` projection can optimize queries by only fetching needed data
- Domain events help separate side effects from main business logic

Good luck! 🚀
