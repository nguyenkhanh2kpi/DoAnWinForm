using DoAnCuoiKy.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnCuoiKy.Gui
{
    public partial class AdminForm : Form
    {
        string manage;
        public AdminForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var loginform = new LoginForm();
            loginform.Show();
            this.Visible = false;
        }


        //show cat
        private void buttonCategory_Click(object sender, EventArgs e)
        {
            this.manage = "Category";
            using(var db = new QuanLyBHEntity())
            {
                var cat = (from c in db.Categories
                           select new { ID = c.cat_id, Name = c.cat_name }).ToList();
                dataGridView1.DataSource = cat;
       
            }
        }


        //show product

        private void buttonProduct_Click(object sender, EventArgs e)
        {
            this.manage = "Product";
            using (var db = new QuanLyBHEntity())
            {
                var pro = (from p in db.Products
                           select new { ID = p.pro_id,Cat =p.Category.cat_name,Name = p.pro_name,Price = p.unit_price, Status = p.pro_status=="active"? true :false }).ToList();
                dataGridView1.DataSource = pro;

            }
        }

        //show emp

        private void buttonEmployee_Click(object sender, EventArgs e)
        {
            this.manage = "Employee";
            using (var db = new QuanLyBHEntity())
            {
                var employee = (from emp in db.People
                           where emp.role_id == "r_2"
                           select new { ID = emp.per_id, Name = emp.per_name,
                               Gender = emp.per_gender=="nam"?"Male":"Female",
                               Email = emp.per_email, 
                               Status = emp.per_status =="active"?true:false }).ToList();
                dataGridView1.DataSource = employee;

            }

        }

        // show cus
        private void buttonCustomer_Click(object sender, EventArgs e)
        {
            this.manage = "Customer";
            using (var db = new QuanLyBHEntity())
            {
                var employee = (from emp in db.People
                                where emp.role_id == "r_1"
                                select new
                                {
                                    ID = emp.per_id,
                                    Name = emp.per_name,
                                    Gender = emp.per_gender == "nam" ? "Male" : "Female",
                                    Email = emp.per_email,
                                    Status = emp.per_status == "active" ? true : false
                                }).ToList();
                dataGridView1.DataSource = employee;
            }
        }


        // show menu trip
        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex != -1 && e.ColumnIndex!= -1)
                {
                    contextMenuStrip1.Show();
                }
            }
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (this.manage)
            {
                case "Customer":
                    var form2 =new  AddPersonForm();
                    form2.ShowDialog();
                    break;
                case "Employee":
                    var form3 = new AddPersonForm();
                    form3.ShowDialog();
                    break;
                case "Category":
                    var form = new AddCategory();
                    form.ShowDialog();
                    break;
                case "Product":
                    var form1 = new AddProduct();
                    form1.ShowDialog();
                    break;
            }
        }

        private void changeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
