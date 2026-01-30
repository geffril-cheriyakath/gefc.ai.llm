// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------
using System.Text;
using System.Text.Json;
using Gefc.AI.Llm.Streaming;

namespace Gefc.AI.Llm.Providers.Gemini;

internal static class GeminiStreamingParser
{
    public static async IAsyncEnumerable<ChatStreamChunk> ParseAsync(
        Stream stream,
        [System.Runtime.CompilerServices.EnumeratorCancellation]
        CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(stream);

        var buffer = new StringBuilder();
        var lastEmittedIndex = -1;

        while (!reader.EndOfStream && !cancellationToken.IsCancellationRequested)
        {
            var line = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(line))
                continue;

            buffer.Append(line);

            JsonDocument doc;
            try
            {
                doc = JsonDocument.Parse(buffer.ToString());
            }
            catch (JsonException)
            {
                // JSON array not complete yet
                continue;
            }

            var root = doc.RootElement;
            if (root.ValueKind != JsonValueKind.Array)
                continue;

            var arrayLength = root.GetArrayLength();
            if (arrayLength == 0 || arrayLength - 1 <= lastEmittedIndex)
                continue;

            var item = root[arrayLength - 1];
            lastEmittedIndex = arrayLength - 1;

            var candidate = item.GetProperty("candidates")[0];

            var delta = string.Empty;
            if (candidate.TryGetProperty("content", out var content) &&
                content.TryGetProperty("parts", out var parts))
            {
                foreach (var part in parts.EnumerateArray())
                {
                    if (part.TryGetProperty("text", out var text))
                        delta += text.GetString();
                }
            }

            var done =
                candidate.TryGetProperty("finishReason", out var reason) &&
                reason.GetString() == "STOP";

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
