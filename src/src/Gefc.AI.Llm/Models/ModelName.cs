// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------

namespace Gefc.AI.Llm.Models;

/// <summary>
/// Represents a strongly-typed model name.
/// </summary>
/// <param name="Value">The string value of the model name.</param>
public readonly record struct ModelName(string Value)
{
    /// <inheritdoc />
    public override string ToString() => Value;
}
