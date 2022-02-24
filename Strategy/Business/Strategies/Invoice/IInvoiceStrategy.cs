using Strategy.Business.Models;

namespace Strategy.Business.Strategies.Invoice
{
    public interface IInvoiceStrategy
    {
        void Generate(Order order);
    }
}
