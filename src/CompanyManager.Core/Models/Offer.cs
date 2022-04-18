using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyManager.Core.Models
{
    public class Offer
    {
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; } = null!;

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        public int TimeInMinutes { get; set; }

        public virtual ICollection<AppointmentOffer> AppointmentOffers { get; set; } = new HashSet<AppointmentOffer>();

        public int OfferCategoryId { get; set; }

        public virtual OfferCategory OfferCategory { get; set; } = null!;
    }
}
