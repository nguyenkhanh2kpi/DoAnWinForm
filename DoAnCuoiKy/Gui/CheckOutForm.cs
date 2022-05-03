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
    public partial class CheckOutForm : Form
    {
        Person person;
        public CheckOutForm(Person person)
        {
            this.person = person;
            InitializeComponent();
        }

        private void CheckOutForm_Load(object sender, EventArgs e)
        {
            using(var db = new QuanLyBHEntity())
            {
                int total = 0;
                if (db.CartItems.Count() == 0)
                {
                    buttonOrder.Enabled = false;
                }
                foreach(var i in db.CartItems)
                {
                    total = total +  ((int)i.quantity * (int)i.totalPrice);
                }
                dataGridView1.DataSource = db.CartItems.Select(a => new
                {
                    Name = a.Product.pro_name,
                    Quantity = a.quantity,
                    Price = a.totalPrice,
                    Total = a.quantity * a.totalPrice
                }).ToList();
                textBoxCusName.Text = person.per_name;
                textBoxCusName.Enabled = false;
                textBoxCusPhone.Text = person.per_phone_number.ToString();
                textBoxCusPhone.Enabled = false;
                textBoxCusAddress.Text = person.per_address;
                textBoxCusAddress.Enabled = false;
                labelTotal.Text = total.ToString();
                buttonSave.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBoxCusAddress.Enabled = true;
            buttonSave.Enabled = true;
            buttonOrder.Enabled = false;

        }
        private bool AddressVaridate()
        {
            if (string.IsNullOrWhiteSpace(textBoxCusAddress.Text))
            {
                errorProvider1.SetError(textBoxCusAddress, "Enter Address");
                return false;
            }
            else
            {
                errorProvider1.SetError(textBoxCusAddress, null);
                return true;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (AddressVaridate())
            {
                textBoxCusAddress.Enabled = false;
                buttonSave.Enabled = false;
                buttonOrder.Enabled = true;
            }
        }

        private void buttonOrder_Click(object sender, EventArgs e)
        {
            var order = CreateOrder();
            using (var db = new QuanLyBHEntity())
            {
                var cartitems = db.CartItems.ToList();
                foreach(var item in cartitems)
                {
                    var i = AddItem(order,item);
                    db.OrderItems.Add(i);
                    ChangeStock(item.Product, item.quantity);
                }
            }
            RemoveCart();
            MessageBox.Show("Order is being processed");
            buttonOrder.Enabled = false;
        }
        // change  stock
        private void ChangeStock(Product product, int quantity)
        {
            using(var db = new QuanLyBHEntity())
            {
                var p = db.Products.FirstOrDefault(s => s.pro_id == product.pro_id);
                p.units_instock = p.units_instock - quantity;
                db.SaveChanges();
            }
        }

        private void RemoveCart()
        {
            using(var db = new QuanLyBHEntity())
            {
                foreach(var i in db.CartItems.ToList())
                {
                    db.CartItems.Remove(i);
                }
                db.SaveChanges();
            }
        }
   
        public Order CreateOrder()
        {
            using(var db = new QuanLyBHEntity())
            {
                Order order = new Order();
                int num = db.Orders.ToList().Count();
                order.order_id = "o_" + (num + 1).ToString().Trim();
                order.cus_id = person.per_id;
                order.emp_id = null;
                order.order_date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                order.shipped_date = null;
                order.require_date = null;
                order.ship_name = person.per_name;
                order.ship_address = textBoxCusAddress.Text.ToString();
                order.ship_phone = person.per_phone_number;
                db.Orders.Add(order);
                db.SaveChanges();
                return order;
            }  
        }
        public OrderItem AddItem(Order oder,CartItem cartitem)
        {
            OrderItem oitem = new OrderItem();
            using (var db = new QuanLyBHEntity())
            {
                var num = db.OrderItems.Count();
                oitem.item_id = num + 1;
                oitem.order_id = oder.order_id;
                oitem.product_id = cartitem.product_id;
                oitem.unit_price = cartitem.Product.unit_price;
                oitem.quantity = cartitem.quantity;
                oitem.discount = 0;
                db.OrderItems.Add(oitem);
                db.SaveChanges();
                
            }
            return oitem;   
        }
    }
}
