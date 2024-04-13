using System.ComponentModel.DataAnnotations;

namespace FinalProject.Domain.AccountModel
{
    public class ChangeNameModel
    {
        [MaxLength(50), MinLength(2)]
        public string? FirstName { get; set; }

        [MaxLength(50), MinLength(2)]
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public int? Age { get; set; }
        public int? ZIP { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? Experience { get; set; }
        public string? Education { get; set; }
        public string? PortfolioURl { get; set; }
        public string? Description { get; set; }
        public string? YourTitle { get; set; }
        public decimal? HourlyRate { get; set; }
    }
}
