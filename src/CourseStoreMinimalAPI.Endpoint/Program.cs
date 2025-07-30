using CourseStoreMinimalAPI.AplicationService;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
//services
builder.Services.AddScoped<CategoryService>();
builder.Services.AddOutputCache();

var app = builder.Build();


//Pipeline
app.UseOutputCache();
app.MapScalarApiReference();
app.MapGet("/", () => "Hello World!").CacheOutput(c =>
{
    c.Expire(TimeSpan.FromHours(1));
});

app.Run();
