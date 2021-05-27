using CrissCross.Models;
using System;

namespace CrissCross.GameLogic
{
    public static class FieldHandler
    {
        public static bool gameRun = true;
        public static GameData gameData = new GameData();

        
        static int MiniMax(bool compMove)
        {
            if (!IsGameRun())
            {
                return EstimatePosition();
            }
            int bestScore;

            if (compMove)
            {
                bestScore = Int32.MinValue;

                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        if (gameData._field[row, col] == ' ')
                        {
                            gameData._field[row, col] = gameData._compSide;
                            int score = MiniMax(!compMove);
                            gameData._field[row, col] = ' ';
                            bestScore = bestScore > score ? bestScore : score;
                        }
                    }
                }
            }
            else
            {
                bestScore = Int32.MaxValue;

                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        if (gameData._field[row, col] == ' ')
                        {
                            gameData._field[row, col] = gameData._playerSide;
                            int score = MiniMax(!compMove);
                            gameData._field[row, col] = ' ';
                            bestScore = bestScore < score ? bestScore : score;
                        }
                    }
                }

            }

            return bestScore;
        }
        public static void CrossOut()
        {
            for (int i = 0; i < 3; i++)
            {
                if (CheckLine(gameData._field[i, 0], gameData._field[i, 1], gameData._field[i, 2], gameData._playerSide) || CheckLine(gameData._field[i, 0], gameData._field[i, 1], gameData._field[i, 2], gameData._compSide))
                {
                    gameData._background[i, 0] = gameData._background[i, 1] = gameData._background[i, 2] = "#eeb5b5";
                    gameRun = false;
                    return;
                }
                if (CheckLine(gameData._field[0, i], gameData._field[1, i], gameData._field[2, i], gameData._playerSide) || CheckLine(gameData._field[0, i], gameData._field[1, i], gameData._field[2, i], gameData._compSide))
                {
                    gameData._background[0, i] = gameData._background[1, i] = gameData._background[2, i] = "#eeb5b5";
                    gameRun = false;
                    return;
                }
            }
            if (CheckLine(gameData._field[2, 0], gameData._field[1, 1], gameData._field[0, 2], gameData._playerSide) || CheckLine(gameData._field[2, 0], gameData._field[1, 1], gameData._field[0, 2], gameData._compSide))
            {
                gameData._background[2, 0] = gameData._background[1, 1] = gameData._background[0, 2] = "#eeb5b5";
                gameRun = false;
                return;
            }
            if (CheckLine(gameData._field[0, 0], gameData._field[1, 1], gameData._field[2, 2], gameData._playerSide) || CheckLine(gameData._field[0, 0], gameData._field[1, 1], gameData._field[2, 2], gameData._compSide))
            {
                gameData._background[0, 0] = gameData._background[1, 1] = gameData._background[2, 2] = "#eeb5b5";
                gameRun = false;
                return;
            }

        }
        public static void GetComputerMove()
        {
            if (!IsGameRun())
            {
                return;
            }
            int[] bestMove = { -1, -1 };
            int bestScore = Int32.MinValue;

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (gameData._field[row, col] == ' ')
                    {
                        gameData._field[row, col] = gameData._compSide;
                        int newScore = MiniMax(false);
                        Console.WriteLine(newScore);
                        if (newScore > bestScore)
                        {
                            bestScore = newScore;
                            bestMove[0] = row;
                            bestMove[1] = col;
                        }
                        gameData._field[row, col] = ' ';
                    }
                }
            }

            gameData._field[bestMove[0], bestMove[1]] = gameData._compSide;
        }
        static bool IsFieldFull()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (gameData._field[i, j] == ' ')
                        return false;
                }
            }
            return true;
        }
        public static void Clear()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    gameData._field[i, j] = ' ';
                    gameData._background[i, j] = "#f8f9fa";
                }
            }
            gameData._isFieldClear = true;
        }
        public static bool CheckWin(char side)
        {
            for (int n = 0; n < 3; n++)
            {
                if (CheckLine(gameData._field[0, n], gameData._field[1, n], gameData._field[2, n], side) ||
                    CheckLine(gameData._field[n, 0], gameData._field[n, 1], gameData._field[n, 2], side))
                {
                    return true;
                }
            }

            if (CheckLine(gameData._field[0, 0], gameData._field[1, 1], gameData._field[2, 2], side) ||
                CheckLine(gameData._field[0, 2], gameData._field[1, 1], gameData._field[2, 0], side))
            {
                return true;
            }
            return false;
        }
        static bool CheckLine(char a1, char a2, char a3, char side)
        {
            if (a1 == side && a2 == side && a3 == side)
                return true;
            return false;
        }
        static bool IsGameRun()
        {
            if (CheckWin(gameData._playerSide) || CheckWin(gameData._compSide))
            {
                return false;
            }

            if (IsFieldFull())
                return false;

            return true;
        }
        static int EstimatePosition()
        {
            if (CheckWin(gameData._compSide))
            {
                return 1000;
            }
            if (CheckWin(gameData._playerSide))
            {
                return -1000;
            }

            return 0;
        }
    }
}
