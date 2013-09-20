using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blokus
{

    public class BlokusGame
    {
        private int mColumns = 20, mRows = 20;
        private byte[] mGameState;
        private List<IBlokusPlayer> mPlayers; 
        public BlokusGame(List<IBlokusPlayer> pPlayers)
        {
            mGameState = new byte[mColumns * mRows];
            mPlayers = pPlayers;
            Shuffle(mPlayers);
        }

        public void NextMove()
        {
            
        }

        public void PrintGameState()
        {
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.Gray;
            // draw border

            for (int i = 0; i < mGameState.Length; i++)
            {
                Console.ForegroundColor = GetColor(i);
                Console.Write( mGameState[i] > 0 ? ((char) 0x25a0).ToString(): " ");// 2588 fyrir solid
                if ((i + 1)%mColumns == 0)
                {
                    Console.WriteLine();
                }
            }
        }

        private ConsoleColor GetColor(int index)
        {
            switch (mGameState[index])
            {
                case 1:
                    return ConsoleColor.Blue;
                case 2:
                    return ConsoleColor.Red;
                case 3:
                    return ConsoleColor.Green;
                case 4:
                    return ConsoleColor.Yellow;
                default:
                    return ConsoleColor.Black;
            }
        }

        private void Shuffle<T>(List<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
