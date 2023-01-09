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
using static System.Net.Mime.MediaTypeNames;

namespace DemoDataGridView
{
    public partial class frmCategory : Form
    {
        private List<ClsCategory> Categories = new List<ClsCategory>();
        private IOManager ioManager = new IOManager();
        private string fileName = "category";
        public frmCategory()
        {
            InitializeComponent();
        }

        private void frmCategory_Load(object sender, EventArgs e)
        {
            Categories = ioManager.Load<List<ClsCategory>>(fileName);
            if(Categories == null)
            {
                Categories = new List<ClsCategory>();
            }
            clear();
        }
        public int GetId()
        {
            ClsCategory cate = Categories.OrderByDescending(c => c.Id).FirstOrDefault();
            if(cate != null)
            {
                return cate.Id + 1;
            }
            return 1;
        }
        private void clear()
        {
            txtId.Text = GetId().ToString();
            txtName.Text = "";

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            frmCategoryView categoryView = new frmCategoryView();
            categoryView.frmCategory = this;
            categoryView.Show();
        }
        public void refreshCategoryById(int categoryId)
        {
            ClsCategory temp = Categories.Where(c => c.Id == categoryId).FirstOrDefault();
            if (temp != null)
            {
                txtId.Text = temp.Id.ToString();
                txtName.Text = temp.Name;
            }
        }
        private void btnADD(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Please input category name");
                return;
            }
            int id = int.Parse(txtId.Text);
            string name = txtName.Text;
            ClsCategory category = new ClsCategory(id, name);
            ClsCategory temp = Categories.Where(c => c.Name == category.Name).FirstOrDefault();
            if (temp == null)
            {
                Categories.Add(category);
                ioManager.Save(Categories, fileName);
                MessageBox.Show("Added!");
                clear();
            }
            else
            {
                MessageBox.Show("Category name already exist");
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int categoryId = int.Parse(txtId.Text);
            ClsCategory cate = Categories.Where(c => c.Id == categoryId).FirstOrDefault();
            if (cate != null)
            {
                Categories.Remove(cate);
                ioManager.Save(Categories, fileName);
                MessageBox.Show("Removed!");
                clear();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int categoryId = int.Parse(txtId.Text);
            ClsCategory cate = Categories.Where(c => c.Id == categoryId).FirstOrDefault();
            if(cate != null)
            {
                cate.Name = txtName.Text;
                ioManager.Save(Categories, fileName);
                MessageBox.Show("Updated!");
            }
        }
    }
}
