using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyManager.Core.Models
{
    public class OfferCategory
    {
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; } = null!;

        public virtual ICollection<Offer> Offers { get; set; } = null!;
    }
}
