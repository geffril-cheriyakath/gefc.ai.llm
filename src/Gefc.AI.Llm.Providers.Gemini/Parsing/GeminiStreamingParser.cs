// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------
using System.Text;
using System.Text.Json;
using Gefc.AI.Llm.Streaming;

namespace Gefc.AI.Llm.Providers.Gemini.Parsing;

internal static class GeminiStreamingParser
{
    public static async IAsyncEnumerable<ChatStreamChunk> ParseAsync(
        Stream stream,
        [System.Runtime.CompilerServices.EnumeratorCancellation]
        CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(stream);

        var sb = new StringBuilder();
        var inArray = false;
        var objStartIndex = -1;
        var braceDepth = 0;
        var inString = false;
        var escape = false;

        var buffer = new char[4096];

        while (!reader.EndOfStream && !cancellationToken.IsCancellationRequested)
        {
            var read = await reader.ReadAsync(buffer, 0, buffer.Length);
            if (read <= 0)
                break;

            for (int i = 0; i < read; i++)
            {
                var ch = buffer[i];
                sb.Append(ch);

                if (!inArray)
                {
                    if (char.IsWhiteSpace(ch))
                        continue;
                    if (ch == '[')
                    {
                        inArray = true;
                        continue;
                    }
                    // ignore until array start
                    continue;
                }

                // If not currently scanning an object, look for object start or array end.
                if (objStartIndex == -1)
                {
                    if (char.IsWhiteSpace(ch) || ch == ',')
                        continue;

                    if (ch == '{')
                    {
                        objStartIndex = sb.Length - 1;
                        braceDepth = 1;
                        inString = false;
                        escape = false;
                        continue;
                    }

                    if (ch == ']')
                    {
                        yield break;
                    }

                    continue;
                }

                // We are scanning an object: update string/escape/braces state.
                if (escape)
                {
                    escape = false;
                }
                else if (ch == '\\')
                {
                    escape = true;
                }
                else if (ch == '"')
                {
                    inString = !inString;
                }
                else if (!inString)
                {
                    if (ch == '{')
                        braceDepth++;
                    else if (ch == '}')
                        braceDepth--;
                }

                // When braces balanced -> complete object
                if (objStartIndex != -1 && braceDepth == 0)
                {
                    var objLen = sb.Length - objStartIndex;
                    var objJson = sb.ToString(objStartIndex, objLen);

                    // Remove processed prefix (object and any leading content before it)
                    sb.Remove(0, objStartIndex + objLen);
                    objStartIndex = -1;

                    // Parse single object and emit chunk
                    JsonDocument doc;
                    try
                    {
                        doc = JsonDocument.Parse(objJson);
                    }
                    catch (JsonException)
                    {
                        // Skip malformed object and continue scanning
                        continue;
                    }

                    using (doc)
                    {
                        var item = doc.RootElement;
                        if (!item.TryGetProperty("candidates", out var candidates) ||
                            candidates.GetArrayLength() == 0)
                        {
                            continue;
                        }

                        var candidate = candidates[0];

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

                    // Continue scanning remaining characters in the current read buffer.
                }
            }
        }
    }
}
