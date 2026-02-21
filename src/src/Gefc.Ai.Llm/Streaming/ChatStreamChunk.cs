// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------

namespace Gefc.AI.Llm.Streaming;

/// <summary>
/// Represents a chunk of a streaming chat response.
/// </summary>
public sealed record ChatStreamChunk
{
    /// <summary>
    /// Gets the text content delta for this chunk.
    /// </summary>
    public required string Delta { get; init; }

    /// <summary>
    /// Gets a value indicating whether this is the final chunk of the stream.
    /// </summary>
    public bool IsCompleted { get; init; }
}