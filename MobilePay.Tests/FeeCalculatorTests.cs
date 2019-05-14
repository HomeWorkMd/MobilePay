using System.IO;
using MobilePay.Calculations;
using MobilePay.Calculations.Rules;
using MobilePay.Models;
using Moq;
using Xunit;

namespace MobilePay.Tests
{
    public class FeeCalculatorTests
    {
        [Theory(DisplayName = "MOBILEPAY-2")]
        [InlineData("2019.01.01 John 100", 1)]
        [InlineData("2019.01.01 John 15", 0.15)]
        [InlineData("2019.01.01 John 0", 0)]
        [InlineData("2019.01.01 John -100", 1)]
        // use cases
        [InlineData("2018-09-02 CIRCLE_K 120", 1.2)]
        [InlineData("2018-09-04 TELIA 200 ", 2.0)]
        [InlineData("2018-10-22 CIRCLE_K 300 ", 3.0)]
        [InlineData("2018-10-29 CIRCLE_K 150 ", 1.5)]
        public void DefaultFeeIs_1_Percent(string input, decimal expected)
        {
            TransactionData.TryParse(input, out var data);
            var result = new FeeCalculator(new DefaultFeePercentageRule()).CalFee(data);
            Assert.Equal(expected, result.Fee);
        }


        [Theory(DisplayName = "MOBILEPAY - 3")]
        [InlineData("2018-09-02 TELIA 120", 1.08)]
        [InlineData("2018-09-04 TELIA 200", 1.80)]
        [InlineData("2018-10-22 TELIA 300", 2.70)]
        [InlineData("2018-10-29 TELIA 150", 1.35)]
        public void BigMerchantDiscount_IsApplied(string input, decimal expected)
        {
            TransactionData.TryParse(input, out var data);
            var result = new FeeCalculator(new DefaultFeePercentageRule(
                    new BigMerchantDiscountRule(null,
                        new MerchantDiscount("TELIA", 10))))
                .CalFee(data);
            Assert.Equal(expected, result.Fee);
        }


        [Theory(DisplayName = "MOBILEPAY - 4")]
        [InlineData("2018-09-02 CIRCLE_K 120", 0.96)]
        [InlineData("2018-09-04 CIRCLE_K 200", 1.60)]
        [InlineData("2018-10-22 CIRCLE_K 300", 2.40)]
        [InlineData("2018-10-29 CIRCLE_K 150", 1.20)]
        public void AnotherBigMerchantDiscount_IsApplied(string input, decimal expected)
        {
            TransactionData.TryParse(input, out var data);
            var result = new FeeCalculator(new DefaultFeePercentageRule(
                    new BigMerchantDiscountRule(null,
                        new MerchantDiscount("CIRCLE_K", 20))))
                .CalFee(data);
            Assert.Equal(expected, result.Fee);
        }

        [Fact]
        public void EmptyInputLine_ProducesAnEmptyLineInOutput()
        {
            var inputMoq = new Mock<ITransactionDataReader>();
            inputMoq.Setup(p => p.ReadData()).Returns(() => new[] {string.Empty});
            var output = new Mock<TextWriter>();
            output.Setup(p => p.WriteLine(It.IsAny<string>()))
                .Verifiable();

            var calc = new FeeCalculator(new DefaultFeePercentageRule());
            calc.ProcessData(inputMoq.Object, output.Object);
            output.Verify(p => p.WriteLine(It.Is<string>(s => string.IsNullOrEmpty(s))));
        }

        [Fact(DisplayName = "MOBILEPAY - 5")]
        public void FixedMonthlyFee_AreAppliedOncePerMonthPerMerchant()
        {
            var monthlyFeeCalculator = new FeeCalculator(
                new DefaultFeePercentageRule(
                    new FixedMonthlyFeeRule(29m)));


            TransactionData.TryParse("2018-09-02 CIRCLE_K  120", out var data);
            //First transaction has 29 added
            Assert.Equal(30.20m, monthlyFeeCalculator.CalFee(data).Fee);


            TransactionData.TryParse("2018-09-04 NETTO     200", out data);
            //First transaction has 29 added
            Assert.Equal(31.00m, monthlyFeeCalculator.CalFee(data).Fee);

            TransactionData.TryParse("2018-10-22 CIRCLE_K  300", out data);
            //First transaction has 29 added
            Assert.Equal(32.00m, monthlyFeeCalculator.CalFee(data).Fee);

            TransactionData.TryParse("2018-10-29 CIRCLE_K  150", out data);
            //Second transaction during the same month does NOT have 29 added
            Assert.Equal(1.50m, monthlyFeeCalculator.CalFee(data).Fee);
        }
    }
}