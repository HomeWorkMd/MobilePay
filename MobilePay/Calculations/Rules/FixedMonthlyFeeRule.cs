using System.Collections.Generic;
using MobilePay.Models;

namespace MobilePay.Calculations.Rules
{
    public class FixedMonthlyFeeRule : BaseRule
    {
        private readonly HashSet<MerchantMonthId> _invoicedMerchants = new HashSet<MerchantMonthId>();
        private readonly decimal _monthlyFee;

        public FixedMonthlyFeeRule(decimal monthlyFee, IFeeCalculationRule next = null) : base(next)
        {
            _monthlyFee = monthlyFee;
        }

        public override void CalculateFee(TransactionData inputData, ref MerchantFee result)
        {
            if (result.Fee <= 0) return;

            var merchantMonthId = new MerchantMonthId(inputData);
            if (MonthlyFeeWasApplied()) return;

            result.Fee += _monthlyFee;
            _invoicedMerchants.Add(merchantMonthId);
            base.CalculateFee(inputData, ref result);

            bool MonthlyFeeWasApplied()
            {
                return _invoicedMerchants.Contains(merchantMonthId);
            }
        }
    }
}