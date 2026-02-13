using Gefc.AI.Llm.Abstractions;
using Gefc.AI.Llm.DependencyInjection;
using Gefc.AI.Llm.Models;
using Gefc.AI.Llm.Providers.Gemini.DependencyInjection;
using Gefc.AI.Llm.Providers.Ollama.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("GEFC.AI.LLM Demo");
Console.WriteLine("================");
Console.WriteLine();

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        // ---------------------------------------
        // Core LLM SDK
        // ---------------------------------------
        services.AddGefcLlm(options =>
        {
            options.DefaultProvider = "ollama";
            options.DefaultModel = "gemma3:4b";
        });

        // ---------------------------------------
        // Ollama (local)
        // ---------------------------------------
        services.AddOllama(options =>
        {
            options.BaseUrl = "http://localhost:11434";
        });

        // ---------------------------------------
        // Gemini (cloud)
        // ---------------------------------------
        services.AddGemini(options =>
        {
            options.ApiKey =
                Environment.GetEnvironmentVariable("GEMINI_API_KEY")
                ?? throw new InvalidOperationException(
                    "GEMINI_API_KEY environment variable is not set.");
        });
    })
    .Build();

var llm = host.Services.GetRequiredService<ILlmService>();

// ==================================================
// 1️⃣ Normal Chat (Ollama - default)
// ==================================================

Console.WriteLine("👉 Normal Chat (Ollama)");
Console.WriteLine("--------------------------------");

var ollamaChat = await llm.ChatAsync(new ChatRequest
{
    Messages =
    [
        new(ChatRole.User, "Explain clean architecture in one paragraph.")
    ]
});

Console.WriteLine(ollamaChat.Content);
Console.WriteLine();

// ==================================================
// 2️⃣ Streaming Chat (Ollama)
// ==================================================

Console.WriteLine("👉 Streaming Chat (Ollama)");
Console.WriteLine("--------------------------------");

await foreach (var chunk in llm.ChatStreamAsync(new ChatRequest
{
    Messages =
    [
        new(ChatRole.User, "Write a short poem about Bangalore.")
    ]
}))
{
    Console.Write(chunk.Delta);
}

Console.WriteLine();
Console.WriteLine();

// ==================================================
// 3️⃣ Normal Chat (Gemini)
// ==================================================

Console.WriteLine("👉 Normal Chat (Gemini)");
Console.WriteLine("--------------------------------");

var geminiChat = await llm.ChatAsync(new ChatRequest
{
    Provider = "gemini",
    Model = "gemini-2.5-flash",
    Messages =
    [
        new(ChatRole.User, "Explain LLMs in simple words.")
    ]
});

Console.WriteLine(geminiChat.Content);
Console.WriteLine();

//==================================================
//4️⃣ Streaming Chat(Gemini)
//==================================================

Console.WriteLine("👉 Streaming Chat (Gemini)");
Console.WriteLine("--------------------------------");

await foreach (var chunk in llm.ChatStreamAsync(new ChatRequest
{
    Provider = "gemini",
    Model = "gemini-2.5-flash",
    Messages =
    [
        new(ChatRole.User, "write me 100 emojis")
    ]
}))
{
    Console.Write(chunk.Delta);
}

Console.WriteLine();
Console.WriteLine();

// ==================================================
// Done
// ==================================================

Console.WriteLine("✅ Demo complete.");

Console.ReadLine();

