using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PikitikosCE
{
    public partial class UserLogin : Form
    {

        private I_Notification callerForm;

        public UserLogin()
        {
            InitializeComponent();
        }

        public UserLogin(I_Notification caller)
        {
            InitializeComponent();
            callerForm = caller;
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            if (this.txtPassword.Text == "123321")
            {
                Configuration.IsLoggedIn = true;
                this.Close();
            } else
            {
                MessageBox.Show("Contraseña Incorrecta", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UserLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Configuration.IsLoggedIn && callerForm != null)
            {
                callerForm.CloseIt();
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnEntrar_Click(this, new EventArgs());
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.txtPassword.Focus();
            }
        }
    }
}
