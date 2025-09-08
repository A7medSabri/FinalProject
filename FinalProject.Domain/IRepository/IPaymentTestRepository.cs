using FinalProject.Domain.Models.Payment;

namespace FinalProject.Domain.IRepository
{
    public interface IPaymentTestRepository
    {
        PaymentTestDto create(string UserId, PaymentTestDto payDto);
        double GetMyMoney(string UserId);

        List<AdminPaymentTest> AllPayment();
    }
}
