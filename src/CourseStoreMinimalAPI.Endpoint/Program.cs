using CourseStoreMinimalAPI.Endpoint.Extensions;

var builder = WebApplication.CreateBuilder(args);
var app = builder.ConfigurServices();
app.ConfigurPipline();
app.Run();
