using Strategy.Business.Models;

namespace Strategy.Business.Strategies.SalesTax
{
    internal class UsaStateSalesTaxStrategy : ISalesTaxStrategy
    {
        public decimal GetTaxFor(Order order)
        {
            return order.ShippingDetails.DestinationState.ToLowerInvariant() switch
            {
                "la" => order.TotalPrice * 0.095m,
                "ny" => order.TotalPrice * 0.04m,
                "nyc" => order.TotalPrice * 0.045m,
                _ => 0m,
            };
        }
    }
}
