using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Configuration;
using System.Data;

namespace ClothingStore
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        SqlConnection sqlconn;
        public MainWindow()

        {

            InitializeComponent();

            string conn = ConfigurationManager.ConnectionStrings["ClothingStore.Properties.Settings.ClothingStoreManagementConnectionString"].ConnectionString;

            sqlconn = new SqlConnection(conn); // Instantiate sql connection

            ShowCustomers();
            ShowAllOrders();

        }


        // Method to retrieve customers info 
        private void ShowCustomers()
        {
            try
            {
                //string quer = "SELECT * FROM Customers";
                string quer = "SELECT *,CONCAT(Id,' - ',name) AS custconcat FROM Customers";


                SqlDataAdapter sqlAdapter = new SqlDataAdapter(quer, sqlconn);

                using (sqlAdapter)
                {
                    DataTable customersTable = new DataTable();  // datatable to store info from customers

                    sqlAdapter.Fill(customersTable);

                    listCustomers.DisplayMemberPath = "custconcat";
                    listCustomers.SelectedValuePath = "Id";
                    listCustomers.ItemsSource = customersTable.DefaultView;
                }
            }
            catch
            {

            }

        }

        private void ShowOrders()
        {
            try
            {
                string quer = "SELECT * FROM Orders o INNER JOIN Customers c ON C.Id=o.cCustomer" +
                " WHERE c.Id=@CustomerId";

                SqlCommand sqlCom = new SqlCommand(quer, sqlconn);

                SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCom);

                using (sqlAdapter)
                {
                    sqlCom.Parameters.AddWithValue("@CustomerId", listCustomers.SelectedValue);


                    DataTable ordersTable = new DataTable();

                    sqlAdapter.Fill(ordersTable);

                    listOrders.DisplayMemberPath = "orderDate";
                    listOrders.SelectedValuePath = "Id";
                    listOrders.ItemsSource = ordersTable.DefaultView;
                }
            }
            catch
            {


            }


        }

        private void ShowAllOrders()
        {
            try
            {
                //string quer = "SELECT * FROM Orders";
                string quer = "SELECT *,CONCAT(cCustomer,' - ',orderDate) AS allOrders FROM Orders";

                SqlDataAdapter sqlAdapter = new SqlDataAdapter(quer, sqlconn);

                using (sqlAdapter)
                {
                    DataTable allOrdersTable = new DataTable();

                    sqlAdapter.Fill(allOrdersTable);

                    listAllOrders.DisplayMemberPath = "allOrders";
                    listAllOrders.SelectedValuePath = "Id";
                    listAllOrders.ItemsSource = allOrdersTable.DefaultView;
                }
            }
            catch
            {

            }

        }




        private void listCustomers_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ShowOrders();

        }

        private void listAllOrders_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void bCustomerDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Customer deleted");

            string quer = "DELETE FROM Customers WHERE Id=@CustomerId";

            SqlCommand sqlCom = new SqlCommand(quer, sqlconn);

            sqlconn.Open();

            sqlCom.Parameters.AddWithValue("@CustomerId", listCustomers.SelectedValue);

            sqlCom.ExecuteNonQuery();

            sqlconn.Close();

            try
            {
                ShowCustomers();
            }
            catch
            {

            }

        }

        private void bOrderDelete_Click(object sender, RoutedEventArgs e)
        {

            MessageBox.Show("Order deleted");

            string quer = "DELETE FROM Orders WHERE Id=@OrderId";

            SqlCommand sqlCom = new SqlCommand(quer, sqlconn);

            sqlconn.Open();

            sqlCom.Parameters.AddWithValue("@OrderId", listOrders.SelectedValue);

            sqlCom.ExecuteNonQuery();

            sqlconn.Close();

            try
            {
                ShowOrders();
                ShowAllOrders();
            }
            catch
            {

            }
        }



        // UPDATE REDIRECTS TO NEW WINDOW

        private void bCustomerInsert_Click(object sender, RoutedEventArgs e)
        {

            Create create = new Create();

            create.ShowDialog();
            ShowCustomers();


        }

        // UPDATE REDIRECTS TO NEW WINDOW
        private void bCustomerUpdate_Click(object sender, RoutedEventArgs e)
        {
            UpdateWindow updateWindow = new UpdateWindow((int)listCustomers.SelectedValue);


            try
            {

                string quer = "SELECT name,address,city,phone FROM Customers where Id=@SelectedCustomer";

                SqlCommand sqlCom = new SqlCommand(quer, sqlconn);


                SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCom);

                using (sqlAdapter)
                {
                    sqlCom.Parameters.AddWithValue("@SelectedCustomer", listCustomers.SelectedValue);

                    DataTable cusTable = new DataTable();

                    sqlAdapter.Fill(cusTable);

                    updateWindow.txtUpdateName.Text = cusTable.Rows[0]["name"].ToString();
                    updateWindow.txtUpdateAddress.Text = cusTable.Rows[0]["address"].ToString();
                    updateWindow.txtUpdateCity.Text = cusTable.Rows[0]["city"].ToString();
                    updateWindow.txtUpdatePhone.Text = cusTable.Rows[0]["phone"].ToString();
                }
            }
            catch
            {

            }

            updateWindow.ShowDialog();
            ShowCustomers();


        }

    }
}