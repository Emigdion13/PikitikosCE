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
    public partial class MainWindow : Form, I_Notification
    {
        public MainWindow()
        {
            InitializeComponent();

            if (!Configuration.IsLoggedIn)
            {
                (new UserLogin(this)).ShowDialog();
            }
        }

        public void CloseIt()
        {
            Application.Exit();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void manejarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new frmInventario()).Show();
        }
    }
}
