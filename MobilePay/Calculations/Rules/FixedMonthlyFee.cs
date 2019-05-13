using System;
using System.Collections.Generic;
using MobilePay.Models;

namespace MobilePay.Calculations.Rules
{
    public class FixedMonthlyFee : IFeeCalculationRule
    {
        private readonly decimal _monthlyFee;
        private readonly HashSet<MerchantMonthId> _invoicedMerchants = new HashSet<MerchantMonthId>();

        public FixedMonthlyFee(decimal monthlyFee)
        {
            _monthlyFee = monthlyFee;
        }

        public void CalculateFee(TransactionData inputData, ref MerchantFee result)
        {
            if (result.Fee <= 0) return;

            var merchantMonthId = new MerchantMonthId(inputData);

            if (MonthlyFeeWasApplied()) return;

            result.Fee += _monthlyFee;
            _invoicedMerchants.Add(merchantMonthId);

            bool MonthlyFeeWasApplied()
            {
                return _invoicedMerchants.Contains(merchantMonthId);
            }
        }

    }
}
