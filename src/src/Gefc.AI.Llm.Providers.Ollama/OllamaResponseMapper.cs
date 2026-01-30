// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------

using System.Text.Json;
using Gefc.AI.Llm.Models;

namespace Gefc.AI.Llm.Providers.Ollama;

internal static class OllamaResponseMapper
{
    public static ChatResponse Map(JsonElement json, ChatRequest request)
    {
        var content = json
            .GetProperty("message")
            .GetProperty("content")
            .GetString() ?? string.Empty;

        return new ChatResponse
        {
            Content = content,
            Provider = "ollama",
            Model = request.Model!
        };
    }
}
