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
using System.Windows.Forms.DataVisualization.Charting;

namespace Restaurant
{
    public partial class homeform : Form
    {
        private const string connectionString = "Server=desktop-eqfbg24\\kunfaris;Database=restaurant;Integrated Security=True";
        public homeform()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string customerQuery = "SELECT COUNT(*) AS TotalCustomers FROM Customer";
                    SqlCommand customerCommand = new SqlCommand(customerQuery, connection);
                    int totalCustomers = (int)customerCommand.ExecuteScalar();
                    lblTotalCustomers.Text = totalCustomers.ToString();

                    string salesQuery = "SELECT Count(*) AS TotalSales FROM Bill";
                    SqlCommand salesCommand = new SqlCommand(salesQuery, connection);
                    object totalSalesResult = salesCommand.ExecuteScalar();
                    int totalSales = totalSalesResult != DBNull.Value ? Convert.ToInt32(totalSalesResult) : 0;
                    lblSales.Text = totalSales.ToString();

                    string profitQuery = "SELECT SUM(TotalPrice) AS TotalProfit FROM Bill";
                    SqlCommand profitCommand = new SqlCommand(profitQuery, connection);
                    object totalProfitResult = profitCommand.ExecuteScalar();
                    int totalProfit = totalProfitResult != DBNull.Value ? Convert.ToInt32(totalProfitResult) : 0;
                    lblProfits.Text = totalProfit.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void homeform_Load(object sender, EventArgs e)
        {

        }
    }
}
