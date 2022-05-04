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
    public partial class ChangeStockForm : Form
    {
        Product product;
        public ChangeStockForm(Product product)
        {
            InitializeComponent();
            this.product = product;
        }

        private void ChangeStockForm_Load(object sender, EventArgs e)
        {
            using(var db = new QuanLyBHEntity())
            {
                labelName.Text = product.pro_name;
                textBoxStock.Text = product.units_instock.ToString();
            }
        }

        // change stock

        private void buttonChange_Click(object sender, EventArgs e)
        {
            using(var db = new QuanLyBHEntity())
            {
                var pro = db.Products.FirstOrDefault(s => s.pro_id == product.pro_id);
                int a;
                if (Int32.TryParse(textBoxStock.Text, out a))
                {
                    pro.units_instock = a;
                    db.SaveChanges();
                    MessageBox.Show("change stock sucess");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Enter a number");
                }

            }
        }
    }
}
