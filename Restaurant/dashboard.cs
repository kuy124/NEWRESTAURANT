using System;
using System.Drawing;
using System.Windows.Forms;

namespace Restaurant
{
    public partial class DashboardForm : Form
    {

        private string username;
        private string userRole;

        public DashboardForm(string username, string role)
        {
            InitializeComponent();
            loadform(new homeform());

            this.username = username;
            this.userRole = role;

            nama.Text = "Username: " + username;
            roleLabel.Text = "Role: " + userRole;

            if (username != null && userRole == "Admin")
            {
                managerToolStripMenuItem.Enabled = false;
                supervisorToolStripMenuItem.Enabled = false;
                salesToolStripMenuItem.Enabled = false;
            }
            if (username != null && userRole == "Manager")
            {
                adminToolStripMenuItem.Enabled = false;
                supervisorToolStripMenuItem.Enabled = false;
                salesToolStripMenuItem.Enabled= false;
            }
            if (username != null && userRole == "Supervisor")
            {
                adminToolStripMenuItem.Enabled = false;
                managerToolStripMenuItem.Enabled = false;
                salesToolStripMenuItem.Enabled = false;
            }
            if (username != null && userRole == "Sales")
            {
                adminToolStripMenuItem.Enabled = false;
                managerToolStripMenuItem.Enabled = false;
                supervisorToolStripMenuItem.Enabled = false;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {

        }

        public void loadform(object Form)
        {
            if (this.mainpanel.Controls.Count > 0)
                this.mainpanel.Controls.RemoveAt(0);
            Form f = Form as Form;
            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            this.mainpanel.Controls.Add(f);
            this.mainpanel.Tag = f;
            f.Show();
        }

        private void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadform(new homeform());
        }

        private void addUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadform(new userform());
        }

        private void categoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadform(new categoryform());
        }

        private void menuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadform(new menuform());
        }

        private void lOGOUTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoginForm lgnform = new LoginForm();
            lgnform.Show();
            this.Hide();
        }

        private void pOSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadform(new paymentform());
        }

        private void salesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void customerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadform(new customerform());
        }
    }
}
