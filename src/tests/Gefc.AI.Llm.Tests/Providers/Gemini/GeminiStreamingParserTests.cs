using System.Text;
using Gefc.AI.Llm.Providers.Gemini.Parsing;
using Gefc.AI.Llm.Streaming;
using Xunit;

namespace Gefc.AI.Llm.Tests.Providers.Gemini;

public class GeminiStreamingParserTests
{
    [Fact]
    public async Task ParseAsync_Parses_JsonArray_Stream_Correctly()
    {
        // Simulate Gemini REST API response (JSON array)
        var jsonResponse = """
            [
              {
                "candidates": [
                  {
                    "content": {
                      "parts": [
                        { "text": "Hello" }
                      ]
                    },
                    "index": 0
                  }
                ]
              },
              {
                "candidates": [
                  {
                    "content": {
                      "parts": [
                        { "text": " World" }
                      ]
                    },
                    "finishReason": "STOP",
                    "index": 0
                  }
                ]
              }
            ]
            """;

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonResponse));
        
        var chunks = new List<ChatStreamChunk>();
        await foreach (var chunk in GeminiStreamingParser.ParseAsync(stream, CancellationToken.None))
        {
            chunks.Add(chunk);
        }

        Assert.Equal(2, chunks.Count);
        Assert.Equal("Hello", chunks[0].Delta);
        Assert.Equal(" World", chunks[1].Delta);
    }
}
