using MobilePay.Models;

namespace MobilePay.Calculations.Rules
{
    public abstract class BaseRule : IFeeCalculationRule
    {
        protected readonly IFeeCalculationRule Next;

        protected BaseRule(IFeeCalculationRule next)
        {
            Next = next;
        }

        public virtual void CalculateFee(TransactionData inputData, ref MerchantFee result)
        {
            Next?.CalculateFee(inputData, ref result);
        }
    }
}