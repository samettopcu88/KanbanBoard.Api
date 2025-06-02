# KanbanBoard API

A RESTful API for a Kanban-style task management system where tasks can be moved between different lists using a drag-and-drop approach. This backend project is built with a clean, modular architecture using ASP.NET Core Web API.

---

## Features

- Create new boards
- Auto-create default task lists (Backlog, To Do, In Progress, Done) when a board is created
- Create, delete, update, and move cards between lists
- Cards are automatically set to appear in the “Backlog” list
- Access boards via a custom PublicId
- The Public Id can be customized by the user and its uniqueness is checked.
- Use of DTOs and AutoMapper for data transformation
- Input validation with FluentValidation
- Layered architecture (Controller - Service - Repository)
- Swagger UI for API testing

---

##  Tech Stack

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- AutoMapper
- FluentValidation
- Swagger
- Postman

---

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- SQL Server
- Visual Studio or Visual Studio Code

### Run the Project

1. Clone the repository:

Clone the repo using the “Clone a repository” tab in Visual Studio.

or with using GitBash:
bash
git clone https://github.com/samettopcu88/KanbanBoard.Api.git
cd KanbanBoard.Api

2. Update appsettings.json with your own SQL Server connection string:

"ConnectionStrings": {
  "DefaultConnection": "Server=(yourlocalserver);Database=KanbanBoardDb;Trusted_Connection=True;"
}

3. Apply migrations and create the database:

Enter the update-database command in the Package Manager Console

4. Run the project:

Start the project by pressing F5 or clicking on the green “Start” button.

