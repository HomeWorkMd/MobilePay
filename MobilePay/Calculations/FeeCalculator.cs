using System;
using System.IO;
using MobilePay.Models;

namespace MobilePay.Calculations
{
    public class FeeCalculator
    {
        private readonly IFeeCalculationRule _ruleChain;

        public FeeCalculator(IFeeCalculationRule ruleChain)
        {
            _ruleChain = ruleChain ?? throw new ArgumentNullException(nameof(ruleChain));
        }

        public MerchantFee CalFee(TransactionData data)
        {
            var fee = new MerchantFee(data);
            _ruleChain.CalculateFee(data, ref fee);

            return fee;
        }

        public void ProcessData(ITransactionDataReader input, TextWriter output)
        {
            foreach (var transactionText in input.ReadData())
            {
                string result = null;
                if (IsNotEmpty(transactionText))
                    if (TransactionData.TryParse(transactionText, out var data))
                        result = CalFee(data).ToString();
                output.WriteLine(result);
            }

            bool IsNotEmpty(string line)
            {
                return !string.IsNullOrEmpty(line?.Trim());
            }
        }
    }
}