using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restaurant
{
    public partial class userform : Form
    {
        public userform()
        {
            InitializeComponent();
            LoadUserData();
        }

        private const string connectionString = "Server=desktop-eqfbg24\\kunfaris;Database=restaurant;Integrated Security=True";

        private void LoadUserData()
        {
            idLabel.Hide();
            idLabel.Hide();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Users";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading user data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                idLabel.Text = row.Cells["UserID"].Value.ToString();
                txtUsername.Text = row.Cells["Username"].Value.ToString();
                txtPassword.Text = row.Cells["Password"].Value.ToString();
                cmbRole.Text = row.Cells["Role"].Value.ToString();
            }
        }

        private void refresh()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            cmbRole.SelectedIndex = -1;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string role = cmbRole.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(role))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO Users (Username, Password, Role) VALUES (@Username, @Password, @Role)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@Role", role);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("User added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadUserData();
                        refresh();
                    }
                    else
                    {
                        MessageBox.Show("Failed to add user.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string role = cmbRole.Text;
            int userID;

            if (!int.TryParse(idLabel.Text, out userID))
            {
                MessageBox.Show("Please select a user to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(role))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "UPDATE Users SET Username = @Username, Password = @Password, Role = @Role WHERE UserID = @UserID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@Role", role);
                    command.Parameters.AddWithValue("@UserID", userID);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("User updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadUserData();
                        refresh();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update user.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int userID;

            if (!int.TryParse(idLabel.Text, out userID))
            {
                MessageBox.Show("Please select a user to remove.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "DELETE FROM Users WHERE UserID = @UserID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@UserID", userID);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("User removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadUserData();
                        refresh();
                    }
                    else
                    {
                        MessageBox.Show("Failed to remove user.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string roleName = cmbSearch.SelectedItem?.ToString();
            string userName = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(roleName))
            {
                MessageBox.Show("Please select a role.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query;
                    SqlCommand command;

                    if (string.IsNullOrEmpty(userName))
                    {
                        query = "SELECT * FROM Users WHERE Role = @Role";
                        command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@Role", roleName);
                    }
                    else
                    {
                        query = "SELECT * FROM Users WHERE Role = @Role AND Username LIKE @UserName";
                        command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@Role", roleName);
                        command.Parameters.AddWithValue("@UserName", "%" + userName + "%");
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        dataGridView2.DataSource = dataTable;
                    }
                    else
                    {
                        MessageBox.Show("No matching users found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
