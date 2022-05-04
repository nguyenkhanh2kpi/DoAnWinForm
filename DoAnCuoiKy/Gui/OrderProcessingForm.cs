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
    public partial class OrderProcessingForm : Form
    {
        Order order;
        Person emp;
        public OrderProcessingForm(Order order,Person emp)
        {
            InitializeComponent();
            this.order = order;
            this.emp = emp;
        }

        private void OrderProcessingForm_Load(object sender, EventArgs e)
        {

                labelID.Text = order.order_id;
                labelCusname.Text = order.Person.per_name;
                labelShipAddress.Text = order.ship_address;
                labelPhone.Text = order.ship_phone;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var db = new QuanLyBHEntity())
            {
                var o = db.Orders.FirstOrDefault(s => s.order_id == order.order_id);
                labelID.Text = o.order_id;
                labelCusname.Text = o.Person.per_name;
                labelShipAddress.Text = o.ship_address;
                labelPhone.Text = o.ship_phone;
                o.emp_id = emp.per_id;
                o.shipped_date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                db.SaveChanges();
                MessageBox.Show("Sucess");
                this.Close();
            }
        }
    }
}
