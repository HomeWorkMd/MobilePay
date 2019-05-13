using System;
using MobilePay.Models;

namespace MobilePay.Calculations.Rules
{
    public class MerchantMonthId
    {
        public string MerchantName { get; }
        public string YearMonthNo { get; }

        public MerchantMonthId(TransactionData data)
        {
            MerchantName = data.MerchantName.Trim();
            YearMonthNo = data.Date.ToString("yyyyMM");
        }

        #region Equals Infrastructure

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MerchantMonthId)obj);
        }

        protected bool Equals(MerchantMonthId other)
        {
            return string.Equals(MerchantName, other.MerchantName, StringComparison.InvariantCultureIgnoreCase) && string.Equals(YearMonthNo, other.YearMonthNo, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((MerchantName != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(MerchantName) : 0) * 397) ^ (YearMonthNo != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(YearMonthNo) : 0);
            }
        }

        public static bool operator ==(MerchantMonthId left, MerchantMonthId right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MerchantMonthId left, MerchantMonthId right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}