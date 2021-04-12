namespace Arvan.Api.Models
{
    using System;

    public class Catalog
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }
    }
}