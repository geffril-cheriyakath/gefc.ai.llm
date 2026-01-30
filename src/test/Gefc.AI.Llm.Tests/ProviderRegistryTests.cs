using Gefc.AI.Llm.Internal;
using Gefc.AI.Llm.Tests.TestDoubles;
using Xunit;

namespace Gefc.AI.Llm.Tests;

public sealed class ProviderRegistryTests
{
    [Fact]
    public void Get_Returns_Registered_Provider()
    {
        var provider = new MockLlmProvider("ollama");
        var registry = new ProviderRegistry(new[] { provider });

        var resolved = registry.Get("ollama");

        Assert.Same(provider, resolved);
    }

    [Fact]
    public void Get_Is_Case_Insensitive()
    {
        var provider = new MockLlmProvider("Gemini");
        var registry = new ProviderRegistry(new[] { provider });

        var resolved = registry.Get("gemini");

        Assert.Same(provider, resolved);
    }

    [Fact]
    public void Get_Throws_For_Missing_Provider()
    {
        var registry = new ProviderRegistry(Array.Empty<MockLlmProvider>());

        Assert.Throws<InvalidOperationException>(() =>
            registry.Get("missing"));
    }
}
