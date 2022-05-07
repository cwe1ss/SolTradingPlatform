namespace FormDraftService.Models
{
    public class Question
    {
        public String QuestionId { get; set; }
        public String Text { get; set; }
        public IEnumerable<string> Options { get; set; }
    }

}
