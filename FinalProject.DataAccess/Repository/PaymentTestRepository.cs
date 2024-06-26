using FinalProject.DataAccess.Data;
using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models.Payment;

namespace FinalProject.DataAccess.Repository
{
    public class PaymentTestRepository : IPaymentTestRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentTestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public PaymentTestDto create(string UserId, PaymentTestDto payDto)
        {
            var NewPaymentTest = new paymentTest // Corrected class name to match your convention
            {
                Owner = payDto.Owner,
                ClientId = UserId,
                CardNumber = payDto.CardNumber,
                CVV = payDto.CVV,
                FreelancerId = payDto.FreelancerId,
                MM = payDto.MM,
                YY = payDto.YY
            };

            _context.PaymentTests.Add(NewPaymentTest);

            return payDto;
        }
    }
}
