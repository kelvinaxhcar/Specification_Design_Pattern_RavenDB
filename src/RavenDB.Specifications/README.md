# RavenDB.Specifications 🚀

[![NuGet](https://img.shields.io/nuget/v/RavenDB.Specifications.svg)](https://www.nuget.org/packages/RavenDB.Specifications)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A robust library implementing the **Specification Design Pattern** specifically for **RavenDB**. Decentralize your business logic from data access infrastructure with reusable, testable, and combinable queries.

## 🚀 Key Features

| Feature | Description |
| :--- | :--- |
| **Search Abstraction** | Automatically converts `Contains` criteria to RavenDB's `.Search()` API for optimized full-text matching. |
| **Rich Operators** | Built-in support for `Equal`, `NotEqual`, `GreaterThan`, `LessThan`, `In`, `Between`, etc. |
| **Logical Composition** | Easily combine multiple business rules using `And`, `Or`, and `Not` methods. |
| **SQL-to-Spec** | Parse simple SQL-like strings into Specification objects for dynamic filtering. |
| **RavenDB Native** | Deep integration with `IAsyncDocumentSession` and `IDocumentSession`. |

## 🛠️ Usage Example

### 1. Define Business Rules
```csharp
public class ActivePremiumProductsSpec : Specification<Product>
{
    public override Expression<Func<Product, bool>> ToExpression()
    {
        return p => p.IsActive && p.Price > 100;
    }
}

// Or use built-in ones:
var brandSpec = new EqualSpecification<Product>("Brand", "Dell");
var searchSpec = new ContainsSpecification<Product>("Name", "Monitor");
```

### 2. Combine and Execute
```csharp
var finalSpec = brandSpec.And(searchSpec).Or(new ActivePremiumProductsSpec());

using var session = documentStore.OpenAsyncSession();
var results = await Queries<Product>
    .Filter(session, finalSpec)
    .ToListAsync();
```

### 🔍 SQL-to-Specification
Convert dynamic SQL filter strings into strongly-typed specifications. Supports `AND`, `OR`, `NOT`, `IN`, `IS NULL`, and `LIKE` (with `%` wildcards).

```csharp
string sql = "Brand = 'Dell' AND (Price < 1000 OR Category IN ('Electronics', 'IT')) AND Name LIKE '%Monitor%'";
var spec = SqlToSpecificationConverter.Convert<Product>(sql);

var products = await Queries<Product>.Filter(session, spec).ToListAsync();
```

## 📦 Installation

```bash
dotnet add package RavenDB.Specifications
```

## 📄 License
This project is licensed under the MIT License.
