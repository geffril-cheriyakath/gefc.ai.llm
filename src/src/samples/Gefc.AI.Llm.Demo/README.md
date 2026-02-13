# Gefc.AI.Llm.Demo (Sample application)

Overview
- A minimal console demo that shows how to wire up the core SDK and provider implementations and perform both normal and streaming chats.
- Demonstrates:
  - Registering the core `GefcLlm` SDK.
  - Registering providers: `AddOllama` and `AddGemini`.
  - Calling `ILlmService.ChatAsync(...)` and `ILlmService.ChatStreamAsync(...)`.

Prerequisites
- .NET 10 SDK installed.
- For Gemini usage: `GEMINI_API_KEY` environment variable set.
- For Ollama usage: running Ollama instance accessible at configured `BaseUrl` (default used in demo: `http://localhost:11434`).

Run the demo
1. Restore and build:
   - `dotnet restore`
   - `dotnet build src\samples\Gefc.AI.Llm.Demo -c Release`
2. Run:
   - `dotnet run --project src\samples\Gefc.AI.Llm.Demo -c Release`
3. For Gemini flows, export environment variable before running:
   - Windows (PowerShell): `$env:GEMINI_API_KEY = "your_key_here"`
   - Linux/macOS: `export GEMINI_API_KEY="your_key_here"`

What the demo does
- Performs a normal chat against the default provider (Ollama).
- Streams responses from the provider.
- Performs explicitly-targeted calls to Gemini (if configured).

Source
- Main sample is at `src\samples\Gefc.AI.Llm.Demo\Program.cs`.

Customization
- Change `DefaultProvider` and `DefaultModel` in demo's `AddGefcLlm` configuration.
- Adjust provider options (`AddOllama`, `AddGemini`) to match your environment.

Troubleshooting
- If streaming output is empty, verify provider connectivity and model availability.
- If Gemini calls fail, verify `GEMINI_API_KEY` and network access.

Contributing
- Add sample scenarios for new features or provider behaviors.