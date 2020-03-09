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
using System.IO;

namespace BlackJack
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //creates the deck
        Deck deck;

        //creates the players hand
        Hand playerHand;
        //creates the dealers hand
        Hand dealerHand;

        //creates the sum of the cards for both the dealer and the player
        int dealerSum;
        int playerSum;

        //create a random num
        Random rnd = new Random();

        string dealerSumString;
        string playerSumString;

        string[] suits = new string[4] { "spades", "clubs", "hearts", "diamonds" };
        string[] faces = new string[13] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "jack", "queen", "king", "ace" };
        //List<string> deck = new List<string>();

        //list of all the current players
        List<Player> players = new List<Player>();

        List<string> allPlayers = new List<string>();

        //bools for each instance of the game not being started correctley it will not start
        bool gameRestarted = true;
        bool gameInProgress = false;
        bool playerReturned = false;
        bool gameStarted = false;
        bool playerFound = false;
        bool playerInFile = false;
        bool ifHit = false;

        //Image image;

        



        public MainWindow()
        {
            InitializeComponent();
        }

        //end game button clicked button
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (gameStarted == false)
            {
                Save();
                MainWindow1.Close();
            }

        }

        //if start button clicked
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            //check if the game has been restarted to 0
            if (gameRestarted == true)
            {
                //check if anything is in text box
                if (String.IsNullOrEmpty(txtBxEnterName.Text))
                {
                    MessageBox.Show("Enter Name First");
                }
                else
                {
                    //else start the game thus turing the game started bool too true
                    gameStarted = true;

                    playerFound = false;

                    //Card card1 = new Card();
                    //Card card2 = new Card();

                    //Card dealerCard = new Card();

                    //int cardNumber1 = (int)card1.cardNumberVar;
                    //if (cardNumber1 > 11)
                    //{
                    //    cardNumber1 = 11;
                    //}

                    //int cardNumber2 = (int)card2.cardNumberVar;
                    //if (cardNumber2 > 10)
                    //{
                    //    cardNumber2 = 10;
                    //}
                    deck = new Deck();

                    //creates the players hand
                    Hand playerHand = new Hand(deck);
                    //creates the dealers hand
                    Hand dealerHand = new Hand(deck);


                    //for first cards weather it be 2 for player and 1 for dealer depending
                    for (int i = 0; i < 2; i++)
                    {
                        
                        playerSum = playerHand.AddValue(deck.DrawCard(playerHand)  , playerSum);
                    }

                    dealerSum = dealerHand.AddValue(deck.DrawCard(dealerHand), dealerSum);


                    ////add players first two cards and turn them into a string too display
                    //playerNum = cardNumber1 + cardNumber2;


                    playerSumString = playerSum.ToString();

                    txtBlPlayerTotal.Text = playerSumString;

                    ////show dealers first card as a string
                    //dealerNum = (int)dealerCard.cardNumberVar;
                    //if (dealerNum > 11)
                    //{
                    //    dealerNum = 11;
                    //}

                    dealerSumString = dealerSum.ToString();

                    txtBlDealerTotal.Text = dealerSumString;

                    //check if player is a returning one
                    foreach (Player returningPlayer in players)
                    {
                        //check if player name can be found in player list if so turn returning player too true and display message
                        if (returningPlayer.PlayerName == txtBxEnterName.Text)
                        {
                            playerReturned = true;
                            MessageBox.Show("Returning Player");
                        }
                    }

                    //if player returning is true
                    if (playerReturned == true)
                    {

                        //display there name
                        foreach (Player returningPlayer in players)
                        {
                            if (returningPlayer.PlayerName == txtBxEnterName.Text)
                            {
                                returningPlayer.PlayerName = txtBxEnterName.Text;

                                txtBlCurrentPlayer.Text = returningPlayer.PlayerName;
                            }
                        }

                        //get whats turned into the txt box turn it into a string
                        string x = txtBxEnterName.Text;

                        gameInProgress = false;

                        //compare the string too the list and see which matches
                        foreach (Player currentPlayer in players)
                        {

                            //if the string matches the players name
                            if (x == currentPlayer.PlayerName)
                            {

                                //if the player num is equal too 21 they win
                                if (playerSum == 21)
                                {

                                    Win();
                                    return;
                                }
                            }
                        }

                    }
                    else
                    {

                        txtBlWin.Text = "0";
                        txtBlLosses.Text = "0";
                        txtBlDraws.Text = "0";
                        //if player is not a returning player create a new player
                        Player newPlayer = new Player();

                        players.Add(newPlayer);


                        newPlayer.PlayerName = txtBxEnterName.Text;

                        txtBlCurrentPlayer.Text = newPlayer.PlayerName;



                        string x = txtBxEnterName.Text;

                        gameInProgress = false;

                        foreach (Player currentPlayer in players)
                        {

                            if (x == currentPlayer.PlayerName)
                            {
                                if (playerSum == 21)
                                {

                                    Win();
                                    return;
                                }
                            }
                        }
                    }






                }
            }

            //if game has not been restarted display message
            else
            {
                MessageBox.Show("Restart Game and press start");
            }




        }
        //on window load
        private void MainWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            //when the main window is loaded set all the values too default
            txtBlCurrentPlayer.Text = "Unknown";
            txtBlDealerTotal.Text = "0";
            txtBlDraws.Text = "0";
            txtBlPlayerTotal.Text = "0";
            txtBlWin.Text = "0";
            txtBlLosses.Text = "0";

            ////Create Deck   
            //foreach (string suit in suits)
            //{
            //    foreach (string face in faces)
            //    {
            //        deck.(string.Format("{0} of {1}", face, suit));
            //    }   
            //}

            //foreach (string x in deck)
            //{
            //    MessageBox.Show(x);
            //}

            RefreshRecords();

        }

        

        //if btn has been clicked
        private void btnHitMe_Click(object sender, RoutedEventArgs e)
        {
            //if game has been restarted and the game has been started
            if (gameRestarted == true && gameInProgress == false && gameStarted == true)
            {
                //name has to be entred to use this button
                if (String.IsNullOrEmpty(txtBxEnterName.Text))
                {
                    MessageBox.Show("Enter Name First");
                }
                else
                {
                    //set player found too false

                    playerFound = false;
                    //get the name in the textbox
                    string x = txtBxEnterName.Text;

                    //check in the list if that name matches with the player that is playing
                    foreach (Player newPlayer in players)
                    {


                        if (newPlayer.PlayerName == x)
                        {
                            //if player found set too true
                            playerFound = true;

                            ifHit = true;
                            //get random card between 1 and 10 and add it too your total
                            Card HitCard = new Card();


                            //int hit = (int)HitCard.cardNumberVar;
                            //if (hit > 10)
                            //{
                            //    hit = 10;
                            //}

                            playerSum = playerHand.AddValue(deck.DrawCard(playerHand), playerSum);

                            txtBlPlayerTotal.Text = playerSum.ToString();

                            //if player gets more then 21 they lose or if player gets exactly 21 they win
                            if (playerSum > 21)
                            {
                                Loss();
                                return;
                            }
                            else if (playerSum == 21)
                            {
                                Win();
                                return;
                            }
                        }

                    }
                    //if player cant be found as the same player in the txtbx get a warning
                    if (playerFound == false)
                    {
                        MessageBox.Show("Player changed, please press start");

                    }

                }
            }
            else
            {
                //warning too restart the game

                MessageBox.Show("Press Restart and then press start game");

            }







        }

        private void btnFold_Click(object sender, RoutedEventArgs e)
        {
            if (gameRestarted == true && gameStarted == true)
            {

                if (String.IsNullOrEmpty(txtBxEnterName.Text))
                {
                    MessageBox.Show("Enter Name First");
                }


                else
                {
                    playerFound = false;

                    string x = txtBxEnterName.Text;

                    foreach (Player newPlayer in players)
                    {


                        if (newPlayer.PlayerName == x)
                        {
                            playerFound = true;

                            //go too dealer method
                            Dealer();

                        }
                    }
                    if (playerFound == false)
                    {
                        MessageBox.Show("Player changed, please press start");
                    }
                }
            }

            else
            {
                MessageBox.Show("Restart Game and press Start");
            }
        }

        //Method if you lost
        public void Loss()
        {
            string x = txtBxEnterName.Text;

            foreach (Player newPlayer in players)
            {


                if (newPlayer.PlayerName == x)
                {
                    newPlayer.DateOfLastGame = DateTime.Now.ToString("MM/dd/yyyy H:mm");
                    gameInProgress = true;
                    newPlayer.Losses++;
                    MessageBox.Show("You Lose. You lost " + newPlayer.Losses + " games");
                    //players.Sort();
                    
                    Restart();
                    RefreshRecords();
                    txtBlLosses.Text = newPlayer.Losses.ToString();
                    return;
                }

            }


        }

        //Method if you won
        public void Win()
        {
            string x = txtBxEnterName.Text;

            foreach (Player newPlayer in players)
            {


                if (newPlayer.PlayerName == x)
                {
                    newPlayer.DateOfLastGame = DateTime.Now.ToString("MM/dd/yyyy H:mm");
                    gameInProgress = true;
                    newPlayer.Wins++;
                    MessageBox.Show("You won. You won " + newPlayer.Wins + " games");
                    //players.Sort();
                    
                    Restart();
                    RefreshRecords();
                    txtBlWin.Text = newPlayer.Wins.ToString();
                    return;
                }

            }

        }

        //method if you drew
        public void Draw()
        {
            string x = txtBxEnterName.Text;

            foreach (Player newPlayer in players)
            {

                if (newPlayer.PlayerName == x)
                {
                    newPlayer.DateOfLastGame = DateTime.Now.ToString("MM/dd/yyyy H:mm");
                    gameInProgress = true;
                    newPlayer.Draws++;
                    MessageBox.Show("You Draw. You drew " + newPlayer.Draws + " games");
                    //players.Sort();
                
                    Restart();
                    RefreshRecords();
                    txtBlDraws.Text = newPlayer.Draws.ToString();
                    return;

                }

            }

        }

        //refreshes the records after a game
        public void RefreshRecords()
        {
            Save();

            FileStream fs = new FileStream(@"H:\Year Two\Semester 4\Programming\Project\Project\PlayerRecords.txt", FileMode.Open, FileAccess.Read);

            StreamReader sr = new StreamReader(fs);

            string[] lines = new string[100];

            string lineIn = sr.ReadLine();

            for (int i = 0; i < 100; i++)
            {
                lines[i] = lineIn;

                allPlayers.Remove(lines[i]);
                lineIn = sr.ReadLine();
            }

            

            foreach (string player in lines)
            {
                allPlayers.Add(player);
            }

            

            allPlayers.Sort();
            allPlayers.Reverse();

            

        }

        //restarts game when clicked
        private void btnRestart_Click(object sender, RoutedEventArgs e)
        {
            //Activates restart method
            Restart();

        }

        //restarts game
        public void Restart()
        {
            //set all the values too default
            gameRestarted = true;
            gameStarted = false;
            playerReturned = false;
            ifHit = false;

            playerSum = 0;
            dealerSum = 0;



            txtBlDealerTotal.Text = "0";
            txtBlPlayerTotal.Text = "0";
        }

        //When double down is clicked
        private void btnDoubleDown_Click(object sender, RoutedEventArgs e)
        {
            if (gameRestarted == true && gameStarted == true)
            {
                if (String.IsNullOrEmpty(txtBxEnterName.Text))
                {
                    MessageBox.Show("Enter Name First");
                }
                else if (ifHit == true)
                {
                    MessageBox.Show("Cannot double down after hit was pressed");
                }
                else
                {
                    playerFound = false;
                    string x = txtBxEnterName.Text;

                    foreach (Player newPlayer in players)
                    {


                        if (newPlayer.PlayerName == x)
                        {
                            playerFound = true;
                            //Card doubleDown = new Card();

                            //int doubleDownNum = (int)doubleDown.cardNumberVar;
                            //if (doubleDownNum > 10)
                            //{
                            //    doubleDownNum = 10;
                            //}

                            playerSum = playerHand.AddValue(deck.DrawCard(playerHand), playerSum);
                            playerSum = playerHand.AddValue(deck.DrawCard(playerHand), playerSum);

                            txtBlPlayerTotal.Text = playerSum.ToString();

                            if (playerSum > 21)
                            {
                                Loss();
                                return;
                            }
                            else if (playerSum == 21)
                            {
                                Win();
                                return;
                            }
                            else
                            {
                                Dealer();
                            }
                        }

                    }
                    if (playerFound == false)
                    {
                        MessageBox.Show("Player changed, please press start");
                    }
                }

            }
            else
            {

                MessageBox.Show("Restart Game and press Start");
            }
        }

        public void Save()
        {
            if (gameStarted == false)
            {
                //message too show records being saved
               

                //foreach player in the list save their record too a list
                foreach (Player newPlayer in players)
                {
                    //playerInFile = false;



                    //foreach (Player player in players)
                    //{
                    //    if (newPlayer.PlayerName == player.PlayerName)
                    //    {
                    //        FileStream fs = new FileStream(@"H:\Year Two\Semester 4\Programming\Project\PlayerRecords.txt", FileMode.Create, FileAccess.Write);


                    //        StreamWriter sw = new StreamWriter(fs);

                    //        sw.WriteLine("Player Name: {0,-15} Wins: {1,-15} Losses: {2,-15} Draws: {3}", newPlayer.PlayerName, newPlayer.Wins, newPlayer.Losses, newPlayer.Draws);
                    //        playerInFile = true;
                    //        sw.Close();

                    //        return;

                    //    }

                    //}

                    //if (playerInFile == false)
                    //{




                    

                    if (playerReturned == true)
                    {
                        FileStream fs1 = new FileStream(@"H:\Year Two\Semester 4\Programming\Project\Project\PlayerRecords.txt", FileMode.Open, FileAccess.Read);

                        StreamReader sr = new StreamReader(fs1);

                        string x = txtBxEnterName.Text;

                        foreach (Player player in players)
                        {
                            if (player.PlayerName == x)
                            {
                                MessageBox.Show("Inside If");
                                string name = player.PlayerName;
                                string[] lines = new string[100];
                                bool found = false;

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

                        }
                    }

                    else if(playerReturned != true)
                    {
                        FileStream fs = new FileStream(@"H:\Year Two\Semester 4\Programming\Project\Project\PlayerRecords.txt", FileMode.Append, FileAccess.Write);
                        StreamWriter sw = new StreamWriter(fs);

                        sw.WriteLine("Player Name: {0,-15} Wins: {1,-15} Losses: {2,-15} Draws: {3,-15} Date Last time player played: {4}", newPlayer.PlayerName, newPlayer.Wins, newPlayer.Losses, newPlayer.Draws, newPlayer.DateOfLastGame);

                        sw.Close();
                    }

                        

                    
                    //}



                }
                //ReadFile();
                
            }
            else
            {
                MessageBox.Show("Finish game before finishing");
            }
        }

        //Method for when it's the dealers turn
        public void Dealer()
        {

            //Card dealerCard = new Card();
            ////get dealers second card
            //int dealerCardNum = (int)dealerCard.cardNumberVar;
            //if (dealerCardNum > 10)
            //{
            //    dealerCardNum = 10;
            //}
            //get dealer total and display it
            dealerSum = dealerHand.AddValue(deck.DrawCard(dealerHand), dealerSum);

            dealerSumString = dealerSum.ToString();
            txtBlDealerTotal.Text = dealerSumString;

            //if dealer has exactly 21 you lose
            if (dealerSum == 21)
            {
                Loss();
                return;
            }

            //if dealer's number is below 21
            else if (dealerSum < 21)
            {
                //go through loop 10 times
                for (int i = 0; i < 10; i++)
                {
                    //if dealer number more then player number and dealer number less then or equal to 21 you lose
                    if (dealerSum > playerSum && dealerSum <= 21)
                    {
                        Loss();
                        return;
                    }

                    //if dealer number below 21 and also below the player's number
                    else if (dealerSum <= 21 && dealerSum < playerSum)
                    {
                        Card newCard = new Card();
                        //give dealer a new card and add it too total
                        int newCardNum = (int)newCard.cardNumberVar;
                        if (newCardNum > 10)
                        {
                            newCardNum = 10;
                        }

                        dealerSum = dealerHand.AddValue(deck.DrawCard(dealerHand), dealerSum);

                        dealerSumString = dealerSum.ToString();
                        txtBlDealerTotal.Text = dealerSumString;

                        //if dealer number = too 21 and equal too player number it's a draw
                        if (dealerSum == 21 && dealerSum == playerSum)
                        {
                            Draw();
                            return;
                        }
                        //if dealer number = to 21 and more then player number you lose
                        else if (dealerSum == 21 && dealerSum > playerSum)
                        {
                            Loss();
                            return;
                        }
                        //if dealer number more then 21 you win
                        else if (dealerSum > 21)
                        {
                            Win();
                            return;
                        }
                    }

                    //if dealer number less than = 21 and less then player num 
                    else if (dealerSum <= 21 && dealerSum < playerSum)
                    {
                        Win();
                        return;
                    }
                    //if dealer number == player number its a draw
                    else if (dealerSum == playerSum)
                    {
                        Draw();
                        return;
                    }
                    //else if dealer num is more than player num
                    else if (dealerSum > playerSum)
                    {
                        Loss();
                        return;
                    }
                    //if dealer number is more than 21
                    else if (dealerSum > 21)
                    {
                        Win();
                        return;
                    }
                }


            }
        }

        public void ReadFile()
        {
            string text = File.ReadAllText(@"H:\Year Two\Semester 4\Programming\Project\Project\PlayerRecords.txt");
            text = text.Replace(string.Format("Player Name: {0,-15} Wins: {1,-15} Losses: {2,-15} Draws: {3}", "Pierce", 1, 0, 0), "new value");
            File.WriteAllText("test.txt", text);
        }

        private void btnSearchForRecord_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window2 = new MainWindow();
            SearchRecordsWindow window = new SearchRecordsWindow();

            Save();

            window.Show();
            window2.Close();
        }

        
    }
}
