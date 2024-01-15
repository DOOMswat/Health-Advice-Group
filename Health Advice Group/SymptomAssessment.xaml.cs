using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
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
        private void btn_moreInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListBoxSymptoms.SelectedItem != null)
                {
                    string selectedItem = (ListBoxSymptoms.SelectedItem as CheckBox).Content.ToString();
                    if (!string.IsNullOrEmpty(selectedItem))
                    {
                        MySqlConnection conn = new MySqlConnection(session.connStr);
                        conn.Open();
                        string query = $"SELECT description FROM healthconditions WHERE conditionName = '{selectedItem}'";
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            using (MySqlDataReader rdr = cmd.ExecuteReader())
                            {
                                if (rdr.Read())
                                {string desc = rdr.GetString(0);txt_information.Text = desc;}
                            }
                        }
                        conn.Close();
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

        private void ListBoxSymptoms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (ListBoxSymptoms.SelectedItem != null)
                {

                    string selectedItem = (ListBoxSymptoms.SelectedItem as CheckBox).Content.ToString();
                    if (!string.IsNullOrEmpty(selectedItem))
                    {
                        MySqlConnection conn = new MySqlConnection(session.connStr);
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
                        }conn.Close();
                    }else {MessageBox.Show("Unable to retrieve the selected symptom.");}
                }else{ MessageBox.Show("Please select a symptom."); }
            }catch (Exception ex){MessageBox.Show($"An error occurred: {ex.Message}");}
        }

        private void btn_next_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListBoxSymptoms.SelectedItems.Count > 0)
                {
                    foreach (CheckBox checkBox in ListBoxSymptoms.SelectedItems)
                    {
                        session.selectedSymptoms.Add(checkBox.Content.ToString());
                    }
                    if (myTabControl.SelectedIndex < myTabControl.Items.Count - 1){ myTabControl.SelectedIndex++; }
                }else{MessageBox.Show("Please select at least one symptom.");}
            }catch (Exception ex){MessageBox.Show($"An error occurred: {ex.Message}");}
        }
        private void confirm()
        {
            try
            {
                listBox_Symptoms.Items.Clear();
                foreach (string symptom in session.selectedSymptoms)
                {
                    listBox_Symptoms.Items.Add(symptom);
                }
                txt_UserDetails.Text = $"Username: {session.username}\n" +
                    $"First Name: {session.fistName}\n" +
                    $"Last name: {session.LastName}\n" +
                    $"Email Address: {session.Email}\n" +
                    $"Postcode/Zipcode: {session.postCode}\n" +
                    $"Weight: {session.weight}kg\n" +
                    $"Height: {session.height}cm";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void btn_nextPI_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txt_homeAddress.Text) || string.IsNullOrWhiteSpace(txt_weight.Text) || string.IsNullOrWhiteSpace(txt_height.Text))
                {
                    MessageBox.Show("Please fill in all the boxes.");
                }
                else
                {
                    confirm();
                    session.postCode = txt_homeAddress.Text;
                    session.weight = txt_weight.Text;
                    session.height = txt_height.Text;
                    if (myTabControl.SelectedIndex < myTabControl.Items.Count - 1){ myTabControl.SelectedIndex++; }
                }
            }catch (Exception ex) { MessageBox.Show($"An error occurred: {ex.Message}");}
        }

        private void btn_confirm_click(object sender, RoutedEventArgs e)
        {

        }
    }
}
 