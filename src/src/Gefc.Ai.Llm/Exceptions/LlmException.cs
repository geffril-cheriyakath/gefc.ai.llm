// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------

namespace Gefc.AI.Llm.Exceptions;

/// <summary>
/// Base exception for all LLM library errors.
/// </summary>
public class LlmException : Exception
{
    /// <summary>
    /// Gets the name of the provider that raised the error (e.g. "gemini", "ollama").
    /// </summary>
    public string ProviderName { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LlmException"/> class.
    /// </summary>
    /// <param name="providerName">The provider that raised the error.</param>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception, if any.</param>
    public LlmException(
        string providerName,
        string message,
        Exception? innerException = null)
        : base(message, innerException)
    {
        ProviderName = providerName;
    }
}
