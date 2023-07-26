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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.OleDb;
using System.Collections.ObjectModel;
using System.Windows.Navigation;


using System.Diagnostics;
using System.Xml.Linq;
using System.Runtime.Remoting.Messaging;
using System.Dynamic;
using System.Security.Policy;

namespace Mood_Feed
{
 /////////////////////////////////////////////////////////////////////////////////
///////////  Back-End for Saved Articles Window  ////////////////////////////////
////////////////////////////////////////////////////////////////////////////////  
    public partial class Window1 : Window
    {      
        public Window1()
        {
            InitializeComponent();
            Connector(); //connecting to the database to populate the listbox upon the saved articles window opening
        }
        public void Connector() //custom method for connecting to database and updating the listbox
        {
            try
            {
                OleDbConnection oleDbConnection = new OleDbConnection("Provider = Microsoft.ACE.OLEDB.12.0;" + @"Data Source = C:\Users\Tenmo\source\repos\Mood_Feed\bin\Database1.mdb;" + "User Id=Admin;Password=;");
                OleDbConnection conn = oleDbConnection; //^connection string
                conn.Open(); //opening a stream to the database
                OleDbCommand comd = conn.CreateCommand(); //custom database query/command
                comd.CommandText = "Select * from Articles"; //query that selects all objects from Articles Table
                comd.ExecuteNonQuery(); //executing/launching that query statement
                OleDbDataReader reader = comd.ExecuteReader(); //opening the reader while every object is selected
                while (reader.Read()) //while it scans through the table
                {
                    SALbox.Items.Add(reader["Id"].ToString()); //grabbing the primary key, the id(date time value) value
                    SALbox.Items.Add(reader["Link"].ToString()); //grabbing the hyperlink, link value from the table                
                }
                conn.Close();//closing the connection
            }
            catch
            {//error message
                MessageBox.Show("unable to connect to database", "alert", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            }
        private int GetIndex()//custom method for grabbing the index value of the currently selected list box item
        {
            return SALbox.SelectedIndex;    
        }       
        private void Button_Click(object sender, RoutedEventArgs e) //the remove article button
        {
            try
            {
                if (SALbox.SelectedItem.ToString().Contains("http"))
                {
                    OleDbConnection oleDbConnection = new OleDbConnection("Provider = Microsoft.ACE.OLEDB.12.0;" + @"Data Source = C:\Users\Tenmo\source\repos\Mood_Feed\bin\Database1.mdb;" + "User Id=Admin;Password=;");
                    OleDbConnection conn = oleDbConnection;//^connection string 
                    string salItem = SALbox.SelectedItem.ToString(); //grabbing the selected listbox item and putting its string value into a variable
                    string newsalItem = salItem.TrimStart('S', 'y', 's', 't', 'e', 'm', '.', 'W', 'i', 'n', 'd', 'o', 'w', 's', '.', 'C', 'o', 'n', 't', 'r', 'o', 'l', 's', 'L', 'i', 's', 't', 'B', 'o', 'x', 'I', 't', 'e', 'm', ':', ' ');
                    int DateIndex = (GetIndex() - 1);// ^programming sin to remove the built in control descriptors from sting so only the raw hyperlink remains
                                                     //^unable to grab the value of two separately selected items- so the single link value will be subtracted by 1 in index to now grab the date time list item separately
                    var newsalItemDate1 = SALbox.Items.GetItemAt(DateIndex); //grabbing the variable/field from the date-time index correlating with the selected article link item
                    string newsalItemDate2 = newsalItemDate1.ToString(); //converting that grabbed field into a string and then trimming it in the next line for final formatting
                    string newsalItemDate3 = newsalItemDate2.TrimStart('S', 'y', 's', 't', 'e', 'm', '.', 'W', 'i', 'n', 'd', 'o', 'w', 's', '.', 'C', 'o', 'n', 't', 'r', 'o', 'l', 's', 'L', 'i', 's', 't', 'B', 'o', 'x', 'I', 't', 'e', 'm', ':', ' ');
                    Debug.Print($"New format:{newsalItemDate3}");//printing variable on debug console for testing                      
                    OleDbCommand deletecmd = conn.CreateCommand(); //new command/query
                    deletecmd.CommandText = "DELETE FROM Articles WHERE Id=@Link";//creating a custom command/query for deleting the hyperlink/link field
                    deletecmd.Parameters.AddWithValue("@Link", newsalItem); //having the query search for the specific value of the variable, and if they match a table field, delete the table entry                 
                    OleDbCommand deletecmdDate = conn.CreateCommand(); //creating a custom command/query for deleting the date/time/primary key ID field
                    deletecmdDate.CommandText = "DELETE FROM Articles WHERE Id=@Id";//having the query search for the specific value of the variable, and if they match a table field, delete the table entry 
                    deletecmdDate.Parameters.AddWithValue("@Id", newsalItemDate3); //using the grabbed/newly formatted date/time string to compare to database fields          
                    conn.Open();//opening the connection for executing the following commands
                    deletecmd.ExecuteNonQuery();//deleting the link/hyperlink field from the database table
                    deletecmdDate.ExecuteNonQuery();//deleting the date time/primary key id field from database table
                    conn.Close(); //closing the connection
                    SALbox.Items.RemoveAt(GetIndex()); //deleting the link listbox item via index
                    SALbox.Items.RemoveAt(DateIndex); //deleting the datetime string item from the listbox                 
                    MessageBox.Show("Article deleted.", "success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("You have to select a Link", "Alert!", MessageBoxButton.OK, MessageBoxImage.Stop);

                }               
            }
            catch
            {
                MessageBox.Show("unable to connect to database", "alert", MessageBoxButton.OK, MessageBoxImage.Error);
            }           
        }
        private void Button_Click_2(object sender, RoutedEventArgs e) /// the launch article button for instant user reading!
        {
            try
            {
                if (SALbox.SelectedItem.ToString().Contains("http"))
                {
                    string newUrl = SALbox.SelectedItem.ToString(); //grabbing user selected link and placing it into a string
                    Debug.Print($"plain string: {newUrl}"); //printing on debug console for testing the value or string
                    string newnewUrl = newUrl.TrimStart('S', 'y', 's', 't', 'e', 'm', '.', 'W', 'i', 'n', 'd', 'o', 'w', 's', '.', 'C', 'o', 'n', 't', 'r', 'o', 'l', 's', 'L', 'i', 's', 't', 'B', 'o', 'x', 'I', 't', 'e', 'm', ':', ' ');
                    Uri newUri = new Uri(newnewUrl); //creating a URI, not "URL" just incase(suppports separate window browser controls)
                    Debug.Print($"trim: {newnewUrl}");//printing on debug console for testing the value or string
                    Debug.Print($"URI format: {newUri}");//printing on debug console for testing the value or string
                    System.Diagnostics.Process.Start(newnewUrl); //using built in c# api to launch the url in the user's default browser
                }
                else
                {
                    //if user does not select a link field of the listbox items
                MessageBox.Show("You have to select a Link", "Alert!", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
            catch
            {//if user has no internet connection(can test this by turning your wifi off)
                MessageBox.Show("No Internet Connection! Please try to connect again", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void SALbox_SelectionChanged(object sender, SelectionChangedEventArgs e)//empty, needed or errors will ocurr?
        {
        }
    }
}
