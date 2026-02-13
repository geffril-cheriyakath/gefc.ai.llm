using System.Text.Json;
using Gefc.AI.Llm.Models;
using Gefc.AI.Llm.Providers.Ollama.Mapping;
using Xunit;

namespace Gefc.AI.Llm.Tests.Providers.Ollama;

public class OllamaRequestMapperTests
{
    [Fact]
    public void Map_Maps_Structure_Correctly()
    {
        var request = new ChatRequest
        {
            Model = "llama3",
            Messages =
            [
                new(ChatRole.User, "Why is sky blue?"),
                new(ChatRole.Assistant, "Rayleigh scattering")
            ]
        };

        var mapped = OllamaRequestMapper.Map(request, stream: true);

        var json = JsonSerializer.Serialize(mapped);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        Assert.Equal("llama3", root.GetProperty("model").GetString());
        Assert.True(root.GetProperty("stream").GetBoolean());

        var messages = root.GetProperty("messages");
        Assert.Equal(2, messages.GetArrayLength());

        var msg1 = messages[0];
        Assert.Equal("user", msg1.GetProperty("role").GetString());
        Assert.Equal("Why is sky blue?", msg1.GetProperty("content").GetString());

        var msg2 = messages[1];
        Assert.Equal("assistant", msg2.GetProperty("role").GetString());
        Assert.Equal("Rayleigh scattering", msg2.GetProperty("content").GetString());
    }

    [Fact]
    public void Map_Lowercase_Roles()
    {
        var request = new ChatRequest
        {
            Messages =
            [
                new(ChatRole.System, "Admin")
            ]
        };

        var mapped = OllamaRequestMapper.Map(request);

        var json = JsonSerializer.Serialize(mapped);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        var msg = root.GetProperty("messages")[0];
        Assert.Equal("system", msg.GetProperty("role").GetString());
    }
}
