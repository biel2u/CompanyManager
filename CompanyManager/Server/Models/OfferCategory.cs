using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyManager.Server.Models
{
    public class OfferCategory
    {
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; } = null!;

        public ICollection<Offer> Offers { get; set; } = null!;
    }
}
