# User Management Service

User Management Service is a RESTful API built with .NET 8 and C# 12. It provides a set of endpoints for managing users and roles in a system. The service supports operations like user registration, authentication, role assignment, and more. It's designed with a focus on clean architecture, using the MediatR library for CQRS pattern implementation.

## Features

* User Registration: Allows new users to register.
* User Authentication: Authenticates users and provides a JWT for authorized requests.
* Role Management: Allows assigning and removing roles from users.
* User Management: Supports operations like getting a user by ID, getting all users, updating a user, and deleting a user.



## Technologies

* [C# 12](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-12)
* [.NET 8](https://docs.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8)
* [MediatR](https://github.com/jbogard/MediatR) (for CQRS pattern implementation)
* [AutoMapper](https://automapper.org/) (for object-to-object mapping)
* [FluentValidation](https://fluentvalidation.net/) (for validating MediatR requests and responses)
* [ASP.NET Core Web API](https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-8.0)
* [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) (for data access)
* [BCrypt.Net-Next](https://github.com/BCrypt-Net/BCrypt.Net-Next) (for password hashing)
* [NLog](https://nlog-project.org/) (for logging)
* [Swagger](https://swagger.io/) (for API documentation)
* [Asp.Versioning](https://github.com/Microsoft/Aspnet-api-versioning) (for API versioning)

## Setup

Before running this service, the following steps are required:
1. Setup database.
2. Configure ConnectionString and JWTAuth in `appsettings.json`.
3. Run the service.

## Usage

The service exposes the following endpoints:

### User Management

* `POST /api/v1/users` - Register a new user
* `GET /api/v1/users/{userId}` - Get a user by ID
* `GET /api/v1/users` - Get all users
* `PUT /api/v1/users/{userId}` - Update a user
* `DELETE /api/v1/users/{userId}` - Delete a user
* `POST /api/v1/users/{userId}/role` - Assign a role to a user
* `DELETE /api/v1/users/{userId}/role/{roleName}` - Remove a role from a user
* `GET /api/v1/users/{userId}/roles` - Get roles assigned to a user
* `POST /api/v1/users/authenticate` - Authenticate a user and obtain a JWT token

### Role Management

* `POST /api/v1/roles` - Create a new role
* `GET /api/v1/roles` - Get all roles
* `DELETE /api/v1/roles/{roleName}` - Delete a role

## Logging

Service utilizes NLog [logging configuration](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-6.0#configure-logging), which looks like this:
```jsonc
{
  "throwConfigExceptions": true,
  "targets": {
    "logfile": {
      "type": "File",
      "fileName": "../../../Logs/Log-${shortdate}.log"
    },
    "logConsole": {
      "type": "Console"
    }
  },
  "rules": [
    {
      "logger": "*",
      "minLevel": "Trace",
      "writeTo": "logconsole"
    },
    {
      "logger": "*",
      "minLevel": "Info",
      "writeTo": "logfile"
    }
  ]
}
```


## Troubleshooting

### Problem: Service doesn't start

* Ensure that the database connection string is configured correctly in the `appsettings.json` file.
* Ensure that the JWT authentication settings are configured correctly in the `appsettings.json` file.
* Check the log files for any error messages that may provide more information about the issue.

### Problem: Database migrations fail

* Ensure that the database server is running and accessible.
* Ensure that the connection string is correct and points to the correct database.
* Check the log files for any error messages related to the database migration.