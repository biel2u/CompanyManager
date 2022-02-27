namespace CompanyManager.Shared
{
    public class CustomerViewModel
    {
        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Note { get; set; }
    }
}
