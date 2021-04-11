using Mongo.Generic.Driver.Core;

namespace Arvan.Api.Models
{
    [DocumentName(nameof(Catalog))]
    public class Catalog : MongoEntityBase
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }
    }
}