﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blokus.Model;

namespace Blokus
{

    public class BlokusGame
    {
        private int mColumns = 20, mRows = 20;
        private byte[] mGameState;
        private List<IBlokusPlayer> mPlayers;
        private int mCurrentPlayerIndex = -1;

        private Dictionary<IBlokusPlayer, IList<IPiece>> mPieceDictionary = new Dictionary<IBlokusPlayer, IList<IPiece>>();

        public BlokusGame(List<IBlokusPlayer> pPlayers)
        {
            mGameState = new byte[mColumns * mRows];

            //AddRandomData();

            mPlayers = pPlayers;
            //Shuffle(mPlayers);

            mPlayers.ForEach(a=> mPieceDictionary.Add(a, PieceFactory.GetPieces()));

        }

        private void AddRandomData()
        {
            Random r = new Random();
            for (int i = 0; i < mGameState.Length; i++)
            {
                mGameState[i] = (byte) r.Next(0, 5);
            }
        }

        public bool Validate(BlokusGameState newState, IBlokusPlayer player)
        {
            byte[] changes = new byte[400];
            for (int i = 0; i < 400; i++)
            {
                if (mGameState[i] != newState.BlokusBoard[i])
                {
                    changes[i] = 1;
                }

            }
            int minx = 20;
            int maxx = 0;
            int miny = 20;
            int maxy = 0;
            int u = 0;
            int v = 0;
            while (u < 20 && v < 20)
            {
                if (changes[20 * v + u] == 1)
                {
                    if (u < minx) minx = u;
                    if (u > maxx) maxx = u;
                    if (v < miny) miny = v;
                    if (v > maxy) maxy = v;
                }
                u++;
                if (u >= 20)
                {
                    u = 0;
                    v++;
                }
            }

            int height = (maxy - miny) + 1;
            int width = (maxx - minx) + 1;
            int maxdim = height > width ? height : width;
            byte[,] piece = new byte[maxdim, maxdim];

            for (u = minx; u <= maxx; u++)
            {
                for (v = miny; v <= maxy; v++)
                {

                    piece[v - miny, u - minx] = changes[v * 20 + u];
                }
            }

            return IsPiece(newState.AvailablePieces.ToList(),new Piece(piece));
        }
        public bool IsPiece(List<IPiece> pieces, IPiece piece)
        {
            bool found = false;
            pieces.ForEach(a =>
            {
                if (a.Equals(piece))
                {
                    found = true;
                }
            });
            return found;
        }

        public void NextMove()
        {
            mCurrentPlayerIndex = (mCurrentPlayerIndex + 1) % 4;

            IBlokusPlayer player = mPlayers[mCurrentPlayerIndex];

            // Time limit? 2 sec

            BlokusGameState playerState = new BlokusGameState(mGameState, mPieceDictionary[player]);
                
            BlokusGameState newState = player.PlayRound(playerState);

            bool isValid = false;
            // bool isValid= Validate(player, mGameState, newState);
              // should validate remote piece?
            // save as new state
        }

        public bool IsCorrectPlayerOnEmptySpace(IBlokusPlayer player, BlokusGameState newState, BlokusGameState oldState)
        {
            List<byte> diff = newState.BlokusBoard.Select((a, i) => (byte) (a - oldState.BlokusBoard[i])).ToList();
            return diff.All(a => a == 0 || a == player.Id);
        }

        public void PrintGameState()
        {
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.Gray;
            // draw border

            for (int i = 0; i < mGameState.Length; i++)
            {
                Console.ForegroundColor = GetColor(mGameState[i]);
                Console.Write( mGameState[i] > 0 ? ((char) 0x25a0).ToString(): " ");// 2588 fyrir solid // 25a0 fyrir semi
                if ((i + 1)%mColumns == 0)
                {
                    Console.WriteLine();
                }
            }

            for (int i = 0; i < mPlayers.Count; i++)
            {
                Console.CursorLeft = mColumns + 5;
                Console.CursorTop = 2 + 2*i;
                Console.ForegroundColor = GetColor(i + 1);
                Console.Write(mPlayers[i].Name);
            }

            Console.CursorLeft = 0;
            Console.CursorTop = mRows + 1;
        }

        private ConsoleColor GetColor(int number)
        {
            switch (number)
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
