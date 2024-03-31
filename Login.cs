using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string stat;
            User obj1 = new User(txtUserID.Text, txtPassword.Text);
            stat = obj1.Login(txtUserID.Text);
            if (stat != null)
            {
                MessageBox.Show(stat);
            }
            txtUserID.Text = String.Empty;
            txtPassword.Text = String.Empty;
        }
    }
}
