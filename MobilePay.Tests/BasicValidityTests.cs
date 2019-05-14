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
            TransactionData.TryParse("1999-01-01 test 1", out var data);
            var result = new MerchantFee(data) { Fee = fee }.ToString();
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
        public void FailureToParseTransactionData_shouldReturnFalse()
        {
            var rez = TransactionData.TryParse("This is wrong", out TransactionData _);
            Assert.False(rez);
        }

        [Fact]
        public void TransactionParser_IgnoresLeadingSpaces()
        {
            var rez = TransactionData.TryParse("   1999-01-01 xxx 1", out TransactionData data);
            Assert.True(rez);
            Assert.Equal(new DateTime(1999, 1, 1), data.Date);
        }

        [Fact]
        public void TransactionParser_IgnoresSpacesAndTabsInBetween()
        {
            Assert.True(TransactionData.TryParse("1999-01-01   xxx    1", out TransactionData data));
            Assert.True(TransactionData.TryParse("1999-01-01    xxx     1", out data));
            Assert.True(TransactionData.TryParse("1999-01-01     xxx     1", out data));
            Assert.True(TransactionData.TryParse("1999-01-01     xxx     1.15", out data));

            Assert.Equal( "xxx", data.Merchant.Name);
            Assert.Equal(1.15m, data.Amount);
            Assert.Equal(new DateTime(1999, 1,1), data.Date);
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
        public void Merchant_MustHaveA_Name()
        {
            Assert.Throws<ArgumentException>(() => { Merchant.Parse(""); });
            Assert.Throws<ArgumentException>(() => { Merchant.Parse("    "); });
            Assert.Throws<ArgumentException>(() => { Merchant.Parse("   "); });
            Assert.Throws<ArgumentException>(() => { Merchant.Parse(null); });
        }

        [Fact]
        public void SpacingCharacters_AreIgnored_whenComparingMerchants()
        {
            var mm1 = Merchant.Parse("Test");
            var mm2 = Merchant.Parse("Test   ");
            var mm3 = Merchant.Parse("Test      ");

            Assert.Equal(mm1, mm2);
            Assert.Equal(mm1, mm3);
            Assert.Equal(mm2, mm3);
        }

        [Fact]
        public void CasingIsIgnored_whenComparingMerchants()
        {
            var mm1 = Merchant.Parse("Test");
            var mm2 = Merchant.Parse("test");

            Assert.Equal(mm1, mm2);
            Assert.True(mm1 == mm2);
        }

        [Fact]
        public void MerchantIsMandatory_whenCreatingDiscount()
        {
            Assert.Throws<ArgumentException>(() => new MerchantDiscount(null as string, 10));
            Assert.Throws<ArgumentException>(() => new MerchantDiscount("", 10));
        }

        [Fact]
        public void DiscountRule_OnlyAcceptsValidDiscountValues()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new BigMerchantDiscountRule(new MerchantDiscount("a", -10)));
            Assert.Throws<ArgumentOutOfRangeException>(() => new BigMerchantDiscountRule(new MerchantDiscount("a", 111)));
        }
    }
}
