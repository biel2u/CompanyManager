using CompanyManager.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyManager.Server.Models
{
    public class Appointment
    {
        public Appointment()
        {
            Offers = new HashSet<Offer>();
        }

        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        
        public int CustomerId { get; set; }

        public Customer Customer { get; set; } = null!;

        [Column(TypeName = "nvarchar(max)")]
        public string? Note { get; set; }

        public AppointmentStatus Status { get; set; }

        public ICollection<Offer> Offers { get; set; } = null!;

        public ICollection<Photo>? Photos { get; set; }
    }
}
