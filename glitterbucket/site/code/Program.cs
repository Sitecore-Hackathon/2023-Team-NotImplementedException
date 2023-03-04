using GlitterBucket.ElasticSearchStorage;
using GlitterBucket.Receive;
using GlitterBucket.Receive.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.AddServerHeader = false;
});

builder.Configuration
    .AddEnvironmentVariables();

builder.Logging.AddConsole().AddConfiguration(builder.Configuration);

builder.Services.AddMvcCore();

builder.AddElasticSearchStorage();
builder.AddReceiveServices();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.MapControllers();

app.UseCors(p => p.AllowAnyOrigin().Build());

app.Run();
