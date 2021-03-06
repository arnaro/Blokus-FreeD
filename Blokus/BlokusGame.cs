﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using log4net;

namespace Blokus
{
    public class BlokusGame
    {
        private ILog mLogger = LogManager.GetLogger(typeof(BlokusGame));


        private int mPlayerTurnMaxTimeMs = 2000;
        private int mColumns = 20, mRows = 20;
        private byte[] mGameState;
        public List<IBlokusPlayer> mPlayers;
        public List<BlokusPlayerState> mPlayerStates; 
        private int mCurrentPlayerIndex = -1;
        private IGameValidator mGameValidator = ValidatorFactory.GetValidator();

        public BlokusGame(List<IBlokusPlayer> pPlayers)
        {
            mGameState = new byte[mColumns * mRows];

            //AddRandomData();
            mPlayers = pPlayers;

            mPlayerStates = mPlayers.Select(a => new BlokusPlayerState {Player = a, Pieces = PieceFactory.GetPieces(), PassLastTurn = false}).ToList();
            mLogger.InfoFormat("Starting new game. Players: {0}", string.Join(",", mPlayers.Select(a=> a.Name)));
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
            DateTime dt = DateTime.Now;
            if (!currentPlayerState.PassLastTurn)
            {
                try
                {
                    // Assuming player will fail.
                    currentPlayerState.PassLastTurn = true;
                    BlokusGameState playerState = new BlokusGameState(mGameState, new List<IPiece>(currentPlayerState.Pieces));
                    BlokusMove move = null;
                    Thread playerMove = new Thread(() => 
                        {
                            move = currentPlayerState.Player.PlayRound(playerState);
                        }
                    );
                    playerMove.Start();
                    bool success = playerMove.Join(TimeSpan.FromMilliseconds(mPlayerTurnMaxTimeMs));
                    bool madeMove = move != null;
                    bool validMove = madeMove && mGameValidator.Validate(currentPlayerState.Player.Id, move, playerState);
                    if (success && move != null && validMove)
                    {
                        //removing piece from available
                        currentPlayerState.Pieces.Remove(move.Piece);
                        //saving state
                        mGameState = move.BlokusBoard;
                        //player no fail!
                        currentPlayerState.PassLastTurn = false;

                    }
                    else if (!success)
                    {
                        mLogger.DebugFormat("{0} failed to make a move within the allowed time period ({1}ms). Will not be allowed to make more moves.", currentPlayerState.Player.Name, mPlayerTurnMaxTimeMs);
                    }
                    else if (!madeMove)
                    {
                        mLogger.DebugFormat("{0} passed.", currentPlayerState.Player.Name);
                    }
                    else if (!validMove)
                    {
                        mLogger.DebugFormat("{0} attempted to make an invlaid move. Will not be allowed to make more moves.", currentPlayerState.Player.Name);
                    }
                }
                catch (Exception ex)
                {
                    mLogger.Error(string.Format("Error in {0}", currentPlayerState.Player.Name), ex);
                }
                mLogger.DebugFormat("{0, -20} finished turn in {1}ms", currentPlayerState.Player.Name, (DateTime.Now - dt).TotalMilliseconds);
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

        public int ScoreGame(int playerId)
        {
            int score = 0;
            BlokusPlayerState state = mPlayerStates.ToList().Where(a => a.Player.Id == playerId).FirstOrDefault();
            if (state != null)
            {
                List<IPiece> remainingPieces = state.Pieces.ToList();
                remainingPieces.ForEach(a =>
                {
                    score -= a.GetScore();
                });
                if (remainingPieces.Count() == 0)
                {
                    score += 15;
                    //BlokusMove lastmove = mPlayers.Where(b => b.Id == playerId).FirstOrDefault().Moves.Last();
                    //if (lastmove.Piece.Equals(PieceFactory.GetPieces().ElementAt(0)))
                    //{
                    //    score += 5;
                    //}
                }
            }
            return score;
        }

        public void PrintScore()
        {
            var scoreList = mGameState.Where(a=> a > 0).GroupBy(a => a).Select(a => new {Id = a.Key, Score = a.Count()}).OrderByDescending(a => a.Score).ToList();
           
            mPlayers.ForEach(a => Console.WriteLine(a.Name + " " + ScoreGame(a.Id)+"     "));

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
    }

    public class BlokusPlayerState
    {
        public bool PassLastTurn { get; set; }

        public IBlokusPlayer Player { get; set; }

        public IList<IPiece> Pieces { get; set; }
    }
}
