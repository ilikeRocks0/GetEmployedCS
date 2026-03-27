namespace Back_end.Endpoints.Models;

class GenericWordsRequest
{
    public string Paragraph { get; set; }
    
    public GenericWordsRequest(string paragraph)
  {
    this.Paragraph = paragraph;
  }
}