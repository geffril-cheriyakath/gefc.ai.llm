// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Gefc.AI.Llm.Providers.Gemini.Mapping;

/// <summary>
/// Concrete request model for the Gemini generateContent endpoint.
/// Used with source-generated JSON serialization for zero-reflection performance.
/// </summary>
internal sealed class GeminiRequest
{
    [JsonPropertyName("contents")]
    public required GeminiContent[] Contents { get; init; }

    [JsonPropertyName("generationConfig")]
    public required GeminiGenerationConfig GenerationConfig { get; init; }
}

internal sealed class GeminiContent
{
    [JsonPropertyName("role")]
    public required string Role { get; init; }

    [JsonPropertyName("parts")]
    public required GeminiPart[] Parts { get; init; }
}

internal sealed class GeminiPart
{
    [JsonPropertyName("text")]
    public required string Text { get; init; }
}

internal sealed class GeminiGenerationConfig
{
    [JsonPropertyName("temperature")]
    public float Temperature { get; init; }

    [JsonPropertyName("maxOutputTokens")]
    public int MaxOutputTokens { get; init; }
}
