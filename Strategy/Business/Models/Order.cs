using Strategy.Business.Strategies.Invoice;
using Strategy.Business.Strategies.SalesTax;
using Strategy.Business.Strategies.Shipping;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Strategy.Business.Models
{
    public class Order
    {
        public Dictionary<Item, int> LineItems { get; set; } = new Dictionary<Item, int>();
        public IList<Payment> SelectedPayments { get; set; } = new List<Payment>();
        public IList<Payment> FinalizedPayments { get; set; } = new List<Payment>();
        public decimal TotalPrice => LineItems.Sum(li => li.Key.Price * li.Value);
        public decimal AmountDue => TotalPrice - FinalizedPayments.Sum(fp => fp.Amount);
        public ShippingStatus ShippingStatus { get; set; } = ShippingStatus.WaitingForPayment;
        public ShippingDetails ShippingDetails { get; set; }
        public ISalesTaxStrategy SalesTaxStrategy { get; set; }
        public IInvoiceStrategy InvoiceStrategy { get; set; }
        public IShippingStrategy ShippingStrategy { get; set; }

        public decimal GetTax(ISalesTaxStrategy salesTaxStrategy = default)
        {
            var strategy = salesTaxStrategy ?? SalesTaxStrategy;
            if (strategy == null)
            {
                return 0m;
            }
            return strategy.GetTaxFor(this);
        }

        public void FinalizeOrder()
        {
            if (SelectedPayments.Any(sp => sp.PaymentProvider == PaymentProvider.Invoice) && 
                AmountDue > 0 &&
                ShippingStatus == ShippingStatus.WaitingForPayment)
            {
                InvoiceStrategy.Generate(this);
                ShippingStatus = ShippingStatus.ReadyForShippment;
            }
            else if (AmountDue > 0)
            {
                throw new Exception("Unable to finalize order");
            }
            ShippingStrategy.Ship(this);
        }

    }

    public class ShippingDetails
    {
        public string Receiver { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string PostalCode { get; set; }
        public string DestinationCountry { get; set; }
        public string DestinationState { get; set; }
        public string OriginCountry { get; set; }
        public string OriginState { get; set; }
    }

    public enum ShippingStatus
    {
        WaitingForPayment,
        ReadyForShippment,
        Shipped
    }

    public class Payment
    {
        public decimal Amount { get; set; }
        public PaymentProvider PaymentProvider { get; set; }
    }

    public enum PaymentProvider
    {
        Paypal,
        CreditCard,
        Invoice
    }

    public class Item
    {
        public Item(string id, string name, decimal price, ItemType type)
        {
            Id = id;
            Name = name;
            Price = price;
            ItemType = type;
        }

        public string Id { get; }
        public string Name { get; }
        public decimal Price { get; }
        public ItemType ItemType { get; }

    }

    public enum ItemType
    {
        Service,
        Food,
        Hardware,
        Literature
    }
}
