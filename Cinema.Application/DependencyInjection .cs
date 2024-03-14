using Cinema.Application.Movies.Services;
using Cinema.Application.Movies.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Cinema.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddMediatR(opt => opt.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
            .AddSingleton<IMediaDownloadService, MediaDownloadService>();
        return services;
    }
}