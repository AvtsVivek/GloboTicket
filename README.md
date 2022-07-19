# GloboTicket ASP.NET Core Microservices Sample Application

This is the new Project Created by Gopal Krishna.
This is copied from Vivek Koppula project Globo Ticket.

GloboTicket is a sample ASP.NET Core Microservices application that you can learn about in the Pluralsight .NET Microservices Learning path. This path consists of the following courses:

- Microservices: The Big Picture
- Getting Started with ASP.NET Core Microservices
- Microservices Communication in ASP.NET Core
- Implementing a data management strategy for an ASP.NET Core Microservices Architecture
- Securing Microservices in ASP.NET Core
- Versioning and Evolving Microservices in ASP.NET Core
- Deploying ASP.NET Core microservices using Kubernetes and AKS
- Implementing cross-cutting concerns for ASP.NET Core microservices
- Strategies for Microservice Scalability and Availability in ASP.NET Core

https://app.pluralsight.com/paths/skills/net-microservices

https://app.pluralsight.com/library/courses/deploying-asp-dot-net-core-microservices-kubernetes-aks/table-of-contents

docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=P(ssw0rd0123)" -p 1436:1433 --name sql1 -h sql1 -d mcr.microsoft.com/mssql/server:2019-latest

docker run --name migrator-container -e "ConnectionStrings__Default=Data Source=.,1436;Initial Catalog=BvhHrSita;Integrated Security=False;User ID=sa;Password=P(ssw0rd0123);Connect Timeout=60;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;" bvhhrsitadbmigrator:dev

docker-compose -f docker-compose.yml -f docker-compose.override.yml build