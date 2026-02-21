using Gefc.AI.Llm.Internal;
using Gefc.AI.Llm.Models;
using Gefc.AI.Llm.Options;
using Xunit;

namespace Gefc.AI.Llm.Tests;

public sealed class DefaultResolutionTests
{
    [Fact]
    public void Defaults_Are_Applied_When_Missing()
    {
        var options = new GefcLlmOptions
        {
            DefaultProvider = "ollama",
            DefaultModel = "llama3"
        };

        var request = new ChatRequest
        {
            Messages =
            [
                new(ChatRole.User, "Hello")
            ]
        };

        var resolved = DefaultResolver.ApplyDefaults(request, options);

        Assert.Equal("ollama", resolved.Provider);
        Assert.Equal("llama3", resolved.Model);
    }

    [Fact]
    public void Explicit_Values_Are_Not_Overridden()
    {
        var options = new GefcLlmOptions
        {
            DefaultProvider = "ollama",
            DefaultModel = "llama3"
        };

        var request = new ChatRequest
        {
            Provider = "gemini",
            Model = "gemini-1.5-pro",
            Messages =
            [
                new(ChatRole.User, "Hello")
            ]
        };

        var resolved = DefaultResolver.ApplyDefaults(request, options);

        Assert.Equal("gemini", resolved.Provider);
        Assert.Equal("gemini-1.5-pro", resolved.Model);
    }
}
