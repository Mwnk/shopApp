using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Windows.Shapes;

namespace shopAppWpf
{
    /// <summary>
    /// Логика взаимодействия для Deliveries.xaml
    /// </summary>
    public partial class Deliveries : Window
    {
		private SqlConnection sqlConnection = null;
		public class DataDelivery
        {
            public int IdDelivery { get; set; }
            public string DateDelivery { get; set; }
            public string AmountDelivery { get; set; }
        }
        public Deliveries()
        {
            InitializeComponent();
			sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["database"].ConnectionString);
			sqlConnection.Open();
		}

        public void RefreshData()
        {
            Data.Items.Clear();
            datedelivery.Text = "";
            amountdelivery.Text = "";

			SqlDataReader dataReader = null;
			try
			{
				SqlCommand command = new SqlCommand(
					$"SELECT Id, date, amount FROM Deliveries",
					sqlConnection);
				dataReader = command.ExecuteReader();

				while (dataReader.Read())
				{
					DataDelivery tempData = new DataDelivery();
					tempData.IdDelivery = Convert.ToInt32(dataReader["Id"]);
					tempData.DateDelivery = Convert.ToString(dataReader["date"]);
					tempData.AmountDelivery = Convert.ToString(dataReader["amount"]);
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

        private void Button_Add(object sender, RoutedEventArgs e)
        {
			if (datedelivery.Text == "" || amountdelivery.Text == "")
			{
				MessageBox.Show("Поле не может быть пустым");
				return;
			}
			SqlCommand command = new SqlCommand(
				"INSERT INTO [Deliveries] (date, amount) " +
				$"VALUES (N'{datedelivery.Text}', N'{amountdelivery.Text}')",
				sqlConnection);
			command.ExecuteNonQuery();

			MessageBox.Show("Успешно добавлено в базу!");
			RefreshData();
		}
    }
}
