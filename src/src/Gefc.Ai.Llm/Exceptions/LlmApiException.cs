// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------

using System.Net;

namespace Gefc.AI.Llm.Exceptions;

/// <summary>
/// Thrown when an LLM provider API returns a non-success HTTP status code.
/// </summary>
public sealed class LlmApiException : LlmException
{
    /// <summary>
    /// Gets the HTTP status code returned by the API.
    /// </summary>
    public HttpStatusCode StatusCode { get; }

    /// <summary>
    /// Gets the raw response body returned by the API, if available.
    /// </summary>
    public string? ResponseBody { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LlmApiException"/> class.
    /// </summary>
    /// <param name="providerName">The provider that raised the error.</param>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <param name="responseBody">The raw error response body.</param>
    /// <param name="innerException">The inner exception, if any.</param>
    public LlmApiException(
        string providerName,
        HttpStatusCode statusCode,
        string? responseBody,
        Exception? innerException = null)
        : base(
            providerName,
            $"The {providerName} API returned HTTP {(int)statusCode} ({statusCode}). {responseBody}",
            innerException)
    {
        StatusCode = statusCode;
        ResponseBody = responseBody;
    }
}
