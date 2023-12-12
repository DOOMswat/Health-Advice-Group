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
using MySql.Data.MySqlClient;
namespace Health_Advice_Group
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection conn;
            string username = txtBox_Username.Text;
            string password = pasBox_Password.Password;
            using (conn = new MySqlConnection(session.connStr))
            {
                conn.Open();
                string query = "SELECT * FROM Customers where username =" +
                    " @Username AND Password = SHA('@Password')";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Homepage homepage = new Homepage();
                        homepage.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password");
                    }
                }


            }

        }

        private void btn_Register_Click(object sender, RoutedEventArgs e)
        {
            //Implement register window.
        }
    }
}
