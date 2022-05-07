namespace FormDraftService.Models
{
    public class FormDraft
    {
        public String FormId { get; set; }
        public String Name { get; set; }
        public String Status { get; set; }
        public String Description { get; set; }
        public IEnumerable<Question> Questions { get; set; }
    }
}
