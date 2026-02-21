// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Gefc.AI.Llm.Providers.Gemini.Mapping;

/// <summary>
/// Source-generated JSON serializer context for Gemini request models.
/// Eliminates reflection-based serialization at runtime.
/// </summary>
[JsonSerializable(typeof(GeminiRequest))]
[JsonSourceGenerationOptions(
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
internal sealed partial class GeminiJsonContext : JsonSerializerContext;
