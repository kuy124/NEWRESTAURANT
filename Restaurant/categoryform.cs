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
    public partial class categoryform : Form
    {
        private const string connectionString = "Server=desktop-eqfbg24\\kunfaris;Database=restaurant;Integrated Security=True";
        public categoryform()
        {
            InitializeComponent();
            LoadCategories();
        }

        private void LoadCategories()
        {
            idLabel.Hide();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Category";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading categories: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Category (CategoryName, CategoryDesc) VALUES (@CategoryName, @CategoryDesc)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CategoryName", txtCategory.Text.Trim());
                    command.Parameters.AddWithValue("@CategoryDesc", txtDesc.Text.Trim());
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Category added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadCategories();
                    }
                    else
                    {
                        MessageBox.Show("Failed to add category.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while adding category: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                txtCategory.Text = row.Cells["CategoryName"].Value.ToString();
                txtDesc.Text = row.Cells["CategoryDesc"].Value.ToString();
                idLabel.Text = row.Cells["ID"].Value.ToString();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idLabel.Text))
            {
                MessageBox.Show("Please select a category to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string categoryName = txtCategory.Text.Trim();
                string categoryDesc = txtDesc.Text.Trim();
                int categoryId = Convert.ToInt32(idLabel.Text);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "UPDATE Category SET CategoryName = @CategoryName, CategoryDesc = @CategoryDesc WHERE ID = @ID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CategoryName", categoryName);
                    command.Parameters.AddWithValue("@CategoryDesc", categoryDesc);
                    command.Parameters.AddWithValue("@ID", categoryId);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Category updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadCategories();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update category.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating category: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idLabel.Text))
            {
                MessageBox.Show("Please select a category to remove.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                int categoryId = Convert.ToInt32(idLabel.Text);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "DELETE FROM Category WHERE ID = @ID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ID", categoryId);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Category removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadCategories();
                    }
                    else
                    {
                        MessageBox.Show("Failed to remove category.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while removing category: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
