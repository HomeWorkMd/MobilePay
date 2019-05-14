using System;
using System.Collections.Generic;
using System.IO;
using MobilePay.Calculations;
using MobilePay.Tests.Properties;
using Xunit;

namespace MobilePay.Tests
{
    public class IntegrationTests
    {
        public static IEnumerable<string> StringToTrimmedLines(string input)
        {
            var reader = new StringReader(input);
            while (true)
            {
                var rez = reader.ReadLine();
                if (rez == null) break;
                yield return rez.Trim();
            }
        }

        private class StringTransactionDataReader : ITransactionDataReader
        {
            private readonly string _input;

            public StringTransactionDataReader(string input)
            {
                _input = input;
            }

            public IEnumerable<string> ReadData()
            {
                return StringToTrimmedLines(_input);
            }
        }

        [Fact]
        public void CalculatorOutput_matchesExpectedOutputCases()
        {
            var calculator = Program.ConfigureNewCalculator();
            var output = new StringWriter();
            var sampleReader = new StringTransactionDataReader(Resources.SampleInput);
            calculator.ProcessData(sampleReader, output);


            var expectedLines = StringToTrimmedLines(Resources.ExpectedOutput);
            var expected = string.Join(Environment.NewLine, expectedLines);
            Assert.Equal(expected, output.ToString());
        }
    }
}