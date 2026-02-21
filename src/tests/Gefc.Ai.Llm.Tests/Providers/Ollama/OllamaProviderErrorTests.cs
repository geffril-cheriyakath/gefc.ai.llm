// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------

using System.Net;
using System.Net.Http.Json;
using Gefc.AI.Llm.Exceptions;
using Gefc.AI.Llm.Models;
using Gefc.AI.Llm.Providers.Ollama;

namespace Gefc.AI.Llm.Tests.Providers.Ollama;

public sealed class OllamaProviderErrorTests
{
    private static OllamaProvider CreateProvider(HttpClient http)
        => new(http);

    private static HttpClient CreateHttpClient(HttpMessageHandler handler)
        => new(handler) { BaseAddress = new Uri("http://localhost:11434") };

    private static ChatRequest SampleRequest => new()
    {
        Provider = "ollama",
        Model = "llama3",
        Messages = [new(ChatRole.User, "Hello")]
    };

    // ── HTTP error scenarios ──────────────────────────

    [Fact]
    public async Task ChatAsync_NonSuccess_Throws_LlmApiException()
    {
        var handler = new FakeHandler(new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent("{\"error\":\"model not found\"}")
        });

        var provider = CreateProvider(CreateHttpClient(handler));

        var ex = await Assert.ThrowsAsync<LlmApiException>(
            () => provider.ChatAsync(SampleRequest, CancellationToken.None));

        Assert.Equal(HttpStatusCode.BadRequest, ex.StatusCode);
        Assert.Equal("ollama", ex.ProviderName);
        Assert.Contains("model not found", ex.ResponseBody);
    }

    [Fact]
    public async Task ChatAsync_ServiceUnavailable_Throws_LlmApiException()
    {
        var handler = new FakeHandler(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
        {
            Content = new StringContent("server down")
        });

        var provider = CreateProvider(CreateHttpClient(handler));

        var ex = await Assert.ThrowsAsync<LlmApiException>(
            () => provider.ChatAsync(SampleRequest, CancellationToken.None));

        Assert.Equal(HttpStatusCode.ServiceUnavailable, ex.StatusCode);
    }

    // ── Parse error scenarios ─────────────────────────

    [Fact]
    public async Task ChatAsync_MalformedJson_Throws_LlmResponseParseException()
    {
        var handler = new FakeHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("this is not json")
        });

        var provider = CreateProvider(CreateHttpClient(handler));

        var ex = await Assert.ThrowsAsync<LlmResponseParseException>(
            () => provider.ChatAsync(SampleRequest, CancellationToken.None));

        Assert.Equal("ollama", ex.ProviderName);
    }

    [Fact]
    public async Task ChatAsync_MissingMessage_Throws_LlmResponseParseException()
    {
        var handler = new FakeHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(new { error = "something went wrong" })
        });

        var provider = CreateProvider(CreateHttpClient(handler));

        var ex = await Assert.ThrowsAsync<LlmResponseParseException>(
            () => provider.ChatAsync(SampleRequest, CancellationToken.None));

        Assert.Contains("something went wrong", ex.Message);
    }

    // ── Streaming error scenarios ─────────────────────

    [Fact]
    public async Task ChatStreamAsync_NonSuccess_Throws_LlmApiException()
    {
        var handler = new FakeHandler(new HttpResponseMessage(HttpStatusCode.NotFound)
        {
            Content = new StringContent("{\"error\":\"model not found\"}")
        });

        var provider = CreateProvider(CreateHttpClient(handler));

        var ex = await Assert.ThrowsAsync<LlmApiException>(async () =>
        {
            await foreach (var _ in provider.ChatStreamAsync(SampleRequest, CancellationToken.None)) { }
        });

        Assert.Equal(HttpStatusCode.NotFound, ex.StatusCode);
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
