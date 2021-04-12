using Arvan.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

await (CreateHostBuilder(args).Build()).RunAsync();

IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
