using System;

namespace CrissCross.Models
{
    public class Field
    {
        public char PlayerSide = 'X';
        public char CompSide = 'O';

        public char[,] field =
        {
            {' ', ' ', ' ' },
            {' ', ' ', ' ' },
            {' ', ' ', ' ' }
        };

        public string[,] background =
        {
            {"#f8f9fa", "#f8f9fa", "#f8f9fa" },
            {"#f8f9fa", "#f8f9fa", "#f8f9fa" },
            {"#f8f9fa", "#f8f9fa", "#f8f9fa" }
        };
       
    }
}
