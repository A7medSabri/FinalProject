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
                price = payDto.price,
                FreelancerId = payDto.FreelancerId,
                MM = payDto.MM,
                YY = payDto.YY
            };

            _context.PaymentTests.Add(NewPaymentTest);

            return payDto;
        }

        public double GetMyMoney(string UserId)
        {
            var MoneyData = _context.PaymentTests
               .Where(c => c.FreelancerId == UserId)
               .GroupBy(c => c.FreelancerId)
               .Select(g => new
               {
                   SumOfMoney = g.Sum(c => c.price)
               })
               .FirstOrDefault();

            if (MoneyData == null)
            {
                return 0;
            }

            var money = MoneyData.SumOfMoney - MoneyData.SumOfMoney * 0.1;

            return money;
        }
    }
}
