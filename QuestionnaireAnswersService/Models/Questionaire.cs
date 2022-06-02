using QuestionnaireAnswersService;

namespace FormDraftService.Models
{
    public class Questionaire
    {
        public String FormId { get; set; }
        public String Name { get; set; }
        public String Status { get; set; }
        public String Description { get; set; }
        public IEnumerable<Question> Questions { get; set; }
    }
}
