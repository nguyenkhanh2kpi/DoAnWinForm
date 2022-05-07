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
    public partial class AddCategoryForm : Form
    {
        Category cat = new Category();
        public AddCategoryForm()
        {
            InitializeComponent();
        }

        private void AddCategoryForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBoxName.Text == null)
            {
                MessageBox.Show("Enter a name");
            }
            else
            {
                using (var db = new QuanLyBHEntity())
                {
                    cat.cat_name = textBoxName.Text.ToString();
                    int num = 0;
                    cat.cat_id = "cat_1";
                    while (FindCatID(cat.cat_id))
                    {
                        cat.cat_id = "cat_" + (num + 1).ToString();
                        num++;
                    }
                    if (FindCat(cat.cat_name) == false)
                    {
                        db.Categories.Add(cat);
                        db.SaveChanges();

                        MessageBox.Show("Sucess");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("The Category Is Exist");
                    }
                }
            }
        }

        // find cat
        private bool FindCat(string cat_name)
        {
            using (var db = new QuanLyBHEntity())
            {
                foreach (var c in db.Categories)
                {
                    if (c.cat_name == cat_name)
                        return true;
                }
                return false;
            }
        }
        //find cat id
        private bool FindCatID(string cat_id)
        {
            using (var db = new QuanLyBHEntity())
            {
                foreach (var c in db.Categories)
                {
                    if (c.cat_id == cat_id)
                        return true;
                }
                return false;
            }
        }
    }
}
