using FinalProject.DataAccess.Data;
using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models.Payment;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.DataAccess.Repository
{
    public class PaymentTestRepository : IPaymentTestRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentTestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<AdminPaymentTest> AllPayment()
        {
            var data =  _context.PaymentTests
                .Include(a=>a.Freelancer)
            .Select(p => new AdminPaymentTest
            {
                price = p.price,
                PayTime = p.PayTime,
                FreelancerId = $"{p.Freelancer.FirstName} {p.Freelancer.LastName}",
                ClientId = $"{p.Client.FirstName} {p.Client.LastName}"
            }).ToList();

            return data;
            
        }


        public PaymentTestDto create(string UserId, PaymentTestDto payDto)
        {
            var NewPaymentTest = new paymentTest
            {
                Owner = payDto.Owner,
                ClientId = UserId,
                CardNumber = payDto.CardNumber,
                CVV = payDto.CVV,
                price = payDto.price,
                FreelancerId = payDto.FreelancerId,
                MM = payDto.MM,
                YY = payDto.YY,
                PayTime = DateTime.Now,
                jobId = payDto.jobId
            };

            var data = _context.JobPosts.FirstOrDefault(a => a.Id == payDto.jobId);
            if (data != null)
            {
                data.IsDeleted = true;
            }

            _context.PaymentTests.Add(NewPaymentTest);
            _context.SaveChanges();

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
