using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Restaurant
{
    public partial class customerform : Form
    {
        private const string connectionString = "Server=desktop-eqfbg24\\kunfaris;Database=restaurant;Integrated Security=True";

        public customerform()
        {
            InitializeComponent();
            LoadCustomers();
            idLabel.Hide();
        }

        private void ClearInputFields()
        {
            txtCustomerName.Text = "";
            txtEmail.Text = "";
            idLabel.Text = "";
        }

        private void LoadCustomers()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Customer";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading customers: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string customerName = txtCustomerName.Text.Trim();
                string email = txtEmail.Text.Trim();

                if (string.IsNullOrEmpty(customerName) || string.IsNullOrEmpty(email))
                {
                    MessageBox.Show("Please enter customer name and email.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO Customer (CustomerName, CustomerEmail) VALUES (@CustomerName, @CustomerEmail)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CustomerName", customerName);
                    command.Parameters.AddWithValue("@CustomerEmail", email);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Customer added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadCustomers();
                        ClearInputFields();
                    }
                    else
                    {
                        MessageBox.Show("Failed to add customer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while adding customer: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            
        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            try
            {
                string customerName = txtCustomerName.Text.Trim();
                string email = txtEmail.Text.Trim();
                int customerId = Convert.ToInt32(idLabel.Text);

                if (string.IsNullOrEmpty(idLabel.Text))
                {
                    MessageBox.Show("Please select a customer to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "UPDATE Customer SET CustomerName = @CustomerName, CustomerEmail = @CustomerEmail WHERE ID = @ID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CustomerName", customerName);
                    command.Parameters.AddWithValue("@CustomerEmail", email);
                    command.Parameters.AddWithValue("@ID", customerId);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Customer updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadCustomers();
                        ClearInputFields();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update customer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating customer: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRemove_Click_1(object sender, EventArgs e)
        {
            try
            {
                int customerId = Convert.ToInt32(idLabel.Text);

                if (string.IsNullOrEmpty(idLabel.Text))
                {
                    MessageBox.Show("Please select a customer to remove.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "DELETE FROM Customer WHERE ID = @ID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ID", customerId);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Customer removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadCustomers();
                        ClearInputFields();
                    }
                    else
                    {
                        MessageBox.Show("Failed to remove customer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while removing customer: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                txtCustomerName.Text = row.Cells["CustomerName"].Value.ToString();
                txtEmail.Text = row.Cells["CustomerEmail"].Value.ToString();
                idLabel.Text = row.Cells["ID"].Value.ToString();
            }
        }
    }
}
