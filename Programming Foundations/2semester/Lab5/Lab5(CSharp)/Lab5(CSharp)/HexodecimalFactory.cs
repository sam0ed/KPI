using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5_CSharp_
{
    internal class HexodecimalFactory : Factory
    {
        public override string Name { get; }="Hexodecimal";
        #region Fields

        #endregion

        #region Constructors
        #endregion

        #region Methods
        #endregion
        public override TIntNumber FactoryMethod(string val)
        {
            return new TIntNumber16(val);
        }
    }
}
