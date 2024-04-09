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
    public partial class menuform : Form
    {
        private const string connectionString = "Server=desktop-eqfbg24\\kunfaris;Database=restaurant;Integrated Security=True";
        public menuform()
        {
            InitializeComponent();
            LoadMenuItems();
            LoadCategories();
        }

        private void ClearInputFields()
        {
            txtMenuName.Text = "";
            cmbCategory.SelectedIndex = -1;
            txtMenuDesc.Text = "";
            txtPrice.Text = "";
        }

        private void LoadMenuItems()
        {
            idLabel.Hide();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Menu";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading menu items: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCategories()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT DISTINCT CategoryName FROM Category";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string category = reader["CategoryName"].ToString();
                        cmbCategory.Items.Add(category);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading categories: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                txtMenuName.Text = row.Cells["MenuName"].Value.ToString();
                cmbCategory.SelectedItem = row.Cells["Category"].Value.ToString();
                txtMenuDesc.Text = row.Cells["MenuDesc"].Value.ToString();
                txtPrice.Text = row.Cells["Price"].Value.ToString();
                idLabel.Text = row.Cells["ID"].Value.ToString();
            }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string menuName = txtMenuName.Text.Trim();
                string category = cmbCategory.SelectedItem?.ToString();
                string menuDesc = txtMenuDesc.Text.Trim();
                int price = int.Parse(txtPrice.Text.Trim());

                if (string.IsNullOrEmpty(menuName) || string.IsNullOrEmpty(category))
                {
                    MessageBox.Show("Please enter menu name, select a category, and enter a price.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO Menu (MenuName, Category, MenuDesc, Price) VALUES (@MenuName, @Category, @MenuDesc, @Price)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MenuName", menuName);
                    command.Parameters.AddWithValue("@Category", category);
                    command.Parameters.AddWithValue("@MenuDesc", menuDesc);
                    command.Parameters.AddWithValue("@Price", price);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Menu item added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadMenuItems();
                        ClearInputFields();
                    }
                    else
                    {
                        MessageBox.Show("Failed to add menu item.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while adding menu item: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string menuName = txtMenuName.Text.Trim();
                string category = cmbCategory.SelectedItem?.ToString();
                string menuDesc = txtMenuDesc.Text.Trim();
                int price = int.Parse(txtPrice.Text.Trim());

                if (string.IsNullOrEmpty(idLabel.Text))
                {
                    MessageBox.Show("Please select a menu item to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int menuId = Convert.ToInt32(idLabel.Text);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "UPDATE Menu SET MenuName = @MenuName, Category = @Category, MenuDesc = @MenuDesc, Price = @Price WHERE ID = @ID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MenuName", menuName);
                    command.Parameters.AddWithValue("@Category", category);
                    command.Parameters.AddWithValue("@MenuDesc", menuDesc);
                    command.Parameters.AddWithValue("@Price", price);
                    command.Parameters.AddWithValue("@ID", menuId);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Menu item updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadMenuItems();
                        ClearInputFields();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update menu item.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating menu item: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(idLabel.Text))
                {
                    MessageBox.Show("Please select a menu item to remove.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int menuId = Convert.ToInt32(idLabel.Text);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "DELETE FROM Menu WHERE ID = @ID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ID", menuId);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Menu item removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadMenuItems();
                        ClearInputFields();
                    }
                    else
                    {
                        MessageBox.Show("Failed to remove menu item.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while removing menu item: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
