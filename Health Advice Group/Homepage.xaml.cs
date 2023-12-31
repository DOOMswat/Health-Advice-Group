﻿using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Nodes;
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
    /// Interaction logic for Homepage.xaml
    /// </summary>
    public partial class Homepage : Window
    {
        private System.Windows.Threading.DispatcherTimer timer;
        public Homepage(string username)
        {
            InitializeComponent();
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timerTickyTocky);
            timer.Interval = new TimeSpan(0, 0, 5); // Update every 5 seconds
            timer.Start();
            weatherStartup();
        }
        private void timerTickyTocky(object sender, EventArgs e)
        {
            weatherStartup();
        }
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.Text = string.Empty;
                textBox.GotFocus -= TextBox_GotFocus;
            }
            else if (sender is PasswordBox passwordBox)
            {
                passwordBox.Password = string.Empty;
                passwordBox.GotFocus -= TextBox_GotFocus;
            }
        }//Delete text when textBox clicked.


        private void weatherStartup()
        {
            HttpClient client = new HttpClient();
            var response = client.GetStringAsync(session.request).Result;
            var jsonObject = JsonNode.Parse(response);
            string temp = jsonObject["current"]["temp_c"].ToString();
            string location = jsonObject["location"]["name"].ToString();
            string text = jsonObject["current"]["condition"]["text"].ToString();
            lbl_temp.Content = $"{temp}°C";
            lbl_date.Content = DateTime.Now.ToString("dddd, dd MMM yyyy");
            lbl_text.Content = $"{text}";
            lbl_location.Content = $"Location:\n{location}";
            lbl_welcomeMsg.Content = $"Welcome back, {session.fistName}!";
        }

    }
}
