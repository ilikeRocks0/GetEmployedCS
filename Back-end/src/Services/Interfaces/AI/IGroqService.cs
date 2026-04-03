namespace Back_end.Services.Interfaces;

public interface IGroqService
{
    
    /// <summary>Sends a chat completion request to the Groq API.</summary>
    /// <param name="systemPrompt">Instructions that define the model's role and behavior stored as a constant.</param>
    /// <param name="userPrompt">The input message to send to the model retrieved from the front end.</param>
    /// <returns>The model's response as a plain string.</returns>
    Task<string> CompleteAsync(string systemPrompt, string userPrompt);

    /// <summary>Indicates whether the Groq API is available for use.</summary>
    /// <returns>True if the API is available, false if otherwise.</returns>
    bool IsAvailable { get; }
}
