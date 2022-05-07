namespace ConfigService.Models
{
    public class Deployment
    {
        public int Id { get; set; }
        public string ServiceType  { get; set; } //z.B. Draft-Service
        public string URL { get; set; }
    }
}
