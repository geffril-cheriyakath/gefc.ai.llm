using System.Text.Json;
using Gefc.AI.Llm.Models;
using Gefc.AI.Llm.Providers.Ollama.Mapping;
using Xunit;

namespace Gefc.AI.Llm.Tests.Providers.Ollama;

public class OllamaResponseMapperTests
{
    [Fact]
    public void Map_Extracts_Content_Correctly()
    {
        // Mock Ollama Response JSON
        var jsonString = """
        {
          "model": "llama3",
          "created_at": "2023-08-04T08:52:19.385407455-07:00",
          "message": {
            "role": "assistant",
            "content": "The sky is blue because..."
          },
          "done": true
        }
        """;

        var request = new ChatRequest { Model = "llama3", Messages = [] };

        var responseDto = JsonSerializer.Deserialize<JsonElement>(
            jsonString
        )!;

        var response = OllamaResponseMapper.Map(responseDto, request);

        Assert.Equal("The sky is blue because...", response.Content);
        Assert.Equal("ollama", response.Provider);
        Assert.Equal("llama3", response.Model);
    }
}
