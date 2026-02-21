// ------------------------------------------------------------
//  This file is part of the gefc.
//  © gefc. All rights reserved.
// ------------------------------------------------------------

namespace Gefc.AI.Llm.Models;

/// <summary>
/// Defines the role of a message sender in a chat conversation.
/// </summary>
public enum ChatRole
{
    /// <summary>
    /// The system role, used for setting instructions or context.
    /// </summary>
    System,

    /// <summary>
    /// The user role, representing input from the human user.
    /// </summary>
    User,

    /// <summary>
    /// The assistant role, representing the AI's response.
    /// </summary>
    Assistant
}
