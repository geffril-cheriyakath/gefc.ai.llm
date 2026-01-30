// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Gefc.AI.Llm.Abstractions;

namespace Gefc.AI.Llm.Providers.Gemini.DependencyInjection;

/// <summary>
/// Extension methods for setting up Gemini services in an <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Gemini LLM provider services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="configure">A delegate to configure the <see cref="GeminiOptions"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddGemini(
        this IServiceCollection services,
        Action<GeminiOptions> configure)
    {
        services.Configure(configure);

        services.AddHttpClient<GeminiProvider>();

        services.AddSingleton<ILlmProvider>(sp =>
            sp.GetRequiredService<GeminiProvider>());

        return services;
    }
}
