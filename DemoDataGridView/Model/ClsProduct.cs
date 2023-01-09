using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDataGridView.Model
{
    public class ClsProduct
    {
        public int id { get; set; }
        public string Name { get; set; }
        public int categoryId { get; set; }
        public string unit { get; set; }
        public double costPrice { get; set; }
        public double sellingPrice { get; set; }
        public ClsProduct() { }
        public ClsProduct(int id, string name, int categoryId,  double costPrice, double sellingPrice,string unit)
        {
            this.id = id;
            this.Name = name;
            this.categoryId = categoryId;
            this.unit = unit;
            this.costPrice = costPrice;
            this.sellingPrice = sellingPrice;
        }
    }
}
