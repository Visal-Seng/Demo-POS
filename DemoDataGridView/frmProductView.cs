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
    public partial class frmProductView : Form
    {
        private List<ClsCategory> Categories = new List<ClsCategory>();
        private IOManager ioManager = new IOManager();
        private List<ClsProduct> Products = new List<ClsProduct> { };
        private string fileProduct = "product";
        private string fileName = "category";
        public frmProduct frmProduct;
        public frmProductView()
        {
            InitializeComponent();
        }

        private void frmProductView_Load(object sender, EventArgs e)
        {
            Categories = ioManager.Load<List<ClsCategory>>(fileName);
            if(Categories == null)
            {
                Categories = new List<ClsCategory>();
            }
            Products = ioManager.Load<List<ClsProduct>>(fileProduct);
            if (Products != null)
            {
                //dgProduct.DataSource = Products;
                foreach(ClsProduct p in Products)
                {
                    string categoryName = "No name";
                    ClsCategory cate = Categories.Where(c=>c.Id == p.id).FirstOrDefault();  
                    if(cate != null)
                    {
                        categoryName = cate.Name;
                    }
                    dgProduct.Rows.Add(p.id, p.Name, categoryName, p.costPrice, p.sellingPrice, p.unit);
                }
            }
            else
            {
                Products = new List<ClsProduct>();
            }
        }

        private void dgProduct_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (dgProduct.SelectedRows.Count > 0)
            {
                
                int productId = (int)dgProduct.SelectedRows[0].Cells[0].Value;
                frmProduct.refreshProductById(productId);
                this.Close();
            }
        }
    }
}
