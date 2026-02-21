// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------

namespace Gefc.AI.Llm.Exceptions;

/// <summary>
/// Thrown when an LLM provider response is received successfully but cannot
/// be deserialized or mapped to the expected model.
/// </summary>
public sealed class LlmResponseParseException : LlmException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LlmResponseParseException"/> class.
    /// </summary>
    /// <param name="providerName">The provider that returned the unparseable response.</param>
    /// <param name="message">A description of what went wrong during parsing.</param>
    /// <param name="innerException">The original parsing exception, if any.</param>
    public LlmResponseParseException(
        string providerName,
        string message,
        Exception? innerException = null)
        : base(providerName, message, innerException)
    {
    }
}
