using Arvan.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Mongo.Generic.Driver.Core;

await (CreateHostBuilder(args).Build()).RunAsync();

IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseGenericMongo();
            webBuilder.UseStartup<Startup>();
        });
