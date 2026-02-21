// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------

using System.Text.Json;
using Gefc.AI.Llm.Exceptions;
using Gefc.AI.Llm.Models;

namespace Gefc.AI.Llm.Providers.Ollama.Mapping;

internal static class OllamaResponseMapper
{
    public static ChatResponse Map(JsonElement json, ChatRequest request)
    {
        if (!json.TryGetProperty("message", out var message))
        {
            // Ollama error responses include an "error" field.
            var errorText = json.TryGetProperty("error", out var errorProp)
                ? errorProp.GetString()
                : null;

            throw new LlmResponseParseException(
                "ollama",
                errorText ?? "Ollama response does not contain a 'message' field.");
        }

        var content = message.TryGetProperty("content", out var contentProp)
            ? contentProp.GetString() ?? string.Empty
            : string.Empty;

        return new ChatResponse
        {
            Content = content,
            Provider = "ollama",
            Model = request.Model!
        };
    }
}
