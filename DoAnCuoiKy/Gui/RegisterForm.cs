using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoAnCuoiKy.Helper;
using DoAnCuoiKy.Models;

namespace DoAnCuoiKy.Gui
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            LoginForm form = new LoginForm();
            form.Show();
            this.Visible = false;
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            using (var db = new QuanLyBHEntity())
            {
                var dateRegis = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                var dateOfBirth = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, dateTimePicker1.Value.Day);

                if (NameVaridate() && EmailVaridate() && PassValidate() && RePassValidate() && PhoneVaridate() && AddressVaridate())
                {
                    if (db.People.FirstOrDefault(s => s.per_email == textBoxEmail.Text) == null)
                    {
                        AddCustomer(textBoxName.Text, textBoxEmail.Text, comboBoxGender.SelectedItem.ToString(), textBoxPhone.Text, textBoxAddress.Text, dateOfBirth, dateRegis, textBoxPassWord.Text);
                    }
                    else
                    {
                        MessageBox.Show("Your Email Is Exist");
                    }

                }
            }

        }
        // add
        public void AddCustomer(string name, string email, string gender, string phone, string address, DateTime dateOb, DateTime dateRegis, string pass)
        {
            try
            {
                using (var db = new QuanLyBHEntity())
                {
                    var per = new Person();
                    var numper = db.People.ToList().Count();
                    per.per_id = ("p_" + (numper + 1).ToString()).ToString();
                    per.role_id = "r_1";
                    per.per_name = name;
                    if (gender == "Male")
                    {
                        per.per_gender = "nam";

                    }
                    else
                    {
                        per.per_gender = "nu";
                    }
                    per.per_date = dateOb;
                    per.per_regis_date = dateRegis;
                    per.per_address = address;
                    per.per_phone_number = phone;
                    per.per_email = email;
                    per.per_password = BCrypt.Net.BCrypt.HashPassword(pass);
                    per.per_status = "active";
                    db.People.Add(per);
                    db.SaveChanges();
                    MessageBox.Show("Add Customer Success");
                }

            }
            catch
            {
                MessageBox.Show("Add Error");
            }
        }

        // varidate 
        private bool NameVaridate()
        {
            var name = textBoxName.Text;
            var match = Regex.Match(name, RegexString.NAME, RegexOptions.IgnoreCase);
            if (string.IsNullOrWhiteSpace(name))
            {
                errorProvider1.SetError(textBoxName, "Must Enter A Name");
                return false;
            }
            else if (!match.Success)
            {
                errorProvider1.SetError(textBoxName, "First Name Must Be Alphabet");
                return false;
            }
            else
            {
                errorProvider1.SetError(textBoxName, null);
                return true;
            }
        }


        // mail varidate
        private bool EmailVaridate()
        {
            var match = Regex.Match(textBoxEmail.Text, RegexString.EMAIL, RegexOptions.IgnoreCase);

            if (string.IsNullOrWhiteSpace(textBoxEmail.Text))
            {
                errorProvider1.SetError(textBoxEmail, "Must Be Enter Email");
                return false;
            }
            else if (!match.Success)
            {
                errorProvider1.SetError(textBoxEmail, "Must Be In Correct Email Format");
                return false;
            }
            else
            {
                errorProvider1.SetError(textBoxEmail, null);
                return true;
            }
        }
        // pass v
        private bool PassValidate()
        {
            if (string.IsNullOrWhiteSpace(textBoxPassWord.Text))
            {
                errorProvider1.SetError(textBoxPassWord, "Must Enter Password");
                return false;
            }
            else if (textBoxPassWord.Text.Length < 6)
            {
                errorProvider1.SetError(textBoxPassWord, "Password 6-16 Character");
                return false;
            }
            else
            {
                errorProvider1.SetError(textBoxPassWord, null);
                return true;
            }
        }
        private bool RePassValidate()
        {
            if (textBoxPassWord.Text != textBoxRepass.Text)
            {
                errorProvider1.SetError(textBoxRepass, "RePass No Match");
                return false;
            }
            else
            {
                errorProvider1.SetError(textBoxRepass, null);
                return true;
            }
        }
        private bool PhoneVaridate()
        {
            int a;
            if (!Int32.TryParse(textBoxPhone.Text, out a))
            {
                errorProvider1.SetError(textBoxPhone, "Must Be A Phone NumBer");
                return false;
            }
            else
            {
                errorProvider1.SetError(textBoxPhone, null);
                return true;
            }
        }
        private bool AddressVaridate()
        {
            if (string.IsNullOrWhiteSpace(textBoxAddress.Text))
            {
                errorProvider1.SetError(textBoxAddress, "Enter Address");
                return false;
            }
            else
            {
                errorProvider1.SetError(textBoxAddress, null);
                return true;
            }
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {
            comboBoxGender.SelectedIndex = 0;
        }
    }
}
