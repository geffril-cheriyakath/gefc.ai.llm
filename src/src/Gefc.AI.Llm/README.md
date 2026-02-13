# Gefc.AI.Llm

Overview
- `Gefc.AI.Llm` is the core library that defines the LLM abstraction, models, and dependency-injection integration used across provider implementations and samples.
- Provides the `ILlmService` surface, basic models (`ChatRequest`, `ChatMessage`, `ChatRole`, `ModelName`) and the DI registration `services.AddGefcLlm(...)`.

Key concepts
- `ILlmService` — main service used by consumers to send chat requests and receive responses or streaming response chunks.
- `ChatRequest` — request object containing `Provider`, `Model`, `Messages`, and other request-level settings.
- Provider registration is extensible — providers implement and register themselves with the core SDK via DI.

Target framework and tooling
- Target Framework: `.NET 10`
- C# Language Version: `14.0`
- Build and tooling: `dotnet` (SDK supporting .NET 10)

Build
- Restore and build:
  - `dotnet restore`
  - `dotnet build -c Release`

Usage (DI)
- Register the core SDK:
  - `services.AddGefcLlm(options => { options.DefaultProvider = "ollama"; options.DefaultModel = "gemma3:4b"; });`
- Resolve:
  - `var llm = serviceProvider.GetRequiredService<ILlmService>();`

Simple code examples
- Synchronous/normal chat:
  - `var response = await llm.ChatAsync(new ChatRequest { Messages = [ new(ChatRole.User, "Hello") ] });`
- Streaming:
  - `await foreach (var chunk in llm.ChatStreamAsync(request)) { Console.Write(chunk.Delta); }`

Where to look in the codebase
- Models: `Gefc.AI.Llm\Models\`
- Registration extensions: `Gefc.AI.Llm.DependencyInjection`
- Interfaces and core abstractions: root of this project

Contributing
- Follow existing coding style and patterns used in this repo.
- Run unit tests locally before pushing: `dotnet test tests\Gefc.AI.Llm.Tests -c Release`
- Create PRs against `main`.

License and support
- See repository root for license information and contribution guidelines.