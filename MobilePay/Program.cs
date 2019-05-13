using System;
using System.Diagnostics;
using MobilePay.Calculations;
using MobilePay.Calculations.Rules;
using MobilePay.Models;

namespace MobilePay
{
    internal class Program
    {
        static void Main()
        {
            SetupApplication();

            var calculator = FeeCalculator.DefaultConfiguration
                                .Using(new BigMerchantDiscountRule(
                                    new MerchantDiscount("TELIA", 10),
                                    new MerchantDiscount("CIRCLE_K", 20)))
                                .Using(new FixedMonthlyFeeRule(29));

            calculator.ProcessData(new ConsoleTransactionFileReader(), Console.Out);

            Console.WriteLine("\r\n---------------------------------------------\r\nPress any key to exit.");
            Console.ReadKey();
        }

        private static void SetupApplication()
        {
            Trace.AutoFlush = true;
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
        }

        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            Trace.TraceError($"Failed to process data: \r\n{e.ExceptionObject}");
            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();
            Environment.Exit(1);
        }
    }
}