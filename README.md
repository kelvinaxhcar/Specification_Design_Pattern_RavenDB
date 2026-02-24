# RavenDB Specification Pattern

Uma biblioteca robusta que implementa o **Specification Design Pattern** especificamente para o **RavenDB**. Esta biblioteca permite desacoplar a lógica de negócio (critérios de filtragem) da infraestrutura de acesso a dados, proporcionando consultas reutilizáveis, testáveis e combináveis.

## 🚀 O Projeto

Este projeto fornece uma abstração sobre as consultas do RavenDB, permitindo que você defina critérios de busca como objetos de "Especificação". Ele resolve problemas comuns de repetição de lógica de `Where` e lida automaticamente com particularidades do RavenDB, como o uso de `Search` para buscas parciais de texto (contendo).

### Principais Funcionalidades:
- **Especificações Base**: `Equal`, `NotEqual`, `GreaterThan`, `LessThan`, etc.
- **Especificação de Busca**: `ContainsSpecification` integrada com o comando `.Search()` do RavenDB.
- **Composição Lógica**: Combine especificações usando `And`, `Or` e `Not`.
- **Conversor SQL**: Capacidade de converter strings SQL simples em objetos de Especificação.
- **Helpers de Consulta**: Métodos utilitários para aplicar especificações em sessões síncronas e assíncronas.

---

## 💡 Casos de Uso

1.  **Regras de Negócio Reutilizáveis**: Defina uma especificação `ProductIsAvailable` uma única vez e use-a em múltiplos controladores ou serviços.
2.  **Consultas Dinâmicas**: Construa filtros complexos em tempo de execução baseados em inputs do usuário sem sujar o código com múltiplos `if (string.IsNullOrEmpty(...))`.
3.  **Abstração de Busca**: Use `Contains` sem se preocupar com a `NotSupportedException` do RavenDB, pois a biblioteca converte isso internamente para a API de busca otimizada.
4.  **Testabilidade**: Teste suas regras de filtragem isoladamente da base de dados, verificando apenas se a expressão gerada está correta.

---

## 🛠️ Como Usar

### 1. Definindo uma Especificação Simples
Você pode usar as especificações pré-definidas ou criar a sua:

```csharp
var spec = new EqualSpecification<Product>("Brand", "Dell");
```

### 2. Combinando Especificações
A verdadeira força do padrão reside na composição:

```csharp
var brandSpec = new EqualSpecification<Product>("Brand", "Dell");
var priceSpec = new LessThanSpecification<Product>("Price", 1000);

// Combinação AND
var affordableDell = brandSpec.And(priceSpec);
```

### 3. Executando a Consulta
Use o helper `Queries<T>` para aplicar as especificações à sua sessão do RavenDB:

```csharp
using var session = documentStore.OpenAsyncSession();

// Aplicando filtros
var products = await Queries<Product>
    .Filter(session, affordableDell)
    .ToListAsync();
```

### 4. Usando Filtro via SQL
A biblioteca também suporta a conversão de filtros em formato SQL para especificações:

```csharp
string sqlFilter = "Brand = 'Dell' AND Price < 1000";
var products = await Queries<Product>
    .FilterBySql(session, sqlFilter)
    .ToListAsync();
```

---

## 🏗️ Estrutura da Solução

- `RavenDB.Specifications`: A biblioteca principal contendo os padrões e helpers.
- `RavenDB.Specifications.Demo`: Uma API de exemplo demonstrando a integração com ASP.NET Core.
- `RavenDB.Specifications.Tests`: Testes unitários para validar a lógica das especificações.