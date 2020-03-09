using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;



namespace BlackJack
{
    public enum CardNumber
    {
        ace = 1,
        two = 2,
        three = 3,
        four = 4,
        five = 5,
        six = 6,
        seven = 7,
        eight = 8,
        nine = 9,
        ten = 10,
        jack = 11,
        queen = 12,
        king = 13
    }

    public enum CardSuit
    {
        hearts = 1,
        clubs = 2,
        spades = 3,
        diamond = 4
    }
    public class Card
    {

        Random rnd = new Random();

        public CardNumber cardNumberVar { get; set; }

        public CardSuit cardSuitVar { get; set; }

       Image cardImage { get; }

        
        public Card()
        {
            cardNumberVar = 0;
            cardSuitVar = 0;
            cardImage = null;
        }

        

        public override string ToString()
        {
            return string.Format("{0} of {1}",cardNumberVar.ToString(),cardSuitVar.ToString());
        }
    }
}
