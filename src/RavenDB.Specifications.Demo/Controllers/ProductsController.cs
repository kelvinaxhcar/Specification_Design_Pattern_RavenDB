using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using RavenDB.Specifications;
using RavenDB.Specifications.Demo.Entities;

namespace RavenDB.Specifications.Demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IDocumentStore _store;

        public ProductsController(IDocumentStore store)
        {
            _store = store;
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] string sql)
        {
            using var session = _store.OpenAsyncSession();
            
            // Using the helper from the library
            var products = await Queries<Product>.FilterBySql(session, sql).ToListAsync();
            
            return Ok(products);
        }

        [HttpPost("seed")]
        public async Task<IActionResult> Seed()
        {
            using var session = _store.OpenAsyncSession();
            
            await session.StoreAsync(new Product { Name = "Laptop", Brand = "Dell", Price = 1200 });
            await session.StoreAsync(new Product { Name = "Monitor", Brand = "Dell", Price = 300 });
            await session.StoreAsync(new Product { Name = "Keyboard", Brand = "Logitech", Price = 50 });
            await session.StoreAsync(new Product { Name = "Mouse", Brand = "Logitech", Price = 25 });
            
            await session.SaveChangesAsync();
            
            return Ok("Sample data seeded.");
        }
    }
}
