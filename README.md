<<<<<<< HEAD
# Matrix QR Challenge

## Overview

Solution for the technical challenge implementing two REST APIs:

- .NET 9 API responsible for QR decomposition.
- Java 25 / Spring Boot API responsible for calculating statistics.

The communication between services is performed through HTTP.

## Architecture

Client -> .NET API -> Java API

The .NET service receives a rectangular matrix, calculates its
QR decomposition using Householder transformations and sends Q
and R to the Java service.

The Java service calculates:

- Maximum
- Minimum
- Average
- Total sum
- Whether any received matrix is diagonal

## Technologies

.NET 9
ASP.NET Core
Entity Framework Core 9
Java 25
Spring Boot 4
Docker
SQLite (development)
PostgreSQL (production recommendation)

## Running locally

docker compose up --build

.NET API:
http://localhost:8080

Java API:
http://localhost:8081

## Endpoint

POST /api/v1/matrices/qr

Example:

{
  "matrix": [
    [12, -51, 4],
    [6, 167, -68],
    [-4, 24, -41]
  ]
}

## Algorithm

QR decomposition is implemented using Householder reflections.

Complexity:
O(m*n²)

Memory:
O(m² + m*n)

## Design decisions

1. QR decomposition is used instead of a generic matrix rotation because
   QR decomposition is explicitly required by the functional requirements.

2. Statistics are calculated over all elements of Q and R.

3. EF Core is used for operation auditing.

4. Docker Compose is used for local orchestration.

5. Java API is stateless.

## Production deployment

Recommended Azure architecture:

- Azure Container Apps
- Azure Container Registry
- Azure Database for PostgreSQL
- Azure API Management
- Azure Key Vault
- Application Insights
- GitHub Actions
=======
# DesafioInterseguro
Desarrollo del desafío técnico solicitado por Interseguro como parte del proceso de selección
>>>>>>> 7aa1b60024bee408f17794a17c4d62378fd426b8
