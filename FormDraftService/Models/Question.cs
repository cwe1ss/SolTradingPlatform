namespace FormDraftService.Models
{
    public class Question
    {
        public string QuestionId { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public IEnumerable<string> Options { get; set; } = new List<string>();
    }

}
