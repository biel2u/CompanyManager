using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyManager.Server.Models
{
    public class Offer
    {
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; } = null!;

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        public int TimeInMinutes { get; set; }

        public ICollection<Appointment>? Appointments { get; set; }

        public int OfferCategoryId { get; set; }

        public OfferCategory OfferCategory { get; set; } = null!;
    }
}
