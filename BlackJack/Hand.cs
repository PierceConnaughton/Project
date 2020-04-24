using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public class Hand
    {
        #region Properties
        public List<Card> cards;

        public static int total = 0;

        #endregion Properties

        #region Constructors
        public Hand(Deck deck)
        {
            //creates new list of cards
            cards = new List<Card>();

        }
        #endregion Constructors

        #region Methods
        //stores the total score the player or dealer currently has
        public int AddValue(Card drawn, int currentSum)
        {
            //if the card drawn is an ace and your score is less then 10 return an 11 otherwise return a 1
            if (drawn.cardNumberVar == CardNumber.ace)
            {
                if (currentSum <= 10)
                {
                    currentSum += 11;
                }
                else
                {
                    currentSum += 1;
                }
            }

            //else if the card drawn is a jack queen or king return a 10
            else if (drawn.cardNumberVar == CardNumber.jack || drawn.cardNumberVar == CardNumber.queen || drawn.cardNumberVar == CardNumber.king)
            {
                currentSum += 10;
            }

            //else just return the number too the total
            else
            {
                currentSum += (int)drawn.cardNumberVar;
            }

            return currentSum;
        }
        #endregion Methods

    }
}
