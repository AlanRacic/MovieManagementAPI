# MovieManagementAPI — ASP.NET Core Web API + MVC Client (.NET 10)

## Overview
MovieManagementAPI is a two-tier movie management system consisting of:
- an **ASP.NET Core Web API** exposing RESTful endpoints, and  
- a separate **ASP.NET Core MVC client** that consumes the API via **HttpClient**.

The project demonstrates clean API design, **DTO-based boundaries** between the API and UI, asynchronous request handling, and a maintainable structure aligned with modern .NET development practices.

---

## Tech Stack
**Core**
- C# · .NET 10 · ASP.NET Core Web API · ASP.NET Core MVC

**Data & Integration**
- SQL Server · Entity Framework Core · LINQ · JSON · HttpClient

**Architecture & Practices**
- Dependency Injection · Repository Pattern · DTO-based contracts · Async/Await

---

## Key Features
- RESTful Web API with full CRUD endpoints for managing movie records  
- Clear separation between API and client using **Data Transfer Objects (DTOs)**  
- MVC client consuming endpoints asynchronously via **HttpClient**  
- Structured JSON request/response handling for reliable data exchange  
- Repository-based data access for maintainable persistence logic  
- Filtering and sorting for list views and query-style workflows  
- Validation and consistent error handling to support predictable API behavior  

---

## Architecture Notes
This repository is intentionally organized as a **backend API + dedicated client**:
- The **Web API** acts as the source of truth (domain and persistence).
- The **MVC client** focuses on UI, request orchestration, and presenting API data.
- **DTOs** define the boundary between layers to avoid leaking domain/persistence models.
- **Dependency Injection** keeps controllers and services loosely coupled and testable.

---

## What This Project Demonstrates
- Designing a maintainable REST API in ASP.NET Core  
- Consuming an API from an MVC client using HttpClient and async/await  
- Applying DTO-based boundaries and repository-style data access  
- Working with SQL Server-backed persistence via Entity Framework Core and LINQ  

---

## Repository
github.com/AlanRacic/MovieManagementAPI

## Status
Actively maintained and iterated to reflect professional API development practices and clean architectural structure.
