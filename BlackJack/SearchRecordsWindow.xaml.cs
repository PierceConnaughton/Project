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
using System.IO;
using System.Data.Entity;
using MaterialDesignThemes.Wpf;

namespace BlackJack
{
    /// <summary>
    /// Interaction logic for SearchRecordsWindow.xaml
    /// </summary>
    public partial class SearchRecordsWindow : Window
    {
        List<Player> allPlayers = new List<Player>();
        PlayerData db = new PlayerData();
        bool found;

        public SearchRecordsWindow()
        {
            InitializeComponent();
        }

        //when button search clicked
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            #region comments
            //H:\Year Two\Semester 4\Programming\Project\Project\PlayerRecords.txt
            //D:\College\Programming\BlackJack-master\Project\PlayerRecords.txt

            //when the search window is loaded
            //FileStream fs = new FileStream(@"D:\College\Programming\BlackJack-master\Project\PlayerRecords.txt", FileMode.Open, FileAccess.Read);

            //StreamReader sr = new StreamReader(fs);


            //string name = TxtBxSearchName.Text;
            //string[] lines = new string[100];
            //bool found = false;
            //string recordInfo = "Error";
            //string result = "Unknown";
            //string searchName = string.Format(" {0,-15} Wins", name);

            //string lineIn = sr.ReadLine();
            //string[] fieldArray1 = new string[5];

            //if (name == "")
            //{
            //    MessageBox.Show("Please enter name first");
            //    return;
            //}

            //for (int i = 0; i < 100; i++)
            //{
            //    lines[i] = lineIn;
            //    lineIn = sr.ReadLine();
            //}

            //for (int i = 0; i < lines.Length; i++)
            //{
            //    fieldArray1 = lines[i].Split(':');
            //    string playerName = string.Format(fieldArray1[1]);
            //    if (fieldArray1[1] == searchName)
            //    {
            //        result = string.Format(lines[i].ToString());

            //        MessageBox.Show(result);
            //        found = true;
            //        break;
            //    }



            //}
            //if (found == false)
            //{
            //    result = "Player not found";
            //    MessageBox.Show(result);


            //}
            //found = false;


            //sr.Close();
            #endregion comments

            //if searchname text is null give an error
            if (TxtBxSearchName.Text == "")
            {
                
                
                txtBlNameFound.Text = "Enter name too search";
                DialogHost.IsOpen = true;
            }
            else
            {
                //search for name in the database that is the same as the name you are searching for
                found = false;
                string searchName = TxtBxSearchName.Text;
                
                
                    var query = from p in db.players
                                where p.PlayerName == searchName
                                select new
                                {
                                    PlayerName = p.PlayerName,
                                    Wins = p.Wins,
                                    Losses = p.Losses,
                                    Draws = p.Draws,
                                    LastTimePlayed = p.DateOfLastGame
                                };

               var x  = query.AsQueryable().FirstOrDefault(name => name.PlayerName == searchName);
                //if the name is nout found
                if (x == null)
                {
                    txtBlNameFound.Text = "Name was not found";
                    DialogHost.IsOpen = true;
                }
                
                else 
                {
                    //display all uselful data for the player
                    string y = string.Format("Player Name: {0}, Wins: {1}, Draws: {2}, Losses: {3} ,Date Of Last Game: {4}", x.PlayerName, x.Wins, x.Draws, x.Losses, x.LastTimePlayed);
                    txtBlNameFound.Text = y;
                    DialogHost.IsOpen = true;
                }



                #region comments
                //foreach (Player player in allPlayers)
                //{
                //    MessageBox.Show(player.PlayerName);
                //    if (player.PlayerName == searchName)
                //    {
                //        MessageBox.Show(player.PlayerName);

                //        using (db)
                //        {
                //            var query = from p in db.players

                //                        select p.PlayerName.ToList();

                //            MessageBox.Show(query.ToString());
                //        }
                //        found = true;
                //        return;
                //    }

                //}
                //if (found == false)
                //{
                //    MessageBox.Show("Name was not found");
                //}
                #endregion comments

                found = false;
                
                
                
            }
            


        }

        private void SearchRecordsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            

        }

        //when home button clicked
        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            //if home clicked show the main window and close the current window
            MainWindow Window1 = new MainWindow();
            Window1.Show();

            SearchRecordsWindow Window2 = new SearchRecordsWindow();
            Window2.Close();
        }

        //when search records window loaded
        private void SearchRecordsWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            //when page is loaded show the database method
            saveToDatabase();
            
                //allPlayers = db.players.ToList(); 
        }

        //load the database too the page
        public void saveToDatabase()
        {
            //get all players from the page
                var query = from p in db.players
                            orderby p.Wins descending
                            select new
                            {
                                PlayerName = p.PlayerName,
                                Wins = p.Wins,
                                Losses = p.Losses,
                                Draws = p.Draws,
                                LastTimePlayed = p.DateOfLastGame
                            };

               //display them in a list
                lstBxRecords.ItemsSource = query.ToList();
               
            
        }

        private void Show_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void ShowDialog_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void Close_ME_Click(object sender, RoutedEventArgs e)
        {
            DialogHost.IsOpen = false;
        }
    }
}
