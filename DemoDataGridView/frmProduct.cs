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
    public partial class frmProduct : Form
    {
        private List<ClsCategory> Categories = new List<ClsCategory>();
        private IOManager ioManager = new IOManager();
        private List<ClsProduct> Products = new List<ClsProduct> { };
        private string fileProduct = "product";
        private string fileName = "category";

        public frmProduct()
        {
            InitializeComponent();
        }

        private void frmProduct_Load(object sender, EventArgs e)
        {
            Categories = ioManager.Load<List<ClsCategory>>(fileName);
            if (Categories == null)
            {
                Categories = new List<ClsCategory>();

            }
            Products = ioManager.Load<List<ClsProduct>>(fileProduct);
            if(Products == null)
            {
                Products = new List<ClsProduct>();
            }
            foreach(ClsCategory cat in Categories)
            {
                cmbCategory.Items.Add(cat.Name);
            }

            clear();
        }
        public int GetId()
        {
            ClsProduct product = Products.OrderByDescending(p => p.id).FirstOrDefault();
            if (product != null)
            {
                return product.id + 1;
            }
            return 1;
        }
        public void refreshProductById(int productId)
        {
            ClsProduct temp = Products.Where(c => c.id == productId).FirstOrDefault();
            if (temp != null)
            {
                txtId.Text = temp.id.ToString();
                txtName.Text = temp.Name;
                cmbCategory.Text = temp.categoryId.ToString();
                txtCostPrice.Text = temp.costPrice.ToString();
                txtSellingPrice.Text = temp.sellingPrice.ToString();
                txtUnit.Text = temp.unit;
            }
        }
        private void clear()
        {
            txtId.Text = GetId().ToString();
            txtName.Text = "";
            cmbCategory.Text = "";
            txtUnit.Text = "";
            txtCostPrice.Text = "0";
            txtSellingPrice.Text = "0";
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(cmbCategory.Text) || string.IsNullOrEmpty(txtUnit.Text) || string.IsNullOrEmpty(txtCostPrice.Text) || string.IsNullOrEmpty(txtSellingPrice.Text))
            {
                MessageBox.Show("Please complete all the field");
                return;
            }
            int id = int.Parse(txtId.Text);
            string name = txtName.Text;
            int categoryId = -1;
            string categoryName = cmbCategory.Text;
            ClsCategory cate = Categories.Where(c=>c.Name == categoryName).FirstOrDefault();
            if(cate != null)
            {
                categoryId = cate.Id;
            }
            double costPrice = double.Parse(txtCostPrice.Text);
            double sellingPrice = double.Parse(txtSellingPrice.Text);
            string unit = txtUnit.Text;
            ClsProduct product = new ClsProduct(id,name,categoryId,costPrice, sellingPrice,unit);
            ClsProduct temp = Products.Where(p => p.Name == product.Name).FirstOrDefault();
            if(temp == null)
            {
                Products.Add(product);
                ioManager.Save(Products, fileProduct);
                MessageBox.Show("Add success");
                clear();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int productId = int.Parse(txtId.Text);
            ClsProduct product = Products.Where(p => p.id == productId).FirstOrDefault();
            if (product != null)
            {
                product.Name = txtName.Text;
                product.categoryId = int.Parse(cmbCategory.Text);
                product.costPrice = double.Parse(txtCostPrice.Text);
                product.sellingPrice = double.Parse(txtSellingPrice.Text);
                product.unit = txtUnit.Text;
                ioManager.Save(Products, fileProduct);
                MessageBox.Show("Updated");
                clear();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int productId = int.Parse(txtId.Text);
            ClsProduct product = Products.Where(p => p.id == productId).FirstOrDefault();
            if (product != null)
            {
                Products.Remove(product);
                ioManager.Save(Categories, fileName);
                MessageBox.Show("Removed!");
                ioManager.Save(Products, fileProduct);
                clear();
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            frmProductView productView = new frmProductView();
            productView.frmProduct = this;
            productView.Show();
        }
    }
}
