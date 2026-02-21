using System.Text.Json;
using Gefc.AI.Llm.Models;
using Gefc.AI.Llm.Providers.Gemini.Mapping;
using Xunit;

namespace Gefc.AI.Llm.Tests.Providers.Gemini;

public class GeminiRequestMapperTests
{
    [Fact]
    public void Map_Maps_Structure_Correctly()
    {
        var request = new ChatRequest
        {
            Messages =
            [
                new(ChatRole.User, "Hello"),
                new(ChatRole.Assistant, "Hi")
            ],
            Temperature = 0.5f,
            MaxTokens = 256
        };

        var mapped = GeminiRequestMapper.Map(request);

        var json = JsonSerializer.Serialize(mapped);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        // Verify generationConfig
        var config = root.GetProperty("generationConfig");
        Assert.Equal(0.5, config.GetProperty("temperature").GetDouble());
        Assert.Equal(256, config.GetProperty("maxOutputTokens").GetInt32());

        // Verify contents
        var contents = root.GetProperty("contents");
        Assert.Equal(2, contents.GetArrayLength());

        var msg1 = contents[0];
        Assert.Equal("user", msg1.GetProperty("role").GetString());
        Assert.Equal("Hello", msg1.GetProperty("parts")[0].GetProperty("text").GetString());

        var msg2 = contents[1];
        Assert.Equal("model", msg2.GetProperty("role").GetString());
        Assert.Equal("Hi", msg2.GetProperty("parts")[0].GetProperty("text").GetString());
    }

    [Fact]
    public void Map_Maps_System_Role_To_User()
    {
        var request = new ChatRequest
        {
            Messages =
            [
                new(ChatRole.System, "Be helpful")
            ]
        };

        var mapped = GeminiRequestMapper.Map(request);

        var json = JsonSerializer.Serialize(mapped);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        var msg = root.GetProperty("contents")[0];
        Assert.Equal("user", msg.GetProperty("role").GetString());
        Assert.Equal("Be helpful", msg.GetProperty("parts")[0].GetProperty("text").GetString());
    }
}
