// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------

namespace Gefc.AI.Llm.Providers.Gemini;

internal static class GeminiEndpoints
{
    public static string GenerateContent(
        string baseUrl,
        string model,
        string apiKey)
        => $"{baseUrl}/models/{model}:generateContent?key={apiKey}";

    public static string StreamGenerateContent(
        string baseUrl,
        string model,
        string apiKey)
        => $"{baseUrl}/models/{model}:streamGenerateContent?key={apiKey}";
}
