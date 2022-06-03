using QuestionnaireAnswersService.Models;

namespace QuestionnaireAnswersService
{
    public class Question
    {
        public String QuestionId { get; set; }
        public String Text { get; set; }
       // public String answer { get; set; }
        public IEnumerable<string> Options { get; set; }
        public Answer Answer { get; set; }

    }
}
