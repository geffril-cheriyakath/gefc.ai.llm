// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------

using System.Net.Http.Json;
using System.Text.Json;
using Gefc.AI.Llm.Abstractions;
using Gefc.AI.Llm.Models;
using Gefc.AI.Llm.Providers.Ollama.Mapping;
using Gefc.AI.Llm.Providers.Ollama.Parsing;
using Gefc.AI.Llm.Streaming;

namespace Gefc.AI.Llm.Providers.Ollama;

/// <summary>
/// Implementation of <see cref="ILlmProvider"/> for Ollama.
/// </summary>
public sealed class OllamaProvider : ILlmProvider
{
    /// <inheritdoc />
    public string Name => "ollama";

    private readonly HttpClient _http;

    public OllamaProvider(HttpClient http)
    {
        _http = http;
    }

    /// <inheritdoc />
    public async Task<ChatResponse> ChatAsync(
        ChatRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _http.PostAsJsonAsync(
            "/api/chat",
            OllamaRequestMapper.Map(request),
            cancellationToken);

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadFromJsonAsync<JsonElement>(
            cancellationToken: cancellationToken);

        return OllamaResponseMapper.Map(json!, request);
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<ChatStreamChunk> ChatStreamAsync(
        ChatRequest request,
        [System.Runtime.CompilerServices.EnumeratorCancellation]
        CancellationToken cancellationToken)
    {
        var httpRequest = new HttpRequestMessage(
            HttpMethod.Post, "/api/chat")
        {
            Content = JsonContent.Create(
                OllamaRequestMapper.Map(request, stream: true))
        };

        var response = await _http.SendAsync(
            httpRequest,
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken);

        response.EnsureSuccessStatusCode();

        await foreach (var chunk in OllamaStreamingParser.ParseAsync(
            await response.Content.ReadAsStreamAsync(cancellationToken),
            cancellationToken))
        {
            yield return chunk;
        }
    }
}
