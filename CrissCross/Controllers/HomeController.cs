using CrissCross.Models;
using Microsoft.AspNetCore.Mvc;
using System;


namespace CrissCross.Controllers
{
    public class HomeController : Controller
    {
        #region ForViews
        static bool gameRun = true;
        static Field gameData = new Field();
        public ViewResult Index()
        {
            return View("Main", gameData);
        }

        public ViewResult HandleButtonClick(string id)
        {
            if (gameRun)
            {
                int Id = Convert.ToInt32(id);
                int buttonI = Id / 10;
                int buttonJ = Id % 10;
                if (gameData.field[buttonI, buttonJ] == ' ')
                {
                    gameData.field[buttonI, buttonJ] = gameData.PlayerSide;
                    gameRun = !CheckWin(gameData.PlayerSide);
                    GetComputerMove();
                    gameRun = !CheckWin(gameData.CompSide);
                    CrossOut();
                }
            }
            return View("Main", gameData);
        }

        public ViewResult NewGame()
        {
            gameRun = true;
            Clear();
            if (gameData.CompSide == 'X')
                GetComputerMove();
            return View("Main", gameData);
        }

        public ViewResult Update()
        {
            return View("Main", gameData);
        }

        public ViewResult SwapSide()
        {
            gameRun = true;
            Clear();
            char swipe = gameData.PlayerSide;
            gameData.PlayerSide = gameData.CompSide;
            gameData.CompSide = swipe;
            if (gameData.CompSide == 'X')
                GetComputerMove();
            return View("Main", gameData);
        }
        #endregion

        #region Logic

        static bool CheckLine(char a1, char a2, char a3, char side)
        {
            if (a1 == side && a2 == side && a3 == side)
                return true;
            return false;
        }

        static void CrossOut()
        {
            for (int i = 0; i < 3; i++)
            {
                if (CheckLine(gameData.field[i, 0], gameData.field[i, 1], gameData.field[i, 2], gameData.PlayerSide) || CheckLine(gameData.field[i, 0], gameData.field[i, 1], gameData.field[i, 2], gameData.CompSide))
                {
                    gameData.background[i, 0] = gameData.background[i, 1] = gameData.background[i, 2] = "#eeb5b5";
                    gameRun = false;
                    return;
                }
                if (CheckLine(gameData.field[0, i], gameData.field[1, i], gameData.field[2, i], gameData.PlayerSide) || CheckLine(gameData.field[0, i], gameData.field[1, i], gameData.field[2, i], gameData.CompSide))
                {
                    gameData.background[0, i] = gameData.background[1, i] = gameData.background[2, i] = "#eeb5b5";
                    gameRun = false;
                    return;
                }
            }
            if (CheckLine(gameData.field[2, 0], gameData.field[1, 1], gameData.field[0, 2], gameData.PlayerSide) || CheckLine(gameData.field[2, 0], gameData.field[1, 1], gameData.field[0, 2], gameData.CompSide))
            {
                gameData.background[2, 0] = gameData.background[1, 1] = gameData.background[0, 2] = "#eeb5b5";
                gameRun = false;
                return;
            }
            if (CheckLine(gameData.field[0, 0], gameData.field[1, 1], gameData.field[2, 2], gameData.PlayerSide) || CheckLine(gameData.field[0, 0], gameData.field[1, 1], gameData.field[2, 2], gameData.CompSide))
            {
                gameData.background[0, 0] = gameData.background[1, 1] = gameData.background[2, 2] = "#eeb5b5";
                gameRun = false;
                return;
            }

        }

        static void GetComputerMove()
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
                    if (gameData.field[row, col] == ' ')
                    {
                        gameData.field[row, col] = gameData.CompSide;
                        int newScore = MiniMax(false);
                        Console.WriteLine(newScore);
                        if (newScore > bestScore)
                        {
                            bestScore = newScore;
                            bestMove[0] = row;
                            bestMove[1] = col;
                        }
                        gameData.field[row, col] = ' ';
                    }
                }
            }

            gameData.field[bestMove[0], bestMove[1]] = gameData.CompSide;
        }
        static bool IsFieldFull()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (gameData.field[i, j] == ' ')
                        return false;
                }
            }
            return true;
        }
        static void Clear()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    gameData.field[i, j] = ' ';
                    gameData.background[i, j] = "#f8f9fa";
                }
            }
            gameData.IsFieldClear = true;
        }
        #endregion

        static bool CheckWin(char side)
        {
            for (int n = 0; n < 3; n++)
            {
                if (CheckLine(gameData.field[0, n], gameData.field[1, n], gameData.field[2, n], side) ||
                    CheckLine(gameData.field[n, 0], gameData.field[n, 1], gameData.field[n, 2], side))
                {
                    return true;
                }
            }

            if (CheckLine(gameData.field[0, 0], gameData.field[1, 1], gameData.field[2, 2], side) ||
                CheckLine(gameData.field[0, 2], gameData.field[1, 1], gameData.field[2, 0], side))
            {
                return true;
            }
            return false;
        }

        static bool IsGameRun()
        {
            if (CheckWin(gameData.PlayerSide) || CheckWin(gameData.CompSide))
            {
                return false;
            }

            if (IsFieldFull())
                return false;

            return true;
        }

        static int MiniMax(bool compMove)
        {
            if (!IsGameRun())
            {
                return Rate();
            }
            int bestScore;

            if (compMove)
            {
                bestScore = Int32.MinValue;

                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        if (gameData.field[row, col] == ' ')
                        {
                            gameData.field[row, col] = gameData.CompSide;
                            int score = MiniMax(!compMove);
                            gameData.field[row, col] = ' ';
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
                        if (gameData.field[row, col] == ' ')
                        {
                            gameData.field[row, col] = gameData.PlayerSide;
                            int score = MiniMax(!compMove);
                            gameData.field[row, col] = ' ';
                            bestScore = bestScore < score ? bestScore : score;
                        }
                    }
                }

            }

            return bestScore;
        }

        static int Rate()
        {
            if (CheckWin(gameData.CompSide))
            {
                return 1000;
            }
            if (CheckWin(gameData.PlayerSide))
            {
                return -1000;
            }

            return 0;
        }
    }
}
