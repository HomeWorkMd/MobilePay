using System;
using MobilePay.Calculations.Rules;
using MobilePay.Models;
using Xunit;

namespace MobilePay.Tests
{
    public class RulesTests
    {
        [Fact]
        public void DefaultRule_AppliesOnePercentFee()
        {
            TransactionData.TryParse("1999-01-01 TestMerchant 150", out var data);
            var rule = new DefaultFeePercentageRule();
            var result = new MerchantFee(data);

            rule.CalculateFee(data, ref result);
            Assert.Equal(1.5m, result.Fee);
        }

        [Theory]
        [InlineData(30, 1.5, 1.05)]
        [InlineData(0, 1.5, 1.5)]
        [InlineData(100, 1.5, 0)]
        public void BigMerchantDiscountRule_AppliesDiscount(decimal discount, decimal input, decimal expectedFee)
        {
            TransactionData.TryParse("1999-01-01 TestMerchant 150", out var data);
            var result = new MerchantFee(data) { Fee = input };

            var discountRule = new BigMerchantDiscountRule(new MerchantDiscount("TestMerchant", discount));
            discountRule.CalculateFee(data, ref result);
            Assert.Equal(expectedFee, result.Fee);
        }

        [Fact]
        public void MerchantCanOnlyHave_OneDiscountSpecified()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var unused = new BigMerchantDiscountRule(
                    new MerchantDiscount("test", 10m),
                    new MerchantDiscount("test", 10m)
                );
            });
        }

        [Fact]
        public void FixedMonthlyFee_IsOnlyAppliedOncePerMonth()
        {
            decimal fixedFee = 15m;
            TransactionData.TryParse("1999-01-01 TestMerchant 150", out var data);
            var result = new MerchantFee(data) { Fee = 0.1m };

            var discountRule = new FixedMonthlyFeeRule(fixedFee);
            discountRule.CalculateFee(data, ref result);
            Assert.Equal(fixedFee + 0.1m, result.Fee);
            
            //------------------Second transaction for the same Merchant same month
            TransactionData.TryParse("1999-01-02 TestMerchant 150", out data);
            result = new MerchantFee(data) { Fee = 0.1m };
            discountRule.CalculateFee(data, ref result);
            Assert.Equal(0.1m, result.Fee);

            //------------------Third transaction for the same Merchant NEXT month
            TransactionData.TryParse("1999-02-02 TestMerchant 150", out data);
            result = new MerchantFee(data) { Fee = 0.1m };
            discountRule.CalculateFee(data, ref result);
            Assert.Equal(fixedFee + 0.1m, result.Fee);
        }
    }
}
