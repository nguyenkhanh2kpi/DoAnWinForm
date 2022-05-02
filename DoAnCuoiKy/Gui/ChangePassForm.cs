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
    public partial class ChangePassForm : Form
    {
        private Person person;
        public ChangePassForm(Person person)
        {
            InitializeComponent();
            this.person = person;
        }

        private void buttonChange_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            using (var db = new QuanLyBHEntity())
            {

                if (PassVaridate() && NewPassValidate() && RePassValidate())
                {
                    var per = db.People.FirstOrDefault(s => s.per_id == this.person.per_id);
                    per.per_password = BCrypt.Net.BCrypt.HashPassword(textBoxNewPass.Text);
                    db.SaveChanges();
                    MessageBox.Show("Change Pass Success");
                }
                else
                {
                    MessageBox.Show("Error !!!");
                }
            }

        }
        private bool PassVaridate()
        {
            if (!BCrypt.Net.BCrypt.Verify(textBoxOldPass.Text,this.person.per_password))
            {

                errorProvider1.SetError(textBoxOldPass, "Invalid PassWord");
                return false;
            }
            else
            {
                errorProvider1.SetError(textBoxOldPass, null);
                return true;
            }
        }

        private bool NewPassValidate()
        {
            if (string.IsNullOrWhiteSpace(textBoxNewPass.Text))
            {
                errorProvider1.SetError(textBoxNewPass, "Must Enter Password");
                return false;
            }
            else if (textBoxNewPass.Text.Length < 6)
            {
                errorProvider1.SetError(textBoxNewPass, "Password 6-16 Character");
                return false;
            }
            else
            {
                errorProvider1.SetError(textBoxNewPass, null);
                return true;
            }
        }
        private bool RePassValidate()
        {
            if (textBoxNewPass.Text != textBoxRePass.Text)
            {
                errorProvider1.SetError(textBoxRePass, "RePass No Match");
                return false;
            }
            else
            {
                errorProvider1.SetError(textBoxRePass, null);
                return true;
            }
        }
    }
}
