// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Gefc.AI.Llm.Providers.Ollama.Mapping;

/// <summary>
/// Concrete request model for the Ollama /api/chat endpoint.
/// Used with source-generated JSON serialization for zero-reflection performance.
/// </summary>
internal sealed class OllamaRequest
{
    [JsonPropertyName("model")]
    public required string Model { get; init; }

    [JsonPropertyName("stream")]
    public bool Stream { get; init; }

    [JsonPropertyName("messages")]
    public required OllamaMessage[] Messages { get; init; }
}

internal sealed class OllamaMessage
{
    [JsonPropertyName("role")]
    public required string Role { get; init; }

    [JsonPropertyName("content")]
    public required string Content { get; init; }
}
