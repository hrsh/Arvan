using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Arvan.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

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
            _repository.List(a => a.Price > 0);

        [HttpGet("{id}")]
        public async Task<Catalog> GetCatalog(Guid id, CancellationToken ct) =>
            await _repository.FindAsync(a => a.Id, id, ct);

        [HttpPost]
        public IActionResult Create([FromBody] Catalog model)
        {
            _repository.Create(model);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            await _repository.DeleteAsync(a => a.Id == id, ct);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] Catalog model,
            CancellationToken ct)
        {
            await _repository
                .UpdateAsync(id, a => a.Id, model, ct);

            //await _repository.UpdateAsync(a => a.Id == id, model, ct);
            return NoContent();
        }

    }
}