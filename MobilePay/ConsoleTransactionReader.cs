using System;
using System.Collections.Generic;
using System.IO;

namespace MobilePay
{
    public class ConsoleTransactionFileReader : ITransactionDataReader
    {
        private readonly string _defaultFileName;

        public ConsoleTransactionFileReader(string defaultFileName = "transactions.txt")
        {
            _defaultFileName = defaultFileName;
        }
        public IEnumerable<string> ReadData()
        {
            Console.WriteLine($"Enter filename or press Enter to use default: ({_defaultFileName})");
            var input = Console.ReadLine();

            var filename = string.IsNullOrEmpty(input) ? _defaultFileName : input.Trim();

            try
            {
                return File.ReadLines(filename);
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to read the file specified, with error: \r\n {e.Message} ", e);
            }
        }
    }
}