
using DoAnCuoiKy.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnCuoiKy.UserController
{
    public partial class ShowCartControl : UserControl
    {
        CartItem cartItem;
        Product product;
        public ShowCartControl()
        {
            InitializeComponent();
        }
        public ShowCartControl(CartItem item)
        {
            this.cartItem = item;
            InitializeComponent();
            LoadCartItem();
        }


        private void LoadCartItem()
        {
            using (var db = new QuanLyBHEntity())
            {
                product = db.Products.FirstOrDefault(s => s.pro_id == cartItem.product_id);
                string path = "../../../Images/Products/" + product.product_img;
                pictureBox1.ImageLocation = File.Exists(path) ? path : "../../Resources/no-avatar.png";
                labelTitle.Text = product.pro_name;
                labelQuantity.Text = cartItem.quantity.ToString();
                labelPrice.Text = product.unit_price.ToString();
                labelTotal.Text = (product.unit_price * cartItem.quantity).ToString();
            }
        }
        //remove a item in cart
        private void pictureBoxRemove_Click(object sender, EventArgs e)
        {
            using (var db = new QuanLyBHEntity())
            {
                CartItem item = db.CartItems.FirstOrDefault(s => s.product_id == this.cartItem.product_id);
                try
                {
                    db.CartItems.Remove(item);
                }
                catch
                {
                    MessageBox.Show("Removed, Press 'Cart' to reload");
                }
                MessageBox.Show("Removed, Press 'Cart' to reload");

                db.SaveChanges();
                LoadCartItem();
            }
        }
        
        // + 
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            using(var db = new QuanLyBHEntity())
            {
                CartItem c =  db.CartItems.FirstOrDefault(s => s.product_id == this.product.pro_id);
                if (c.quantity < c.Product.units_instock)
                {
                    c.quantity += 1;
                }
                db.SaveChanges();
                labelQuantity.Text = c.quantity.ToString();
            }
        }
        // -
        private void buttonSub_Click(object sender, EventArgs e)
        {
            using (var db = new QuanLyBHEntity())
            {
                CartItem c = db.CartItems.FirstOrDefault(s => s.product_id == this.product.pro_id);
                if (c!=null)
                {
                    c.quantity -= 1;
                    if (c.quantity <= 0)
                    {
                        db.CartItems.Remove(c);
                    }
                    labelQuantity.Text = c.quantity.ToString();
                }
                db.SaveChanges();
            }
        }
    }
}
