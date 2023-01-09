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
    public partial class frmSaleView : Form
    {   
        //Sale summaries
        private string fileSaleSummaries = "sale";
        private List<ClsSaleSummaries> SalesSummaries = new List<ClsSaleSummaries> { };

        // Customer 
        private string fileCustomer = "customer";
        private List<ClsCustomer> Customers = new List<ClsCustomer> { };

        //Employee 
        private string fileEmployee = "employee";
        private List<ClsEmployee> Employees = new List<ClsEmployee> { };

        private POSUtil util = new POSUtil();
        public frmPOS frmPOS;
        public frmSaleView()
        {
            InitializeComponent();
        }

        private void frmSaleView_Load(object sender, EventArgs e)
        {
            SalesSummaries = util.LoadSaleSummaries(fileSaleSummaries);
            Customers = util.LoadCustomer(fileCustomer);
            Employees = util.LoadEmployee(fileEmployee);
            foreach (ClsSaleSummaries s in SalesSummaries)
            {
                int invoiceId = s.InvoiceId;
                string customerName = GetCustomerName(s.CustomerId);
                string employeeName = GetEmployeeName(s.EmployeeId);
                double totalPrice = s.TotalPrice;
                dgvSellHistory.Rows.Add(invoiceId,customerName, employeeName, totalPrice);
            }
        }

        private string GetCustomerName(int id)
        {
            ClsCustomer cus = Customers.Where(c=>c.Id== id).FirstOrDefault();
            if(cus != null)
                return cus.Name;
            return "";
        }

        private string GetEmployeeName(int id)
        {
            ClsEmployee emp = Employees.Where(e=>e.Id== id).FirstOrDefault();
            if(emp != null) 
                return emp.Name;
            return "";
        }

        private void dgvSellHistory_DoubleClick(object sender, EventArgs e)
        {
            if(dgvSellHistory.SelectedRows.Count > 0)
            {
                int invoiceId = (int)dgvSellHistory.SelectedRows[0].Cells[0].Value;
                frmPOS.RefreshPosUI(invoiceId);
                this.Close();
            }
        }
    }
}
