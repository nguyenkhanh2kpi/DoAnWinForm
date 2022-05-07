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
    public partial class RemovePerson : Form
    {
        string person_id;
        public RemovePerson(string person_id)
        {
            this.person_id = person_id;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using(var db = new QuanLyBHEntity())
            {
                try
                {
                    var removePer = db.People.FirstOrDefault(s => s.per_id == person_id);
                    db.Entry(removePer).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    MessageBox.Show("Sucess");
                }
                catch
                {
                    MessageBox.Show("plesase complete all order");
                }
            }
            this.Close();
        }
    }
}
