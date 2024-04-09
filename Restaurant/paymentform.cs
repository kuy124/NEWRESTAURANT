using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace Restaurant
{
    public partial class paymentform : Form
    {
        private const string connectionString = "Server=desktop-eqfbg24\\kunfaris;Database=restaurant;Integrated Security=True";
        private bool customerSelected = false;

        public paymentform()
        {
            InitializeComponent();
            LoadMenuItems();
            LoadCustomerName();
            LoadBillData();

            dataGridView2.ColumnCount = 4;
            dataGridView2.Columns[0].Name = "ID";
            dataGridView2.Columns[1].Name = "MenuName";
            dataGridView2.Columns[2].Name = "Category";
            dataGridView2.Columns[3].Name = "Price";
        }

        private void LoadMenuItems()
        {
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

        private void LoadBillData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Bill";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView3.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading bill data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCustomerName()
        {
            cmbCustomerName.Items.Clear();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT CustomerName FROM Customer";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string customerName = reader["CustomerName"].ToString();
                        cmbCustomerName.Items.Add(customerName);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading customers: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int CalculateTotalPrice()
        {
            int totalPrice = 0;

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (!row.IsNewRow && row.Cells["Price"].Value != null)
                {
                    string priceString = row.Cells["Price"].Value.ToString().Trim();
                    int price;
                    if (int.TryParse(priceString, out price))
                    {
                        totalPrice += price;
                    }
                    else
                    {
                        MessageBox.Show("Invalid price format: " + priceString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            return totalPrice;
        }

        private void InsertOrderIntoBill(string orderNumber, string customerName, string menuName, DateTime date, int totalPrice)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string orderQuery = "INSERT INTO Bill (OrderNumber, CustomerName, MenuName, Date, TotalPrice) VALUES (@OrderNumber, @CustomerName, @MenuName, @Date, @TotalPrice)";
                    SqlCommand orderCommand = new SqlCommand(orderQuery, connection);
                    orderCommand.Parameters.AddWithValue("@OrderNumber", orderNumber);
                    orderCommand.Parameters.AddWithValue("@CustomerName", customerName);
                    orderCommand.Parameters.AddWithValue("@MenuName", menuName);
                    orderCommand.Parameters.AddWithValue("@Date", date);
                    orderCommand.Parameters.AddWithValue("@TotalPrice", totalPrice);
                    orderCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while inserting order details into the Bill table: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string orderNumber = txtNoOrder.Text;
            string customerName = cmbCustomerName.Text;
            DateTime date = DateTime.Now;
            int totalPrice = CalculateTotalPrice();

            Dictionary<string, int> menuCounts = new Dictionary<string, int>();
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (row.Cells["MenuName"].Value != null)
                {
                    string menuName = row.Cells["MenuName"].Value.ToString();
                    if (!menuCounts.ContainsKey(menuName))
                    {
                        menuCounts[menuName] = 1;
                    }
                    else
                    {
                        menuCounts[menuName]++;
                    }
                }
            }

            string menuNames = "";
            foreach (var item in menuCounts)
            {
                if (menuNames != "")
                    menuNames += ", ";
                menuNames += $"{item.Value} {item.Key}";
            }

            InsertOrderIntoBill(orderNumber, customerName, menuNames, date, totalPrice);

            LoadBillData();

            dataGridView2.Rows.Clear();

            txtNoOrder.Text = "";
            cmbCustomerName.SelectedIndex = -1;

            PrintOrderDocument printDocument = new PrintOrderDocument(dataGridView2, cmbCustomerName.Text, totalPrice, orderNumber, menuCounts);
            PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
            printPreviewDialog.Document = printDocument;
            printPreviewDialog.ShowDialog();
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    string menuName = row.Cells["MenuName"].Value.ToString();
                    string category = row.Cells["Category"].Value.ToString();
                    string menuDesc = row.Cells["MenuDesc"].Value.ToString();
                    int price = Convert.ToInt32(row.Cells["Price"].Value);

                    dataGridView2.Rows.Add("", menuName, category, price);
                }
            }
            else
            {
                MessageBox.Show("Please select a menu item to add.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public class PrintOrderDocument : PrintDocument
        {
            public DataGridView DataGridView { get; set; }
            public string CustomerName { get; set; }
            public int TotalPrice { get; set; }
            public string OrderNumber { get; set; }
            public Dictionary<string, int> MenuItemCounts { get; set; }

            public PrintOrderDocument(DataGridView dataGridView, string customerName, int totalPrice, string orderNumber, Dictionary<string, int> menuItemCounts)
            {
                DataGridView = dataGridView;
                CustomerName = customerName;
                TotalPrice = totalPrice;
                OrderNumber = orderNumber;
                MenuItemCounts = menuItemCounts;

                DocumentName = "Order";
                PrinterSettings.PrinterName = PrinterSettings.InstalledPrinters[0];
            }

            protected override void OnPrintPage(PrintPageEventArgs e)
            {
                base.OnPrintPage(e);

                Graphics graphics = e.Graphics;
                Font headerFont = new Font("Arial", 16, FontStyle.Bold);
                Font textFont = new Font("Arial", 14);

                string header = "Order Details";
                string line = "--------------------------";
                string customer = "Customer: " + CustomerName;
                string orderNo = "Order Number: " + OrderNumber;
                string date = "Date: " + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                string total = "Total Price: Rp " + TotalPrice.ToString();

                graphics.DrawString(header, headerFont, Brushes.Black, new PointF(100, 100));
                graphics.DrawString(line, textFont, Brushes.Black, new PointF(100, 120));
                graphics.DrawString(customer, textFont, Brushes.Black, new PointF(100, 140));
                graphics.DrawString(orderNo, textFont, Brushes.Black, new PointF(100, 160));
                graphics.DrawString(date, textFont, Brushes.Black, new PointF(100, 180));
                graphics.DrawString(total, textFont, Brushes.Black, new PointF(100, 200));

                int yPos = 240;
                foreach (var menuItemCount in MenuItemCounts)
                {
                    string menuItem = menuItemCount.Key;
                    int count = menuItemCount.Value;
                    string item = $"{count} {menuItem}";
                    graphics.DrawString(item, textFont, Brushes.Black, new PointF(100, yPos));
                    yPos += 30;
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string orderNumber = txtSearchOrder.Text.Trim();

            string query = "SELECT * FROM Bill WHERE 1=1";

            if (!string.IsNullOrEmpty(orderNumber))
            {
                query += $" AND OrderNumber = '{orderNumber}'";
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView3.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while searching: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
