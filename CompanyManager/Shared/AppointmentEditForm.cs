using System.ComponentModel.DataAnnotations;

namespace CompanyManager.Shared
{
    public class AppointmentEditForm
    {
        [Required(ErrorMessage = "Należy podać datę.")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "Należy podać godzinę.")]
        public TimeSpan? Time { get; set; }

        [Range(5, int.MaxValue, ErrorMessage = "Minimalny czas wizyty wynosi 5 minut.")]
        public int TotalMinutes { get; set; }

        [Required(ErrorMessage = "Należy wybrać klienta.")]
        public string CustomerNameAndPhone { get; set; } = string.Empty;

        public string? Note { get; set; }

        public bool Confirmed { get; set; }

        [MinLength(1, ErrorMessage = "Należy wybrać co najmniej jedną usługę.")]
        public List<OfferViewModel> Offers { get; set; } = new List<OfferViewModel>();
    }
}
