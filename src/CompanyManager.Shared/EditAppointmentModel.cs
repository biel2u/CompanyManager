using System.ComponentModel.DataAnnotations;

namespace CompanyManager.Shared
{
    public class EditAppointmentModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Należy podać datę.")]
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Należy podać godzinę.")]
        public TimeSpan Time { get; set; }

        [Required(ErrorMessage = "Należy wybrać klienta.")]
        public string CustomerNameAndPhone { get; set; } = string.Empty;

        public string? Note { get; set; }

        public bool Confirmed { get; set; }

        [MinLength(1, ErrorMessage = "Należy wybrać co najmniej jedną usługę.")]
        public List<DisplayOfferModel> Offers { get; set; } = new List<DisplayOfferModel>();
    }
}
