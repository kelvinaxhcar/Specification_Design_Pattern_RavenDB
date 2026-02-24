# RavenDB-Specifications 🚀

[![NuGet](https://img.shields.io/nuget/v/RavenDB-Specifications.svg)](https://www.nuget.org/packages/RavenDB-Specifications)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Build Status](https://img.shields.io/badge/Build-Success-brightgreen.svg)]()

A robust library implementing the **Specification Design Pattern** specifically for **RavenDB**. Decentralize your business logic from data access infrastructure with reusable, testable, and combinable queries.

---

### 🌐 Language / Idioma
[English](#-english-version) | [Português](#-versão-em-português)

---

## 🇺🇸 English Version

### 🚀 Why Use This?
RavenDB's LINQ provider is powerful but has specific behaviors (like `.Search()` for partial matches). This library abstracts those complexities, allowing you to define business rules as "Specifications" that are:
- **Reusable**: Use the same filter across multiple services.
- **Testable**: Verify logic without a database connection.
- **Combinable**: Merge logic with `And`, `Or`, and `Not`.

### 🛠️ Key Features
- **Search Abstraction**: Automatically converts `Contains` criteria to RavenDB's `.Search()` API.
- **SQL-to-Spec**: Dynamic filtering using simple SQL-like strings.
- **Async Support**: Native support for `IAsyncDocumentSession`.
- **Thread-Safe**: Designed for modern high-concurrency applications.

### 📦 Installation
```bash
dotnet add package RavenDB-Specifications
```

### 💻 Quick Example
```csharp
// 1. Define
var brandSpec = new EqualSpecification<Product>("Brand", "Dell");
var searchSpec = new ContainsSpecification<Product>("Name", "Monitor");

// 2. Combine
var finalSpec = brandSpec.And(searchSpec);

// 3. Execute
var results = await Queries<Product>.Filter(session, finalSpec).ToListAsync();
```

### 🔍 SQL Filtering
Convert SQL-style strings directly into specifications:
```csharp
string sql = "Brand = 'Dell' AND (Price < 1000 OR Category IN ('Electronics', 'IT')) AND Name LIKE '%Monitor%'";
var spec = SqlToSpecificationConverter.Convert<Product>(sql);

var products = await Queries<Product>.Filter(session, spec).ToListAsync();
```

---

## 🇧🇷 Versão em Português

### 🚀 Por que usar?
O provedor LINQ do RavenDB é excelente, mas exige cuidados específicos (como o uso de `.Search()` para buscas parciais). Esta biblioteca abstrai essa complexidade, permitindo definir regras de negócio como "Especificações" que são:
- **Reutilizáveis**: Use o mesmo filtro em múltiplos serviços.
- **Testáveis**: Valide a lógica sem precisar de conexão com o banco.
- **Combináveis**: Una lógicas complexas com `And`, `Or` e `Not`.

### 🛠️ Funcionalidades Principais
- **Abstração de Busca**: Converte automaticamente critérios `Contains` para a API `.Search()` do RavenDB.
- **SQL-to-Spec**: Filtros dinâmicos via strings SQL simples.
- **Suporte Assíncrono**: Totalmente compatível com `IAsyncDocumentSession`.
- **Thread-Safe**: Pronto para aplicações modernas escaláveis.

### 📦 Instalação
```bash
dotnet add package RavenDB-Specifications
```

### 💻 Exemplo Rápido
```csharp
// 1. Definir
var marcaSpec = new EqualSpecification<Product>("Brand", "Dell");
var buscaSpec = new ContainsSpecification<Product>("Name", "Monitor");

// 2. Combinar
var specFinal = marcaSpec.And(buscaSpec);

// 3. Executar
var resultados = await Queries<Product>.Filter(session, specFinal).ToListAsync();
```

### 🔍 Filtros via SQL
Converta strings no estilo SQL diretamente para especificações:
```csharp
string sql = "Brand = 'Dell' AND (Price < 1000 OR Category IN ('Electronics', 'IT')) AND Name LIKE '%Monitor%'";
var spec = SqlToSpecificationConverter.Convert<Product>(sql);

var produtos = await Queries<Product>.Filter(session, spec).ToListAsync();
```

---

## 🏗️ Solution Structure
- `RavenDB.Specifications`: The core library.
- `RavenDB.Specifications.Demo`: ASP.NET Core API showcasing integration.
- `RavenDB.Specifications.Tests`: Comprehensive test suite.

## 📄 License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.