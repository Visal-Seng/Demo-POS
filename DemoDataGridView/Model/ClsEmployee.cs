using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDataGridView.Model
{
    public class ClsEmployee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public double Salary { get; set; }
        public ClsEmployee () { }
        public ClsEmployee(int id, string name, int age, string phone, string address, double salary)
        {
            Id = id;
            Name = name;
            Age = age;
            Phone = phone;
            Address = address;
            Salary = salary;
        }
    }
}
