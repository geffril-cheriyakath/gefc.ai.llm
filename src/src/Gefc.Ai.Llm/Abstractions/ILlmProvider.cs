// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------

using Gefc.AI.Llm.Models;
using Gefc.AI.Llm.Streaming;

namespace Gefc.AI.Llm.Abstractions;

/// <summary>
/// Defines the contract for an LLM provider that can handle chat requests.
/// </summary>
public interface ILlmProvider
{
    /// <summary>
    /// Gets the unique name of the provider.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Sends a chat request to the provider asynchronously and returns the complete response.
    /// </summary>
    /// <param name="request">The chat request containing messages and configuration.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation, containing the <see cref="ChatResponse"/>.</returns>
    Task<ChatResponse> ChatAsync(ChatRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a chat request to the provider asynchronously and streams the response chunks.
    /// </summary>
    /// <param name="request">The chat request containing messages and configuration.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An asynchronous stream of <see cref="ChatStreamChunk"/>.</returns>
    IAsyncEnumerable<ChatStreamChunk> ChatStreamAsync(ChatRequest request, CancellationToken cancellationToken = default);
}
