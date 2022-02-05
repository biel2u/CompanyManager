using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyManager.Server.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string Name { get; set; } = null!;

        [Column(TypeName = "nvarchar(30)")]
        public string Surname { get; set; } = null!;

        [Column(TypeName = "varchar(11)")]
        public string Phone { get; set; } = null!;

        [Column(TypeName = "nvarchar(255)")]
        public string Email { get; set; } = null!;

        [Column(TypeName = "nvarchar(max)")]
        public string? Note { get; set; }

        public Consent Consent { get; set; } = null!;

        public ICollection<Photo> Photos { get; set; } = null!;
    }
}
