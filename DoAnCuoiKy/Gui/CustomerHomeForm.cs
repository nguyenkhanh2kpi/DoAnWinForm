
using DoAnCuoiKy.Models;
using DoAnCuoiKy.UserController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnCuoiKy.Gui
{
    public partial class CustomerHomeForm : Form
    {
        private Person person;


        public CustomerHomeForm()
        {
            InitializeComponent();
        }
        public CustomerHomeForm(Person person) {
            InitializeComponent();
            this.person = person;
         
        }
   
        // load customer form
        private void CustomerHomeForm_Load(object sender, EventArgs e)
        {
            LoadProduct();
            DeleteCart();
            comboBox1.Items.Add("All");
            using (var q = new QuanLyBHEntity())
            {
                var cat = from c in q.Categories select c;
                foreach (var i in cat)
                {
                    comboBox1.Items.Add(i.cat_name.ToString());
                }
            }
            comboBox1.SelectedIndex = 0;
        }
        private void buttonProduct_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            LoadProduct();
        }

        // xem san pham
        private void LoadProduct()
        {
            using (var db = new QuanLyBHEntity())
            {
                flowLayoutPanelProduct.Controls.Clear();

                var a = db.Products.ToList();
                foreach(var i in a)
                {
                    flowLayoutPanelProduct.Controls.Add(new ShowProductControl(i));
                }
                //a.ForEach(x => flowLayoutPanelProduct.Controls.Add(new ShowProductControl(x)));
            }
        }
      
        // xoa gio hang
        private void DeleteCart()
        {
            using (var q = new QuanLyBHEntity())
            {
                foreach (var item in q.CartItems)
                {
                    q.CartItems.Remove(item);
                }
                q.SaveChanges();
            }
        }

        // tìm theo category
        private void LoadProductByCat(string category)
        {
            using (var db = new QuanLyBHEntity())
            {

                flowLayoutPanelProduct.Controls.Clear();

                var a = from p in db.Products
                        where p.Category.cat_name.ToString() == category
                        select p;
                foreach(var i in a)
                {
                    flowLayoutPanelProduct.Controls.Add(new ShowProductControl(i));
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var loginform = new LoginForm();
            loginform.Show();
            this.Visible = false;
        }

        private void flowLayoutPanelProduct_Paint(object sender, PaintEventArgs e)
        {

        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() =="All")
            {
                LoadProduct();
            }
            else
            {
                LoadProductByCat(comboBox1.SelectedItem.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var f = new AccountForm(person);
            f.Show();
        }
    
        // show cart
        private void buttonCart_Click(object sender, EventArgs e)
        {
            flowLayoutPanelProduct.Controls.Clear();
            using(var db = new QuanLyBHEntity())
            {
                int total = 0;
                var c = db.CartItems.ToList();
                foreach(var item in c)
                {
                    flowLayoutPanelProduct.Controls.Add(new ShowCartControl(item));
                    total = total + (item.quantity * item.Product.unit_price);
                }

                labelTotalCart.Text = total.ToString();
            } 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            buttonCart_Click(sender, e);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            buttonCart_Click(sender, e);
            var form = new CheckOutForm(this.person);
            form.Show();
        }
    }
}
