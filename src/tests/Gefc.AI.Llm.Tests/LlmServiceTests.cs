using Gefc.AI.Llm.Abstractions;
using Gefc.AI.Llm.Internal;
using Gefc.AI.Llm.Models;
using Gefc.AI.Llm.Options;
using Gefc.AI.Llm.Tests.TestDoubles;

namespace Gefc.AI.Llm.Tests;

public sealed class LlmServiceTests
{
    private static ILlmService CreateService(
        params ILlmProvider[] providers)
    {
        var registry = new ProviderRegistry(providers);

        var options = Microsoft.Extensions.Options.Options.Create(new GefcLlmOptions
        {
            DefaultProvider = "ollama",
            DefaultModel = "llama3"
        });

        return new LlmService(registry, options);
    }

    [Fact]
    public async Task ChatAsync_Uses_Default_Provider()
    {
        var ollama = new MockLlmProvider("ollama");
        var service = CreateService(ollama);

        var response = await service.ChatAsync(new ChatRequest
        {
            Messages =
            [
                new(ChatRole.User, "Hello")
            ]
        });

        Assert.Equal("ollama", response.Provider);
        Assert.Equal("llama3", response.Model);
    }

    [Fact]
    public async Task ChatAsync_Uses_Explicit_Provider()
    {
        var ollama = new MockLlmProvider("ollama");
        var gemini = new MockLlmProvider("gemini");

        var service = CreateService(ollama, gemini);

        var response = await service.ChatAsync(new ChatRequest
        {
            Provider = "gemini",
            Model = "gemini-1.5-pro",
            Messages =
            [
                new(ChatRole.User, "Hello")
            ]
        });

        Assert.Equal("gemini", response.Provider);
        Assert.Equal("gemini-1.5-pro", response.Model);
    }

    [Fact]
    public async Task ChatStreamAsync_Streams_From_Correct_Provider()
    {
        var ollama = new MockLlmProvider("ollama");
        var service = CreateService(ollama);

        await foreach (var chunk in service.ChatStreamAsync(new ChatRequest
        {
            Messages =
            [
                new(ChatRole.User, "Hello")
            ]
        }))
        {
            Assert.Equal("Stream from ollama", chunk.Delta);
            Assert.True(chunk.IsCompleted);
        }
    }
}
