namespace ConfigService.Models
{
    public class Deployment
    {
        public int Id { get; set; }
        public string ServiceType { get; set; } = string.Empty; // z.B. FormDraftService
        public string URL { get; set; } = string.Empty;
    }
}
