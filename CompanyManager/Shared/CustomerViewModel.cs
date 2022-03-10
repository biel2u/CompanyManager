using System.ComponentModel.DataAnnotations;

namespace CompanyManager.Shared
{
    public class CustomerViewModel
    {
        public int? Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Surname { get; set; } = null!;

        [Required]
        [MinLength(9, ErrorMessage = "Nieprawidłowy numer telefonu")]
        public string Phone { get; set; } = null!;
        
        [EmailAddress]
        public string? Email { get; set; }

        public string? Note { get; set; }
    }
}
