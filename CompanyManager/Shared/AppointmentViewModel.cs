using System.ComponentModel.DataAnnotations;

namespace CompanyManager.Shared
{
    public class AppointmentViewModel
    {
        [Required]
        public DateTime? Date { get; set; }

        [Required]
        public TimeSpan? Time { get; set; }

        [Required]
        public string CustomerNameAndPhone { get; set; } = string.Empty;

        public string? Note { get; set; }

        public bool Confirmed { get; set; }

        [Required]
        public IEnumerable<OfferViewModel> Offers { get; set; } = Enumerable.Empty<OfferViewModel>();
    }
}
