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
    public partial class EmployeeForm : Form
    {
        Person employee;
        public EmployeeForm(Person p)
        {
            InitializeComponent();
            this.employee = p;
        }

        private void LogoutButton_Click(object sender, EventArgs e)
        {
            var loginform = new LoginForm();
            loginform.Show();
            this.Visible = false;
        }

        private void buttonOrder_Click(object sender, EventArgs e)
        {
            labelOder.Text = "Order";
            labelOrderDetail.Text = "Order Detail";
            dataGridView1.ContextMenuStrip = contextMenuStrip1;
            using (var db = new QuanLyBHEntity())
            {
                var od = (from o in db.Orders
                          select new { id = o.order_id, customer = o.ship_name, order_date = o.order_date, ship_address = o.ship_address, phone = o.ship_phone, status = o.shipped_date == null ? false : true }).ToList();
                dataGridView1.DataSource = od;
            }
        }

        /// cell click(Order)
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (labelOder.Text == "Order")
            {
                if (e.RowIndex != -1)
                {
                    string orderid = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    using (var db = new QuanLyBHEntity())
                    {
                        var order = db.Orders.FirstOrDefault(s => s.order_id == orderid);
                        var items = (from i in db.OrderItems
                                     where i.order_id == order.order_id
                                     select new { product = i.Product.pro_name, price = i.unit_price, quantity = i.quantity, total = i.unit_price * i.quantity }).ToList();
                        dataGridView2.DataSource = items;
                    }
                }
            }
        }

        //right click cell

        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (labelOder.Text == "Order")
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (e.RowIndex != -1)
                    {
                        contextMenuStrip1.Show();
                    }
                }
            }
            else
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (e.RowIndex != -1)
                    {
                        contextMenuStrip2.Show();
                    }
                }
            }
        }


        // handle context menu trip(Order)
        private void dksbfkToolStripMenuItem_Click(object sender, EventArgs e)
        {

            using (var db = new QuanLyBHEntity())
            {
                var order_id = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString();
                Order order = db.Orders.FirstOrDefault(s => s.order_id == order_id);
                OrderProcessingForm form = new OrderProcessingForm(order, employee);
                form.ShowDialog();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            labelOrderDetail.Text = "";
            labelOder.Text = "Product";
            dataGridView1.ContextMenuStrip = contextMenuStrip2;
            using (var db = new QuanLyBHEntity())
            {
                var product = (from p in db.Products
                               select new { id = p.pro_id,name = p.pro_name, stock = p.units_instock, status = p.pro_status == null ? false : true }).ToList();
                dataGridView1.DataSource = product;
            }
        }

        //change stock
        private void changeStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(var db = new QuanLyBHEntity())
            {
                var pro_id = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString();
                var product = db.Products.FirstOrDefault(s => s.pro_id == pro_id);
                var form = new ChangeStockForm(product);
                form.ShowDialog();
            }

        }
    }
}
