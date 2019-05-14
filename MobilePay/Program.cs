using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using MobilePay.Calculations;
using MobilePay.Calculations.Rules;
using MobilePay.Models;

[assembly: InternalsVisibleTo("MobilePay.Tests")]
namespace MobilePay
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SetupApplication();

            var calculator = ConfigureNewCalculator();

            var customFileName = args.Length > 0 ? args[0] : null;
            calculator.ProcessData(new ConsoleTransactionFileReader(customFileName), Console.Out);

        }

        internal static FeeCalculator ConfigureNewCalculator()
        {
            var calculator = FeeCalculator.DefaultConfiguration
                                .Add(new BigMerchantDiscountRule(
                                    new MerchantDiscount("TELIA", 10),
                                    new MerchantDiscount("CIRCLE_K", 20)))
                                .Add(new FixedMonthlyFeeRule(29));
            return calculator;
        }

        private static void SetupApplication()
        {
            Trace.AutoFlush = true;
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
            AppDomain.CurrentDomain.ProcessExit += (sender, args) =>
            {
                if (Debugger.IsAttached)
                {
                    Console.WriteLine("\r\n---------------------------------------------\r\nPress any key to exit.");
                    Console.ReadKey();
                }
            };
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