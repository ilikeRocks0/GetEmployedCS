namespace Back_end.Endpoints.Models;

public class GenericWordsAnalysis
{
    public List<int> Positions { get; set; } = [];
    public string Advice { get; set; } = "";
    public List<WordRecommendation> Recommendations { get; set; } = [];
}

public class WordRecommendation
{
    public string Word { get; set; } = "";
    public string Suggestion { get; set; } = "";
}
