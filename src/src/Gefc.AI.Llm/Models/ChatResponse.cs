// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------

namespace Gefc.AI.Llm.Models;

/// <summary>
/// Represents the response from an LLM chat request.
/// </summary>
public sealed record ChatResponse
{
    /// <summary>
    /// Gets the content of the response message.
    /// </summary>
    public required string Content { get; init; }

    /// <summary>
    /// Gets the name of the provider that generated the response.
    /// </summary>
    public required string Provider { get; init; }

    /// <summary>
    /// Gets the model identifier used to generate the response.
    /// </summary>
    public required string Model { get; init; }
}
