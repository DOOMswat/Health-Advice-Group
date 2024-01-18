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
        private System.Windows.Threading.DispatcherTimer timer;
        public SymptomAssessment()
        {
            InitializeComponent();
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timerTickyTocky);
            timer.Interval = new TimeSpan(0, 0, 2); // Update every 2+ seconds cuz not work without tick for some reason (almost realtime)
            timer.Start();
            confirm();

            foreach (var symptomCheckBox in ListBoxSymptoms.Items.OfType<CheckBox>())
            {
                symptomCheckBox.Checked += SymptomCheckBox_Checked;
                symptomCheckBox.Unchecked += SymptomCheckBox_Unchecked;
            }
        }
    
        private void timerTickyTocky(object sender, EventArgs e)
        {
            confirm();
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
            }catch (Exception ex){}
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
                    session.postCode = txt_homeAddress.Text;
                    session.weight = txt_weight.Text;
                    session.height = txt_height.Text;
                    if (myTabControl.SelectedIndex < myTabControl.Items.Count - 1){ myTabControl.SelectedIndex++; }
                }
            }catch (Exception ex) { MessageBox.Show($"An error occurred: {ex.Message}");}
        }

        private void btn_confirm_click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(session.connStr))
                {
                    conn.Open();
                    string updateWeightAndHeight = $"UPDATE Customer SET weight = '{session.weight}', height = '{session.height}', location = '{session.postCode}' WHERE userID = '{session.userID}'";
                    session.request = $"http://api.weatherapi.com/v1/forecast.json?key=daf5fcbcd2384c6eb9791539232311&q={session.postCode}&days=5&aqi=yes&alerts=no";
                    using (MySqlCommand cmd = new MySqlCommand(updateWeightAndHeight, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    string getLastCustomerIDQuery = $"SELECT userID FROM Customer WHERE username = '{session.username}'";
                    int customerID;
                    using (MySqlCommand cmd1 = new MySqlCommand(getLastCustomerIDQuery, conn))
                    {
                        customerID = Convert.ToInt32(cmd1.ExecuteScalar());
                    }
                    session.userID = customerID;



                    foreach (string symptom in session.selectedSymptoms)
                    {

                        string getConditionIDQuery = $"SELECT conditionID FROM healthconditions WHERE conditionName = '{symptom}'";
                        int conditionID;
                        using (MySqlCommand cmd = new MySqlCommand(getConditionIDQuery, conn)){conditionID = Convert.ToInt32(cmd.ExecuteScalar()); }
                        string insertCustomerConditionsQuery = $"INSERT INTO Customerconditions (conditionID, customerID) VALUES ({conditionID}, {session.userID})";
                        using (MySqlCommand cmd = new MySqlCommand(insertCustomerConditionsQuery, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                    conn.Close();
                    MessageBox.Show("Information added to the database successfully!");
                    Homepage homepage = new Homepage(session.username);
                    homepage.Show();
                    this.Close();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }


        private void SymptomCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                // Add the checked symptom to the list
                string symptom = checkBox.Content.ToString();
                if (!session.selectedSymptoms.Contains(symptom))
                {
                    session.selectedSymptoms.Add(symptom);
                }
            }
        }

        private void SymptomCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                // Remove the unchecked symptom from the list
                string symptom = checkBox.Content.ToString();
                session.selectedSymptoms.Remove(symptom);
            }
        }
    }
}





