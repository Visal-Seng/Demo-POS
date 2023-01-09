using DemoDataGridView.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoDataGridView
{
    public partial class frmPOS : Form
    {
        // Product 
        private string fileProduct = "product";
        private List<ClsProduct> Products = new List<ClsProduct> { };

        // Customer 
        private string fileCustomer = "customer";
        private List<ClsCustomer> Customers = new List<ClsCustomer> { };

        //Employee 
        private string fileEmployee = "employee";
        private List<ClsEmployee> Employees = new List<ClsEmployee> { };

        //Sale summaries
        private string fileSaleSummaries = "sale";
        private List<ClsSaleSummaries> SalesSummaries = new List<ClsSaleSummaries> { };

        //Sale detail
        private string fileSaleDetail = "sale-detail";
        private List<ClsSaleDetail> SaleDetails = new List<ClsSaleDetail> { };

        //Stock
        private string fileStock = "stock";
        private List<ClsStock> Stocks = new List<ClsStock> { };

        private IOManager ioManager = new IOManager();
        private POSUtil util = new POSUtil();
        public frmPOS()
        {
            InitializeComponent();
        }

        private void frmPOS_Load(object sender, EventArgs e)
        {
            Products = util.LoadProduct(fileProduct);
            Customers = util.LoadCustomer(fileCustomer);
            Employees = util.LoadEmployee(fileEmployee);
            SalesSummaries = util.LoadSaleSummaries(fileSaleSummaries);
            SaleDetails = util.LoadSaleDetail(fileSaleDetail);
            Stocks = util.LoadStock(fileStock);
            foreach (ClsProduct product in Products)
                cmbProduct.Items.Add(product.Name);
            foreach (ClsCustomer customer in Customers)
                cmbCustomer.Items.Add(customer.Name);
            foreach (ClsEmployee employee in Employees)
                cmbEmployee.Items.Add(employee.Name);
            util.InsertStockSampleData(Products, fileStock);
            ClearAll();
        }
        private int GetInvoiceId()
        {
            ClsSaleSummaries sale = SalesSummaries.OrderByDescending(s => s.InvoiceId).FirstOrDefault();
            if(sale != null)
            {
                return sale.InvoiceId + 1;
            }
            return 1;
        }

        private void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            string proName = cmbProduct.Text;
            ClsProduct prod = Products.Where(p=>p.Name == proName).FirstOrDefault();
            if(prod != null)
            {
                int defaultUnit = 1;
                txtPrice.Text = prod.sellingPrice.ToString();
                lblUnit.Text = prod.unit.ToString();
                txtQuantity.Text = defaultUnit.ToString();
                txtTotalPrice.Text = (prod.sellingPrice * defaultUnit).ToString();

                ClsStock stock = Stocks.Where(s => s.ProductId == prod.id).FirstOrDefault();
                if(stock != null)
                {
                    lblStock.Text = stock.Quantity.ToString();
                }
            }
        }

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtQuantity.Text))
            {
                return;
            }
            string proName = cmbProduct.Text;
            int qty = int.Parse(txtQuantity.Text);
            ClsProduct prod = Products.Where(p => p.Name == proName).FirstOrDefault();
            if (prod != null)
            {
                double totatPrice = qty * prod.sellingPrice;
                txtTotalPrice.Text = totatPrice.ToString();
            }
        }
        private void Clear()
        {
            cmbProduct.Text= string.Empty;
            txtPrice.Text= string.Empty;
            txtQuantity.Text= string.Empty;
            txtTotalPrice.Text= string.Empty;
            lblUnit.Text= string.Empty;
            lblStock.Text= "0";
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private List<ClsSaleItem> Sales = new List<ClsSaleItem>();
        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClsSaleItem item = new ClsSaleItem();
            item.ProductName= cmbProduct.Text;
            item.Price= double.Parse(txtQuantity.Text);
            item.Quantity = int.Parse(txtQuantity.Text);
            item.TotalPrice = double.Parse(txtTotalPrice.Text);
            item.Unit = lblUnit.Text;
            Sales.Add(item);
            AddToDataGridView();
            Clear();

        }
        private void AddToDataGridView()
        {
            dgOrderItem.Rows.Clear();
            int no = 1;
            foreach(ClsSaleItem item in Sales)
            {
                dgOrderItem.Rows.Add(no,
                                    item.ProductName,
                                    item.Quantity,
                                    item.Price,
                                    item.TotalPrice,
                                    item.Unit
                    );
                no++;
            }
        }
        private void Remove()
        {
            if (dgOrderItem.SelectedRows.Count > 0)
            {
                string product = dgOrderItem.SelectedRows[0].Cells[1].Value.ToString();
                for (int i = 0; i < Sales.Count; i++)
                {
                    ClsSaleItem sale = Sales[i];
                    if (sale.ProductName == product)
                    {
                        Sales.Remove(sale);
                    }
                }
                AddToDataGridView();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            Remove();
        }

        private void dgOrderItem_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Remove();
        }
        private void ClearAll()
        {
            cmbCustomer.Text = "";
            cmbEmployee.Text = "";
            Clear();
            dgOrderItem.Rows.Clear();
            txtInvoiceId.Text = GetInvoiceId().ToString();  
            Sales.Clear();
        }
        private void btnClearAll_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        private void dgOrderItem_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cmbEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(dgOrderItem.Rows.Count <= 0) 
            {
                MessageBox.Show("Please add som item");
            }
            //Save sale summaries
            ClsSaleSummaries sale = new ClsSaleSummaries();
            sale.InvoiceId= GetInvoiceId();
            sale.EmployeeId= GetEmployeeId();
            sale.CustomerId= GetCustomerId();
            sale.TotalPrice= GetTotalPrice();
            SalesSummaries.Add(sale);
            ioManager.Save(SalesSummaries, fileSaleSummaries);
            
            //Save sale detail
            foreach(ClsSaleItem item in Sales)
            {
                ClsSaleDetail sd = new ClsSaleDetail();
                sd.InvoiceId = sale.InvoiceId;
                sd.ProductId = GetProductId(item.ProductName);
                sd.Quantity= item.Quantity;
                sd.Price= item.Price;
                sd.TotalPrice= item.TotalPrice;
                SaleDetails.Add(sd);
            }
            ioManager.Save(SaleDetails, fileSaleDetail);

            //Save stock
            foreach (ClsSaleItem item in Sales)
            {
                int productId = GetProductId(item.ProductName);
                ClsStock stock = Stocks.Where(s=>s.ProductId == productId).FirstOrDefault();    
                if(stock != null)
                {
                    stock.Quantity -= item.Quantity;
                }
            }
            ioManager.Save(Stocks, fileStock);

            MessageBox.Show("Save successfully");
            ClearAll();

        }
        private int GetProductId(string name )
        {
            ClsProduct p = Products.Where(pro=>pro.Name == name).FirstOrDefault();
            if(p != null)
            {
                return p.id;
            }
            return -1;
        }
        private double GetTotalPrice()
        {
            double totalPriceAllProduct = 0;
            foreach(ClsSaleItem item in Sales) 
            { 
                totalPriceAllProduct+= item.TotalPrice;
            }
            return totalPriceAllProduct;
        }
        private int GetEmployeeId()
        {
            ClsEmployee emp = Employees.Where(e => e.Name == cmbEmployee.Text).FirstOrDefault();    
            if(emp != null)
            {
                return emp.Id;
            }
            return -1;
        }

        private int GetCustomerId()
        {
            ClsCustomer cus = Customers.Where(c=>c.Name == cmbCustomer.Text).FirstOrDefault();
            if(cus != null)
            {
                return cus.Id;
            }
            return -1;
        }

        private void txtTotalPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            frmSaleView frmSaleView = new frmSaleView();
            frmSaleView.frmPOS = this;
            frmSaleView.Show();
        }

        public void RefreshPosUI(int invoiceId)
        {
            var sds = SaleDetails.Where(sd => sd.InvoiceId == invoiceId);
            if(sds != null)
            {
                foreach(var sd in sds)
                {
                    ClsSaleItem item = new ClsSaleItem();
                    item.ProductName = GetProductName(sd.ProductId);
                    item.Quantity= sd.Quantity;
                    item.Price = sd.Price;
                    item.TotalPrice= sd.TotalPrice;
                    item.Unit = GetProductUnit(sd.ProductId);

                    Sales.Add(item);
                }
                AddToDataGridView();
            }
        }
        private string GetProductName (int id)
        {
            ClsProduct prod = Products.Where(p=>p.id== id).FirstOrDefault();
            if(prod != null)
            {
                return prod.Name;
            }
            return "";
        }
        private string GetProductUnit(int id)
        {
            ClsProduct prod = Products.Where(p => p.id == id).FirstOrDefault();
            if (prod != null)
            {
                return prod.unit;
            }
            return "";
        }

    }
}
