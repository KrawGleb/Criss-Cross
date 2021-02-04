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
            if(gameRun)
            {
                int Id = Convert.ToInt32(id);
                int buttonI = Id / 10;
                int buttonJ = Id % 10;
                if(gameData.field[buttonI, buttonJ] == ' ')
                {
                    gameData.field[buttonI, buttonJ] = gameData.PlayerSide;
                    CheckWin(gameData.PlayerSide);
                    if (gameRun)
                    {
                        GetComputerMove();
                        CheckWin(gameData.CompSide);
                    }
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

        public ViewResult SwipeSide()
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
        static void CheckWin(char side)
        {
            for (int i = 0; i < 3; i++)
            {
                if(CheckLine(gameData.field[i, 0], gameData.field[i, 1], gameData.field[i, 2], side))
                {
                    gameData.background[i, 0] = gameData.background[i, 1] = gameData.background[i, 2] = "#eeb5b5";
                    gameRun = false;
                    return;
                }
                if (CheckLine(gameData.field[0, i], gameData.field[1, i], gameData.field[2, i], side))
                {
                    gameData.background[0, i] = gameData.background[1, i] = gameData.background[2, i] = "#eeb5b5";
                    gameRun = false;
                    return;
                }
            }
            if(CheckLine(gameData.field[2,0], gameData.field[1,1], gameData.field[0, 2], side))
            {
                gameData.background[2, 0] = gameData.background[1, 1] = gameData.background[0, 2] = "#eeb5b5";
                gameRun = false;
                return;
            }
            if (CheckLine(gameData.field[0, 0], gameData.field[1, 1], gameData.field[2, 2], side))
            {
                gameData.background[0, 0] = gameData.background[1, 1] = gameData.background[2, 2] = "#eeb5b5";
                gameRun = false;
                return;
            }
        }
        static bool CheckLine(char a1, char a2, char a3, char side)
        {
            if (a1 == side && a2 == side && a3 == side)
                return true;
            return false;
        }

        static bool CompWin(int i1, int j1, int i2, int j2, int i3, int j3, char side)
        {
            bool result = false;
            if(gameData.field[i1,j1] == side &&
                gameData.field[i2,j2] == side &&
                gameData.field[i3, j3] == ' ')
            {
                gameData.field[i3, j3] = gameData.CompSide;
                result = true;
            }

            if (gameData.field[i1, j1] == side &&
                gameData.field[i2, j2] == ' ' &&
                gameData.field[i3, j3] == side)
            {
                gameData.field[i2, j2] = gameData.CompSide;
                result = true;
            }
            if (gameData.field[i1, j1] == ' ' &&
                gameData.field[i2, j2] == side &&
                gameData.field[i3, j3] == side)
            {
                gameData.field[i1, j1] = gameData.CompSide;
                result = true;
            }
            return result;
        }
        public int CountFreeAngles()
        {
            int result = 0;
            if (gameData.field[0, 0] == ' ')
                result++;
            if (gameData.field[0, 2] == ' ')
                result++;
            if (gameData.field[2, 0] == ' ')
                result++;
            if (gameData.field[2, 2] == ' ')
                result++;
            return result;
        }
        public int[] FindPlayersMove()
        {
            int a = -1, b = -1;
            
            for (int n = 0; n < 3; n++)
            {
                if (gameData.field[n, 1] == gameData.PlayerSide)
                    a = n;
            }
            for (int t = 0; t < 3; t++)
            {
                if (gameData.field[1, t] == gameData.PlayerSide)
                    b = t;
            }
            int[] result = new int[] { a, b};
            return result;
        }

        public void GetComputerMove()
        {
            for (int n = 0; n < 3; n++)
            {
                if (CompWin(n, 0, n, 1, n, 2, gameData.CompSide))
                    return;
                if (CompWin(0, n, 1, n, 2, n, gameData.CompSide))
                    return;
            }
            if (CompWin(0, 0, 1, 1, 2, 2, gameData.CompSide))
                return;
            if (CompWin(2, 0, 1, 1, 0, 2, gameData.CompSide))
                return;

            for (int n = 0; n < 3; n++)
            {
                if (CompWin(n, 0, n, 1, n, 2, gameData.PlayerSide))
                    return;
                if (CompWin(0, n, 1, n, 2, n, gameData.PlayerSide))
                    return;
            }
            if (CompWin(0, 0, 1, 1, 2, 2, gameData.PlayerSide))
                return;
            if (CompWin(2, 0, 1, 1, 0, 2, gameData.PlayerSide))
                return;

            while (true)
            {
                if (IsFieldFull())
                    break;

                if (gameData.field[1, 1] == ' ')
                {
                    gameData.field[1, 1] = gameData.CompSide;
                    break;
                }

                if (CountFreeAngles() == 2 && gameData.field[1, 1] == gameData.CompSide)
                {
                    if (gameData.field[0, 1] == ' ')
                    {
                        gameData.field[0, 1] = gameData.CompSide;
                        break;
                    }

                    if (gameData.field[1, 0] == ' ')
                    {
                        gameData.field[1, 0] = gameData.CompSide;
                        break;
                    }
                    if (gameData.field[1, 2] == ' ')
                    {
                        gameData.field[1, 2] = gameData.CompSide;
                        break;
                    }
                    if (gameData.field[2, 1] == ' ')
                    {
                        gameData.field[2, 1] = gameData.CompSide;
                        break;
                    }
                }
                if (CountFreeAngles() == 4 && gameData.field[1, 1] == gameData.CompSide)
                {
                    int[] result = FindPlayersMove();
                    if (result[0] != -1 && result[1] != -1)
                    {
                        if (gameData.field[result[0], result[1]] == ' ')
                        {
                            gameData.field[result[0], result[1]] = gameData.CompSide;
                            break;
                        }
                    }
                }
                if (CountFreeAngles() == 3 && gameData.field[1, 1] == gameData.CompSide)
                {
                    int[] result = FindPlayersMove();
                    if (result[0] == -1 && result[1] != -1)
                    {
                        if (gameData.field[0, result[1]] == ' ')
                        {
                            gameData.field[0, result[1]] = gameData.CompSide;
                            break;
                        }
                        else if (gameData.field[2, result[1]] == ' ')
                        {
                            gameData.field[2, result[1]] = gameData.CompSide;
                            break;
                        }
                    }
                    if (result[0]!= -1 && result[1] == -1)
                    {
                        if (gameData.field[result[0], 0] == ' ')
                        {
                            gameData.field[result[0], 0] = gameData.CompSide;
                            break;
                        }
                        else if (gameData.field[result[0], 2] == ' ')
                        {
                            gameData.field[result[0], 2] = gameData.CompSide;
                            break;
                        }
                    }
                }

                if (gameData.field[0, 0] == ' '|| gameData.field[0, 2] == ' '||
                    gameData.field[2, 0] == ' ' || gameData.field[2, 2] == ' ')
                {
                    if (gameData.field[0, 0] == ' ')
                    {
                        gameData.field[0, 0] = gameData.CompSide;
                        break;
                    }
                        
                    if (gameData.field[0, 2] == ' ')
                    {
                        gameData.field[0, 2] = gameData.CompSide;
                        break;
                    }
                       
                    if (gameData.field[2, 0] == ' ')
                    {
                        gameData.field[2, 0] = gameData.CompSide;
                        break;
                    }
                        
                    if (gameData.field[2, 2] == ' ')
                    {
                        gameData.field[2, 2] = gameData.CompSide;
                        break;
                    }

                }
                else
                {
                    if (gameData.field[0, 1] == ' ')
                    {
                        gameData.field[0, 1] = gameData.CompSide;
                        break;
                    }

                    if (gameData.field[1, 0] == ' ')
                    {
                        gameData.field[1, 0] = gameData.CompSide;
                        break;
                    }

                    if (gameData.field[2, 1] == ' ')
                    {
                        gameData.field[2, 1] = gameData.CompSide;
                        break;
                    }

                    if (gameData.field[1, 2] == ' ')
                    {
                        gameData.field[1, 2] = gameData.CompSide;
                        break;
                    }
                    
                }
            }
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
        }
        #endregion
    }
}
