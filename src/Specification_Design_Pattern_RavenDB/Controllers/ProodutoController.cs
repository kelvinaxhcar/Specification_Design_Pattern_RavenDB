using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using Specification_Design_Pattern_RavenDB.Especificacoes;

namespace Specification_Design_Pattern_RavenDB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProodutoController : ControllerBase
    {

        public IActionResult Get()
        {

            return Ok();
        }
    }
}
