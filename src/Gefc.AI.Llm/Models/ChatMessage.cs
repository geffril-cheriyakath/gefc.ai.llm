// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------

namespace Gefc.AI.Llm.Models;

/// <summary>
/// Represents a single message in a chat conversation.
/// </summary>
/// <param name="Role">The role of the message sender (e.g., System, User, Assistant).</param>
/// <param name="Content">The text content of the message.</param>
public record ChatMessage(ChatRole Role, string Content);
