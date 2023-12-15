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
            try
            {
                MySqlConnection conn;
                using (conn = new MySqlConnection(session.connStr))
                {
                    conn.Open();
                    string query = "SELECT * FROM Customer WHERE username =" +
                        " @Username AND Password = SHA(@Password)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Username", txtBox_Username.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@Password", pasBox_Password.Password);

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            getUserID();
                            Homepage homepage = new Homepage(txtBox_Username.Text.ToUpper());
                            homepage.Show();
                            this.Close();
                        }
                        else
                        {MessageBox.Show("Invalid username or password");}
                    }
                }

            }
            catch (Exception ex) {MessageBox.Show("Error: " + ex);}
        }

        private void getUserID()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(session.connStr))
                {
                    conn.Open();
                    string query = "SELECT userID FROM Customer WHERE username = @Username";
                    using(MySqlCommand cmd = new MySqlCommand( query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", txtBox_Username.Text.ToUpper());
                        
                        using (MySqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                string userID = rdr["userID"].ToString();
                                session.userID = userID;

                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }

        }


        private void btn_Register_Click(object sender, RoutedEventArgs e)
        {
            registerWindow registerWindow = new registerWindow();
            registerWindow.Show();
            this.Close();
        }
    }
}
