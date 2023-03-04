using Elasticsearch.Net;
using GlitterBucket.Receive.Configuration;
using Nest;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddEnvironmentVariables();

builder.Services.AddMvcCore();

builder.Services.AddTransient(sp => sp.GetRequiredService<IConfiguration>().GetSection("Sitecore").Get<ReceivingInstance>());
builder.Services.AddTransient(sp => sp.GetRequiredService<IConfiguration>().GetSection("ElasticSearch").Get<ElasticSearchStorage>());
builder.Services.AddTransient<IConnectionPool>(sp => new SingleNodeConnectionPool(sp.GetRequiredService<ElasticSearchStorage>().Uri));
builder.Services.AddTransient(sp => new ElasticClient(new ConnectionSettings(sp.GetRequiredService<IConnectionPool>())));


var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
}

app.UseStaticFiles();

app.MapControllers();

app.Run();
