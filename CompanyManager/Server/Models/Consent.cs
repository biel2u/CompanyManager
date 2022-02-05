namespace CompanyManager.Server.Models
{
    public class Consent
    {
        public int Id { get; set; }
        
        public bool Contact { get; set; }

        public bool PublicImage { get; set; }

        public int CustomerId { get; set; }

        public Customer Customer { get; set; } = null!;
    }
}
