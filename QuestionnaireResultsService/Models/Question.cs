namespace QuestionnaireResultsService.Models
{
    public class Question
    {
        public string? QuestionId { get; set; }
        public string? Text { get; set; }
        public IEnumerable<string> Options { get; set; } = new List<string>();
    }
}
