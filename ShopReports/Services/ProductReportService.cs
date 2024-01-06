using Microsoft.EntityFrameworkCore;
using ShopReports.Models;
using ShopReports.Reports;

namespace ShopReports.Services
{
    public class ProductReportService
    {
        private readonly ShopContext shopContext;

        public ProductReportService(ShopContext shopContext)
        {
            this.shopContext = shopContext;
        }

        public ProductCategoryReport GetProductCategoryReport()
        {
            var productCategories = this.shopContext.Categories
                .OrderBy(c => c.Name)
                .Select(productCategory => new ProductCategoryReportLine
                {
                    CategoryId = productCategory.Id,
                    CategoryName = productCategory.Name,
                }).ToList();

            return new ProductCategoryReport(productCategories, DateTime.Now);
        }

        public ProductReport GetProductReport()
        {
            var products = this.shopContext.Products
                    .OrderByDescending(p => p.Title.Title)
                    .Select(p => new ProductReportLine()
                    {
                        Manufacturer = p.Manufacturer.Name,
                        Price = p.UnitPrice,
                        ProductId = p.Id,
                        ProductTitle = p.Title.Title,
                    }).ToList();

            return new ProductReport(products, DateTime.Now);
        }

        public FullProductReport GetFullProductReport()
        {
            var products = this.shopContext.Products
                    .OrderBy(p => p.Title.Title)
                    .Select(p => new FullProductReportLine()
                    {
                        Manufacturer = p.Manufacturer.Name,
                        Price = p.UnitPrice,
                        ProductId = p.Id,
                        Category = p.Title.Category.Name,
                        Name = p.Title.Title,
                        CategoryId = p.Title.CategoryId,
                    }).ToList();

            return new FullProductReport(products, DateTime.Now);
        }

        public ProductTitleSalesRevenueReport GetProductTitleSalesRevenueReport()
        {
            var customerOrderDetails = this.shopContext.OrderDetails
                .GroupBy(od => od.Product.Title.Title)
                .OrderByDescending(od => od.Sum(ordereDetails => ordereDetails.PriceWithDiscount))
                .Select(group => new ProductTitleSalesRevenueReportLine
                {
                    ProductTitleName = group.Key,
                    SalesRevenue = group.Sum(g => g.PriceWithDiscount),
                    SalesAmount = group.Sum(g => g.ProductAmount),
                }).ToList();

            return new ProductTitleSalesRevenueReport(customerOrderDetails, DateTime.Now);
        }
    }
}
