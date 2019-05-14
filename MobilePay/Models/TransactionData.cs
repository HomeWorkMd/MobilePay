using System;
using System.Diagnostics;

namespace MobilePay.Models
{
    public class TransactionData
    {
        static readonly char[] DefaultDelimiters = { ' ', ',', '.', ':', '\t' };

        public DateTime Date { get; private set; }
        public Merchant Merchant { get; private set; }
        public decimal Amount { get; private set; }


        public static bool TryParse(string input, out TransactionData data, char[] delimiters = null)
        {
            data = null;
            try
            {
                var lastIdx = input.Trim().LastIndexOfAny(delimiters ?? DefaultDelimiters);
                data = new TransactionData
                {
                    Date = DateTime.Parse(input.Substring(0, 11)).Date,
                    Merchant = Merchant.Parse(input.Substring(11, lastIdx-11)),
                    Amount = decimal.Parse(input.Substring(lastIdx))
                };

                return true;
            }
            catch (Exception ex)
            {
                Trace.TraceWarning($"Failed to parse input data, with error : {ex }");
                return false;
            }
        }
    }
}