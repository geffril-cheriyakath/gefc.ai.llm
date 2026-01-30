using Gefc.AI.Llm.Abstractions;
using Gefc.AI.Llm.Models;
using Gefc.AI.Llm.Streaming;

namespace Gefc.AI.Llm.Tests.TestDoubles;

internal sealed class MockLlmProvider : ILlmProvider
{
    public string Name { get; }

    public MockLlmProvider(string name)
    {
        Name = name;
    }

    public Task<ChatResponse> ChatAsync(
        ChatRequest request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(new ChatResponse
        {
            Content = $"Response from {Name}",
            Provider = Name,
            Model = request.Model!
        });
    }

    public async IAsyncEnumerable<ChatStreamChunk> ChatStreamAsync(
        ChatRequest request,
        [System.Runtime.CompilerServices.EnumeratorCancellation]
        CancellationToken cancellationToken)
    {
        yield return new ChatStreamChunk
        {
            Delta = $"Stream from {Name}",
            IsCompleted = true
        };

        await Task.CompletedTask;
    }
}
