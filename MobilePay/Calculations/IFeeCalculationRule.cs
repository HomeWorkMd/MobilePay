using MobilePay.Models;

namespace MobilePay.Calculations
{
    public interface IFeeCalculationRule
    {
        void CalculateFee(TransactionData inputData, ref MerchantFee result);
    }
}