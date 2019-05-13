using System;
using System.Collections.Generic;
using System.IO;
using MobilePay.Calculations.Rules;
using MobilePay.Models;

namespace MobilePay.Calculations
{
    public class FeeCalculator
    {
        private readonly IList<IFeeCalculationRule> _rules = new List<IFeeCalculationRule>();

        private FeeCalculator(params IFeeCalculationRule[] rules)
        {
            foreach (var rule in rules)
            {
                _rules.Add(rule);
            }
        }

        public static FeeCalculator DefaultConfiguration => new FeeCalculator(new DefaultFeePercentage());


        public MerchantFee CalFee(TransactionData data)
        {
            var fee = new MerchantFee()
            {
                Date = data.Date,
                MerchantName = data.MerchantName
            };

            foreach (var feeCalculationRule in _rules)
            {
                feeCalculationRule.CalculateFee(data, ref fee);
            }

            return fee;
        }

        public FeeCalculator Using(IFeeCalculationRule rule)
        {
            _rules.Add(rule);
            return this;
        }

        public void ProcessData(IDataReader input, TextWriter output)
        {
            foreach (var dataLine in input.ReadData())
            {
                string result = null;
                if (IsNotEmpty(dataLine.Trim()))
                {
                    TransactionData.TryParse(dataLine, out var data);
                    result = CalFee(data).ToString();
                }
                output.WriteLine(result);
            }

            bool IsNotEmpty(string line) => !string.IsNullOrEmpty(line.Trim());
        }
    }
}
