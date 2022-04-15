using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyManager.Core.Models
{
    public class AppointmentOffer
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Models.Appointment))]
        public int AppointmentId { get; set; }

        [ForeignKey(nameof(Models.Offer))]
        public int OfferId { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal CustomOfferPrice { get; set; }

        public int CustomOfferTime { get; set; }

        public virtual Appointment Appointment { get; set; } = null!;

        public virtual Offer Offer { get; set; } = null!;
    }
}
