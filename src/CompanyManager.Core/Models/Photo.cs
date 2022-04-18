using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyManager.Core.Models
{
    public class Photo
    {
        [Key]
        [Column(TypeName = "varchar(100)")]
        public string FileName { get; set; } = null!;

        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; } = null!;

        public int? AppointmentId { get; set; }

        public virtual Appointment? Appointment { get; set; }
    }
}
