using GlitterBucket.BrowserExtension;
using GlitterBucket.ElasticSearchStorage;
using GlitterBucket.Receive;

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
builder.Services.AddCors(s => s.AddPolicy(BrowserExtensionRetrieveController.CorsPolicyName, p => p.AllowAnyOrigin().Build()));

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.MapControllers();

app.Run();
