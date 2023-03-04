using Elasticsearch.Net;
using GlitterBucket.Receive.Configuration;
using Nest;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddEnvironmentVariables();

builder.Services.AddTransient(sp => sp.GetRequiredService<IConfiguration>().GetSection("Sitecore").Get<ReceivingInstance>());
builder.Services.AddTransient(sp => sp.GetRequiredService<IConfiguration>().GetSection("ElasticSearch").Get<ElasticSearchStorage>());
builder.Services.AddTransient<IConnectionPool>(sp => new SingleNodeConnectionPool(sp.GetRequiredService<ElasticSearchStorage>().Uri));
builder.Services.AddTransient(sp => new ElasticClient(new ConnectionSettings(sp.GetRequiredService<IConnectionPool>())));


// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
