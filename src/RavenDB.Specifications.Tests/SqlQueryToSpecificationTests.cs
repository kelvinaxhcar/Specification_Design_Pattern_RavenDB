using Raven.TestDriver;
using RavenDB.Specifications.Tests.Entities;
using RavenDB.Specifications;
using Xunit;

namespace RavenDB.Specifications.Tests
{
    public class SqlQueryToSpecificationTests : RavenTestDriver
    {
        [Fact]
        public void Should_Filter_By_Sql_Simple_Equality()
        {
            using var store = GetDocumentStore();
            using (var session = store.OpenSession())
            {
                session.Store(new Product { Id = "1", Name = "Product 1" });
                session.Store(new Product { Id = "2", Name = "Product 2" });
                session.SaveChanges();
            }

            using (var session = store.OpenSession())
            {
                var filteredProducts = Queries<Product>.FilterBySql(session, "Name = 'Product 1'").ToList();

                Assert.Single(filteredProducts);
                Assert.Equal("Product 1", filteredProducts[0].Name);
            }
        }

        [Fact]
        public void Should_Filter_By_Sql_With_And()
        {
            using var store = GetDocumentStore();
            using (var session = store.OpenSession())
            {
                session.Store(new Product { Id = "1", Name = "Product 1", Brand = "Brand A" });
                session.Store(new Product { Id = "2", Name = "Product 2", Brand = "Brand A" });
                session.Store(new Product { Id = "3", Name = "Product 1", Brand = "Brand B" });
                session.SaveChanges();
            }

            using (var session = store.OpenSession())
            {
                var filteredProducts = Queries<Product>.FilterBySql(session, "Name = 'Product 1' AND Brand = 'Brand A'").ToList();

                Assert.Single(filteredProducts);
                Assert.Equal("Product 1", filteredProducts[0].Name);
                Assert.Equal("Brand A", filteredProducts[0].Brand);
            }
        }

        [Fact]
        public void Should_Filter_By_Sql_With_Or()
        {
            using var store = GetDocumentStore();
            using (var session = store.OpenSession())
            {
                session.Store(new Product { Id = "1", Name = "Product 1" });
                session.Store(new Product { Id = "2", Name = "Product 2" });
                session.Store(new Product { Id = "3", Name = "Product 3" });
                session.SaveChanges();
            }

            using (var session = store.OpenSession())
            {
                var filteredProducts = Queries<Product>.FilterBySql(session, "Name = 'Product 1' OR Name = 'Product 2'").ToList();

                Assert.Equal(2, filteredProducts.Count);
                Assert.Contains(filteredProducts, x => x.Name == "Product 1");
                Assert.Contains(filteredProducts, x => x.Name == "Product 2");
            }
        }

        [Fact]
        public void Should_Filter_By_Sql_With_GreaterThan()
        {
            using var store = GetDocumentStore();
            using (var session = store.OpenSession())
            {
                session.Store(new Product { Id = "1", Name = "P1", Price = 10 });
                session.Store(new Product { Id = "2", Name = "P2", Price = 20 });
                session.SaveChanges();
            }

            using (var session = store.OpenSession())
            {
                var filteredProducts = Queries<Product>.FilterBySql(session, "Price > 15").ToList();

                Assert.Single(filteredProducts);
                Assert.Equal("P2", filteredProducts[0].Name);
            }
        }

        [Fact]
        public void Should_Filter_By_Sql_With_Like_As_StartsWith()
        {
            using var store = GetDocumentStore();
            using (var session = store.OpenSession())
            {
                session.Store(new Product { Id = "1", Name = "Apple" });
                session.Store(new Product { Id = "2", Name = "Banana" });
                session.SaveChanges();
            }

            using (var session = store.OpenSession())
            {
                var filteredProducts = Queries<Product>.FilterBySql(session, "Name LIKE 'Ap%'").ToList();

                Assert.Single(filteredProducts);
                Assert.Equal("Apple", filteredProducts[0].Name);
            }
        }

        [Fact]
        public void Should_Filter_By_Sql_With_Parentheses()
        {
            using var store = GetDocumentStore();
            using (var session = store.OpenSession())
            {
                session.Store(new Product { Name = "Monitor", Brand = "Dell", Price = 200 });
                session.Store(new Product { Name = "Mouse", Brand = "Logitech", Price = 25 });
                session.Store(new Product { Name = "Keyboard", Brand = "Dell", Price = 50 });
                session.SaveChanges();
            }

            using (var session = store.OpenSession())
            {
                var query = "(Brand = 'Dell' OR Brand = 'Logitech') AND Price < 100";
                var filteredProducts = Queries<Product>.FilterBySql(session, query).ToList();

                Assert.Equal(2, filteredProducts.Count);
                Assert.Contains(filteredProducts, x => x.Name == "Mouse");
                Assert.Contains(filteredProducts, x => x.Name == "Keyboard");
            }
        }

        [Fact]
        public void Should_Filter_By_Sql_With_Not()
        {
            using var store = GetDocumentStore();
            using (var session = store.OpenSession())
            {
                session.Store(new Product { Id = "1", Name = "Product 1" });
                session.Store(new Product { Id = "2", Name = "Product 2" });
                session.SaveChanges();
            }

            using (var session = store.OpenSession())
            {
                var filteredProducts = Queries<Product>.FilterBySql(session, "NOT Name = 'Product 1'").ToList();

                Assert.Single(filteredProducts);
                Assert.Equal("Product 2", filteredProducts[0].Name);
            }
        }

