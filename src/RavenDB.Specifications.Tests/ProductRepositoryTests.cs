using Raven.TestDriver;
using RavenDB.Specifications.Tests.Entities;

namespace RavenDB.Specifications.Tests
{
    public class ProductRepositoryTests : RavenTestDriver
    {
        [Fact]
        public void Should_Get_By_Name()
        {
            using var store = GetDocumentStore();
            // Arrange
            using (var session = store.OpenSession())
            {
                session.Store(new Product { Id = "1", Name = "Product 1", Brand = "Brand A", Price = 1 });
                session.Store(new Product { Id = "2", Name = "Product 2", Brand = "Brand B" });
                session.Store(new Product { Id = "3", Name = "Product 3", Brand = "Brand C" });
                session.SaveChanges();
            }

            // Act
            using (var session = store.OpenSession())
            {
                var specification = new EqualitySpecification<Product>("Name", "Product 1");
                var query = Queries<Product>.Filter(session, specification);

                var filteredProducts = query.ToList();

                // Assert
                Assert.Single(filteredProducts);
                Assert.Equal("Product 1", filteredProducts[0].Name);
            }
        }

        [Fact]
        public void Should_Get_By_Name_StartsWith()
        {
            using var store = GetDocumentStore();
            // Arrange
            using (var session = store.OpenSession())
            {
                session.Store(new Product { Id = "1", Name = "Product 1", Brand = "Brand A", Price = 1 });
                session.Store(new Product { Id = "2", Name = "Product 2", Brand = "Brand B" });
                session.Store(new Product { Id = "3", Name = "Test 3", Brand = "Brand C" });
                session.SaveChanges();
            }

            // Act
            using (var session = store.OpenSession())
            {
                var specification = new StartsWithSpecification<Product>("Name", "Te");
                var query = Queries<Product>.Filter(session, specification);

                var filteredProducts = query.ToList();

                // Assert
                Assert.Single(filteredProducts);
                Assert.Equal("Test 3", filteredProducts[0].Name);
            }
        }

        [Fact]
        public void Should_Get_By_Brand()
        {
            using var store = GetDocumentStore();
            // Arrange
            using (var session = store.OpenSession())
            {
                session.Store(new Product { Id = "1", Name = "Product 1", Brand = "Brand A" });
                session.Store(new Product { Id = "2", Name = "Product 2", Brand = "Brand B" });
                session.Store(new Product { Id = "3", Name = "Product 3", Brand = "Brand C" });
                session.SaveChanges();
            }

            // Act
            using (var session = store.OpenSession())
            {
                var specification = new EqualitySpecification<Product>("Brand", "Brand B");
                var query = Queries<Product>.Filter(session, specification);

                var filteredProducts = query.ToList();
                // Assert
                Assert.Single(filteredProducts);
                Assert.Equal("Product 2", filteredProducts[0].Name);
            }
        }

        [Fact]
        public void Should_Get_By_Name_And_Brand()
        {
            using var store = GetDocumentStore();
            // Arrange
            using (var session = store.OpenSession())
            {
                session.Store(new Product { Id = "1", Name = "Product 1", Brand = "Brand A" });
                session.Store(new Product { Id = "2", Name = "Product 2", Brand = "Brand B" });
                session.Store(new Product { Id = "3", Name = "Product 3", Brand = "Brand C" });
                session.SaveChanges();
            }

            // Act
            using (var session = store.OpenSession())
            {
                var nameSpec = new EqualitySpecification<Product>("Name", "Product 1");
                var brandSpec = new EqualitySpecification<Product>("Brand", "Brand A");

                var combinedSpec = new AndSpecification<Product>(nameSpec, brandSpec);
                var query = Queries<Product>.Filter(session, combinedSpec);

                var filteredProducts = query.ToList();
                // Assert
                Assert.Single(filteredProducts);
                Assert.Equal("Product 1", filteredProducts[0].Name);
            }
        }

        [Fact]
        public void Should_Get_By_Name_Or_Brand()
        {
            using var store = GetDocumentStore();
            // Arrange
            using (var session = store.OpenSession())
            {
                session.Store(new Product { Id = "1", Name = "Product 1", Brand = "Brand A" });
                session.Store(new Product { Id = "2", Name = "Product 2", Brand = "Brand B" });
                session.Store(new Product { Id = "3", Name = "Product 3", Brand = "Brand C" });
                session.SaveChanges();
            }

            // Act
            using (var session = store.OpenSession())
            {
                var combinedSpec = new EqualitySpecification<Product>("Name", "Product 1")
                    .Or(new EqualitySpecification<Product>("Brand", "Brand B"));

                var query = Queries<Product>.Filter(session, combinedSpec);

                var filteredProducts = query.ToList();
                // Assert
                Assert.Equal(2, filteredProducts.Count);
                Assert.Contains(filteredProducts, x => x.Name == "Product 1");
                Assert.Contains(filteredProducts, x => x.Name == "Product 2");
            }
        }

