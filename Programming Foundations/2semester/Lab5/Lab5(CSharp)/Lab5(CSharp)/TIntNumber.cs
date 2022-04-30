using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5_CSharp_
{
    internal class TIntNumber
    {
        #region Fields

        #endregion

        #region Properties
        public string Value { get; set; }
        public int Base { get; set; }
        #endregion

        #region Constructors
        protected TIntNumber(string v, int b)
        {
            Value = v;
            Base = b;
        }
        #endregion

        #region Methods
        public static TIntNumber operator++(TIntNumber intNumber)
        {
            int number = Convert.ToInt32(intNumber.Value, intNumber.Base);
            number++;
            intNumber.Value = Convert.ToString(number, intNumber.Base);
            return intNumber;
        }
        public static TIntNumber operator--(TIntNumber intNumber)
        {
            int number = Convert.ToInt32(intNumber.Value, intNumber.Base);
            number--;
            intNumber.Value = Convert.ToString(number, intNumber.Base);
            return intNumber;
        }
        public int ConvertToDecimal()
        {
            int number = Convert.ToInt32(this.Value, this.Base);
            return number;
        }
        public void Print() => Console.Write($"({Value} - {ConvertToDecimal()}) ");
        #endregion
    }
}
