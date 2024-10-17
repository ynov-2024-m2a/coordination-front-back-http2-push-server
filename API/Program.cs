using API.ServiceImpl;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();

// Add services to the container.

var app = builder.Build();

app.MapGrpcService<PersonneServiceImpl>();
app.MapGrpcService<MathServiceImpl>();

app.Run();
