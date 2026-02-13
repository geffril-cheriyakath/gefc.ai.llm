// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------

namespace Gefc.AI.Llm.Models;

/// <summary>
/// Represents a request to initiate or continue a chat session with an LLM.
/// </summary>
/// <param name="Model">The identifier of the model to use (optional, may be resolved by defaults).</param>
/// <param name="Messages">The list of messages in the conversation history.</param>
/// <param name="Temperature">The sampling temperature to use, between 0 and 1.</param>
/// <param name="MaxTokens">The maximum number of tokens to generate.</param>
/// <param name="Provider">The specific provider to use (optional, may be resolved by defaults).</param>
public sealed record ChatRequest
{
    public string? Provider { get; init; }
    public string? Model { get; init; }
    public required IReadOnlyList<ChatMessage> Messages { get; init; }
    public float Temperature { get; init; } = 0.7f;
    public int MaxTokens { get; init; } = 10000;
}

