# 🛒 E-Commerce Web API

A **scalable and secure E-Commerce Web API** built with **ASP.NET Core, Entity Framework Core, MediatR, AutoMapper, and Redis (for caching)**.  
It provides robust support for **user management, product catalog, orders, and payments**, with clean architecture and modular services.

---

## 🚀 Features

### 👤 **User & Authentication**
- JWT-based authentication and role-based authorization (`Admin`, `Manager`, `Customer`)
- Secure password hashing with Identity

### 🏷️ **Product & Category Management**
- CRUD operations for Products and Categories
- Pagination and filtering support
- AutoMapper-powered DTO transformations
- Image, currency, and metadata support

### 🛍️ **Order Management**
- Create and manage customer orders
- Real-time calculation of total order amount
- Automatic tracking of order status

### 💳 **Payment System**
- Full or rejected payment flow (no partials)
- Linked `Order ↔ Payment` relationship
- Cached results for better performance
- Clear transaction status tracking (Pending, Success, Failed)

### ⚙️ **Core Architecture**
- Clean Architecture + Repository + Unit of Work pattern
- Strongly typed ServiceResponse wrapper
- Global exception handling and validation
- In-memory caching with unique keys for users and pages

---

## 🧩 **Tech Stack**

| Layer | Technology |
|-------|-------------|
| **API Framework** | ASP.NET Core 8 Web API |
| **ORM** | Entity Framework Core |
| **Database** | SQL Server |
| **Authentication** | JWT & ASP.NET Identity |
| **Mapping** | AutoMapper |
| **Caching** | IMemoryCache / Redis (optional) |
| **Architecture** | Clean Architecture + Repository Pattern |

---

## 🧠 **Highlights**
- Built with **Clean Code principles**
- **Pagination** & **caching** for efficiency
- **ServiceResponse pattern** for consistent API results
- **Dependency Injection** across all services

---

## 📦 **Setup Instructions**

1. **Clone the repo**
   ```bash
   git clone https://github.com/your-username/ecommerce-webapi.git
   cd ecommerce-webapi
