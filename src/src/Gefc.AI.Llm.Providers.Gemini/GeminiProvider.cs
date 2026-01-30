// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------

using System.Net.Http.Json;
using System.Text.Json;
using Gefc.AI.Llm.Abstractions;
using Gefc.AI.Llm.Models;
using Gefc.AI.Llm.Streaming;
using Microsoft.Extensions.Options;

namespace Gefc.AI.Llm.Providers.Gemini;

/// <summary>
/// Implementation of <see cref="ILlmProvider"/> for the Google Gemini API.
/// </summary>
public sealed class GeminiProvider : ILlmProvider
{
    /// <inheritdoc />
    public string Name => "gemini";

    private readonly HttpClient _http;
    private readonly GeminiOptions _options;

    public GeminiProvider(
        HttpClient http,
        IOptions<GeminiOptions> options)
    {
        _http = http;
        _options = options.Value;
    }

    /// <inheritdoc />
    public async Task<ChatResponse> ChatAsync(
        ChatRequest request,
        CancellationToken cancellationToken)
    {
        var url = GeminiEndpoints.GenerateContent(
            _options.BaseUrl,
            request.Model!,
            _options.ApiKey);

        var response = await _http.PostAsJsonAsync(
            url,
            GeminiRequestMapper.Map(request),
            cancellationToken);

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadFromJsonAsync<JsonElement>(
            cancellationToken: cancellationToken);

        return GeminiResponseMapper.Map(json!, request);
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<ChatStreamChunk> ChatStreamAsync(
        ChatRequest request,
        [System.Runtime.CompilerServices.EnumeratorCancellation]
        CancellationToken cancellationToken)
    {
        var url = GeminiEndpoints.StreamGenerateContent(
            _options.BaseUrl,
            request.Model!,
            _options.ApiKey);

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(
                GeminiRequestMapper.Map(request))
        };

        var response = await _http.SendAsync(
            httpRequest,
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken);

        response.EnsureSuccessStatusCode();

        await foreach (var chunk in GeminiStreamingParser.ParseAsync(
            await response.Content.ReadAsStreamAsync(cancellationToken),
            cancellationToken))
        {
            yield return chunk;
        }
    }
}


