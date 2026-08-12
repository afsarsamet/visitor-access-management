# Visitor Access Management System

[🇹🇷 Türkçe README](README_TR.md)

A web-based visitor and vehicle access management system built with **ASP.NET Core MVC**, **Entity Framework Core**, **SQL Server**, and **SignalR**.

The application is designed to manage visitor vehicle entry and exit operations, track vehicles currently inside the facility, handle company approval workflows, detect overstays, and generate Excel reports.

> Originally developed as a software engineering internship project for visitor and vehicle access management in an industrial facility.

---

## Features

* Visitor and vehicle entry registration
* Vehicle exit tracking
* Company autocomplete search
* Company approval workflow for unregistered companies
* Real-time vehicle overstay notifications with SignalR
* Background vehicle duration monitoring
* Cookie-based authentication
* Role-based authorization for security personnel and administrators
* Secure password hashing with BCrypt
* Temporary password generation using the Web Crypto API
* Date-based report filtering
* Excel report generation with ClosedXML
* SQL Server data persistence with Entity Framework Core

---

## Tech Stack

| Technology             | Purpose                              |
| ---------------------- | ------------------------------------ |
| .NET 8                 | Application runtime                  |
| ASP.NET Core MVC       | Backend web framework                |
| C#                     | Backend programming language         |
| Entity Framework Core  | ORM and database access              |
| SQL Server             | Relational database                  |
| SignalR                | Real-time communication              |
| Razor Views            | Server-side rendered UI              |
| JavaScript / jQuery    | Client-side functionality            |
| jQuery UI Autocomplete | Company search suggestions           |
| Bootstrap              | Responsive UI design                 |
| BCrypt.Net             | Password hashing                     |
| ClosedXML              | Excel report generation              |
| Web Crypto API         | Secure temporary password generation |

---

## Application Architecture

The application follows the **MVC (Model-View-Controller)** architecture.

```text
Browser / User
      ↓ HTTP
Razor View + JavaScript
      ↓
Controller
      ↓
Entity Framework Core
      ↓
SQL Server
```

For real-time overstay notifications:

```text
Background Service
      ↓
Database Check
      ↓
SignalR Hub
      ↓
Connected Browsers
      ↓
UI Update Without Page Refresh
```

---

## Authentication & Authorization

The application uses cookie-based authentication.

After a successful login:

1. The user's credentials are verified.
2. The password is compared against its BCrypt hash.
3. User information such as employee number, full name, and role is stored as claims.
4. ASP.NET Core creates a protected authentication cookie.
5. The authentication middleware restores the authenticated user on subsequent requests.

The application currently contains two main roles:

* **Security**
* **Admin**

Role-based authorization is used to restrict access to administrative operations.

---

## Vehicle Entry & Exit Management

Security personnel can register visitor vehicles with information such as:

* License plate
* Visitor full name
* Phone number
* Number of visitors
* Visit reason
* Company
* Entry time

The entry time is generated on the server.

When a vehicle leaves the facility, the application stores:

* Exit time
* Employee number of the security personnel
* Full name of the security personnel

This provides traceability for entry and exit operations.

---

## Company Autocomplete

The company field uses an AJAX-based autocomplete system.

When the user starts typing a company name:

```text
User Input
   ↓
AJAX GET Request
   ↓
Company Search Endpoint
   ↓
Entity Framework Query
   ↓
SQL Server
   ↓
JSON Response
   ↓
Autocomplete Suggestions
```

Only active companies are returned.

Instead of storing the company name directly in every visitor record, the system stores the related `CompanyId`.

Conceptually:

```text
VisitorLog.CompanyId
        ↓
Company.CompanyId
```

This provides a normalized relational database structure and reduces duplicated or inconsistent company names.

---

## Company Approval Workflow

If a company cannot be found through autocomplete, security personnel can submit a new company request.

The workflow is:

```text
Security personnel submits company name
        ↓
Company request is stored
        ↓
Administrator reviews the request
        ↓
Approved request becomes an active company
        ↓
Company becomes available in autocomplete
```

The employee information of the person submitting the request is obtained from authenticated claims rather than user-editable form values.

---

## Real-Time Overstay Detection

A background service periodically checks vehicles that are still inside the facility.

If a vehicle remains inside for more than the configured duration:

```text
Background Service
       ↓
Vehicle Duration Check
       ↓
Overstay Detected
       ↓
Database Updated
       ↓
SignalR Event
       ↓
Connected Browser
       ↓
Vehicle Row Updated
```

SignalR allows the server to push notifications directly to connected clients without requiring continuous AJAX polling.

---

## Temporary Password Generation

When a new security employee is created, the application generates a temporary password on the client side.

The password generator uses:

