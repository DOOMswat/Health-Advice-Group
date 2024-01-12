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
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
namespace Health_Advice_Group
{
    /// <summary>
    /// Interaction logic for SymptomAssessment.xaml
    /// </summary>
    public partial class SymptomAssessment : Window
    {
        public SymptomAssessment()
        {
            InitializeComponent();
        }

        //        private void btn_moreInfo_Click(object sender, RoutedEventArgs e)
        //        {
        //            try
        //            {
        //                using (MySqlConnection conn = new MySqlConnection(session.connStr))
        //                {
        //                    string selectedItem = ListBoxSymptoms.SelectedItem.ToString();
        //                    conn.Open();
        //                    string query = $"SELECT description FROM healthconditions Where conditionName = '{selectedItem}'";
        //                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
        //                    {
        //                        using (MySqlDataReader rdr = cmd.ExecuteReader())
        //                        {
        //                            if (rdr.Read())
        //                            {
        //                                string desc = rdr.GetString(0);
        //                                txt_information.Text = desc;
        //                            }
        //                        }
        //                    }

        //                }
        //            }
        //            catch { MessageBox.Show("Error"); }
        //        }   
        //    }
        //}



        private void btn_moreInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListBoxSymptoms.SelectedItem != null)
                {
                    string selectedItem = (ListBoxSymptoms.SelectedItem as CheckBox).Content.ToString();

                    if (!string.IsNullOrEmpty(selectedItem))
                    {
                        using (MySqlConnection conn = new MySqlConnection(session.connStr))
                        {
                            conn.Open();
                            string query = $"SELECT description FROM healthconditions WHERE conditionName = '{selectedItem}'";
                            using (MySqlCommand cmd = new MySqlCommand(query, conn))
                            {
                                using (MySqlDataReader rdr = cmd.ExecuteReader())
                                {
                                    if (rdr.Read())
                                    {
                                        string desc = rdr.GetString(0);
                                        txt_information.Text = desc;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Unable to retrieve the selected symptom.");
                    }
                }
                else
                {
                    MessageBox.Show("Please select a symptom.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

    }
}

