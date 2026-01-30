// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------

using Gefc.AI.Llm.Abstractions;
using Gefc.AI.Llm.Internal;
using Gefc.AI.Llm.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Gefc.AI.Llm;

/// <summary>
/// Extension methods for setting up LLM services in an <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Gefc AI LLM services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="configure">A delegate to configure the <see cref="GefcLlmOptions"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddGefcLlm(
        this IServiceCollection services,
        Action<GefcLlmOptions> configure)
    {
        services.Configure(configure);

        services.AddSingleton<ProviderRegistry>();
        services.AddSingleton<ILlmService, LlmService>();

        return services;
    }
}
