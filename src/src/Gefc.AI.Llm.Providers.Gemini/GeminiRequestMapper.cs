using Gefc.AI.Llm.Models;

// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------

namespace Gefc.AI.Llm.Providers.Gemini;

internal static class GeminiRequestMapper
{
    public static object Map(ChatRequest request)
        => new
        {
            contents = request.Messages.Select(m => new
            {
                role = MapRole(m.Role),
                parts = new[]
                {
                    new { text = m.Content }
                }
            }),
            generationConfig = new
            {
                temperature = request.Temperature,
                maxOutputTokens = request.MaxTokens
            }
        };

    private static string MapRole(ChatRole role)
        => role switch
        {
            ChatRole.User => "user",
            ChatRole.Assistant => "model",
            ChatRole.System => "user", // Gemini has no system role
            _ => "user"
        };
}
