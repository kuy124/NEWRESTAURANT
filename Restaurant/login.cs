using System;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Restaurant
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void lblClear_Click(object sender, EventArgs e)
        {
            textBoxUsername.Clear();
            textBoxPassword.Clear();
            comboBoxRole.SelectedIndex = -1;
        }

        private void buttonLogin_Click_1(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text;
            string password = textBoxPassword.Text;
            string role = comboBoxRole.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string connectionString = "Data Source=desktop-eqfbg24\\kunfaris;Initial Catalog=restaurant;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Users WHERE Username=@Username AND Password=@Password AND Role=@Role";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@Role", role);

                try
                {
                    connection.Open();

                    int count = (int)command.ExecuteScalar();

                    if (count > 0)
                    {

                        MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DashboardForm formdash = new DashboardForm(username, role);
                        formdash.Show();
                        this.Hide();

                    }
                    else
                    {
                        MessageBox.Show("Invalid username, password, or role.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}