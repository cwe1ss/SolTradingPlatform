namespace QuestionnaireAnswersService.Models
{
    public class Questionnaire
    {
        public string? QuestionnaireId { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }
        public IEnumerable<Question> Questions { get; set; }
}
}
