using System;

namespace MobilePay.Models
{
    public class MerchantFee
    {
        public MerchantFee(TransactionData transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            Date = transaction.Date;
            Merchant = transaction.Merchant;
        }

        public DateTime Date { get; }
        public Merchant Merchant { get; }
        internal decimal Fee { get; set; }

        public override string ToString()
        {
            return string.Format($"{Date:yyyy-MM-dd} {Merchant.OriginalName} {Fee:0.00}");
        }
    }
}