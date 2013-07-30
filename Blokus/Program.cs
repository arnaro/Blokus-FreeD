using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blokus.Players;

namespace Blokus
{
    class Program
    {
        public byte[] Board = new byte[400];
        //public byte[,] Board = new byte[20, 20];
        static void Main(string[] args)
        {
            List<BlokusPlayer> Players = new List<BlokusPlayer>();
            Players.Add(new BlokusTestPlayer());
            Players.Add(new BlokusTestPlayer());
            Players.Add(new BlokusTestPlayer());
            Players.Add(new BlokusTestPlayer());
        }
    }
}
