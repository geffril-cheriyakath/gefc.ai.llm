// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------

namespace Gefc.AI.Llm.Options;

/// <summary>
/// Configuration options for the Gefc LLM services.
/// </summary>
public sealed class GefcLlmOptions
{
    /// <summary>
    /// Gets or sets the default provider to use if none is specified in the request.
    /// </summary>
    public string DefaultProvider { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the default model to use if none is specified in the request.
    /// </summary>
    public string DefaultModel { get; set; } = string.Empty;
}
