using GlitterBucket.ElasticSearchStorage;
using GlitterBucket.Receive.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddEnvironmentVariables();

builder.Services.AddMvcCore();

builder.Services.AddTransient(sp => sp.GetRequiredService<IConfiguration>().GetSection("Sitecore").Get<ReceivingInstance>());
builder.AddElasticSearchStorage();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.MapControllers();

app.Run();
