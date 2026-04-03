namespace Back_end.Services.Implementations.AI.Prompts;

public static class GenericWordsPrompts
{
    public const string SystemPrompt =
        """
        You are an expert resume writing coach. Your job is to analyze resume text and identify
        weak or generic words, provide actionable advice, and suggest stronger alternatives.
        You must always respond with valid JSON only. No explanation, no markdown, no extra text.
        """;

    public const string AnalyzeParagraph =
        """
        Analyze the following resume text and return a JSON object with this exact structure:
        {"positions": [0-based word indices of weak/generic words], "advice": "2-3 sentences of general writing advice", "recommendations": [{"word": "original word", "suggestion": "stronger alternative"}]}

        Rules:
        - Only flag words that are genuinely weak or generic in a resume context (e.g. "worked", "helped", "did", "good", "responsible for")
        - Position indices must match the 0-based word position in the original text (split by spaces)
        - Suggestions should be specific, action-oriented resume verbs or descriptors
        - Advice should be encouraging but direct

        Resume text: "<<PARAGRAPH>>"
        """;
}
