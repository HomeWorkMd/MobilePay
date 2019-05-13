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
            var rule = new DefaultFeePercentage();
            var result = new MerchantFee();

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
            var result = new MerchantFee() { Fee = input };

            var discountRule = new BigMerchantDiscount(new MerchantDiscount("TestMerchant", discount));
            discountRule.CalculateFee(data, ref result);
            Assert.Equal(expectedFee, result.Fee);


        }

        [Fact]
        public void DiscountRule_OnlyAcceptsValidDiscountValues()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new BigMerchantDiscount(new MerchantDiscount("", -10)));
            Assert.Throws<ArgumentOutOfRangeException>(() => new BigMerchantDiscount(new MerchantDiscount("", 111)));
        }

        [Fact]
        public void MerchantCanOnlyHave_OneDiscountSpecified()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new BigMerchantDiscount(
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
            var result = new MerchantFee() { Fee = 0.1m };

            var discountRule = new FixedMonthlyFee(fixedFee);
            discountRule.CalculateFee(data, ref result);
            Assert.Equal(fixedFee + 0.1m, result.Fee);
            
            //------------------Second transaction for the same Merchant same month
            TransactionData.TryParse("1999-01-02 TestMerchant 150", out data);
            result = new MerchantFee() { Fee = 0.1m };
            discountRule.CalculateFee(data, ref result);
            Assert.Equal(0.1m, result.Fee);

            //------------------Third transaction for the same Merchant NEXT month
            TransactionData.TryParse("1999-02-02 TestMerchant 150", out data);
            result = new MerchantFee() { Fee = 0.1m };
            discountRule.CalculateFee(data, ref result);
            Assert.Equal(fixedFee + 0.1m, result.Fee);

        }

    }
}
