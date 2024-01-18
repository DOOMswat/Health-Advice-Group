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
                    string query = "SELECT * FROM Customer WHERE username = @Username AND Password = SHA(@Password)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Username", txtBox_Username.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@Password", pasBox_Password.Password);

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            getUserDetails();

                            double weight;
                            if (double.TryParse(session.weight, out weight) && weight > 0)
                            {
                                session.request = $"http://api.weatherapi.com/v1/forecast.json?key=daf5fcbcd2384c6eb9791539232311&q={session.postCode}&days=5&aqi=no&alerts=no";
                                Homepage homepage = new Homepage(txtBox_Username.Text.ToUpper());
                                homepage.Show();
                            }
                            else
                            {
                                SymptomAssessment symptomAssessment = new SymptomAssessment();
                                symptomAssessment.Show();
                            }

                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Invalid username or password");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }


        private void getUserDetails()//gets the username from the textBox, inserts the read data into session class.
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(session.connStr))
                {
                    conn.Open();
                    string query = "SELECT firstName, lastName, email, weight, location FROM Customer WHERE username = @Username";
                    using(MySqlCommand cmd = new MySqlCommand( query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", txtBox_Username.Text.ToUpper());
                        
                        using (MySqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                string firstName = rdr["firstName"].ToString();
                                string lastName = rdr["lastName"].ToString() ;
                                session.username = txtBox_Username.Text;
                                session.fistName = firstName;
                                session.LastName = lastName;
                                session.weight = rdr["weight"].ToString();
                                session.Email = rdr["email"].ToString();
                                session.postCode = rdr["location"].ToString();
                            }
                        }
                        string getLastCustomerIDQuery = $"SELECT userID FROM Customer WHERE username = '{session.username}'";
                        int customerID;
                        using (MySqlCommand cmd1 = new MySqlCommand(getLastCustomerIDQuery, conn))
                        {
                            customerID = Convert.ToInt32(cmd1.ExecuteScalar());
                        }
                        session.userID = customerID;


                    }
                }
            }
            catch (Exception ex){MessageBox.Show("Error: " + ex);}

        }


        private void btn_Register_Click(object sender, RoutedEventArgs e)
        {
            registerWindow registerWindow = new registerWindow();
            registerWindow.Show();
            this.Close();
        }
    }
}
