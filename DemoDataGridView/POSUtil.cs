using DemoDataGridView.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDataGridView
{
    public class POSUtil
    {
        private IOManager ioManager = new IOManager();
        public List<ClsEmployee> LoadEmployee(string fileEmployee)
        {
            List<ClsEmployee> Employees = ioManager.Load<List<ClsEmployee>>(fileEmployee);
            if (Employees.Count == 0)
            {
                Employees = new List<ClsEmployee>();
                for (int i = 1; i <= 10; i++)
                {
                    ClsEmployee emp = new ClsEmployee(i, "employee" + i, 25, "089715963", "PP", 1000);
                    Employees.Add(emp);
                }
                ioManager.Save(Employees, fileEmployee);
            }   
            return Employees;
        }
        public List<ClsCustomer> LoadCustomer(string fileCustomer)
        {
            List<ClsCustomer>  Customers = ioManager.Load<List<ClsCustomer>>(fileCustomer);
            if (Customers.Count == 0)
            {
                Customers = new List<ClsCustomer>();
                for (int i = 1; i <= 10; i++)
                {
                    ClsCustomer cus = new ClsCustomer(i, "customer-" + i, "089715963", "Phnom penh");
                    Customers.Add(cus);
                }
                ioManager.Save(Customers, fileCustomer);
            }
            return Customers;
        }
        public List<ClsProduct> LoadProduct(string fileProduct)
        {
            List<ClsProduct> Products = ioManager.Load<List<ClsProduct>>(fileProduct);
            if (Products == null)
            {
                Products = new List<ClsProduct>();
            }
            return Products;
        }
        public List<ClsSaleSummaries> LoadSaleSummaries(string fileSaleSummaries)
        {
            List<ClsSaleSummaries> saleSummaries = ioManager.Load<List<ClsSaleSummaries>>(fileSaleSummaries);
            if (saleSummaries.Count == 0)
            {
                saleSummaries = new List<ClsSaleSummaries>();
            }
            return saleSummaries;
        }
        public List<ClsSaleDetail> LoadSaleDetail(string fileSaleDetail)
        {
            List<ClsSaleDetail> saleDetail = ioManager.Load<List<ClsSaleDetail>>(fileSaleDetail);
            if (saleDetail.Count == 0)
            {
                saleDetail = new List<ClsSaleDetail>();
            }
            return saleDetail;
        }
        public List<ClsStock> LoadStock(string fileStock)
        {
            List<ClsStock> stock = ioManager.Load<List<ClsStock>>(fileStock);
            if (stock.Count == 0)
            {
                stock = new List<ClsStock>();
            }
            return stock;
        }
        public void InsertStockSampleData(List<ClsProduct> products, string fileStock)
        {
            List<ClsStock> sampleStocks = new List<ClsStock>();
            foreach (ClsProduct product in products)
            {
                ClsStock stock = new ClsStock();
                stock.ProductId = product.id;
                stock.Quantity = 100;
                sampleStocks.Add(stock);
            }
            ioManager.Save(sampleStocks, fileStock);
        }
    }
}
