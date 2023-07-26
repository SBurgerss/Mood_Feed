using HtmlAgilityPack;
using Microsoft.VisualBasic;
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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Reflection;

using Mood_Feed.Properties;
using System.Data.OleDb;
namespace Mood_Feed
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Application Name: Mood Feed
    /// Name: Savannah Brookover
    /// Date: 07/14/2023
    /// Class: Programming Capstone COP 2939-78883
    /// Instructor: Dr. Mubarak Banisakher
    /// Purpose: This Application is catered to the end-user that doesn't feel like browsing through main newsites
    /// to find what they are in the "mood" for. This application offers three different reading "mood" categories:
    /// when you are feeling positive and want positive reads, when you are feeling morbidly curious and want unfiltered
    /// or possibly negative reads, and when you are feeling random and just want something neutral or weird to read.
    /// When the user clicks on the 'mood' representing emoji, the back-end of this application scrapes filtered news
    /// articles and fetches the article hyperlink for user accessing, and the article title. After the specifics 
    /// are fetched, the scraped results are populated into the main listbox for the user to scroll through. The main Window  
    /// features a 'launch' button that will open the user-selected article from the listbox/webscraped results within the
    /// user's default web browser for reading. If the user is not interested in reading at this moment, then 
    /// the program also features a red 'bookmark' button that will save a user selected article link into a local database. Once 
    /// the bookmarked/saved article is sent to the database- the user is able to launch a 'saved articles' window via clicking
    /// on the grey 'safe' icon. This saved articles window will have all the saved article items/database feed availible for the
    /// user to scroll through. This saved article window has a launch button that allows the user to open the selected article
    /// link in their default browser, and also a 'remove article' button that allows the user to delete the selected article 
    /// from the database. If the database is viewed independant of the program running, the articles will remain populated or 
    /// removed if the user specified for them to be deleted.
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public partial class MainWindow : Window
    {
        Window1 savedArticleWindow = new Window1();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;    //initializing context       
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FeelingPositive win = new FeelingPositive();
            win.Show();
            MainLBox.Items.Clear();
            PositiveScrapeData();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FeelingCurious win = new FeelingCurious();
            win.Show();
            MainLBox.Items.Clear();
            NegativeScrapeData();
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            FeelingRandom win = new FeelingRandom();
            win.Show();
            MainLBox.Items.Clear();
            RandomScrapeData();
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            savedArticleWindow.Show();
        }
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill(); //garaunteed to kill the program
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///  Below is the 'save article' bookmark button that will update database and publish items in saved articles window  // 
        ////////////////////////////////////////////////////////////////////////////////////////// ////////////////////////////  
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MainLBox.SelectedItem.ToString().Contains("http"))
                {
                    var ID = DateTime.Now; // variable/data field to use as primary key or first entry in articles table. will be set as the time for user reference
                    var Link = MainLBox.SelectedItem.ToString(); //selected article link that user wants to save for later
                    string newUrl = Link.TrimStart('S', 'y', 's', 't', 'e', 'm', '.', 'W', 'i', 'n', 'd', 'o', 'w', 's', '.', 'C', 'o', 'n', 't', 'r', 'o', 'l', 's', 'L', 'i', 's', 't', 'B', 'o', 'x', 'I', 't', 'e', 'm', ':', ' ');
                    ///// ^crazy programming sin/workaround that will get rid of the builtin object description/ eliminate it from the string that is returned /////
                    OleDbConnection oleDbConnection = new OleDbConnection("Provider = Microsoft.ACE.OLEDB.12.0;" + @"Data Source = C:\Users\Tenmo\source\repos\Mood_Feed\bin\Database1.mdb;" + "User Id=Admin;Password=;");
                    OleDbConnection conn = oleDbConnection;//<^^ connection string //
                    conn.Open(); //opening connection
                    OleDbCommand cmd = new OleDbCommand(); // creating a command or query to interact with database
                    cmd = conn.CreateCommand(); // initializing command
                    cmd.CommandType = System.Data.CommandType.Text; //type of query
                    cmd.CommandText = "insert into Articles values('" + ID + "','" + newUrl + "')"; //selecting fields for inserting new values into the table
                    cmd.ExecuteNonQuery(); //making query run
                    conn.Close(); //closing connection
                    savedArticleWindow.SALbox.Items.Clear();
                    savedArticleWindow.Connector();
                    MessageBox.Show("Article saved.", "success", MessageBoxButton.OK, MessageBoxImage.Hand); //success message
                }
                else
                {
                    MessageBox.Show("You have to select a Link", "Alert!", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
                }
            catch
            {
                MessageBox.Show("Something went wrong...", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning); //fail message
            }
        }
        private void MainLBox_SelectionChanged(object sender, SelectionChangedEventArgs e)//empty/errors will trigger unless it exists
        {
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////  ALL SCRAPERS BEYOND THIS POINT......../////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////
        public void PositiveScrapeData()
        {
            try
            {
                string url = "https://abcnews.go.com/alerts/good-news";
                var web = new HtmlWeb();
                var doc = web.Load(url);
                try
                {
                    foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//*[@class = 'ContentRoll__Headline']//a"))
                    {
                        var Header = link.InnerText;
                        var Link = link.Attributes["href"].Value;//the scraped 

                        Debug.Print($"Title: {Header}"); //check the Output/Debug window for raw results
                        Debug.Print($"\n Link: {Link}"); //check the Output/Debug window for raw results

                        ListBoxItem itm = new ListBoxItem();//making a new custom lbox item
                        itm.Content = Header;//setting the new item's value to the header variable
                        itm.FontStyle = FontStyles.Italic;//custom font style
                        itm.FontWeight = FontWeights.Bold;//custom font weight
                        MainLBox.Items.Add(itm);//adding this Header variable into the listbox for user view
                        ListBoxItem itm2 = new ListBoxItem();//creating 2nd custom lbox item
                        itm2.Content = Link;//setting item value to the scraped hyperlink
                        MainLBox.Items.Add(itm2);//adding item to listbox
                        MainLBox.Items.Add(Environment.NewLine);//adding a line for user friendly formatting
                    }
                }
                catch
                {//error message
                    MessageBox.Show("Uh Oh! We were unable to connect to the news source!", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch
            {//if user has no internet connection(can test this by turning your wifi off)
                MessageBox.Show("No Internet Connection! Please try to connect again", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        public void NegativeScrapeData()
        {
            try
            {
                string url = "https://apnews.com/us-news";//website to scrape from for this instance
                var web = new HtmlWeb();//opening a web instance
                var doc = web.Load(url);//loading the specified url into the web instance(C# API does so for you)
                try
                {    //("//span[@class= 'PagePromoContentIcons-text']");///html/body/div[3]/main/div[1]/div[2]/div[4]/div/div[2]/div[1]/a/span[1]
                    foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@class = 'Link']"))//main ParentNode/Xpath class to scrape from
                    {
                        var Header = link.Attributes["aria-label"].Value;//the inner text of the Link class for the header string
                        var Link = link.Attributes["href"].Value;//the scraped hyperlink

                        Debug.Print($"Title: {Header}");//debug lines for testing
                        Debug.Print($"\n Link: {Link}");//debug lines for testing
                        ListBoxItem itm = new ListBoxItem();//mkaing a new custom lbox item
                        itm.Content = Header;//setting the new item's value to the header variable
                        itm.FontStyle = FontStyles.Italic;//custom font style
                        itm.FontWeight = FontWeights.Bold;//custom font weight
                        MainLBox.Items.Add(itm);//adding this Header variable into the listbox for user view
                        ListBoxItem itm2 = new ListBoxItem();//creating 2nd custom lbox item
                        itm2.Content = Link;//setting item value to the scraped hyperlink
                        MainLBox.Items.Add(itm2);//adding item to listbox
                        MainLBox.Items.Add(Environment.NewLine);//adding a line for user friendly formatting
                    }
                }
                catch
                {//error message
                    MessageBox.Show("Uh Oh! We were unable to connect to the news source!", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch
            {//if user has no internet connection(can test this by turning your wifi off)
                MessageBox.Show("No Internet Connection! Please try to connect again", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        public void RandomScrapeData()
        {
            try
            {
                string url = "https://www.foxnews.com/category/odd-news"; //website to scrape from for this instance
                var web = new HtmlWeb(); //opening a web instance
                var doc = web.Load(url); //loading the specified url into the web instance(C# API does so for you)
                try
                {
                    foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//*[@class = 'm']//a"))//article section node
                    {
                        var Header = link.FirstChild.Attributes["alt"].Value;//xpath element for the article title
                        string Link = link.Attributes["href"].Value; //xpath element for the article hyperlink

                        Debug.Print($"Title: {Header}");//debug lines for testing
                        Debug.Print($"\n Link: {Link}");//debug lines for testing
                        ListBoxItem itm = new ListBoxItem();//mkaing a new custom lbox item
                        itm.Content = Header;//setting the new item's value to the header variable
                        itm.FontStyle = FontStyles.Italic;//custom font style
                        itm.FontWeight = FontWeights.Bold;//custom font weight
                        MainLBox.Items.Add(itm);//adding this Header variable into the listbox for user view
                        ListBoxItem itm2 = new ListBoxItem();//creating 2nd custom lbox item
                        itm2.Content = "https://www.foxnews.com" + Link; //setting item value to the scraped hyperlink but adding this extra link string due to error in parsing.
                        MainLBox.Items.Add(itm2);//adding item to listbox
                        MainLBox.Items.Add(Environment.NewLine);//adding a line for user friendly formatting
                    }
                }
                catch
                {//error message
                    MessageBox.Show("Uh Oh! We were unable to connect to the news source!", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch
            {//if user has no internet connection(can test this by turning your wifi off)
                MessageBox.Show("No Internet Connection! Please try to connect again", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void Button_Click_6(object sender, RoutedEventArgs e)//launch article button
        { /// This button will launch the selected article link(listview item) in the user's default browser for instant reading
            try
            {
                if (MainLBox.SelectedItem.ToString().Contains("http"))
                {
                    string newUrl = MainLBox.SelectedItem.ToString(); //grabbing user selected url
                    Debug.Print($"plain string: {newUrl}");   //debug lines for testing        
                    string newnewUrl = newUrl.TrimStart('S', 'y', 's', 't', 'e', 'm', '.', 'W', 'i', 'n', 'd', 'o', 'w', 's', '.', 'C', 'o', 'n', 't', 'r', 'o', 'l', 's', 'L', 'i', 's', 't', 'B', 'o', 'x', 'I', 't', 'e', 'm', ':', ' ');
                    Uri newUri = new Uri(newnewUrl); //^trimming the built in object descriptors from the string, so only the raw url remains
                    Debug.Print($"trim: {newnewUrl}");  //debug lines for testing
                    Debug.Print($"URI format: {newUri}"); //debug lines for testing
                    System.Diagnostics.Process.Start(newnewUrl); //using built in c# api to launch the url in the user's default browser
                }
                else
                {
                    MessageBox.Show("You have to select a Link", "Alert!", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
            catch
            {//if user has no internet connection(can test this by turning your wifi off)
                MessageBox.Show("No Internet Connection! Please try to connect again", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void ListBoxItem_DoubleClick(object sender, MouseButtonEventArgs e)//empty method/errors occur if doesnt exist
        {

        }
    }
}



















