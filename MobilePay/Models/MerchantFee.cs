using System;

namespace MobilePay.Models
{
    public class MerchantFee
    {
        public DateTime Date { get; set; }
        public string MerchantName { get; set; }
        public decimal Fee { get; set; }

        public override string ToString()
        {
            return string.Format($"{Date:yyyy-MM-dd} {MerchantName} {Fee:0.00}");
        }
    }
}