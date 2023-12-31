﻿using System;
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
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
namespace Health_Advice_Group
{
    /// <summary>
    /// Interaction logic for registerWindow.xaml
    /// </summary>
    public partial class registerWindow : Window
    {

        public registerWindow()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string password = txt_Password.Password;
            string conpass = txt_ConfirmPassword.Password;
            if (password == conpass)
            {
                using (MySqlConnection conn = new MySqlConnection(session.connStr))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM customer WHERE username = @userName";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {//used to check if the username exists in the database or not
                        cmd.Parameters.AddWithValue("@userName", txt_Username.Text.ToUpper());
                        int userCount = Convert.ToInt32(cmd.ExecuteScalar());
                        if (userCount > 0){MessageBox.Show("Username already exists.");return;}
                    }

                    string insertCustomerQuery = "INSERT INTO customer (username, " +
                        "firstName, lastName, email, password) " +
                        "VALUES(@userName,@firstName,@lastName,@emailAddress,SHA(@passWord));";
                    MySqlCommand insertCustomerCommand = new MySqlCommand(@insertCustomerQuery, conn);
                    insertCustomerCommand.Parameters.AddWithValue("@userName", txt_Username.Text.ToUpper());
                    insertCustomerCommand.Parameters.AddWithValue("@firstName",txt_Forename.Text);
                    insertCustomerCommand.Parameters.AddWithValue("@lastName",txt_Surname.Text);
                    insertCustomerCommand.Parameters.AddWithValue("@emailAddress", txt_Email.Text);
                    insertCustomerCommand.Parameters.AddWithValue("@passWord", txt_ConfirmPassword.Password);
                    insertCustomerCommand.ExecuteNonQuery();
                    MessageBox.Show("Account sucessfully created");
                    LoginWindow loginWindow = new LoginWindow();
                    loginWindow.Show();
                    this.Close();
                }


            }
            else
            {
                MessageBox.Show("Password does not match.");
            }
        }

        private void LOGINBUTTON(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
