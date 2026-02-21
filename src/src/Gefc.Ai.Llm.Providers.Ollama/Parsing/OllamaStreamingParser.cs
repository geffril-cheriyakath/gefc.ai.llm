using System.Text.Json;
using Gefc.AI.Llm.Streaming;

// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------

namespace Gefc.AI.Llm.Providers.Ollama.Parsing;

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

            JsonDocument doc;
            try
            {
                doc = JsonDocument.Parse(line);
            }
            catch (JsonException)
            {
                // Skip malformed NDJSON lines and continue scanning.
                continue;
            }

            using (doc)
            {
                var json = doc.RootElement;

                var done = json.TryGetProperty("done", out var doneProp)
                    && doneProp.GetBoolean();

                var delta = json.TryGetProperty("message", out var message)
                    && message.TryGetProperty("content", out var content)
                        ? content.GetString() ?? string.Empty
                        : string.Empty;

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
}
