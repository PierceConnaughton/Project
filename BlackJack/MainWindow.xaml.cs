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

using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;

namespace BlackJack
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PlayerData db = new PlayerData();

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
        
        bool ifHit = false;

        bool win = false;
        bool draw = false;
        bool lose = false;

        

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

        private void Close_ME_Click(object sender, RoutedEventArgs e)
        {
            DialogHost.IsOpen = false;
        }
        //if start button clicked
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            //clears card value text
            txtBlCardValue.Text = "";
            //check if the game has been restarted to 0
            if (gameRestarted == true && gameStarted == false)
            {
                //check if nothing is in text box
                if (String.IsNullOrEmpty(txtBxEnterName.Text))
                {
                    
                    txtBlNameFound.Text = "Enter Name First";
                    DialogHost.IsOpen = true;
                }
                else
                {
                    //else start the game thus turing the game started bool too true
                    gameStarted = true;

                    playerFound = false;

                    deck = new Deck();

                    #region PlayerHand
                    //creates the players hand
                    Hand playerHand = new Hand(deck);
                    
                    //draws 2 cards for player hand
                    Card firstCard = deck.DrawCard(playerHand);
                    Card secondCard = deck.DrawCard(playerHand);

                    //gets the sums of these cards
                    int firstCardNum = playerHand.AddValue(firstCard, playerSum);
                    int secondCardNum = playerHand.AddValue(secondCard,playerSum);

                    //equal them too overall player sum
                    playerSum = firstCardNum + secondCardNum;

                    //display sum
                    playerSumString = playerSum.ToString();

                    txtBlPlayerTotal.Text = playerSumString;

                    //display the image of the first 2 cards
                    BitmapImage userFirstbitmapImage = Convert(firstCard.ReturnImage());

                    ImgUserFirstCard.Source = userFirstbitmapImage;

                    BitmapImage userSecondbitmapImage = Convert(secondCard.ReturnImage());

                    ImgUserSecondCard.Source = userSecondbitmapImage;

                    #endregion PlayerHand


                    #region DealerHand
                    //creates the dealers hand
                    Hand dealerHand = new Hand(deck);

                    //draws dealers first card
                    Card dealerCard = deck.DrawCard(dealerHand);

                    //gets card value and equal it too dealer sum
                    dealerSum = dealerHand.AddValue(dealerCard,dealerSum);

                    
                    //display dealer sum
                    dealerSumString = dealerSum.ToString();

                    txtBlDealerTotal.Text = dealerSumString;

                    //display dealer card image
                    BitmapImage dealerbitmapImage = Convert(dealerCard.ReturnImage());

                    ImgDealerFirstCard.Source = dealerbitmapImage;

                    #endregion DealerHand



                    //check if player is a returning one
                    foreach (Player returningPlayer in players)
                    {
                        //check if player name can be found in player list if so turn returning player too true and display message
                        if (returningPlayer.PlayerName == txtBxEnterName.Text)
                        {
                            playerReturned = true;

                            txtBlNameFound.Text = "Returning Player";
                            DialogHost.IsOpen = true;
                            
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
                        //clears all wins losses and drwas for new player
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
                txtBlNameFound.Text = "Restart Game and press start";
                DialogHost.IsOpen = true;
                
            }

        }

        //converts image too bitmap image toio display card on the screen
        public BitmapImage Convert(System.Drawing.Image img)
        {
            using (var memory = new MemoryStream())
            {
                img.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
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

            //RefreshRecords();

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

                    txtBlNameFound.Text = "Enter Name First";
                    DialogHost.IsOpen = true;
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
                            playerHand = new Hand(deck);

                            Card HitCard = deck.DrawCard(playerHand);
                            
                            int HitCardNum = playerHand.AddValue(HitCard, playerSum);


                            playerSum = HitCardNum;

                            playerSumString = playerSum.ToString();

                            txtBlPlayerTotal.Text = playerSumString;

                            BitmapImage userHitbitmapImage = Convert(HitCard.ReturnImage());

                            if (ImgUserThirdCard.Source ==  null)
                            {
                                ImgUserThirdCard.Source = userHitbitmapImage;
                            }

                            else if (ImgUserFourthCard.Source == null)
                            {
                                ImgUserFourthCard.Source = userHitbitmapImage;
                            }

                            else if (ImgUserFifthCard.Source == null)
                            {
                                ImgUserFifthCard.Source = userHitbitmapImage;
                            }

                            else
                            {
                                ImgUserFirstCard.Source = userHitbitmapImage;
                            }
                                 

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
                        
                        txtBlNameFound.Text = "Player changed, please press start";
                        DialogHost.IsOpen = true;
                    }

                }
            }
            else
            {
                //warning too restart the game
                txtBlNameFound.Text = "Press Restart and then press start game";
                DialogHost.IsOpen = true;
                

            }

        }

        private void btnFold_Click(object sender, RoutedEventArgs e)
        {
            //check if game is in progress
            if (gameRestarted == true && gameStarted == true)
            {

                if (String.IsNullOrEmpty(txtBxEnterName.Text))
                {
                    txtBlNameFound.Text = "Enter Name First";
                    DialogHost.IsOpen = true;
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
                    //if player changed his name halfway through game
                    if (playerFound == false)
                    {
                        
                        txtBlNameFound.Text = "Player changed, please press start";
                        DialogHost.IsOpen = true;
                    }
                }
            }

            else
            {
                
                txtBlNameFound.Text = "Restart Game and press Start";
                DialogHost.IsOpen = true;
            }
        }

        //Method if you lost
        public void Loss()
        {
            //get name in textbox
            string x = txtBxEnterName.Text;

            //go through all players
            foreach (Player newPlayer in players)
            {

                //check if 1 of the players in the players list is same as the player in txtbox
                if (newPlayer.PlayerName == x)
                {
                    //change date of last game too current
                    newPlayer.DateOfLastGame = DateTime.Now.ToString("MM/dd/yyyy H:mm");
                    gameInProgress = true;
                    //add loss too player
                    newPlayer.Losses++;
                    //display result on screen
                    txtBlCardValue.Text = string.Format("Your Sum: " + playerSumString + " Dealer sum: " + dealerSumString);

                    //display player overall losses
                    txtBlNameFound.Text = string.Format(" You Lose. You lost " + newPlayer.Losses + " game(s)");
                    DialogHost.IsOpen = true;
                   
                    //players.Sort();
                    lose = true;
                    Restart();
                    RefreshRecords();
                    txtBlLosses.Text = newPlayer.Losses.ToString();
                    lose = false;
                    return;
                }

            }


        }

        //Method if you won
        public void Win()
        {
            //same as loss method with minor changes
            string x = txtBxEnterName.Text;

            foreach (Player newPlayer in players)
            {


                if (newPlayer.PlayerName == x)
                {
                    newPlayer.DateOfLastGame = DateTime.Now.ToString("MM/dd/yyyy H:mm");
                    gameInProgress = true;
                    newPlayer.Wins++;
                    txtBlCardValue.Text = string.Format("Your Sum: " + playerSumString + " Dealer sum: " + dealerSumString);

                    txtBlNameFound.Text = string.Format(" You won. You won " + newPlayer.Wins + " game(s)");
                    DialogHost.IsOpen = true;
                    
                    //players.Sort();
                    win = true;
                    Restart();
                    RefreshRecords();
                    txtBlWin.Text = newPlayer.Wins.ToString();
                    win = false;
                    return;
                }

            }

        }

        //method if you drew
        public void Draw()
        {
            //same as loss method with minor changes
            string x = txtBxEnterName.Text;

            foreach (Player newPlayer in players)
            {

                if (newPlayer.PlayerName == x)
                {
                    newPlayer.DateOfLastGame = DateTime.Now.ToString("MM/dd/yyyy H:mm");
                    gameInProgress = true;
                    newPlayer.Draws++;
                    txtBlCardValue.Text = string.Format("Your Sum: " + playerSumString + " Dealer sum: " + dealerSumString);
                    txtBlNameFound.Text = string.Format(" You Draw. You drew " + newPlayer.Draws + " game(s)");
                    DialogHost.IsOpen = true; 
                    //players.Sort();
                    draw = true;
                    Restart();
                    RefreshRecords();
                    txtBlDraws.Text = newPlayer.Draws.ToString();
                    draw = false;
                    return;


                }

            }

        }

        //refreshes the records after a game
        public void RefreshRecords()
        {
            //go too save method
            Save();

            //find file and put in list of players into the txt file
            FileStream fs = new FileStream(@"D:\College\Programming\BlackJack-master\Project\PlayerRecords.txt", FileMode.Open, FileAccess.Read);

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

            sr.Close();

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

            ImgUserFirstCard.Source = null;
            ImgUserSecondCard.Source = null;
            ImgUserThirdCard.Source = null;
            ImgUserFourthCard.Source = null;
            ImgUserFifthCard.Source = null;
            
            ImgDealerFirstCard.Source = null;
            ImgDealerSecondCard.Source = null;
            ImgDealerThirdCard.Source = null;
            ImgDealerFourthCard.Source = null;
            ImgDealerFifthCard.Source = null;

            
        }

        //When double down is clicked
        private void btnDoubleDown_Click(object sender, RoutedEventArgs e)
        {
            if (gameRestarted == true && gameStarted == true)
            {
                if (String.IsNullOrEmpty(txtBxEnterName.Text))
                {
                    
                    txtBlNameFound.Text = string.Format("Enter Name First");
                    DialogHost.IsOpen = true;
                }
                //if hit button has been pressed cant double down
                else if (ifHit == true)
                {
                    
                    txtBlNameFound.Text = string.Format("Cannot double down after hit was pressed");
                    DialogHost.IsOpen = true;
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

                            //get 2 new cards add them too the total and display them
                            playerHand = new Hand(deck);

                            Card firstCardDoubleDown = deck.DrawCard(playerHand);
                            Card secondCardDoubleDown = deck.DrawCard(playerHand);

                            int firstCardDoubleDownNum = playerHand.AddValue(firstCardDoubleDown, playerSum);
                            int secondCardDoubleDownNum = playerHand.AddValue(secondCardDoubleDown, playerSum);

                            int total = (firstCardDoubleDownNum + secondCardDoubleDownNum) - playerSum;
                            playerSum = total;

                            

                            playerSumString = playerSum.ToString();

                            txtBlPlayerTotal.Text = playerSumString;

                            BitmapImage userFirstbitmapImage = Convert(firstCardDoubleDown.ReturnImage());
                            BitmapImage userSecondbitmapImage = Convert(secondCardDoubleDown.ReturnImage());


                            ImgUserThirdCard.Source = userFirstbitmapImage;
                            ImgUserFourthCard.Source = userSecondbitmapImage;

                           
                           
                            //if player sum over 21 they lose same as 21 they win otherwise the dealer can now play
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
                        txtBlNameFound.Text = string.Format("Player changed, please press start");
                        DialogHost.IsOpen = true;
                       
                    }
                }

            }
            else
            {
                txtBlNameFound.Text = string.Format("Restart Game and press Start");
                DialogHost.IsOpen = true;
                
            }
        }

        public void Save()
        {
            if (gameStarted == false)
            {
                //query too get all players where player name and txtbx name are same
                var query = from p in db.players
                            where p.PlayerName == txtBxEnterName.Text 
                            select new
                            {
                                PlayerName = p.PlayerName,
                                Wins = p.Wins,
                                Losses = p.Losses,
                                Draws = p.Draws,
                                LastTimePlayed = p.DateOfLastGame
                            };

                var x = query.AsQueryable().FirstOrDefault(name => name.PlayerName == txtBxEnterName.Text);
               //if name is found in database
                if (x != null)
                {
                   //if player won or if they lose or if they draw use that method
                    if (win == true)
                    {
                        int winsInt = x.Wins;
                        winsInt++;
                        UpdatePlayer(winsInt);
                    }
                    else if (lose == true)
                    {
                        int LossesInt = x.Losses;
                        LossesInt++;
                        UpdatePlayer(LossesInt);
                    }
                    else if (draw == true)
                    {
                        int drawsInt = x.Draws;
                        drawsInt++;
                        UpdatePlayer(drawsInt);
                    }
                    else
                    {

                    }
                    
                }
                else
                {
                    //if player not found in database add new player
                    InsertPlayer();
                }

                SaveFile();


                #region Comments


                //message too show records being saved


                //foreach player in the list save their record too a list
                //foreach (Player newPlayer in players)
                //{
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






                //if (playerReturned == true)
                //{
                //    FileStream fs1 = new FileStream(@"D:\College\Programming\BlackJack-master\Project\PlayerRecords.txt", FileMode.Open, FileAccess.Read);

                //    StreamReader sr = new StreamReader(fs1);

                //    string x = txtBxEnterName.Text;

                //    foreach (Player player in players)
                //    {
                //        if (player.PlayerName == x)
                //        {
                //            MessageBox.Show("Inside If");
                //            string name = player.PlayerName;
                //            string[] lines = new string[100];
                //            bool found = false;

                //            string result = "Unknown";

                //            string searchName = string.Format(" {0,-15} Wins", name);

                //            string lineIn = sr.ReadLine();
                //            string[] fieldArray1 = new string[5];

                //            if (name == "")
                //            {
                //                MessageBox.Show("Please enter name first");
                //                return;
                //            }

                //            for (int i = 0; i < 100; i++)
                //            {
                //                lines[i] = lineIn;
                //                lineIn = sr.ReadLine();
                //            }

                //            for (int i = 0; i < lines.Length; i++)
                //            {
                //                fieldArray1 = lines[i].Split(':');
                //                string playerName = string.Format(fieldArray1[1]);
                //                if (fieldArray1[1] == searchName)
                //                {
                //                    result = string.Format(lines[i].ToString());

                //                    MessageBox.Show(result);
                //                    found = true;
                //                    break;
                //                }


                //            }
                //            if (found == false)
                //            {
                //                result = "Player not found";
                //                MessageBox.Show(result);


                //            }
                //            found = false;


                //            sr.Close();
                //        }

                //    }
                //}

                //else if(playerReturned != true)
                //{
                //    FileStream fs = new FileStream(@"D:\College\Programming\BlackJack-master\Project\PlayerRecords.txt", FileMode.Append, FileAccess.Write);
                //    StreamWriter sw = new StreamWriter(fs);

                //    sw.WriteLine("Player Name: {0,-15} Wins: {1,-15} Losses: {2,-15} Draws: {3,-15} Date Last time player played: {4}", newPlayer.PlayerName, newPlayer.Wins, newPlayer.Losses, newPlayer.Draws, newPlayer.DateOfLastGame);

                //   sw.Close();
                //}




                //}



                //}
                //ReadFile();

                #endregion Comments

            }
            else
            {
                MessageBox.Show("Finish game before finishing");
            }
        }

        //Method for when it's the dealers turn
        public void Dealer()
        {
            //gets dealers second card displays total and card image
            #region DealerCard
            dealerHand = new Hand(deck);

            Card firstCard = deck.DrawCard(dealerHand);
            
            int firstCardNum = dealerHand.AddValue(firstCard, dealerSum);



            dealerSum = firstCardNum;

            dealerSumString = dealerSum.ToString();

            txtBlDealerTotal.Text = dealerSumString;

            BitmapImage userFirstbitmapImage = Convert(firstCard.ReturnImage());
            

           ImgDealerSecondCard.Source = userFirstbitmapImage;

            #endregion DealerCard

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
                        Card newCard = deck.DrawCard(dealerHand);

                        int newCardNum = dealerHand.AddValue(newCard, dealerSum);
                        

                        dealerSum = newCardNum;

                        dealerSumString = dealerSum.ToString();

                        txtBlDealerTotal.Text = dealerSumString;

                        BitmapImage dealerNewCardbitmapImage = Convert(newCard.ReturnImage());

                        //depending on if the place for the new card image is null display it this place if it is not null look at the next spot
                        if (ImgDealerThirdCard.Source == null)
                        {
                            ImgDealerThirdCard.Source = dealerNewCardbitmapImage;
                        }

                        else if (ImgDealerFourthCard.Source == null)
                        {
                            ImgDealerFourthCard.Source = dealerNewCardbitmapImage;
                        }

                        else if (ImgDealerFifthCard.Source == null)
                        {
                            ImgDealerFifthCard.Source = dealerNewCardbitmapImage;
                        }

                        else
                        {
                            ImgDealerFirstCard.Source = dealerNewCardbitmapImage;
                        }




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

        private void InsertPlayer()
        {
            //if its a new player set all values too default
            int winint = 0;
            int drawint = 0;
            int loseint = 0;
            //dependning on if they won draw or lost add 1 too the int thats right
            if (win == true)
            {
                winint++;
            }
            else if (draw == true)
            {
                drawint++;
            }
            else if (lose == true)
            {
                loseint++;
            }
            else
            {
                return;
            }
            //connect too the database and insert the new player with the values given
            string connection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MyPlayerData;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection sqlConnection = new SqlConnection(connection);
            string query = "INSERT INTO Players " +
                            "(PlayerName, Wins, Losses, Draws, DateOfLastGame) " +
                            "VALUES ( @PlayerName, @Wins, @Losses, @Draws, @DateOfLastGame) ";
            SqlCommand cmd = new SqlCommand(query, sqlConnection);
            cmd.Parameters.Add("@PlayerName", SqlDbType.NVarChar, 100).Value = txtBlCurrentPlayer.Text;
            cmd.Parameters.Add("@Wins", SqlDbType.Int).Value = winint;
            cmd.Parameters.Add("@Losses", SqlDbType.Int).Value = loseint;
            cmd.Parameters.Add("@Draws", SqlDbType.Int).Value = drawint;
            cmd.Parameters.Add("@DateOfLastGame", SqlDbType.NVarChar).Value = DateTime.Now.ToShortDateString();
            
            sqlConnection.Open();
            cmd.ExecuteNonQuery();
            sqlConnection.Close();


        }

        private void UpdatePlayer(int y)
        {
            //if the player is already in database find the player from the database
            var query2 = from p in db.players
                        where p.PlayerName == txtBxEnterName.Text
                        select new
                        {
                            PlayerName = p.PlayerName,
                            Wins = p.Wins,
                            Losses = p.Losses,
                            Draws = p.Draws,
                            LastTimePlayed = p.DateOfLastGame
                        };

            var x = query2.AsQueryable().FirstOrDefault(name => name.PlayerName == txtBxEnterName.Text);

            //have there results equal the results of the returning player
            //and depending on if they won lost or drawn add there new results too the database
            int lossesInt = x.Losses;
            int winsInt = x.Wins;
            int drawsInt = x.Draws;
            if (win == true)
            {
                winsInt = y;
            }
            else if (draw == true)
            {
                drawsInt = y;
            }
            else
            {
                lossesInt = y;
            }
            string currentPlayerName = txtBxEnterName.Text;
            //connect too database and update where player namae is equal too the currentplayer.text
            string connection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MyPlayerData;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection sqlConnection = new SqlConnection(connection);
            string query = "UPDATE Players " +
                "SET Wins = @Wins, Draws = @Draws, Losses = @Losses, DateOfLastGame = @DateOfLastGame WHERE PlayerName =@PlayerName";
                
                            
            SqlCommand cmd = new SqlCommand(query, sqlConnection);
            cmd.Parameters.Add("@PlayerName", SqlDbType.NVarChar, 100).Value = txtBlCurrentPlayer.Text;
            cmd.Parameters.Add("@Wins", SqlDbType.Int).Value = winsInt;
            cmd.Parameters.Add("@Losses", SqlDbType.Int).Value = lossesInt;
            cmd.Parameters.Add("@Draws", SqlDbType.Int).Value = drawsInt;
            cmd.Parameters.Add("@DateOfLastGame", SqlDbType.NVarChar).Value = DateTime.Now.ToShortDateString();

            sqlConnection.Open();
            cmd.ExecuteNonQuery();
            sqlConnection.Close();
        }

        private void btnSearchForRecord_Click(object sender, RoutedEventArgs e)
        {
            //open new window for search records and close the current window open
            MainWindow window2 = new MainWindow();
            SearchRecordsWindow window = new SearchRecordsWindow();

            Save();

            window.Show();
            window2.Close();
        }

        public void SaveFile()
        {
            //save players data too json format
            string data = JsonConvert.SerializeObject(GetPlayers(), Formatting.Indented);

            //write the players data json into a json file
            using(StreamWriter sw = new StreamWriter("C:/Users/Pierce/OneDrive/College/Semester 4/Programming/Project/Project/PlayerRecords.json"))
            {
                sw.Write(data);
                sw.Close();
            }
        }

        public static List<Player> GetPlayers()
        {
            //get player daya and put the list of player data into a players list too return
            PlayerData db = new PlayerData();

            List<Player> players = new List<Player>();
            players = db.players.ToList();

            return players;
        }


    }
}
