using System;

namespace MobilePay.Models
{
    public class Merchant
    {
        private Merchant(string input)
        {
            OriginalName = input.TrimStart();
            Name = input.Trim();
        }

        public string OriginalName { get; }
        public string Name { get; }

        public static Merchant Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input?.Trim()))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(input));

            return new Merchant(input);
        }

        #region Equals infrastructure

        public override bool Equals(object obj)
        {
            return obj as Merchant != null && Equals((Merchant) obj);
        }

        protected bool Equals(Merchant other)
        {
            return string.Equals(Name, other.Name, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public static bool operator ==(Merchant left, Merchant right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Merchant left, Merchant right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}