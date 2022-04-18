using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyManager.Core.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        
        public int CustomerId { get; set; }

        public Customer Customer { get; set; } = null!;

        [Column(TypeName = "nvarchar(max)")]
        public string? Note { get; set; }

        public AppointmentStatus Status { get; set; }

        public virtual ICollection<AppointmentOffer> AppointmentOffers { get; set; } = null!;

        public virtual ICollection<Photo>? Photos { get; set; }
    }
}
