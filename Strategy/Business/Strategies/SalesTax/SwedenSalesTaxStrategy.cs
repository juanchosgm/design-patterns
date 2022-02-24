using Strategy.Business.Models;

namespace Strategy.Business.Strategies.SalesTax
{
    public class SwedenSalesTaxStrategy : ISalesTaxStrategy
    {
        public decimal GetTaxFor(Order order)
        {
            var totalTax = 0m;
            foreach (var item in order.LineItems)
            {
                switch (item.Key.ItemType)
                {
                    case ItemType.Hardware:
                    case ItemType.Service:
                        totalTax += (item.Key.Price * 0.25m) * item.Value;
                        break;
                    case ItemType.Food:
                        totalTax += (item.Key.Price * 0.06m) * item.Value;
                        break;
                    case ItemType.Literature:
                        totalTax += (item.Key.Price * 0.08m) * item.Value;
                        break;
                    default:
                        break;
                }
            }
            return totalTax;
        }
    }
}
