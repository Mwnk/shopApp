using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace shopAppWpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
		private SqlConnection sqlConnection = null;
		public MainWindow()
		{
			InitializeComponent();
			sqlConnection = new SqlConnection(
				ConfigurationManager.ConnectionStrings["database"].ConnectionString);
			sqlConnection.Open();

			RefreshData();
		}
		public class DataProduct
		{
			public int IdProduct { get; set; }
			public string NameProduct { get; set; }
			public string PriceProduct { get; set; }
			public string AmountProduct { get; set; }
			public string CategoryProduct { get; set; }
		}
		public void RefreshData()
        {
			Data.Items.Clear();
			product.Text = "";
			category.Text = "";
			price.Text = "";
			amount.Text = "";

			SqlDataReader dataReader = null;
			try
			{
				SqlCommand command = new SqlCommand(
					$"SELECT Id, Name, Category, Price, Amount FROM Products",
					sqlConnection);
				dataReader = command.ExecuteReader();

				while (dataReader.Read())
				{
					DataProduct tempData = new DataProduct();
					tempData.IdProduct = Convert.ToInt32(dataReader["Id"]);
					tempData.NameProduct = Convert.ToString(dataReader["Name"]);
					tempData.CategoryProduct = Convert.ToString(dataReader["Category"]);
					tempData.PriceProduct = $"{dataReader["Price"]:N2} ₽";
					tempData.AmountProduct = $"{dataReader["Amount"]} шт.";
					Data.Items.Add(tempData);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			finally
			{
				if (dataReader != null && !dataReader.IsClosed)
				{
					dataReader.Close();
				}
			}
		}

        private void Button_AddProduct(object sender, RoutedEventArgs e)
        {
			if (product.Text == "" || price.Text == "" || amount.Text == "" || category.Text == "")
            {
				MessageBox.Show("Поле не может быть пустым!");
				return;
            }

			SqlCommand command = new SqlCommand(
				"INSERT INTO [Products] (Name, Category, Price, Amount) " +
				$"VALUES (N'{product.Text}', N'{category.Text}', N'{price.Text}', N'{amount.Text}')",
				sqlConnection);
			command.ExecuteNonQuery();

			MessageBox.Show("Товар успешно добавлен в базу!");
			RefreshData();
		}

        private void Button_Deliveries(object sender, RoutedEventArgs e)
        {
			Deliveries form = new Deliveries();
			form.Show();
			form.RefreshData();
		}

        private void Button_Statistics(object sender, RoutedEventArgs e)
        {
			Statistics form = new Statistics();
			form.Show();
			form.RefreshData();
		}
    }
}
