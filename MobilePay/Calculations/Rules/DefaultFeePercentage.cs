using System;
using MobilePay.Models;

namespace MobilePay.Calculations.Rules
{
    public class DefaultFeePercentage : IFeeCalculationRule
    {
        public void CalculateFee(TransactionData inputData, ref MerchantFee result)
        {
            result.Fee = Math.Abs(inputData.Amount * 1 / 100);
        }
    }
}