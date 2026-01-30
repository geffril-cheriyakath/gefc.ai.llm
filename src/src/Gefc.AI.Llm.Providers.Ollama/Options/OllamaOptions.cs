// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------

namespace Gefc.AI.Llm.Providers.Ollama;

/// <summary>
/// Configuration options for the Ollama LLM provider.
/// </summary>
public sealed class OllamaOptions
{
    /// <summary>
    /// Gets or sets the base URL for the Ollama API.
    /// Default is "http://localhost:11434".
    /// </summary>
    public string BaseUrl { get; set; } = "http://localhost:11434";
}
