using System.Collections.Generic;
using MobilePay.Models;

namespace MobilePay.Calculations.Rules
{
    public class BigMerchantDiscountRule : BaseRule
    {
        private readonly Dictionary<Merchant, decimal> _merchantDiscounts
            = new Dictionary<Merchant, decimal>();

        public BigMerchantDiscountRule(IFeeCalculationRule next, params MerchantDiscount[] merchantDiscounts) :
            base(next)
        {
            foreach (var discount in merchantDiscounts)
                _merchantDiscounts.Add(discount.Merchant, discount.DiscountPercent);
        }

        public override void CalculateFee(TransactionData inputData, ref MerchantFee result)
        {
            var name = inputData.Merchant;
            if (_merchantDiscounts.TryGetValue(name, out var discountPercentage))
                result.Fee *= (100 - discountPercentage) / 100;
            base.CalculateFee(inputData, ref result);
        }
    }
}