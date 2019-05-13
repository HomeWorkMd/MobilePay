using System;

namespace MobilePay.Models
{
    public class MerchantFee
    {
        public MerchantFee(TransactionData data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            Date = data.Date;
            MerchantName = data.MerchantName;
        }

        public DateTime Date { get; }
        public string MerchantName { get; }
        public decimal Fee { get; set; }

        public override string ToString()
        {
            return string.Format($"{Date:yyyy-MM-dd} {MerchantName} {Fee:0.00}");
        }
    }
}