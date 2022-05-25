using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKR
{
    internal class Number
    {
        #region Fields
        public int Whole { get; set; }
        public int Decimal { get; set; }

        #endregion

        #region Constructors
        public Number()
        {
            Whole = 0;
            Decimal = 0;
        }
        public Number(string myNum)
        {
            SetNumber(myNum);
        }
        #endregion

        #region Methods
        public void printNum()
        {
            Console.WriteLine($"{Whole}.{Decimal}");
        }
        public void SetNumber(string myNum)
        {
            int dotIndex = myNum.IndexOf(".");
            Whole = int.Parse(myNum.Substring(0, dotIndex));
            Decimal = int.Parse(myNum.Substring(dotIndex+1));
        }
        public static Number operator ++(Number number)
        {
            number.Whole+=1;
            return number;
        }
        public static bool operator>(Number num1, Number num2)
        {
            if (num1.Whole > num2.Whole)
            {
                return true;
            }
            else if (num1.Whole == num2.Whole && num1.Decimal > num2.Decimal)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool operator <(Number num1, Number num2)
        {
            if (num1.Whole < num2.Whole)
            {
                return true;
            }
            else if (num1.Whole == num2.Whole && num1.Decimal < num2.Decimal)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
