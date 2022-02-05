using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyManager.Server.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        
        public int CustomerId { get; set; }

        public Customer Customer { get; set; } = null!;

        [Column(TypeName = "nvarchar(max)")]
        public string? Note { get; set; }

        public AppointmentStatus Status { get; set; }

        public int? OfferId { get; set; }

        public Offer? Offer { get; set; }

        public ICollection<Photo>? Photos { get; set; }
    }
}
