using Cinema.Application;
using Cinema.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    opt => opt.CustomSchemaIds(type => type.FullName));
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(
    opt => opt.AddDefaultPolicy(
        policy => policy
            .WithOrigins("http://localhost:5173")
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod()));

var app = builder.Build();

if (!Directory.Exists(Path.Combine(app.Environment.WebRootPath, "i")))
    Directory.CreateDirectory(Path.Combine(app.Environment.WebRootPath, "i"));
if (!Directory.Exists(Path.Combine(app.Environment.WebRootPath, "v")))
    Directory.CreateDirectory(Path.Combine(app.Environment.WebRootPath, "v"));

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseStaticFiles();

app.MapControllers();
app.MapSwagger();

app.Run();