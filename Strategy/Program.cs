using Strategy.Business.Models;
using Strategy.Business.Strategies.Invoice;
using Strategy.Business.Strategies.SalesTax;
using Strategy.Business.Strategies.Shipping;
using System;

namespace Strategy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Input
            Console.WriteLine("Please select an origin country: ");
            var origin = Console.ReadLine().Trim();
            Console.WriteLine("Please select a destination country");
            var destination = Console.ReadLine().Trim();
            Console.WriteLine("Choose one of the following shipping providers.");
            Console.WriteLine("1. PostNord (Swedish Postal Service)");
            Console.WriteLine("2. DHL");
            Console.WriteLine("3. FedEx");
            Console.WriteLine("Select shipping provider: ");
            var provider = Convert.ToInt32(Console.ReadLine().Trim());
            Console.WriteLine("Choose one of the following invoice delivery options.");
            Console.WriteLine("1. Email");
            Console.WriteLine("2. File (download later)");
            Console.WriteLine("3. Mail");
            Console.WriteLine("Select invoice delivery options: ");
            var invoiceOption = Convert.ToInt32(Console.ReadLine().Trim());
            #endregion
            var order = new Order
            {
                ShippingDetails = new ShippingDetails
                {
                    OriginCountry = "Sweden",
                    DestinationCountry = "Sweden"
                },
                SalesTaxStrategy = GetSalesStrategyFor(origin),
                InvoiceStrategy = GetInvoiceStrategyFor(invoiceOption),
                ShippingStrategy = GetShippingStrategyFor(provider)
            };
            order.LineItems.Add(new Item("CSHARP_SMORGASBORD", "C# Smorgasbord", 100m, ItemType.Literature), 1);
            order.SelectedPayments.Add(new Payment
            {
                PaymentProvider = PaymentProvider.Invoice
            });
            Console.WriteLine(order.GetTax());
            order.FinalizeOrder();
        }

        private static IShippingStrategy GetShippingStrategyFor(int provider)
        {
            return provider switch
            {
                1 => new SwedishPostalServiceShippingStrategy(),
                2 => new DhlShippingStrategy(),
                3 => new FedexShippingStrategy(),
                _ => throw new Exception("Unsupported shipping method")
            };
        }

        private static IInvoiceStrategy GetInvoiceStrategyFor(int invoiceOption)
        {
            return invoiceOption switch
            {
                1 => new EmailInvoiceStrategy(),
                2 => new FileInvoiceStrategy(),
                3 => new EmailInvoiceStrategy(),
                _ => throw new Exception("Unsupported invoice delivery option")
            };
        }

        private static ISalesTaxStrategy GetSalesStrategyFor(string origin)
        {
            return origin.ToLowerInvariant() switch
            {
                "sweden" => new SwedenSalesTaxStrategy(),
                "usa" => new UsaStateSalesTaxStrategy(),
                _ => throw new Exception("Unsupported region")
            };
        }
    }
}
