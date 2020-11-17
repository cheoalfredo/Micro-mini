using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.IO;
using System.Text;

await Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder => {    
    webBuilder.Configure(app =>
    {           
        app.UseRouting();
        app.UseEndpoints(route =>
        {
            route.MapGet("/", context => { return context.Response.WriteAsync("un micro bien mini"); });
            route.MapGet("/api/sample/{a:int}/{b:int}", context => {                 
                var entries =  context.GetRouteData().Values;
                return context.Response.WriteAsync($"a = {entries["a"]},  b = {entries["b"]}");
            });
            route.MapPost("/api/sample", async(context) => { 
                using (StreamReader reader = new StreamReader(context.Request.Body, Encoding.UTF8)) {  
                    var request = await reader.ReadToEndAsync();
                    await context.Response.WriteAsync(request);
                }
            });
        });
    });
}).ConfigureLogging(logging =>  {
    logging.ClearProviders();
    logging.AddConsole().SetMinimumLevel(LogLevel.Debug);
}).Build().RunAsync();