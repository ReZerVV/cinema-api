using Cinema.Api.Middlewares;
using Cinema.Application;
using Cinema.Infrastructure;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication(builder.Configuration)
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

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 268435456;
});

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

app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseCors();
app.UseStaticFiles();

app.MapControllers();
app.MapSwagger();

app.Run();