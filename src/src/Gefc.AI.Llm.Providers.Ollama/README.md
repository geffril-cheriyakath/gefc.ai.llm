# Gefc.AI.Llm.Providers.Ollama

Overview
- Provider for local Ollama instances (local LLM host). Provides mapping from `ChatRequest` to Ollama HTTP API and supports streaming and non-streaming responses.

Configuration
- The provider exposes `OllamaOptions` with `BaseUrl` (default should be `http://localhost:11434` in samples).
- Register in DI:
  - `services.AddOllama(options => { options.BaseUrl = "http://localhost:11434"; });`

Target and dependencies
- Target Framework: `.NET 10`
- Packages:
  - `Microsoft.Extensions.Http`
- Project reference: depends on the core `Gefc.AI.Llm` project.

Usage
- The demo configures Ollama as the default provider:
  - `options.DefaultProvider = "ollama";`
- Examples:
  - Normal chat: `await llm.ChatAsync(new ChatRequest { Messages = [ new(ChatRole.User, "Explain X") ] });`
  - Streaming chat: `await foreach (var chunk in llm.ChatStreamAsync(request)) { ... }`

Testing
- Tests for Ollama request/response mapping are in `tests\Providers\Ollama\`.
- To run tests: `dotnet test tests\Gefc.AI.Llm.Tests -c Release`

Local development
- Ensure Ollama is running locally and accessible at the configured `BaseUrl`.
- For local debugging, point `OllamaOptions.BaseUrl` to your running instance.

Notes
- The provider relies on `IHttpClientFactory` for creating HttpClient instances.
- Maintain mapping logic in `Mapping\` (request/response translators).

Contributing
- Add tests when changing mapping behavior.