using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Back_end.Services.Interfaces;

namespace Back_end.Services.Implementations.AI;

public class GroqService : IGroqService
{
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(10);

    private readonly HttpClient httpClient;
    private readonly string apiUrl;
    private readonly string model;

    public bool IsAvailable { get; }

    public GroqService(IConfiguration config)
    {
        var apiKey = config["GROQ_API_KEY"];
        IsAvailable = !string.IsNullOrEmpty(apiKey);

        apiUrl = config["GROQ_API_URL"] ?? throw new InvalidOperationException("GROQ_API_URL is not configured.");
        model = config["GROQ_MODEL"] ?? throw new InvalidOperationException("GROQ_MODEL is not configured.");

        httpClient = new HttpClient { Timeout = Timeout };

        if (IsAvailable)
        {
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);
        }
    }

    public async Task<string> CompleteAsync(string systemPrompt, string userPrompt)
    {
        if (!IsAvailable)
            throw new InvalidOperationException("Groq API key is not configured.");

        var requestBody = new
        {
            model = model,
            messages = new[]
            {
                new { role = "system", content = systemPrompt },
                new { role = "user", content = userPrompt }
            },
            // parameter for controlling randomness of the response
            temperature = 0.3
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(apiUrl, content);
        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"{(int)response.StatusCode} {response.ReasonPhrase}: {errorBody}");
        }

        var responseJson = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(responseJson);

        return doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString() ?? throw new InvalidOperationException("Empty response from Groq.");
    }
}
