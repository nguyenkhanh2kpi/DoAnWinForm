using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoAnCuoiKy.Models;
using System.Data.Entity;
using System.Globalization;
namespace DoAnCuoiKy.Gui
{


    public partial class AddCategory : Form
    {
        public AddCategory()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (QuanLyBHEntity db = new QuanLyBHEntity())
            {
                var q1 = from cat in db.Categories
                         select new { cat.cat_id, cat.cat_name };

                CatGridView.DataSource = q1.ToList();
                this.CatGridView.RowHeadersVisible = false;
                this.CatGridView.Columns[0].HeaderText = "ID";
                this.CatGridView.Columns[1].HeaderText = "Name";
                foreach (DataGridViewColumn col in CatGridView.Columns)
                {
                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

            }
        }
        public bool NameValidate(TextBox textBox)
        {
            using (QuanLyBHEntity db = new QuanLyBHEntity())
            {
                var q1 = db.Categories.FirstOrDefault(cat => cat.cat_name == textBox.Text);
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
            errorProvider1.Clear();
            UpperEachWord();
            using (QuanLyBHEntity db = new QuanLyBHEntity())
            {
                Category cat = new Category();
                var q1 = db.Categories.ToList().LastOrDefault();
                string strnum = q1.cat_id;
                cat.cat_name = catTextBox.Text;
                int num = Convert.ToInt32(strnum[strnum.Length - 1].ToString()) + 1;
                cat.cat_id = "cat_" + num.ToString();

                if (NameValidate(catTextBox))
                {
                    try
                    {
                        db.Categories.Add(cat);
                        db.SaveChanges();
                        LoadData();
                        MessageBox.Show("Add successfully!");
                    }
                    catch
                    {
                        MessageBox.Show("Error! Cannot add category!");
                    }
                }

            }
        }


        private void UpperEachWord()
        {
            string str = catTextBox.Text;
            str = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str);
            catTextBox.Text = str;
            catTextBox.Refresh();
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            UpperEachWord();
            foreach (DataGridViewRow row in CatGridView.Rows)
            {
                if (row.Cells[1].Value.ToString() == catTextBox.Text)
                {
                    row.Selected = true;
                    CatGridView.FirstDisplayedScrollingRowIndex = row.Index;
                    return;
                }

            }
            errorProvider1.SetError(catTextBox, "Name doesn't exist!");

        }

        private void AddCategory_Load(object sender, EventArgs e)
        {

        }

        private void delButton_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            UpperEachWord();
            using (QuanLyBHEntity db = new QuanLyBHEntity())
            {
                Category cat = db.Categories.FirstOrDefault(s => s.cat_name == catTextBox.Text);
                if (cat == null)
                {
                    errorProvider1.SetError(catTextBox, "Cannot find!!!");
                    return;
                }
                db.Categories.Remove(cat);
                db.SaveChanges();
                MessageBox.Show("Delete successfully!");
                LoadData();
            }
        }

        private void CatGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            catTextBox.Text = CatGridView.CurrentRow.Cells[1].Value.ToString();
            catTextBox.Refresh();
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            using (QuanLyBHEntity db = new QuanLyBHEntity())
            {
                Category cat = db.Categories.FirstOrDefault(s => s.cat_name == catTextBox.Text);
                if (cat==null)
                {
                    errorProvider1.SetError(editTextBox, "Name doesn't exist!!!");
                }
                else
                {
                    if (NameValidate(editTextBox))
                    {
                        try
                        {
                            cat.cat_name = editTextBox.Text;
                            db.SaveChanges();
                            MessageBox.Show("Edit successfully");
                        }
                        catch
                        {
                            MessageBox.Show("Error!!!");

                        }
                    }
                }

                editTextBox.Text = "";
                editTextBox.Refresh();
                LoadData();
            }
        }
    }

}
