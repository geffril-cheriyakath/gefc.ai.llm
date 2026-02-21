using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Gefc.AI.Llm.Abstractions;
using Gefc.AI.Llm.DependencyInjection;
using Gefc.AI.Llm.Models;
using Gefc.AI.Llm.Providers.Gemini.DependencyInjection;
using Gefc.AI.Llm.Providers.Ollama.DependencyInjection;

namespace GefcLlmInteractiveDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Because Main cannot be async
            MainAsync(args).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            Console.WriteLine("GEFC.AI.LLM Interactive Chatbot");
            Console.WriteLine("================================\n");

            using var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    // Core LLM SDK
                    services.AddGefcLlm(options =>
                    {
                        options.DefaultProvider = "ollama";
                        options.DefaultModel = "gemma3:4b";
                    });

                    // Ollama
                    services.AddOllama(options =>
                    {
                        options.BaseUrl = "http://localhost:11434";
                    });

                    // Gemini
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

            // -----------------------------
            // Provider selection
            // -----------------------------
            Console.WriteLine("Select Provider:");
            Console.WriteLine("1) Ollama (Local)");
            Console.WriteLine("2) Gemini (Cloud)");
            Console.Write("> ");

            var providerChoice = Console.ReadLine();
            string provider = providerChoice == "2" ? "gemini" : "ollama";

            // -----------------------------
            // Model selection
            // -----------------------------
            Console.WriteLine("\nEnter model name:");
            Console.WriteLine(provider == "ollama"
                ? "Example: gemma3:4b, llama3, mistral"
                : "Example: gemini-2.5-flash, gemini-2.5-pro");

            Console.Write("> ");
            string model = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(model))
            {
                model = provider == "ollama"
                    ? "gemma3:4b"
                    : "gemini-2.5-flash";
            }

            // -----------------------------
            // Streaming option
            // -----------------------------
            Console.Write("\nEnable streaming? (y/n): ");
            bool streaming = Console.ReadLine()?.Trim().ToLower() == "y";

            Console.WriteLine("\n🤖 Chat started. Type 'exit' to quit.\n");

            // -----------------------------
            // Chat loop
            // -----------------------------
            while (true)
            {
                Console.Write("You: ");
                var userInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userInput))
                    continue;

                if (userInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    break;

                var request = new ChatRequest
                {
                    Provider = provider,
                    Model = model,
                    Messages =
                    [
                        new(ChatRole.User, userInput)
                    ]
                };

                Console.Write("AI: ");

                if (streaming)
                {
                    await foreach (var chunk in llm.ChatStreamAsync(request))
                    {
                        Console.Write(chunk.Delta);
                    }
                    Console.WriteLine();
                }
                else
                {
                    var response = await llm.ChatAsync(request);
                    Console.WriteLine(response.Content);
                }

                Console.WriteLine();
            }

            Console.WriteLine("👋 Chat ended.");
        }
    }
}