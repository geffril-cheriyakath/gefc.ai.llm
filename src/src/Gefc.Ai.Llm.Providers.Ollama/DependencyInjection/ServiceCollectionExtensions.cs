// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------

using Gefc.AI.Llm.Abstractions;
using Gefc.AI.Llm.Providers.Ollama.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Gefc.AI.Llm.Providers.Ollama.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOllama(
        this IServiceCollection services,
        Action<OllamaOptions> configure)
    {
        services.Configure(configure);

        services.AddHttpClient<OllamaProvider>((sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<OllamaOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl);
        });

        services.AddSingleton<ILlmProvider>(sp =>
            sp.GetRequiredService<OllamaProvider>());

        return services;
    }
}
