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
        Image image;
        CardNumber cardNumber;
        CardSuit cardSuit;

        Random rnd = new Random();

        public CardNumber cardNumberVar {
            get
            {
                return this.cardNumber;
            }
            set
            {
                this.cardNumber = value;
                GetImage();
            }
        }

        public CardSuit cardSuitVar
        {
            get
            {
                return this.cardSuit;
            }
            set
            {
                this.cardSuit = value;
                GetImage();
            }
        }
            

       Image cardImage {
            get
            {
                return this.image;
            }
        }

        
        public Card()
        {
            cardNumber = 0;
            cardSuit = 0;
            image = null;
        }

        public Card(CardNumber cardnumber, CardSuit cardsuit)
        {
            cardSuit = cardsuit;
            cardNumber = cardnumber;
        }

        public override string ToString()
        {
            return string.Format("{0} of {1}",cardNumberVar.ToString(),cardSuitVar.ToString());
        }

        private void GetImage()
        {
            if (this.cardSuit != 0 && this.cardNumber != 0)//so it must be a valid card (see the Enums)
            {
                int x = 0;//starting point from the left
                int y = 0;//starting point from the top. Can be 0, 98, 196 and 294
                int height = 97;
                int width = 73;

                switch (this.cardSuit)
                {
                    case CardSuit.hearts:
                        y = 196;
                        break;
                    case CardSuit.spades:
                        y = 98;
                        break;
                    case CardSuit.clubs:
                        y = 0;
                        break;
                    case CardSuit.diamond:
                        y = 294;
                        break;
                }

                x = width * ((int)this.cardNumber - 1);//the Ace has the value of 1 (see the Enum), so the X coordinate will be the starting (first one), that's why we have to subtract 1. The card 6 has the total width of the first 6 cards (6*73=438) minus the total width of the first 5 cards (5*73=365). Of course it is 73. The starting X coordinate is at the end of the 5th card (or the start of the left side of the 6th card). Hope you understand. :)


                Bitmap source = new Bitmap(@"C:\Users\S00185812\OneDrive\College\Semester 4\Programming\Project\Project\BlackJack\Images\cards.png");//the original cards.png image
                Bitmap img = new Bitmap(width, height);//this will be the created one for each card
                Graphics g = Graphics.FromImage(img);
                g.DrawImage(source, new Rectangle(0, 0, width, height), new Rectangle(x, y, width, height), GraphicsUnit.Pixel);//here we slice the original into pieces
                g.Dispose();
                this.image = img;
            }
        }

        public Image DisplayImage()
        {
            return cardImage;
        }
    }
}
