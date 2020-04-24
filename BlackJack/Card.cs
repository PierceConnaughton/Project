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
        #region Properties

        Bitmap image;
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
            

       Bitmap cardImage {
            get
            {
                return this.image;
            }
        }

        #endregion Properies

        #region Constructors
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

        #endregion Constructors

        #region Methods
        public override string ToString()
        {
            //return the card num and card suit
            return string.Format("{0} of {1}",cardNumberVar.ToString(),cardSuitVar.ToString());
        }

        private void GetImage()
        {
            if (this.cardSuit != 0 && this.cardNumber != 0)//so it must be a valid card (see the Enums)
            {
                //starting point from the left
                int x = 0;
                //starting point from the top. Can be 0, 98, 196 and 294
                int y = 0;

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

                x = width * ((int)this.cardNumber - 1);


                //gets the big image of cards
                Bitmap source = new Bitmap(@"D:\College\Programming\ImageConversion-master\WpfApp1\cards.png");
                //sets the width and height of the card
                Bitmap img = new Bitmap(width, height);//this will be the created one for each card
                //Create graphics from the current card that can be used too edit
                Graphics g = Graphics.FromImage(img);
                //from the big card cut out the card you want from the selected height and width from all cards have and from the x axis and y axis we set earlier too find the card we want
                //and we use pixels as our measure of units
                g.DrawImage(source, new System.Drawing.Rectangle(0, 0, width, height), new System.Drawing.Rectangle(x, y, width, height), GraphicsUnit.Pixel);
                //gets rid of the resources we used
                g.Dispose();
                //set the image we created as the image for this card
                this.image = img;







            }
        }

        public Bitmap ReturnImage()
        {
            return image;
        }

        
        #endregion Methods

    }
}
