// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------

using Gefc.AI.Llm.Abstractions;

namespace Gefc.AI.Llm.Internal;

internal sealed class ProviderRegistry
{
    private readonly Dictionary<string, ILlmProvider> _providers;

    public ProviderRegistry(IEnumerable<ILlmProvider> providers)
    {
        _providers = providers.ToDictionary(
            p => p.Name,
            StringComparer.OrdinalIgnoreCase);
    }

    public ILlmProvider Get(string name)
        => _providers.TryGetValue(name, out var provider)
            ? provider
            : throw new InvalidOperationException(
                $"LLM provider '{name}' is not registered.");
}
