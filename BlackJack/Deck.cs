using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public class Deck
    {
        public List<Card> cards;

        public Deck()
        {
            cards = new List<Card>();
            ShuffleNewDeck();
        }

        public void ShuffleNewDeck()
        {
            //clear the list of cards
            cards.Clear();
            //foreach suit in the deck
            for (int i = 1; i < 5; i++)
            {
                //for each number in the suit 
                for (int j = 1; j < 14; j++)
                {
                  //create a new Card
                  Card card = new Card();
                  //assign a number too the card
                  card.cardNumberVar = (CardNumber)j;
                  //assign a suit too the card
                  card.cardSuitVar = (CardSuit)i;
                  //add the card too the deck
                  cards.Add(card);
                }
            }
            //create a new random number
            Random rnd = new Random();

            //Order the cards in the list in random order
            cards = cards.OrderBy(x => rnd.Next()).ToList();

           
        }

        //draws a card at random too your hand
        public Card DrawCard(Hand hand)
        {
            //takes away 1 from the amount of cards
            Card drawn = cards[cards.Count - 1];
            //takes away the card that was drawn form the list of cards
            cards.Remove(drawn);
            //adds the card too your current hand
            //hand.cards.Add(drawn);
            return drawn;
        }
    }
}
