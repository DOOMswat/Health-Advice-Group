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
            try//validation
            {
                MySqlConnection conn;
                using (conn = new MySqlConnection(session.connStr))
                {
                    conn.Open();
                    string query = "SELECT * FROM Customer WHERE username =" +//search username and password from enterered credintials
                        " @Username AND Password = SHA(@Password)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Username", txtBox_Username.Text.ToUpper());//all usernames must be all uppercase.
                    cmd.Parameters.AddWithValue("@Password", pasBox_Password.Password);//Paramaters = anti-sqlInjection and stuff

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read()) 
                        {//if credential are found and read go through if statement.
                            getUserDetails();//runs sub-routine
                            Homepage homepage = new Homepage(txtBox_Username.Text.ToUpper());//after read corerctly, closes this window and opens homepage.
                            SymptomAssessment symptomAssessment = new SymptomAssessment();
                            symptomAssessment.Show();
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
            catch (Exception ex) {MessageBox.Show("Error: " + ex);}
        }

        private void getUserDetails()//gets the username from the textBox, inserts the read data into session class.
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(session.connStr))
                {
                    conn.Open();
                    string query = "SELECT userID, firstName, lastName, email FROM Customer WHERE username = @Username";
                    using(MySqlCommand cmd = new MySqlCommand( query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", txtBox_Username.Text.ToUpper());
                        
                        using (MySqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                string userID = rdr["userID"].ToString();
                                string firstName = rdr["firstName"].ToString();
                                string lastName = rdr["lastName"].ToString() ;
                                session.userID = userID;
                                session.fistName = firstName;
                                session.LastName = lastName;
                            }
                        }

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
