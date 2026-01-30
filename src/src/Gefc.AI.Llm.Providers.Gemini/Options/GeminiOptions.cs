// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------

namespace Gefc.AI.Llm.Providers.Gemini;

/// <summary>
/// Configuration options for the Gemini LLM provider.
/// </summary>
public sealed class GeminiOptions
{
    /// <summary>
    /// Gets or sets the API key for authenticating with the Gemini API.
    /// </summary>
    public required string ApiKey { get; set; }

    /// <summary>
    /// Gets or sets the base URL for the Gemini API.
    /// Default is "https://generativelanguage.googleapis.com/v1beta".
    /// </summary>
    public string BaseUrl { get; init; }
        = "https://generativelanguage.googleapis.com/v1beta";
}
