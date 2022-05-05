using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoAnCuoiKy.Models;
using DoAnCuoiKy.Gui;
using System.IO;

namespace DoAnCuoiKy.Gui
{
    public partial class AddProduct : Form
    {
        string picturePath = null;
        OpenFileDialog opf = new OpenFileDialog();
        public AddProduct()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using(QuanLyBHEntity db = new QuanLyBHEntity())
            {
                foreach (var category in db.Categories)
                {
                    catComboBox.Items.Add(category.cat_name.ToString());
                }
            }
        }


        public bool NameValidate(TextBox textBox)
        {
            using (QuanLyBHEntity db = new QuanLyBHEntity())
            {
                var q1 = db.Products.FirstOrDefault(product => product.pro_name == textBox.Text);
                if (q1 != null)
                {
                    errorProvider1.SetError(textBox, "This name already exist!");
                    return false;
                }
                else return true;
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (NameValidate(nameTextBox))
            {
                try
                {
                    using (QuanLyBHEntity db = new QuanLyBHEntity())
                    {
                        Product product = new Product();

                        int indexCategories = catComboBox.SelectedIndex;
                        var var_last_pro_id = db.Products.OrderByDescending(s => s.pro_id).FirstOrDefault();
                        string strLastProId=var_last_pro_id.pro_id.ToString();
                        string newID = "";

                        for (int i = strLastProId.Length-1;i!=0;i--)
                        {
                            if (int.TryParse(strLastProId[i].ToString(),out int n)==true)
                            {
                                if (newID=="")
                                {
                                    newID = strLastProId[i].ToString();
                                }
                                else
                                {
                                    newID.Insert(newID.Length, strLastProId[i].ToString());
                                }
                            }
                        }
                        newID = "pro_" + (Convert.ToInt32(newID)+5).ToString();

                        product.pro_id = newID;

                        var var_cat_id = db.Categories.OrderBy(s => s.cat_id).Skip(indexCategories).FirstOrDefault();
                        product.cat_id = var_cat_id.cat_id;

                        product.pro_name = nameTextBox.Text;

                        product.quantity_perUnit = unitTextBox.Text;

                        product.unit_price = Convert.ToInt32(priceTextBox.Text);
                        product.units_instock = Convert.ToInt32(InstockTextBox.Text);
                        product.units_onOrder = Convert.ToInt32(onorderTextBox.Text);
                        product.pro_status = "active";
                        product.product_img = opf.SafeFileName;

                        db.Products.Add(product);
                        db.SaveChanges();
                        MessageBox.Show("Add Successfully!!!");

                    }
                }
                catch
                {
                    MessageBox.Show("Error!!!");
                }
            }

            
        }

        
        private void AddPicIntoForm()
        {

            opf.Title = "Select product image";
            opf.Filter = "Image File (*.jpg; *jpeg; *.png;) |*.jpg; *jpeg; *.png;";
            opf.Multiselect = false;
            if (opf.ShowDialog() == DialogResult.OK)
            {
                Bitmap image = new Bitmap(opf.FileName);
                productPic.Image = image;
                string sFileName = opf.FileName;
            }

        }

        private void CopyPicToResource()
        {
            try
            {
                File.Copy(opf.FileName, "../../Resources/no-avatar.png");
            }
            catch
            {
                MessageBox.Show("Cannot copy picture!");
            }


        }

        private void productPic_Click(object sender, EventArgs e)
        {
            AddPicIntoForm();
        }
        
    }
}
