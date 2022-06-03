using QuestionnaireAnswersService;
using QuestionnaireAnswersService.Models;

namespace QuestionnaireAnswersService
{
    public class Questionaire
    {
        public String QuestionaireId { get; set; }
        public String Name { get; set; }
        public String Status { get; set; }
        public String Description { get; set; }
        public IEnumerable<Question> Questions { get; set; }
}
}