* Uppercase characters
* Lowercase characters
* Numbers
* Special characters
* `crypto.getRandomValues()`
* Fisher-Yates shuffle algorithm

The Web Crypto API is used instead of `Math.random()` because it provides cryptographically secure random values.

The generated password is sent to the backend over HTTPS and hashed with BCrypt before being stored in the database.

Plain-text passwords are not stored in the database.

---

## Reporting

Vehicle records can be filtered by:

* Start date
* End date
* Overstay status

Entity Framework Core dynamically builds the database query based on the selected filters.

Reports can also be exported as `.xlsx` files using **ClosedXML**.

The generated report may include:

* License plate
* Visitor information
* Company
* Entry time
* Exit time
* Overstay status
* Security personnel responsible for the exit

---

## Project Structure

```text
WebApplication1/
├── Controllers/
│   ├── FirmaController.cs
│   ├── GirisController.cs
│   ├── PersonelController.cs
│   ├── ReportsController.cs
│   ├── visitorLogsController.cs
│   ├── YoneticiAuthController.cs
│   └── YoneticiController.cs
│
├── Hubs/
│   └── AracHub.cs
│
├── Migrations/
│
├── Models/
│   ├── dbContextClass.cs
│   ├── Firma.cs
│   ├── FirmaTalep.cs
│   ├── Personel.cs
│   └── visitorLog.cs
│
├── Services/
│   └── SureKontrolIscisi.cs
│
├── Views/
│
├── wwwroot/
│   └── js/
│       └── sayac.js
│
├── Program.cs
├── appsettings.json
└── WebApplication1.csproj
```

---

## Database Models

### Personel

Represents users who can authenticate to the system.

Includes information such as:

* Employee number
* Full name
* Role
* Account status
* Password hash

### Firma

Represents companies that can be selected during visitor registration.

### FirmaTalep

Represents company registration requests waiting for administrator approval.

### visitorLog

Represents visitor vehicle entry and exit records.

A company can be associated with multiple visitor records:

```text
One Company
    ↓
Many Visitor Logs
```

---

## Getting Started

### Requirements

Make sure the following tools are installed:

* .NET 8 SDK
* SQL Server
* Visual Studio 2022 or Visual Studio Code
* Git

---

### Clone the Repository

```bash
git clone <REPOSITORY_URL>
cd WebApplication1
```

---

### Restore Dependencies

```bash
dotnet restore
```

---

### Configure the Database Connection

Sensitive configuration values should not be stored directly in the repository.

For local development, .NET User Secrets can be used:

```bash
dotnet user-secrets init
```

Set the SQL Server connection string:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=YOUR_SERVER;Database=VisitorAccessDb;Trusted_Connection=True;TrustServerCertificate=True;"
```

If an initial administrator password is required:

```bash
dotnet user-secrets set "InitialAdminPassword" "YOUR_SECURE_PASSWORD"
```

---

### Apply Database Migrations

If the Entity Framework CLI is not installed:

```bash
dotnet tool install --global dotnet-ef
```

Apply the migrations:

```bash
dotnet ef database update
```

---

### Run the Application

```bash
dotnet run
```

Open the HTTPS address displayed in the terminal.

---

## Security Considerations

The project applies several security-related practices, including:

* BCrypt password hashing
* Cookie-based authentication
* Role-based authorization
* Claims-based user information
* Server-generated entry timestamps
* User Secrets for sensitive local configuration
* Entity Framework parameterized queries

As the project evolves, additional improvements may include:

* Anti-forgery validation for all state-changing requests
* Server-side validation with dedicated ViewModels
* Login rate limiting
* First-login password reset
* Audit logging
* UTC-based time handling
* Database-level unique constraints
* Additional authorization for real-time communication endpoints

---

## Possible Future Improvements

* Unit and integration tests
* GitHub Actions CI pipeline
* Docker support
* Audit log system
* Account lockout and password reset
* Dashboard and statistics
* Pagination and advanced filtering
* Improved validation with ViewModels
* Email or SMS notifications
* Centralized exception handling and logging
* Deployment to a cloud environment

---

## What I Practiced

This project provided practical experience with:

* ASP.NET Core MVC
* Entity Framework Core
* SQL Server
* LINQ
* Dependency Injection
* HTTP request pipeline
* Middleware
* Cookie Authentication
* Claims
* Role-based Authorization
* Code First Migrations
* Foreign Keys
* Navigation Properties
* AJAX
* JSON
* SignalR
* Background Services
* BCrypt
* Web Crypto API
* Fisher-Yates Shuffle
* Excel report generation

---

## Project Status

The application was developed as a functional internship project and is currently maintained as a personal software development portfolio project.

Further refactoring, testing, security hardening, and deployment improvements may be added over time.
