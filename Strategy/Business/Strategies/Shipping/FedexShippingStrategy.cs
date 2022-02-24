using Strategy.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Strategy.Business.Strategies.Shipping
{
    public class FedexShippingStrategy : IShippingStrategy
    {
        public void Ship(Order order)
        {
            using var client = new HttpClient();
            // TODO: Implement FedEx Shipping Integration
            Console.WriteLine("Order is shipped with FedEx");
        }
    }
}
