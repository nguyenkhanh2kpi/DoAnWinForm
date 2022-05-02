
using DoAnCuoiKy.Gui;
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
    public partial class ShowProductControl : UserControl
    {
        private Product product;
        public ShowProductControl()
        {
            InitializeComponent();
        }

        public ShowProductControl(Product product)
        {
            InitializeComponent();
            this.product = product;
            LoadProduct();

        }

        private void LoadProduct()
        {
            string path = "../../../Images/Products/" + product.product_img;
            pictureBox1.ImageLocation = File.Exists(path) ? path : "../../Resources/no-avatar.png";
            labelProduct.Text = product.pro_name;
            labelPrice.Text = product.unit_price.ToString();
        }



        private void button3_Click(object sender, EventArgs e)
        {
            var f = new ShowProductForm(product);
            f.Show();
        }


        // event AddToCart
        private void buttonAddToCart_Click(object sender, EventArgs e)
        {
            using (var db = new QuanLyBHEntity())
            {
                if (!IsExistInCart())
                {
                    CartItem c = CreCartItem(this.product, 1);
                    db.CartItems.Add(c);
                    db.SaveChanges();
                }
            }
        }

        // create cartitem
        public CartItem CreCartItem(Product pro, int quantity)
        {
            CartItem item = new CartItem();
            item.product_id = pro.pro_id;
            item.quantity = quantity;
            item.totalPrice = pro.unit_price * quantity;
            return item;

        }


        // kiem tra neu san pham da ton tai trong gio hang thi tang quantity them 1 don vi
        private bool IsExistInCart()
        {
            using (var db = new QuanLyBHEntity())
            {
                bool find = false;
                List<CartItem> cart = db.CartItems.ToList();
                foreach (var item in cart)
                {
                    if (item.product_id == this.product.pro_id)
                    {
                        item.quantity += 1;
                        item.totalPrice = item.Product.unit_price * item.quantity;
                        find = true;
                    }
                }
                if (find)
                {
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }
    }
}
