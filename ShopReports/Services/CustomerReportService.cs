using Microsoft.EntityFrameworkCore;
using ShopReports.Models;
using ShopReports.Reports;

namespace ShopReports.Services
{
    public class CustomerReportService
    {
        private readonly ShopContext shopContext;

        public CustomerReportService(ShopContext shopContext)
        {
            this.shopContext = shopContext;
        }

        public CustomerSalesRevenueReport GetCustomerSalesRevenueReport()
        {
            var customerSalesRevenueReportLines = this.shopContext.Persons
                .GroupBy(p => new { p.Id, p.LastName, p.FirstName })
                .Select(g => new CustomerSalesRevenueReportLine
                {
                    CustomerId = g.Key.Id,
                    PersonFirstName = g.Key.FirstName,
                    PersonLastName = g.Key.LastName,
                    SalesRevenue = g.Sum(p => p.Customer.Orders.Count > 0 ? p.Customer.Orders.Sum(o => o.Details.Count > 0 ? o.Details.Sum(od => od.PriceWithDiscount) : 0) : 0),
                })
                .Where(line => line.SalesRevenue > 0)
                .OrderByDescending(g => g.SalesRevenue)
                .ToList();

            return new CustomerSalesRevenueReport(customerSalesRevenueReportLines, DateTime.Now);
        }
    }
}
