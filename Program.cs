using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.WebEncoders.Testing;
using Microsoft.OpenApi.Models;
// inotify
Environment.SetEnvironmentVariable("DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE", "false");
// Use polling file watcher instead of inotify
Environment.SetEnvironmentVariable("DOTNET_USE_POLLING_FILE_WATCHER", "true");

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

// var usePolling = builder.Configuration.GetSection("FileWatching:UsePolling").Get<bool>();
// var pollingInterval = builder.Configuration.GetSection("FileWatching:PollingIntervalSeconds").Get<int>();
// // Simply enable reloadOnChange; polling is environment-dependent
// builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// builder.Services.AddSingleton<IConfigurationManager>(new ConfigurationManager(builder.Configuration));
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddLogging();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins(
                "http://10.100.13.44:3004",
                "https://10.100.13.44:3004",
                "http://localhost:3004",
                "http://172.16.239.169:3000"
                )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});
var app = builder.Build();
// if (usePolling)
// {
//     Task.Run(async () =>
//     {
//         var config = app.Services.GetRequiredService<IConfiguration>();
//         while (true)
//         {
//             await Task.Delay(TimeSpan.FromSeconds(pollingInterval)); // Poll every 5 seconds
//             if (config is IConfigurationRoot configRoot)
//             {
//                 configRoot.Reload();
//                 app.Logger.LogInformation("Configuration reloaded via polling at {Time}", DateTime.Now);
//             }
//         }
//     });
// }
app.Use(async (context, next) =>
{
    if (context.Request.Method == HttpMethods.Options)
    {
        context.Response.Headers.Append("Access-Control-Allow-Origin", "http://172.16.239.169:3000");
        context.Response.Headers.Append("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
        context.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type, Accept, Authorization, Origin, X-Requested-With");
        context.Response.Headers.Append("Access-Control-Allow-Credentials", "true");
        context.Response.StatusCode = 204; // No Content
        return;
    }
    await next();
});
// Apply Middleware
// app.UseMiddleware<RequestLoggingMiddleware>();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Budget Plan");
        c.RoutePrefix = string.Empty;
    });
}
app.MapGet("/", () => "this is test for hello");
app.MapGet("/test", (IConfiguration config) =>
    $"Hello! UsePolling: {config["FileWatching:UsePolling"]}, Interval: {config["FileWatching:PollingIntervalSeconds"]} seconds (Updated: {DateTime.Now})");
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowSpecificOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();
