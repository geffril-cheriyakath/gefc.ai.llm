using System.Text.Json;
using Gefc.AI.Llm.Streaming;

// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------

namespace Gefc.AI.Llm.Providers.Ollama;

internal static class OllamaStreamingParser
{
    public static async IAsyncEnumerable<ChatStreamChunk> ParseAsync(
        Stream stream,
        [System.Runtime.CompilerServices.EnumeratorCancellation]
        CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(stream);

        while (!reader.EndOfStream && !cancellationToken.IsCancellationRequested)
        {
            var line = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var json = JsonDocument.Parse(line).RootElement;

            var done = json.GetProperty("done").GetBoolean();
            var delta = json.GetProperty("message")
                            .GetProperty("content")
                            .GetString() ?? string.Empty;

            yield return new ChatStreamChunk
            {
                Delta = delta,
                IsCompleted = done
            };

            if (done)
                yield break;
        }
    }
}
