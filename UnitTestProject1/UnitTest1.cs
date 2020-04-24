using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlackJack;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestPlayerDraws()
        {
            //Tests if the draw method for player works
            //ARRANGE
            Player player1 = new Player();
            player1.Draws = 5;
            int expectedDraws = 6;

            //ACT
            player1.DrawMethod();

            //ASSERT
            Assert.AreEqual(expectedDraws, player1.Draws);
        }

        [TestMethod]
        public void TestPlayerWins()
        {
            //Tests if the Win method for player works

            //ARRANGE
            Player player1 = new Player();
            player1.Wins = 5;
            int expectedWins = 6;

            //ACT
            player1.WinMethod();

            //ASSERT
            Assert.AreEqual(expectedWins, player1.Wins);
        }

        [TestMethod]
        public void TestPlayerLosses()
        {
            //Tests if the Loss method for player works

            //ARRANGE
            Player player1 = new Player();
            player1.Losses = 5;
            int expectedLosses = 6;

            //ACT
            player1.LoseMethod();

            //ASSERT
            Assert.AreEqual(expectedLosses, player1.Losses);
        }

        [TestMethod]
        public void TestPlayerToString()
        {
            //Tests if the ToString method for player works

            //ARRANGE
            Player player1 = new Player();
            player1.PlayerID = 36223;
            player1.PlayerName = "Pierce";
            player1.Losses = 5;
            player1.Wins = 9;
            player1.Draws = 2;
            

            string expectedToString = String.Format("{0,-35}{1,-14}{2,-14}{3,-14}{4,-14}", player1.PlayerID, player1.PlayerName, player1.Wins, player1.Losses, player1.Draws);




            //ACT
            string actualResult = player1.ToString();

            //ASSERT
            Assert.AreEqual(expectedToString, actualResult);
        }

        [TestMethod]
        public void TestHandAddValue()
        {
            //Tests if the Add Value method for hand works

            //ARRANGE
            Deck newDeck = new Deck();

            Hand playerHand = new Hand(newDeck);

            Card card = new Card();
            card.cardSuitVar = CardSuit.clubs;
            card.cardNumberVar = CardNumber.five;

            int expectedSum = 20;

            int currentSum = 15;

            //ACT
            int newSum = playerHand.AddValue(card, currentSum);

            //ASSERT
            Assert.AreEqual(expectedSum, newSum);
        }

        [TestMethod]
        public void TestHandAddValueBlackJack()
        {
            //Tests if the Add Value method for hand can check an ace for value of 11 in certain cases

            //ARRANGE
            Deck newDeck = new Deck();

            Hand playerHand = new Hand(newDeck);

            Card card = new Card();
            card.cardSuitVar = CardSuit.diamond;
            card.cardNumberVar = CardNumber.ace;

            int expectedSum = 21;

            int currentSum = 10;

            //ACT
            int newSum = playerHand.AddValue(card, currentSum);

            //ASSERT
            Assert.AreEqual(expectedSum, newSum);
        }

        [TestMethod]
        public void TestHandAddValueNotBlackJack()
        {
            //Tests if the Add Value method for hand can check an ace for value of 1 in certain cases

            //ARRANGE
            Deck newDeck = new Deck();

            Hand playerHand = new Hand(newDeck);

            Card card = new Card();
            card.cardSuitVar = CardSuit.diamond;
            card.cardNumberVar = CardNumber.ace;

            int expectedSum = 12;

            int currentSum = 11;

            //ACT
            int newSum = playerHand.AddValue(card, currentSum);

            //ASSERT
            Assert.AreEqual(expectedSum, newSum);
        }

        [TestMethod]
        public void TestHandAddValueFaceCards()
        {
            //Tests if the Add Value method for hand can check that face cards equal 10

            //ARRANGE
            Deck newDeck = new Deck();

            Hand playerHand = new Hand(newDeck);

            Card card = new Card();
            card.cardSuitVar = CardSuit.diamond;
            card.cardNumberVar = CardNumber.queen;

            int currentSum = 5;
            int expectedSum = 15;

            

            //ACT
            int newSum = playerHand.AddValue(card, currentSum);

            //ASSERT
            Assert.AreEqual(expectedSum, newSum);
        }





    }
}
