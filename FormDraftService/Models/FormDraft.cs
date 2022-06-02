namespace FormDraftService.Models
{
    public class FormDraft
    {
        public string FormId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IEnumerable<Question> Questions { get; set; } = new List<Question>();
    }
}
