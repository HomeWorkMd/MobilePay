using System;

namespace MobilePay.Models
{
    public class MerchantDiscount
    {
        public MerchantDiscount(string merchantName, decimal discountPercent) :
            this(Merchant.Parse(merchantName), discountPercent)
        {
        }

        public MerchantDiscount(Merchant merchant, decimal discountPercent)
        {
            if (discountPercent < 0 || discountPercent > 100)
                throw new ArgumentOutOfRangeException(nameof(discountPercent), "Must be between 0 and 100");

            DiscountPercent = discountPercent;
            Merchant = merchant ?? throw new ArgumentNullException(nameof(merchant));
        }
        public Merchant Merchant { get; }
        public decimal DiscountPercent { get; } 
    }
}