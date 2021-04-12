using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Arvan.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) =>
            _configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

            var settings = _configuration
                .GetSection(nameof(MongoOptions))
                .Get<MongoOptions>();

            services.AddSingleton<IMongoClient>(m =>
            {
                return new MongoClient(settings.ConnectionString);
            });

            services.Configure<MongoOptions>(
               _configuration.GetSection(nameof(MongoOptions)),
               opt => _configuration.Bind(opt));

            services.AddScoped(
                typeof(IMongoRepository<>),
                typeof(MongoRepository<>));
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseHttpsRedirection();
            }

            app.UseRouting();

            app.UseEndpoints(e =>
            {
                e.MapGet("/", async a =>
                    await a.Response.WriteAsync("Arvan API with mongodb :)"));
                e.MapControllers();
            });
        }
    }
}
