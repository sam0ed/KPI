using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5_CSharp_
{
    internal class BinaryFactory : Factory
    {
        #region Fields
        public override string Name { get; } = "Binary";
        #endregion

        #region Constructors
        #endregion

        #region Methods

        #endregion
        public override TIntNumber FactoryMethod(string val)
        {
            return new TIntNumber2(val);
        }
    }
}
