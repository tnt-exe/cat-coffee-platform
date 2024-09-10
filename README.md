# Cat Coffee Platform

<div align="left">
  <a href="./LICENSE"><img src="https://img.shields.io/badge/License-MIT-yellow.svg" alt="license"/></a>
</div>

<div align="left">
  <a href="https://dotnet.microsoft.com/"><img src="https://img.shields.io/badge/.NET-6.0-5C2D91.svg" alt="dotnet" /></a>
  <a href="https://learn.microsoft.com/en-us/aspnet/core/razor-pages/"><img src="https://img.shields.io/badge/Razor-%235C2D91?style=flat&logo=blazor" alt="razor" /></a>
  <a href="https://getbootstrap.com/docs/5.1"><img src="https://img.shields.io/badge/Bootstrap-5.1.0-%238511FA?style=flat&logo=bootstrap&logoColor=white" alt="bootstrap" /></a>
  <a href="https://www.microsoft.com/en-us/sql-server"><img src="https://img.shields.io/badge/SQL_Server-CC2927?style=flat&logo=microsoft%20sql%20server" alt="sql" /></a>
</div>

## Introduction

The Cat Coffee Platform is a platform connecting cat coffee shops. It is design to make ease for customer to make a booking at their favorite cat coffee shop as well as ease for shop manager to manage their income. The project is for the course Building Cross-platform Backend Application with .NET at FU.

## System Design

These are the technologies that will be used in the making of this project:

- Database: MS SQL
- ORM: Microsoft Entity Framework Core
- Framework: Microsoft .NET
- Backend API: ASP.NET Core API with OData, JWT Authenticated
- View: Razor Pages

The system utilize the power of .NET tech stack, Razor Pages client and ASP.NET Web API. API implement using Repository pattern along with Singleton pattern, AutoMapper will also be use for DTO. OData and JWT authenticated are also support.

<div align="center">
  <img width="90%" src="./res/architecture.png" alt="architecture"/>
  <p>Architecture Design</p>
</div>
<br/>
<div align="center">
  <img width="90%" src="./res/conceptual-db.png" alt="concept-db"/>
  <p>Conceptual Database Design</p>
</div>
<br/>
<div align="center">
  <img width="90%" src="./res/logical-db.png" alt="logical-db"/>
  <p>Implemented Database</p>
</div>

## Contributors

<a href="https://github.com/tnt-exe/cat-coffee-platform/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=tnt-exe/cat-coffee-platform" alt="contributor" />
</a>
