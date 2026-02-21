// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------


// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------

using Gefc.AI.Llm.Models;

namespace Gefc.AI.Llm.Providers.Ollama.Mapping;

internal static class OllamaRequestMapper
{
    public static object Map(ChatRequest request, bool stream = false)
        => new
        {
            model = request.Model,
            stream,
            messages = request.Messages.Select(m => new
            {
                role = m.Role.ToString().ToLowerInvariant(),
                content = m.Content
            })
        };
}