        [Fact]
        public void Should_Filter_By_Sql_With_In()
        {
            using var store = GetDocumentStore();
            using (var session = store.OpenSession())
            {
                session.Store(new Product { Id = "1", Name = "P1", Brand = "A" });
                session.Store(new Product { Id = "2", Name = "P2", Brand = "B" });
                session.Store(new Product { Id = "3", Name = "P3", Brand = "C" });
                session.SaveChanges();
            }

            using (var session = store.OpenSession())
            {
                var filteredProducts = Queries<Product>.FilterBySql(session, "Brand IN ('A', 'B')").ToList();

                Assert.Equal(2, filteredProducts.Count);
                Assert.Contains(filteredProducts, x => x.Brand == "A");
                Assert.Contains(filteredProducts, x => x.Brand == "B");
            }
        }

        [Fact]
        public void Should_Filter_By_Sql_With_NotEqual()
        {
            using var store = GetDocumentStore();
            using (var session = store.OpenSession())
            {
                session.Store(new Product { Name = "P1", Brand = "A" });
                session.Store(new Product { Name = "P2", Brand = "B" });
                session.SaveChanges();
            }

            using (var session = store.OpenSession())
            {
                var filteredProducts = Queries<Product>.FilterBySql(session, "Brand != 'A'").ToList();

                Assert.Single(filteredProducts);
                Assert.Equal("P2", filteredProducts[0].Name);
            }
        }

        [Fact]
        public void Should_Filter_By_Sql_With_GreaterOrEqual()
        {
            using var store = GetDocumentStore();
            using (var session = store.OpenSession())
            {
                session.Store(new Product { Name = "P1", Price = 10 });
                session.Store(new Product { Name = "P2", Price = 20 });
                session.SaveChanges();
            }

            using (var session = store.OpenSession())
            {
                var filteredProducts = Queries<Product>.FilterBySql(session, "Price >= 15").ToList();

                Assert.Single(filteredProducts);
                Assert.Equal("P2", filteredProducts[0].Name);
                
                var filteredProducts2 = Queries<Product>.FilterBySql(session, "Price >= 10").ToList();
                Assert.Equal(2, filteredProducts2.Count);
            }
        }

        [Fact]
        public void Should_Filter_By_Sql_With_Complex_Nested_And_Not()
        {
            using var store = GetDocumentStore();
            using (var session = store.OpenSession())
            {
                session.Store(new Product { Name = "P1", Brand = "A", Price = 10 });
                session.Store(new Product { Name = "P2", Brand = "A", Price = 20 });
                session.Store(new Product { Name = "P3", Brand = "B", Price = 30 });
                session.SaveChanges();
            }

            using (var session = store.OpenSession())
            {
                // (Brand = 'A' OR Brand = 'B') AND NOT Price < 25
                var query = "(Brand = 'A' OR Brand = 'B') AND NOT Price < 25";
                var filteredProducts = Queries<Product>.FilterBySql(session, query).ToList();

                Assert.Single(filteredProducts);
                Assert.Equal("P3", filteredProducts[0].Name);
            }
        }

        [Fact]
        public void Should_Filter_By_Sql_With_IsNull()
        {
            using var store = GetDocumentStore();
            using (var session = store.OpenSession())
            {
                session.Store(new Product { Id = "1", Name = "P1", Brand = null });
                session.Store(new Product { Id = "2", Name = "P2", Brand = "B" });
                session.SaveChanges();
            }

            using (var session = store.OpenSession())
            {
                var filteredProducts = Queries<Product>.FilterBySql(session, "Brand IS NULL").ToList();

                Assert.Single(filteredProducts);
                Assert.Equal("P1", filteredProducts[0].Name);
            }
        }

        [Fact]
        public void Should_Filter_By_Sql_With_IsNotNull()
        {
            using var store = GetDocumentStore();
            using (var session = store.OpenSession())
            {
                session.Store(new Product { Id = "1", Name = "P1", Brand = null });
                session.Store(new Product { Id = "2", Name = "P2", Brand = "B" });
                session.SaveChanges();
            }

            using (var session = store.OpenSession())
            {
                var filteredProducts = Queries<Product>.FilterBySql(session, "Brand IS NOT NULL").ToList();

                Assert.Single(filteredProducts);
                Assert.Equal("P2", filteredProducts[0].Name);
            }
        }

        [Fact]
        public void Should_Filter_By_Sql_With_Like_As_Contains()
        {
            using var store = GetDocumentStore();
            using (var session = store.OpenSession())
            {
                session.Store(new Product { Id = "1", Name = "Pineapple" });
                session.Store(new Product { Id = "2", Name = "Apple" });
                session.Store(new Product { Id = "3", Name = "Banana" });
                session.SaveChanges();
            }

            using (var session = store.OpenSession())
            {
                var filteredProducts = Queries<Product>.FilterBySql(session, "Name LIKE '%app%'").ToList();

                Assert.Equal(2, filteredProducts.Count);
                Assert.Contains(filteredProducts, x => x.Name == "Pineapple");
                Assert.Contains(filteredProducts, x => x.Name == "Apple");
            }
        }

        [Fact]
        public void Should_Filter_By_Sql_With_Like_As_EndsWith()
        {
            using var store = GetDocumentStore();
            using (var session = store.OpenSession())
            {
                session.Store(new Product { Id = "1", Name = "Pineapple" });
                session.Store(new Product { Id = "2", Name = "Apple" });
                session.Store(new Product { Id = "3", Name = "Banana" });
                session.SaveChanges();
            }

            using (var session = store.OpenSession())
            {
                var filteredProducts = Queries<Product>.FilterBySql(session, "Name LIKE '%apple'").ToList();

                Assert.Equal(2, filteredProducts.Count);
                Assert.Contains(filteredProducts, x => x.Name == "Pineapple");
                Assert.Contains(filteredProducts, x => x.Name == "Apple");
            }
        }
    }
}
