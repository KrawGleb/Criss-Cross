using CrissCross.Models;
using CrissCross.GameLogic; 
using Microsoft.AspNetCore.Mvc;
using System;


namespace CrissCross.Controllers
{
    public class HomeController : Controller
    {
        #region ForViews
        
        public ViewResult Index()
        {
            return View("Main", FieldHandler.gameData);
        }

        public ViewResult HandleButtonClick(string id)
        {
            if (FieldHandler.gameRun)
            {
                int Id = Convert.ToInt32(id);
                int buttonI = Id / 10;
                int buttonJ = Id % 10;
                if (FieldHandler.gameData._field[buttonI, buttonJ] == ' ')
                {
                    FieldHandler.gameData._field[buttonI, buttonJ] = FieldHandler.gameData._playerSide;
                    FieldHandler.gameRun = !FieldHandler.CheckWin(FieldHandler.gameData._playerSide);
                    FieldHandler.GetComputerMove();
                    FieldHandler.gameRun = !FieldHandler.CheckWin(FieldHandler.gameData._compSide);
                    FieldHandler.CrossOut();
                }
            }
            return View("Main", FieldHandler.gameData);
        }

        public ViewResult NewGame()
        {
            FieldHandler.gameRun = true;
            FieldHandler.Clear();
            if (FieldHandler.gameData._compSide == 'X')
                FieldHandler.GetComputerMove();
            return View("Main", FieldHandler.gameData);
        }

        public ViewResult Update()
        {
            return View("Main", FieldHandler.gameData);
        }

        public ViewResult SwapSide()
        {
            FieldHandler.gameRun = true;
            char swipe = FieldHandler.gameData._playerSide;
            FieldHandler.gameData._playerSide = FieldHandler.gameData._compSide;
            FieldHandler.gameData._compSide = swipe;
            return NewGame();    
        }
        #endregion
        
    }
}
