// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------

using Gefc.AI.Llm.Models;
using Gefc.AI.Llm.Options;

namespace Gefc.AI.Llm.Internal;

internal static class DefaultResolver
{
    public static ChatRequest ApplyDefaults(
        ChatRequest request,
        GefcLlmOptions options)
    {
        return request with
        {
            Provider = request.Provider ?? options.DefaultProvider,
            Model = request.Model ?? options.DefaultModel
        };
    }
}
