using System;

namespace MobilePay.Models
{
    public class MerchantDiscount
    {
        public MerchantDiscount(string merchantName, decimal discountPercent)
        {
            if (discountPercent < 0 || discountPercent > 100)
                throw new ArgumentOutOfRangeException(nameof(discountPercent), "Must be between 0 and 100");
            if (string.IsNullOrEmpty(merchantName))
                throw new ArgumentException("Value cannot be null or empty.", nameof(merchantName));
            
            MerchantName = merchantName;
            DiscountPercent = discountPercent;
        }
        public string MerchantName { get; }
        public decimal DiscountPercent { get; }
    }
}