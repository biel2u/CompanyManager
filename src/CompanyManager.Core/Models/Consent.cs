namespace CompanyManager.Core.Models
{
    public class Consent
    {
        public int Id { get; set; }
        
        public bool Contact { get; set; }

        public bool PublicImage { get; set; }

        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; } = null!;
    }
}
