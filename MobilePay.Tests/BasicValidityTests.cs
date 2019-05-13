using System;
using MobilePay.Calculations.Rules;
using MobilePay.Models;
using Xunit;

namespace MobilePay.Tests
{
    public class BasicValidityTests
    {
        [Theory]
        [InlineData(25, "25.00")]
        [InlineData(1, "1.00")]
        [InlineData(0.05001, "0.05")]
        public void FeeIsFormatted_AccordingToSpec(decimal fee, string expected)
        {
            var result = new MerchantFee(new TransactionData()) { Fee = fee }.ToString();
            Assert.EndsWith(expected, result);
        }

        [Theory]
        [InlineData("1901.01.01 mmm 12", "1901-01-01")]
        public void DateIsFormatted_AccordingToSpec(string input, string expected)
        {
            TransactionData.TryParse(input, out var data);
            var result = new MerchantFee(data).ToString();
            Assert.StartsWith(expected, result);
        }

        [Fact]
        public void TwoMerchantMonthIdInstances_AreEqual()
        {
            TransactionData.TryParse("2018-09-02 CIRCLE_K  120", out var data);
            TransactionData.TryParse("2018-09-15 Circle_K  120", out var data2);
            var mm1 = new MerchantMonthId(data);
            var mm2 = new MerchantMonthId(data2);

            Assert.Equal(mm1, mm2);
            Assert.True(mm1 == mm2);
        }

        [Fact]
        public void TwoDifferentMerchantMonthIdInstances_AreNotEqual()
        {
            TransactionData.TryParse("2018-09-02 CIRCLE_K  120", out var data);
            TransactionData.TryParse("2018-09-15 TELIA  120", out var data2);
            var mm1 = new MerchantMonthId(data);
            var mm2 = new MerchantMonthId(data2);

            Assert.NotEqual(mm1, mm2);
            Assert.True(mm1 != mm2);
        }

        [Fact]
        public void MerchantIsMandatory_whenCreatingDiscount()
        {
            Assert.Throws<ArgumentNullException>(() => new MerchantDiscount(null, 10));
            Assert.Throws<ArgumentNullException>(() => new MerchantDiscount("", 10));
        }

        [Fact]
        public void DiscountRule_OnlyAcceptsValidDiscountValues()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new BigMerchantDiscountRule(new MerchantDiscount("a", -10)));
            Assert.Throws<ArgumentOutOfRangeException>(() => new BigMerchantDiscountRule(new MerchantDiscount("a", 111)));
        }
    }
}
