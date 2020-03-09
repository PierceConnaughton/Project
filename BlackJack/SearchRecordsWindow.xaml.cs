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

namespace BlackJack
{
    /// <summary>
    /// Interaction logic for SearchRecordsWindow.xaml
    /// </summary>
    public partial class SearchRecordsWindow : Window
    {
        List<string> allPlayers = new List<string>();

        public SearchRecordsWindow()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

            //H:\Year Two\Semester 4\Programming\Project\Project\PlayerRecords.txt
            //D:\College\Programming\BlackJack-master\Project\PlayerRecords.txt

            //when the search window is loaded
            FileStream fs = new FileStream(@"H:\Year Two\Semester 4\Programming\Project\Project\PlayerRecords.txt", FileMode.Open, FileAccess.Read);

            StreamReader sr = new StreamReader(fs);

            
            string name = TxtBxSearchName.Text;
            string[] lines = new string[100];
            bool found = false;
            string recordInfo = "Error";
            string result = "Unknown";
            string searchName = string.Format(" {0,-15} Wins", name);

            string lineIn = sr.ReadLine();
            string[] fieldArray1 = new string[5];

            if (name == "")
            {
                MessageBox.Show("Please enter name first");
                return;
            }

            for (int i = 0; i < 100; i++)
            {
                lines[i] = lineIn;
                lineIn = sr.ReadLine();
            }

            for (int i = 0; i < lines.Length; i++)
            {
                fieldArray1 = lines[i].Split(':');
                string playerName = string.Format(fieldArray1[1]);
                if (fieldArray1[1] == searchName)
                {
                    result = string.Format(lines[i].ToString());

                    MessageBox.Show(result);
                    found = true;
                    break;
                }
                
                

            }
            if (found == false)
            {
                result = "Player not found";
                MessageBox.Show(result);


            }
            found = false;


            sr.Close();
        }

        private void SearchRecordsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            

        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            MainWindow Window1 = new MainWindow();
            Window1.Show();

            SearchRecordsWindow Window2 = new SearchRecordsWindow();
            Window2.Close();
        }

        private void SearchRecordsWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            FileStream fs = new FileStream(@"H:\Year Two\Semester 4\Programming\Project\Project\PlayerRecords.txt", FileMode.Open, FileAccess.Read);

            StreamReader sr = new StreamReader(fs);

            string[] lines = new string[100];

            string lineIn = sr.ReadLine();

            for (int i = 0; i < 100; i++)
            {
                lines[i] = lineIn;

                
                lineIn = sr.ReadLine();
            }



            foreach (string player in lines)
            {
                allPlayers.Add(player);
            }



            allPlayers.Sort();
            allPlayers.Reverse();

            lstBxRecords.ItemsSource = null;

            lstBxRecords.ItemsSource = allPlayers;
        }
    }
}
