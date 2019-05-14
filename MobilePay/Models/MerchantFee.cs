using System;

namespace MobilePay.Models
{
    public class MerchantFee
    {
        public MerchantFee(TransactionData data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            Date = data.Date;
            Merchant = data.Merchant;
        }

        public DateTime Date { get; }
        public Merchant Merchant { get; }
        public decimal Fee { get; set; }

        public override string ToString()
        {
            return string.Format($"{Date:yyyy-MM-dd} {Merchant.OriginalName} {Fee:0.00}");
        }
    }
}