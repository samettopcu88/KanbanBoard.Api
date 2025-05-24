# KanbanBoard API

A clean and modular Kanban board RESTful API built with ASP.NET Core. This project is designed using layered architecture (Repository-Service-Controller), AutoMapper, and FluentValidation. It's ideal as a project foundation for Kanban-style task management applications.

---

## Features

- Create boards with default task lists (Backlog, To Do, In Progress, Done)
- Add, move, and delete cards
- Retrieve boards by public ID
- Entity-DTO separation using AutoMapper
- Input validation with FluentValidation
- Clean architecture (Repository & Service layers)
- Swagger integrated for API testing

---

##  Tech Stack

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- AutoMapper
- FluentValidation
- Swagger (Swashbuckle)

---

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- SQL Server
- Visual Studio or Visual Studio Code

### Run the Project

1. Clone the repository:

bash
git clone https://github.com/samettopcu88/KanbanBoard.Api.git
cd KanbanBoard.Api

2. Update appsettings.json with your own SQL Server connection string:

"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=KanbanBoardDb;Trusted_Connection=True;"
}

3. Apply migrations and create the database:

dotnet ef database update

4. Run the project:

dotnet run
