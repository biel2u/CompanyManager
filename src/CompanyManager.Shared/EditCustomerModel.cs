using System.ComponentModel.DataAnnotations;

namespace CompanyManager.Shared
{
    public class EditCustomerModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Należy podać imię.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Należy podać nazwisko.")]
        public string Surname { get; set; } = null!;

        [MinLength(9, ErrorMessage = "Nieprawidłowy numer telefonu")]
        public string Phone { get; set; } = null!;
        
        [EmailAddress(ErrorMessage = "Nieprawidłowy adres e-mail")]
        public string? Email { get; set; }

        public string? Note { get; set; }
    }
}