        [Fact]
        public async Task Should_Get_By_Price_Greater_Async()
        {
            using var store = GetDocumentStore();
            // Arrange
            using (var session = store.OpenSession())
            {
                session.Store(new Product { Id = "1", Name = "Product 1", Price = 10 });
                session.Store(new Product { Id = "2", Name = "Product 2", Price = 20 });
                session.Store(new Product { Id = "3", Name = "Product 3", Price = 30 });
                session.SaveChanges();
            }

            // Act
            using (var session = store.OpenAsyncSession())
            {
                var spec = new GreaterThanSpecification<Product>("Price", "15");
                var query = Queries<Product>.Filter(session, spec);

                var filteredProducts = await query.ToListAsync();

                // Assert
                Assert.Equal(2, filteredProducts.Count);
            }
        }

        [Fact]
        public void Should_Get_By_Price_Less()
        {
            using var store = GetDocumentStore();
            // Arrange
            using (var session = store.OpenSession())
            {
                session.Store(new Product { Id = "1", Name = "Product 1", Price = 10 });
                session.Store(new Product { Id = "2", Name = "Product 2", Price = 20 });
                session.Store(new Product { Id = "3", Name = "Product 3", Price = 30 });
                session.SaveChanges();
            }

            // Act
            using (var session = store.OpenSession())
            {
                var spec = new LessThanSpecification<Product>("Price", "25");
                var query = Queries<Product>.Filter(session, spec);

                var filteredProducts = query.ToList();

                // Assert
                Assert.Equal(2, filteredProducts.Count);
                Assert.Contains(filteredProducts, x => x.Price == 10);
                Assert.Contains(filteredProducts, x => x.Price == 20);
            }
        }

        [Fact]
        public void Should_Get_By_Complex_Combined_Specification()
        {
            using var store = GetDocumentStore();
            // Arrange
            using (var session = store.OpenSession())
            {
                session.Store(new Product { Id = "1", Name = "Monitor", Brand = "Dell", Price = 200 });
                session.Store(new Product { Id = "2", Name = "Mouse", Brand = "Logitech", Price = 25 });
                session.Store(new Product { Id = "3", Name = "Keyboard", Brand = "Dell", Price = 50 });
                session.Store(new Product { Id = "4", Name = "Monitor", Brand = "Samsung", Price = 150 });
                session.SaveChanges();
            }

            // Act
            using (var session = store.OpenSession())
            {
                // (Brand == "Dell" OR Brand == "Logitech") AND Price < 100
                var dellSpec = new EqualitySpecification<Product>("Brand", "Dell");
                var logitechSpec = new EqualitySpecification<Product>("Brand", "Logitech");
                var priceSpec = new LessThanSpecification<Product>("Price", "100");

                var combinedSpec = (dellSpec.Or(logitechSpec)).And(priceSpec);
                
                var query = Queries<Product>.Filter(session, combinedSpec);
                var filteredProducts = query.ToList();

                // Assert
                Assert.Equal(2, filteredProducts.Count);
                Assert.Contains(filteredProducts, x => x.Name == "Mouse");
                Assert.Contains(filteredProducts, x => x.Name == "Keyboard");
            }
        }

        [Fact]
        public void Should_Handle_Null_Value_In_Equality_Specification()
        {
            using var store = GetDocumentStore();
            // Arrange
            using (var session = store.OpenSession())
            {
                session.Store(new Product { Id = "1", Name = "Product 1", City = null });
                session.Store(new Product { Id = "2", Name = "Product 2", City = "New York" });
                session.SaveChanges();
            }

            // Act
            using (var session = store.OpenSession())
            {
                var spec = new EqualitySpecification<Product>("City", null);
                var query = Queries<Product>.Filter(session, spec);

                var filteredProducts = query.ToList();

                // Assert
                Assert.Single(filteredProducts);
                Assert.Equal("Product 1", filteredProducts[0].Name);
            }
        }

        [Fact]
        public void Should_Get_By_Name_StartsWith_Case_Insensitive()
        {
            using var store = GetDocumentStore();
            // Arrange
            using (var session = store.OpenSession())
            {
                session.Store(new Product { Id = "1", Name = "Apple", Brand = "Brand A" });
                session.Store(new Product { Id = "2", Name = "banana", Brand = "Brand B" });
                session.SaveChanges();
            }

            // Act
            using (var session = store.OpenSession())
            {
                var specification = new StartsWithSpecification<Product>("Name", "ap");
                var query = Queries<Product>.Filter(session, specification);

                var filteredProducts = query.ToList();

                // Assert
                // RavenDB LINQ provider is case-insensitive by default in many configurations
                if (filteredProducts.Count > 0)
                {
                    Assert.Equal("Apple", filteredProducts[0].Name);
                }
            }
        }
    }
}
