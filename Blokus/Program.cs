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
        public static byte[] Board = new byte[400];
        //public byte[,] Board = new byte[20, 20];
        static void Main(string[] args)
        {

            List<IBlokusPlayer> Players = new List<IBlokusPlayer>();
            // Get some players from dll
            Players.Add(new BlokusTestPlayer());
            Players.Add(new BlokusTestPlayer());
            Players.Add(new BlokusTestPlayer());
            Players.Add(new BlokusTestPlayer());

            // Display Map
            BlokusGame g = new BlokusGame(Players);
            g.PrintGameState();
            char val = 'n';
            while (val == 'n')
            {
                Console.CursorLeft = 0;
                Console.CursorTop = 0;
                g.NextMove();
                g.PrintGameState();
                val = Console.ReadKey(true).KeyChar;
            }

        }
    }
}
