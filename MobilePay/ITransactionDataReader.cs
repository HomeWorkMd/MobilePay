using System.Collections.Generic;

namespace MobilePay
{
    public interface ITransactionDataReader
    {
        IEnumerable<string> ReadData();
    }
}