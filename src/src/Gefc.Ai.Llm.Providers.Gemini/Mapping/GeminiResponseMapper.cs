// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------

using System.Text.Json;
using Gefc.AI.Llm.Exceptions;
using Gefc.AI.Llm.Models;

namespace Gefc.AI.Llm.Providers.Gemini.Mapping;

internal static class GeminiResponseMapper
{
    public static ChatResponse Map(
        JsonElement json,
        ChatRequest request)
    {
        if (!json.TryGetProperty("candidates", out var candidates) ||
            candidates.GetArrayLength() == 0)
        {
            throw new LlmResponseParseException(
                "gemini",
                "Gemini response does not contain any candidates.");
        }

        var candidate = candidates[0];

        if (!candidate.TryGetProperty("content", out var content) ||
            !content.TryGetProperty("parts", out var parts) ||
            parts.GetArrayLength() == 0)
        {
            throw new LlmResponseParseException(
                "gemini",
                "Gemini response candidate does not contain content parts.");
        }

        var text = parts[0].TryGetProperty("text", out var textProp)
            ? textProp.GetString() ?? string.Empty
            : string.Empty;

        return new ChatResponse
        {
            Content = text,
            Provider = "gemini",
            Model = request.Model!
        };
    }
}
