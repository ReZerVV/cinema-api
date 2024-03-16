using Cinema.Application.Utils.Api.Kinopoisk.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Cinema.Application.Utils.Api.Kinogo;
using Cinema.Application.Utils.Api.Kinopoisk;
using Cinema.Application.Services;
using Cinema.Application.Services.Abstractions;

namespace Cinema.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KinopoiskApiOptions>(
            opt => configuration.GetSection(nameof(KinopoiskApiOptions)).Bind(opt));
        services
            .AddTransient<KinogoApi>()
            .AddTransient<KinopoiskApi>();

        services
            .AddMediatR(opt => opt.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
            .AddSingleton<IMediaDownloadService, MediaDownloadService>();
        return services;
    }
}