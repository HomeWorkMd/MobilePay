using System;
using MobilePay.Models;

namespace MobilePay.Calculations.Rules
{
    public class DefaultFeePercentageRule : BaseRule
    {
        public DefaultFeePercentageRule(IFeeCalculationRule next = null) : base(next)
        {
        }

        public override void CalculateFee(TransactionData inputData, ref MerchantFee result)
        {
            result.Fee = Math.Abs(inputData.Amount * 1 / 100);
            base.CalculateFee(inputData, ref result);
        }
    }
}