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
    /// Логика взаимодействия для Statistics.xaml
    /// </summary>
    public partial class Statistics : Window
    {
        private SqlConnection sqlConnection = null;
        public class DataStatistics
        {
            public int IdStatistics { get; set; }
            public string DateStatistics { get; set; }
            public string ExpenditureStatistics { get; set; }
            public string ProfitStatistics { get; set; }
        }
        public Statistics()
        {
            InitializeComponent();
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["database"].ConnectionString);
            sqlConnection.Open();
        }
		public void RefreshData()
		{
			Data.Items.Clear();
			date.Text = "";
			expenditure.Text = "";
			profit.Text = "";

			SqlDataReader dataReader = null;
			try
			{
				SqlCommand command = new SqlCommand(
					$"SELECT Id, Date, Expenditure, Profit FROM [Statistics]",
					sqlConnection);
				dataReader = command.ExecuteReader();

				while (dataReader.Read())
				{
					DataStatistics tempData = new DataStatistics();
					tempData.IdStatistics = Convert.ToInt32(dataReader["Id"]);
					tempData.DateStatistics = Convert.ToString(dataReader["Date"]);
					tempData.ExpenditureStatistics = $"{dataReader["Expenditure"]:N2} ₽";
					tempData.ProfitStatistics = $"{dataReader["Profit"]:N2} ₽";
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
			if (date.Text == "" || expenditure.Text == "" || profit.Text == "")
			{
				MessageBox.Show("Поле не может быть пустым");
				return;
			}
			SqlCommand command = new SqlCommand(
				"INSERT INTO [Statistics] (Date, Expenditure, Profit) " +
				$"VALUES (N'{date.Text}', N'{expenditure.Text}', N'{profit.Text}')",
				sqlConnection);
			command.ExecuteNonQuery();

			MessageBox.Show("Успешно добавлено в базу!");
			RefreshData();
		}
    }
}
