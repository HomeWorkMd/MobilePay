using System;
using System.Collections.Generic;
using MobilePay.Models;

namespace MobilePay.Calculations.Rules
{
    public class BigMerchantDiscount : IFeeCalculationRule
    {
        private readonly Dictionary<string, decimal> _merchantDiscounts = new Dictionary<string, decimal>();

        public BigMerchantDiscount(params MerchantDiscount[] merchantDiscounts)
        {
            foreach (var discount in merchantDiscounts)
            {
                _merchantDiscounts.Add(discount.MerchantName, discount.DiscountPercent);
            }
        }

        public void CalculateFee(TransactionData inputData, ref MerchantFee result)
        {
            var name = inputData.MerchantName.Trim();
            if (_merchantDiscounts.TryGetValue(name, out var discountPercentage))
                result.Fee *= (100 - discountPercentage) / 100;
        }
    }
}