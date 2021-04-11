using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Arvan.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mongo.Generic.Driver.Core;

namespace Arvan.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly ILogger<CatalogController> _logger;

        private readonly IMongoRepository<Catalog> _repository;

        private readonly MongoOptions _options;

        public CatalogController(
            ILogger<CatalogController> logger,
            IMongoRepository<Catalog> repository,
            IOptions<MongoOptions> options
        ) =>
        (_logger, _repository, _options) = (logger, repository, options.Value);

        [HttpGet]
        public IEnumerable<Catalog> GetCatalogs() =>
            _repository.List(a => a.Name, DocumentSortOrder.Asc);

        [HttpGet("{id}")]
        public async Task<Catalog> GetCatalog(string id, CancellationToken ct) =>
            await _repository.FindAsync(a => a.Id == id, ct);

        [HttpPost]
        public IActionResult Create([FromBody] Catalog model)
        {
            _repository.Create(model);
            return Ok();
        }
    }
}