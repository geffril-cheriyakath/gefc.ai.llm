// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------

using Gefc.AI.Llm.Models;
using Gefc.AI.Llm.Streaming;

namespace Gefc.AI.Llm.Abstractions;

/// <summary>
/// Defines the contract for the high-level LLM service that orchestrates requests to providers.
/// </summary>
public interface ILlmService
{
    /// <summary>
    /// Sends a chat request to the configured provider asynchronously and returns the complete response.
    /// </summary>
    /// <param name="request">The chat request details.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation, containing the <see cref="ChatResponse"/>.</returns>
    Task<ChatResponse> ChatAsync(ChatRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a chat request to the configured provider asynchronously and streams the response chunks.
    /// </summary>
    /// <param name="request">The chat request details.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An asynchronous stream of <see cref="ChatStreamChunk"/>.</returns>
    IAsyncEnumerable<ChatStreamChunk> ChatStreamAsync(ChatRequest request, CancellationToken cancellationToken = default);
}
