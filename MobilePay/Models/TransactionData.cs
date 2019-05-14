using System;
using System.Diagnostics;

namespace MobilePay.Models
{
    public class TransactionData
    {
        private const int DatePartLength = 10;
        private static readonly char[] DefaultDelimiters = {' ', ':', '|', '\t'};

        public DateTime Date { get; private set; }
        public Merchant Merchant { get; private set; }
        public decimal Amount { get; private set; }


        public static bool TryParse(string input, out TransactionData data, char[] delimiters = null)
        {
            data = null;
            try
            {
                var lastIdx = input.Trim().LastIndexOfAny(delimiters ?? DefaultDelimiters);
                input = input.TrimStart();
                data = new TransactionData
                {
                    Date = DateTime.Parse(input.Substring(0, DatePartLength)).Date,
                    Merchant = Merchant.Parse(input.Substring(DatePartLength, lastIdx - DatePartLength)),
                    Amount = decimal.Parse(input.Substring(lastIdx + 1))
                };

                return true;
            }
            catch (Exception ex)
            {
                Trace.TraceWarning($"Failed to parse input data, with error : {ex}");
                return false;
            }
        }
    }
}