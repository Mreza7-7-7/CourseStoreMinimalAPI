using CourseStoreMinimalAPI.Endpoint.Extensions;

var builder = WebApplication.CreateBuilder(args);
var app = builder.ConfigurService();
app.ConfigurPipline();
app.Run();
