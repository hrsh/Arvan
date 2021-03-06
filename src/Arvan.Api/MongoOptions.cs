namespace Arvan.Api
{
    public class MongoOptions
    {
        public string Database { get; set; }

        public string Collection { get; set; }

        public string Host { get; set; }

        public string Port { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        public string ConnectionString => $"mongodb://{User}:{Password}@{Host}:{Port}";
    }
}