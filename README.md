# AI Campus Assistant

An academic campus assistant system designed to bring common university services and information into one place. The supplied project contains the ASP.NET Core API and SQL Server data layer that support student and campus workflows.

## What the system supports

- User registration, login, and profile endpoints.
- Student and professor records.
- Course and exam information.
- Lecture schedules, materials, and attendance records.
- Announcements, notifications, filters, and dashboard endpoints.
- SQL Server persistence through Entity Framework Core.
- Swagger/OpenAPI documentation in Development and a `/health` endpoint.

The project is titled **AI Campus Assistant**, but the supplied archive contains the API/backend code; it does not include a separate AI model/service or client application.

## My contribution

I was responsible for the database part of the project: the SQL Server data layer and its integration with the API through Entity Framework Core. This includes `ApplicationDbContext`, entity relationships, constraints, and migrations.

## Database design

The data model covers users, students, professors, courses, exams, announcements, notifications, filters, lectures, lecture materials, and lecture attendance. The EF Core configuration includes:

- A unique index on user email.
- Professor-to-course and course-to-exam relationships.
- User-linked filters with cascading deletion.
- Courses linked to lectures; lectures linked to materials and attendance.
- Attendance linked to lectures and students, with a unique constraint for each lecture/student pair.

## Technology stack

- C# and ASP.NET Core 8
- Entity Framework Core 8
- Microsoft SQL Server
- Swagger / OpenAPI
- BCrypt.Net

## Project structure

```text
.
├── Controllers/       # REST API endpoints
├── Data/              # ApplicationDbContext and seed data
├── Migrations/        # EF Core schema migrations
├── Models/            # Database entities and request models
├── Properties/        # Local launch profiles
├── Program.cs         # API startup and service configuration
├── appsettings.json   # Safe connection-string template
├── .gitignore
└── AlCampusAssistant.csproj
```

## Run locally

### Requirements

- .NET 8 SDK
- Microsoft SQL Server (local or remote)

### Setup

1. Clone the repository and open a terminal in its root folder.
2. Configure `ConnectionStrings:DefaultConnection` locally. The committed `appsettings.json` contains a placeholder. Use .NET User Secrets or the `ConnectionStrings__DefaultConnection` environment variable; do not commit real database credentials.

   ```bash
   dotnet user-secrets init --project AlCampusAssistant.csproj
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<your SQL Server connection string>" --project AlCampusAssistant.csproj
   ```

3. Restore dependencies and run the API:

   ```bash
   dotnet restore AlCampusAssistant.csproj
   dotnet run --project AlCampusAssistant.csproj
   ```

4. With the Development launch profile, open `/swagger` at the local URL shown by the app. The health check is available at `/health`.

The application applies the checked-in EF Core migrations at startup. A SQL Server instance must be available and the connection string must be configured first.

## Current prototype limitations

- The password-reset endpoint currently does not validate its token, so authentication endpoints should be treated as a classroom prototype, not used for real accounts.
- Settings endpoints currently return defaults and do not persist changes. A `UserSettings` model exists, but it is not currently registered in the EF Core context/migration.
- Some profile statistics are placeholder values.
- CORS currently allows all origins; restrict it before deploying publicly.

These are known implementation gaps in the supplied source, documented here so the GitHub page does not overstate what is complete.

## GitHub preparation

This reviewed package excludes generated `bin/` and `obj/` output, local development settings, and the Visual Studio user file. It includes a `.gitignore` and a placeholder database connection string so local machine settings are not published.

No open-source license was included. Add a license file if you want to grant reuse permissions.
