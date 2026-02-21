using System;
using System.Text.Json;
using Gefc.AI.Llm.Models;
using Gefc.AI.Llm.Providers.Gemini.Mapping;
using Gefc.AI.Llm.Providers.Gemini.Models;
using Xunit;

namespace Gefc.AI.Llm.Tests.Providers.Gemini;

public class GeminiResponseMapperTests
{
    [Fact]
    public void Map_When_Full_Response_Is_Valid_Returns_Content()
    {
        // Mock Gemini Response JSON
        var jsonString = """
        {
          "candidates": [
            {
              "content": {
                "parts": [
                  {
                    "text": "Hello world"
                  }
                ],
                "role": "model"
              },
              "finishReason": "STOP",
              "index": 0,
              "safetyRatings": []
            }
          ]
        }
        """;

        var request = new ChatRequest { Model = "gemini-pro", Messages = Array.Empty<ChatMessage>() };

        var responseJson = JsonSerializer.Deserialize<JsonElement>(
            jsonString
        )!;

        var response = GeminiResponseMapper.Map(responseJson, request);

        Assert.Equal("Hello world", response.Content);
        Assert.Equal("gemini", response.Provider);
        Assert.Equal("gemini-pro", response.Model);
    }
}
