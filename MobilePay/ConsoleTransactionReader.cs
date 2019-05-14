using System;
using System.Collections.Generic;
using System.IO;

namespace MobilePay
{
    public class ConsoleTransactionFileReader : ITransactionDataReader
    {
        private readonly string _fileName;
        private const string DefaultFileName = "Transactions.txt";

        public ConsoleTransactionFileReader(string customFileName = null)
        {
            if (string.IsNullOrEmpty(customFileName))
            {
                Console.WriteLine($"Enter filename or press Enter to use default: ({DefaultFileName})");
                var input = Console.ReadLine();
                _fileName = string.IsNullOrEmpty(input) ? DefaultFileName : input.Trim();
            }
            else
                _fileName = customFileName;
        }
        public IEnumerable<string> ReadData()
        {
            try
            {
                return File.ReadLines(_fileName);
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to read the file specified, with error: \r\n {e.Message} ", e);
            }
        }
    }
}