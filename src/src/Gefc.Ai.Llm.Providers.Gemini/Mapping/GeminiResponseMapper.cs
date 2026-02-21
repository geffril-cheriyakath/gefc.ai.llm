// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------

using System.Text.Json;
using Gefc.AI.Llm.Models;

namespace Gefc.AI.Llm.Providers.Gemini.Mapping;

internal static class GeminiResponseMapper
{
    public static ChatResponse Map(
        JsonElement json,
        ChatRequest request)
    {
        var text = json
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString() ?? string.Empty;

        return new ChatResponse
        {
            Content = text,
            Provider = "gemini",
            Model = request.Model!
        };
    }
}
