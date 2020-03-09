using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public class Player : IComparable
    {
        public string PlayerName { get; set; }

        public int Wins { get; set; }

        public int Losses { get; set; }

        public int Draws { get; set; }

        public string DateOfLastGame { get; set; }

        #region Constructors
        public Player()
        {

        }

        public Player(string playerName, int wins, int losses, int draws, string dateOfLastGame)
        {
            PlayerName = playerName;
            Wins = wins;
            Losses = losses;
            Draws = draws;
            DateOfLastGame = dateOfLastGame;
        }
        #endregion Constructors


        #region Methods
        public override string ToString()
        {

            return String.Format("{0,-35}{1,-14}{2,-14}{3,-14}", PlayerName, Wins, Losses, Draws);
        }

        public int CompareTo(object obj)
        {
            //compare object scores too each other how we sort the list by score
            Player playerObj = (Player)obj;

            return this.Wins.CompareTo(playerObj.Wins);
        }
        #endregion Methods

    }
}
