Movie Management API

A robust ASP.NET Core Web API project designed to manage movies, actors, producers, genres, and reviews. The project implements a layered architecture with **Service** and **Repository** layers, follows best practices for validation, authentication, and database interaction, and uses MSSQL with stored procedures and Dapper for data access.

---

## Table of Contents

* [Features](#features)
* [Architecture](#architecture)
* [Models](#models)
* [API Endpoints](#api-endpoints)
* [Setup Instructions](#setup-instructions)
* [Authentication](#authentication)
* [Dependencies](#dependencies)
* [License](#license)

---

## Features

* **Layered Architecture**: Service and Repository layers with interfaces and CRUD operations.
* **Validation**: Request validation and proper HTTP status codes for errors (e.g., 404 Not Found).
* **Dependency Injection**: Proper service lifetime configuration for DI.
* **Authentication**: JWT-based authentication for secured endpoints.
* **Database Integration**: MSSQL integration with Dapper for queries and stored procedures.
* **Image Storage**: Upload and manage movie poster images in Supabase.
* **Query Parameter Filtering**: Filter movies by release year using query parameters.

---

## Architecture

This project follows a clean architecture pattern:

* **Controllers**: Handle HTTP requests and route to appropriate services.
* **Services**: Contain business logic and validation.
* **Repositories**: Handle database queries using Dapper and stored procedures.
* **Models**: Separate classes for Request, Response, and DB entities.
* **Authentication**: JWT-based authentication for all endpoints except login and signup.

---

## Models

### Movie

* Id
* Name
* YearOfRelease
* Plot
* Collection of Actors (not in DB Model)
* Collection of Genres (not in DB Model)
* Producer
* CoverImage (string URL stored in Supabase)

### Actor

* Id
* Name
* Bio
* DOB
* Gender

### Producer

* Id
* Name
* Bio
* DOB
* Gender

### Genre

* Id
* Name

### Review

* Id
* Message
* MovieId

---

## API Endpoints

### Movies

* `GET /movies?year={year}` - Fetch all movies for a given year.
* `POST /movies` - Add a new movie via stored procedure.
* `PUT /movies/{id}` - Update a movie via stored procedure.
* `DELETE /movies/{id}` - Delete a movie.
* `POST /movies/{id}/poster` - Upload movie poster to Supabase.

### Actors

* `GET /actors` - Fetch all actors.
* `GET /actors/{id}` - Fetch actor by id.
* `POST /actors` - Add a new actor.
* `PUT /actors/{id}` - Update actor.
* `DELETE /actors/{id}` - Delete actor.

### Producers, Genres, Reviews

* Similar CRUD endpoints with proper validation.

### Authentication

* `POST /auth/signup` - Create a new user account.
* `POST /auth/login` - Login and get a JWT token.

---

## Setup Instructions

1. Clone this repository:

   ```bash
   git clone https://github.com/yourusername/moviemanagementapi.git
   ```
2. Open project in Visual Studio / VS Code.
3. Configure MSSQL database connection in `appsettings.json`.
4. Run database migrations and create stored procedures.
5. Install NuGet packages:

   ```bash
   dotnet restore
   ```
6. Run the API:

   ```bash
   dotnet run
   ```
7. Test endpoints using Postman or Swagger UI.

---

## Authentication

All API endpoints except `/auth/signup` and `/auth/login` require a valid JWT token in the `Authorization` header:

```
Authorization: Bearer <access_token>
```



## Dependencies

* .NET 7 / .NET Core
* Dapper
* MSSQL
* Supabase C# SDK
* JWT Authentication
* FluentValidation



