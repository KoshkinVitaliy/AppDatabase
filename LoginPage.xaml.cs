using Npgsql;
using System;
using System.Collections.Generic;
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

namespace AppDatabase
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private string sql = "Server=localhost; Port=5432; Database=wpf_db; User Id = postgres; Password=admin";
        public LoginPage()
        {
            InitializeComponent();
        }

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            string login = LoginTextBox.Text;
            string password = PasswordBox.Password;

            CheckData(login, password);
        }






        private void CheckData(string login, string password)
        {

            NpgsqlConnection sqlConnection = new NpgsqlConnection(sql);
            sqlConnection.Open();
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = sqlConnection;
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = "SELECT * FROM public.users";
            NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                User user = new User();
                user.Login = reader.GetString(1);
                user.Password = reader.GetString(2);
                
                if(user.Login.Equals(login) && user.Password.Equals(password))
                {
                    user.Id = reader.GetInt32(0);
                    user.Name = reader.GetString(3);
                    user.Surname = reader.GetString(4);
                    NavigationService.Navigate(new MainPage(user));
                }
                else
                {
                    MessageBox.Show("Incorrect Data. Please, try again");
                    break;
                }

            }

            command.Dispose();
            sqlConnection.Close();
        }
    }
}
