using System;

namespace CrissCross.Models
{
    public class GameData
    {
        public char _playerSide = 'X';
        public char _compSide = 'O';
        public bool _isFieldClear = true;

        public char[,] _field =
        {
            {' ', ' ', ' ' },
            {' ', ' ', ' ' },
            {' ', ' ', ' ' }
        };

        public string[,] _background =
        {
            {"#f8f9fa", "#f8f9fa", "#f8f9fa" },
            {"#f8f9fa", "#f8f9fa", "#f8f9fa" },
            {"#f8f9fa", "#f8f9fa", "#f8f9fa" }
        };
       
    }
}
