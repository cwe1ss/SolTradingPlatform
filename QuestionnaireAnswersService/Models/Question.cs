namespace QuestionnaireAnswersService.Models
{
    public class Question
    {
        public string? QuestionId { get; set; }
        public string? Text { get; set; }
        // public String answer { get; set; }
        public IEnumerable<string> Options { get; set; } = new List<string>();
        public Answer? Answer { get; set; }
    }
}
