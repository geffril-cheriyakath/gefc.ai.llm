# Gefc.AI.Llm.Tests

Overview
- Unit and integration tests for the core SDK and provider mappings.
- Test framework: `xUnit`
- Mocking: `Moq`
- Coverage: `coverlet.collector` (configured in test project)

Running tests
- Restore and run all tests:
  - `dotnet restore`
  - `dotnet test tests\Gefc.AI.Llm.Tests -c Release`
- To run a single test class or filter:
  - `dotnet test --filter FullyQualifiedName~DefaultResolutionTests`

Test organization
- Provider mapping and parser tests:
  - `tests\Providers\Ollama\*`
  - `tests\Providers\Gemini\*`
- Core behavior tests:
  - `test\DefaultResolutionTests.cs`
  - `test\LlmServiceTests.cs`
  - `test\ProviderRegistryTests.cs`
- Test utilities and fakes:
  - `test\MockLlmProvider.cs` (simple mock provider used by some unit tests)

Writing tests
- Keep tests small and deterministic.
- Use `MockLlmProvider` for tests that don't require real HTTP calls.
- For provider mapping tests, include representative request/response payloads.

CI and coverage
- CI should run `dotnet test` and collect coverage with `coverlet`.
- Ensure any external integration tests that require network or keys are clearly marked and skipped in CI unless secrets are provided.

Contributing
- Add tests for any bug fixes or new behavior.
- Follow existing naming and organization conventions for tests.