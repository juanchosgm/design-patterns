using Strategy.Business.Models;
using System;
using System.Net.Http;
using System.Text.Json;

namespace Strategy.Business.Strategies.Invoice
{
    public class PrintOnDemandInvoiceStrategy : IInvoiceStrategy
    {
        public void Generate(Order order)
        {
            using var client = new HttpClient();
            var content = JsonSerializer.Serialize(order);
            client.BaseAddress = new Uri("https://pluralsight.com");
            client.PostAsync("/print-on-demand", new StringContent(content));
        }
    }
}
