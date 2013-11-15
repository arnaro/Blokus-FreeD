using System;
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
        private List<BlokusPlayerState> mPlayerStates; 
        private int mCurrentPlayerIndex = -1;
        private IGameValidator mGameValidator = ValidatorFactory.GetValidator();

        public BlokusGame(List<IBlokusPlayer> pPlayers)
        {
            mGameState = new byte[mColumns * mRows];

            //AddRandomData();
            mPlayers = pPlayers;
            //Shuffle(mPlayers);
            mPlayerStates = mPlayers.Select(a => new BlokusPlayerState {Player = a, Pieces = PieceFactory.GetPieces(), PassLastTurn = false}).ToList();
        }

        public void PlayGame()
        {
            while (!IsGameOver())
            {
                NextMove();
            }
        }

        private void AddRandomData()
        {
            Random r = new Random();
            for (int i = 0; i < mGameState.Length; i++)
            {
                mGameState[i] = (byte) r.Next(0, 5);
            }
        }



        public void NextMove()
        {
            mCurrentPlayerIndex = (mCurrentPlayerIndex + 1) % mPlayerStates.Count;

            BlokusPlayerState currentPlayerState = mPlayerStates[mCurrentPlayerIndex];
            if (!currentPlayerState.PassLastTurn)
            {
                // Time limit? 2 sec
                BlokusGameState playerState = new BlokusGameState(mGameState, new List<IPiece>(currentPlayerState.Pieces));
                BlokusMove move = currentPlayerState.Player.PlayRound(playerState);
                //BlokusGameState newState = new BlokusGameState(move.BlokusBoard, currentPlayerState.Pieces);

                if (mGameValidator.Validate(currentPlayerState.Player.Id, move, playerState))
                {
                    //removing piece from available
                    currentPlayerState.Pieces.Remove(move.Piece);
                    //saving state
                    mGameState = move.BlokusBoard;

                }
                //validate failed setting player's move as passed
                else
                {
                    currentPlayerState.PassLastTurn = true;
                }
            }
        }

        internal int GetScore(int playerId, BlokusGameState state)
        {
            return state.BlokusBoard.Count(x => x == playerId);
        }

        public bool IsGameOver()
        {
            return mPlayerStates.All(a => a.PassLastTurn);
        }

        public void PrintScore()
        {
            var scoreList = mGameState.Where(a=> a > 0).GroupBy(a => a).Select(a => new {Id = a.Key, Score = a.Count()}).OrderByDescending(a => a.Score).ToList();
            
            mPlayers.ForEach(a => Console.WriteLine(a.Name + " " + ((scoreList.SingleOrDefault(b => b.Id == a.Id) == null) ? 0 : scoreList.Single(b => b.Id == a.Id).Score)));

            Console.SetCursorPosition(0, Console.CursorTop - scoreList.Count);
        }

        public List<IBlokusPlayer> GetWinners()
        {
            if (IsGameOver())
            {
                var scoreList = mGameState.Where(a => a > 0).GroupBy(a => a).Select(a => new { Id = a.Key, Score = a.Count() }).OrderByDescending(a => a.Score).ToList();
                if (scoreList.Count > 0)
                {
                    int max = scoreList.Max(a => a.Score);
                    return scoreList.Where(a => a.Score == max).Select(a => mPlayers.Single(b => b.Id == a.Id)).ToList();
                }
            }
            return new List<IBlokusPlayer>();
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

            PrintScore();
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

    public class BlokusPlayerState
    {
        public bool PassLastTurn { get; set; }

        public IBlokusPlayer Player { get; set; }

        public IList<IPiece> Pieces { get; set; }
    }
}
