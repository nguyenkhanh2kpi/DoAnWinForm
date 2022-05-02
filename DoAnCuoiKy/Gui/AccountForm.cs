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
    public partial class AccountForm : Form
    {
        private Person person = new Person();
        public AccountForm()
        {
            InitializeComponent();
        }
        public AccountForm(Person person)
        {
            InitializeComponent();
            this.person = person;
        }

        private void AccountForm_Load(object sender, EventArgs e)
        {
            using(var db = new QuanLyBHEntity())
            {
                var p = db.People.FirstOrDefault(s => s.per_id == person.per_id);
                textBoxName.Text = p.per_name.ToString();
                textBoxAddress.Text = p.per_address.ToString();
                textBoxEmail.Text = p.per_email.ToString();
                textBoxPhone.Text = p.per_phone_number.ToString();
                if(p.per_gender == "nam")
                {
                    comboBox1.SelectedIndex = 0;
                }
                else
                {
                    comboBox1.SelectedIndex = 1;
                }
          
            }
       
            textBoxName.Enabled = false;
            textBoxAddress.Enabled = false;
            textBoxEmail.Enabled = false;
            textBoxPhone.Enabled = false;
            comboBox1.Enabled = false;
            buttonSave.Enabled = false;




        }

        private void buttonChangepassword_Click(object sender, EventArgs e)
        {
            var form = new ChangePassForm(person);
            form.Show();
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            textBoxName.Enabled = true;
            textBoxAddress.Enabled = true;
            textBoxPhone.Enabled = true;
            comboBox1.Enabled = true;
            buttonSave.Enabled = true;
        }


        private void buttonSave_Click(object sender, EventArgs e)
        {
            using (var db = new QuanLyBHEntity())
            {
                var p = db.People.FirstOrDefault(s => s.per_id == person.per_id);
                if (textBoxName.Text != "" && textBoxAddress.Text != "" && textBoxPhone.Text != "")
                {
                    p.per_name = textBoxName.Text;
                    p.per_address = textBoxAddress.Text;
                    p.per_phone_number = textBoxPhone.Text;
                    if (comboBox1.SelectedIndex == 0)
                    {
                        p.per_gender = "nam";
                    }
                    else
                    {
                        p.per_gender = "nu";
                    }
                    db.SaveChanges();
                    MessageBox.Show("Success");
                }
                else
                {
                    AccountForm_Load(sender,e);
                    MessageBox.Show("Invalid");
                }
                textBoxName.Enabled = false;
                textBoxAddress.Enabled = false;
                textBoxEmail.Enabled = false;
                textBoxPhone.Enabled = false;
                comboBox1.Enabled = false;
                buttonSave.Enabled = false;

            }
        }
    }
}
