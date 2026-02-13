# Gefc.AI.Llm.Providers.Gemini

Overview
- Provider implementation for Google Gemini (cloud). Implements request/response mapping and streaming parsing for Gemini APIs.
- Maps core `ChatRequest` to Gemini HTTP requests and parses Gemini responses into the core models.

Configuration
- Environment variable `GEMINI_API_KEY` is required for authenticated requests:
  - `setx GEMINI_API_KEY "<your-api-key>"` (Windows) or `export GEMINI_API_KEY="<your-api-key>"` (Linux/macOS)
- Options class: configured via `services.AddGemini(options => { options.ApiKey = "..."; })` (see sample usage in demo).

Target and dependencies
- Target Framework: `.NET 10`
- Packages:
  - `Microsoft.Extensions.Http`
- Project references: depends on the core `Gefc.AI.Llm` project.

Streaming
- Streaming responses are parsed by `GeminiStreamingParser`. The provider exposes streaming methods that enumerate parsed chunks for consumer code.

Testing
- Unit tests exercising mapping and streaming parsing are located under `tests\Providers\Gemini\`.
- Run tests: `dotnet test tests\Gefc.AI.Llm.Tests -c Release --filter Category=Gemini` (or run all tests).

Examples (DI registration)
- In `ConfigureServices`:
  - `services.AddGemini(options => { options.ApiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY") ?? throw new InvalidOperationException("GEMINI_API_KEY is not set."); });`

Notes for developers
- Keep request mapping in `Mapping\GeminiRequestMapper.cs`.
- Streaming parser lives in `Parsing\GeminiStreamingParser.cs`. Streaming protocols and boundary handling must be kept robust.
- Observe HTTP client lifetime via `IHttpClientFactory` to avoid socket exhaustion.

Troubleshooting
- `401` or `403` responses: verify `GEMINI_API_KEY`.
- Unexpected streaming tokens: add unit tests reproducing raw stream content in `tests\Providers\Gemini\GeminiStreamingParserTests.cs`.

Contributing
- Add new integration tests when protocol changes occur.