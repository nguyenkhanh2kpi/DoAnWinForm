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
    
    public partial class AddPersonForm : Form
    {
        public AddPersonForm()
        {
            InitializeComponent();
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            var adminform = new AdminForm();
            adminform.Show();
            this.Hide();


        }
        private Person GetInformation()
        {
            errorProvider1.Clear();
            Person person = new Person();
            using (QuanLyBHEntity db = new QuanLyBHEntity())
            {
                var numper = db.People.ToList().Count();
                person.per_id = "p_" + (numper+1).ToString();
            }

            person.per_email = emailText.Text;
            person.per_password = BCrypt.Net.BCrypt.HashPassword(passText.Text);
            person.per_name = nameText.Text;

            if (genderBox.SelectedIndex == 0)
            {
                person.per_gender = "nam";
            }
            else person.per_gender = "nu";

            person.per_address = addressText.Text;
            person.per_phone_number = phoneText.Text;
            person.per_regis_date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            person.per_status = "active";
            person.per_date = new DateTime(birthdayPicker.Value.Year, birthdayPicker.Value.Month, birthdayPicker.Value.Day);

            if (roleComboBox.SelectedIndex == 0)
            {
                person.role_id = "r_1";
            }
            else if (roleComboBox.SelectedIndex == 1)
            {
                person.role_id = "r_2";
            }
            else if (roleComboBox.SelectedIndex == 2)
            {
                person.role_id = "r_3";
            }

            return person;

        }
        



        private void addButton_Click(object sender, EventArgs e)
        {
            if (VaridateAll() == true)
            {
                Person person = GetInformation();
                using (QuanLyBHEntity db = new QuanLyBHEntity())
                {
                    try
                    {
                        db.People.Add(person);
                        db.SaveChanges();
                        MessageBox.Show("Add successfully!!!");
                    }
                    catch
                    {
                        MessageBox.Show("Cannot add. Error!!!");
                    }
                }
            }
            else
            {
                return;
            }

        }

        public bool NameVaridate()
        {
            var name = nameText.Text;
            var match = Regex.Match(name, RegexString.NAME, RegexOptions.IgnoreCase);
            if (string.IsNullOrWhiteSpace(name))
            {
                errorProvider1.SetError(nameText, "Must Enter A Name");
                return false;
            }
            else if (!match.Success)
            {
                errorProvider1.SetError(nameText, "First Name Must Be Alphabet");
                return false;
            }
            else
            {
                errorProvider1.SetError(nameText, null);
                return true;
            }
        }


        // mail varidate
        public bool EmailVaridate()
        {
            var match = Regex.Match(emailText.Text, RegexString.EMAIL, RegexOptions.IgnoreCase);
            using (QuanLyBHEntity db = new QuanLyBHEntity())
            {
                if (db.People.FirstOrDefault(s => s.per_email == emailText.Text) != null)
                {
                    MessageBox.Show("This email already exist!!!");
                    return false;
                }

            }
            if (string.IsNullOrWhiteSpace(emailText.Text))
            {
                errorProvider1.SetError(emailText, "Must Be Enter Email");
                return false;
            }
            else if (!match.Success)
            {
                errorProvider1.SetError(emailText, "Must Be In Correct Email Format");
                return false;
            }
            else
            {
                errorProvider1.SetError(emailText, null);
                return true;
            }
        }
        // pass v
        public bool PassValidate()
        {
            if (string.IsNullOrWhiteSpace(passText.Text))
            {
                errorProvider1.SetError(passText, "Must Enter Password");
                return false;
            }
            else if (passText.Text.Length < 6)
            {
                errorProvider1.SetError(passText, "Password 6-16 Character");
                return false;
            }
            else
            {
                errorProvider1.SetError(passText, null);
                return true;
            }
        }
        /*public bool RePassValidate()
        {
            if (passText.Text != textBoxRepass.Text)
            {
                errorProvider1.SetError(textBoxRepass, "RePass No Match");
                return false;
            }
            else
            {
                errorProvider1.SetError(textBoxRepass, null);
                return true;
            }
        }*/
        public bool PhoneVaridate()
        {
            int a;
            if (!Int32.TryParse(phoneText.Text, out a))
            {
                errorProvider1.SetError(phoneText, "Must Be A Phone NumBer");
                return false;
            }
            else
            {
                errorProvider1.SetError(phoneText, null);
                return true;
            }
        }
        public bool AddressVaridate()
        {
            if (string.IsNullOrWhiteSpace(addressText.Text))
            {
                errorProvider1.SetError(addressText, "Enter Address");
                return false;
            }
            else
            {
                errorProvider1.SetError(addressText, null);
                return true;
            }
        }

        public bool VaridateAll()
        {
            if (NameVaridate() && EmailVaridate() && PassValidate() && /*RePassValidate() &&*/ PhoneVaridate() && AddressVaridate())
            {
                return true;
            }
            else return false;
        }



    }
}
