// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------

using Gefc.AI.Llm.Abstractions;
using Gefc.AI.Llm.Models;
using Gefc.AI.Llm.Options;
using Gefc.AI.Llm.Streaming;
using Microsoft.Extensions.Options;

namespace Gefc.AI.Llm.Internal;

internal sealed class LlmService : ILlmService
{
    private readonly ProviderRegistry _registry;
    private readonly GefcLlmOptions _options;

    public LlmService(
        ProviderRegistry registry,
        IOptions<GefcLlmOptions> options)
    {
        _registry = registry;
        _options = options.Value;
    }

    public Task<ChatResponse> ChatAsync(
        ChatRequest request,
        CancellationToken cancellationToken)
    {
        var resolved = DefaultResolver.ApplyDefaults(request, _options);
        var provider = _registry.Get(resolved.Provider!);
        return provider.ChatAsync(resolved, cancellationToken);
    }

    public IAsyncEnumerable<ChatStreamChunk> ChatStreamAsync(
        ChatRequest request,
        CancellationToken cancellationToken)
    {
        var resolved = DefaultResolver.ApplyDefaults(request, _options);
        var provider = _registry.Get(resolved.Provider!);
        return provider.ChatStreamAsync(resolved, cancellationToken);
    }
}
