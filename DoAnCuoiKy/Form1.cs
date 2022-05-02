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

namespace DoAnCuoiKy
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

            using(var q = new QuanLyBHEntity())
            {
                var c = new Category();
                c.cat_id = "cat_1";
                c.cat_name = "rau cu";
                q.Categories.Add(c);
                q.SaveChanges();
            }
        }
    }
}
