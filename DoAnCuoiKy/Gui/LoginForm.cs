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
using DoAnCuoiKy.Gui;

namespace DoAnCuoiKy.Gui
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            passwordTxt.PasswordChar = '*';
        }

        private Person Find(string email)
        {
            using(var p = new QuanLyBHEntity())
            {
                return p.People.FirstOrDefault(s => s.per_email == email);
            }
        }

        public Person CheckPerson(string email, string password)
        {
            var per = Find(email);
            if(per != null)
            {
                if(BCrypt.Net.BCrypt.Verify(password, per.per_password))
                {
                    return per;
                }
                else
                {
                    MessageBox.Show("Password invalid");
                }
            }
            else
            {
                MessageBox.Show("Email is not Exist");
            }
            return null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using(var q = new QuanLyBHEntity())
            {
                var email = emailTxt.Text.Trim();
                var password = passwordTxt.Text.Trim();
                var per = CheckPerson(email, password);
                if (per != null)
                {
                    if (per.per_status == "active")
                    {
                        switch (per.role_id)
                        {
                            case "r_1":
                                var form1 = new CustomerHomeForm(per);
                                form1.Show();
                                this.Visible = false;
                                break;
                            case "r_2":
                                var form2 = new EmployeeForm();
                                form2.Show();
                                this.Visible = false;
                                break;

                            case "r_3":
                                var form3 = new AdminForm();
                                form3.Show();
                                this.Visible = false;
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Your Account Is Not Active");
                    }
                }
                else
                {
                    MessageBox.Show("Account DOESN'T exist");
                }
                
            }
            
        }
        //xem mk
        private void pictureBox3_Click_1(object sender, EventArgs e)
        {
            if (passwordTxt.PasswordChar.ToString().Contains('*'))
            {
                passwordTxt.PasswordChar = (char)0;
                pictureBox3.Image = Properties.Resources.open_eye;
            }
            else
            {
                passwordTxt.PasswordChar = '*';
                pictureBox3.Image = Properties.Resources.visibility;
            }
        }

        private void Register_Button_Click(object sender, EventArgs e)
        {
            RegisterForm form = new RegisterForm();
            form.Show();
            this.Visible = false;
        }
    }
}
