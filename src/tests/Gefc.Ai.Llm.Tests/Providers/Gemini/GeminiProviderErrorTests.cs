// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------

using System.Net;
using System.Net.Http.Json;
using Gefc.AI.Llm.Exceptions;
using Gefc.AI.Llm.Models;
using Gefc.AI.Llm.Providers.Gemini;
using Gefc.AI.Llm.Providers.Gemini.Options;

namespace Gefc.AI.Llm.Tests.Providers.Gemini;

public sealed class GeminiProviderErrorTests
{
    private static GeminiProvider CreateProvider(HttpClient http)
    {
        var options = Microsoft.Extensions.Options.Options.Create(new GeminiOptions
        {
            BaseUrl = "https://fake.api",
            ApiKey = "test-key"
        });
        return new GeminiProvider(http, options);
    }

    private static HttpClient CreateHttpClient(HttpMessageHandler handler)
        => new(handler) { BaseAddress = new Uri("https://fake.api") };

    private static ChatRequest SampleRequest => new()
    {
        Provider = "gemini",
        Model = "gemini-1.5-pro",
        Messages = [new(ChatRole.User, "Hello")]
    };

    // ── HTTP error scenarios ──────────────────────────

    [Fact]
    public async Task ChatAsync_NonSuccess_Throws_LlmApiException()
    {
        var handler = new FakeHandler(new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent("{\"error\":{\"message\":\"bad request\"}}")
        });

        var provider = CreateProvider(CreateHttpClient(handler));

        var ex = await Assert.ThrowsAsync<LlmApiException>(
            () => provider.ChatAsync(SampleRequest, CancellationToken.None));

        Assert.Equal(HttpStatusCode.BadRequest, ex.StatusCode);
        Assert.Equal("gemini", ex.ProviderName);
        Assert.Contains("bad request", ex.ResponseBody);
    }

    [Fact]
    public async Task ChatAsync_Unauthorized_Throws_LlmApiException()
    {
        var handler = new FakeHandler(new HttpResponseMessage(HttpStatusCode.Unauthorized)
        {
            Content = new StringContent("invalid api key")
        });

        var provider = CreateProvider(CreateHttpClient(handler));

        var ex = await Assert.ThrowsAsync<LlmApiException>(
            () => provider.ChatAsync(SampleRequest, CancellationToken.None));

        Assert.Equal(HttpStatusCode.Unauthorized, ex.StatusCode);
    }

    // ── Parse error scenarios ─────────────────────────

    [Fact]
    public async Task ChatAsync_MalformedJson_Throws_LlmResponseParseException()
    {
        var handler = new FakeHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("not valid json")
        });

        var provider = CreateProvider(CreateHttpClient(handler));

        var ex = await Assert.ThrowsAsync<LlmResponseParseException>(
            () => provider.ChatAsync(SampleRequest, CancellationToken.None));

        Assert.Equal("gemini", ex.ProviderName);
    }

    [Fact]
    public async Task ChatAsync_MissingCandidates_Throws_LlmResponseParseException()
    {
        var handler = new FakeHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(new { })
        });

        var provider = CreateProvider(CreateHttpClient(handler));

        var ex = await Assert.ThrowsAsync<LlmResponseParseException>(
            () => provider.ChatAsync(SampleRequest, CancellationToken.None));

        Assert.Contains("candidates", ex.Message);
    }

    // ── Streaming error scenarios ─────────────────────

    [Fact]
    public async Task ChatStreamAsync_NonSuccess_Throws_LlmApiException()
    {
        var handler = new FakeHandler(new HttpResponseMessage(HttpStatusCode.TooManyRequests)
        {
            Content = new StringContent("rate limited")
        });

        var provider = CreateProvider(CreateHttpClient(handler));

        var ex = await Assert.ThrowsAsync<LlmApiException>(async () =>
        {
            await foreach (var _ in provider.ChatStreamAsync(SampleRequest, CancellationToken.None)) { }
        });

        Assert.Equal(HttpStatusCode.TooManyRequests, ex.StatusCode);
    }

    // ── Helper ────────────────────────────────────────

    private sealed class FakeHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage _response;

        public FakeHandler(HttpResponseMessage response) => _response = response;

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
            => Task.FromResult(_response);
    }
}
