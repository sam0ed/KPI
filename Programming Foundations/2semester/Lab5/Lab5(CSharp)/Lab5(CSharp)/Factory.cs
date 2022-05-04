using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5_CSharp_
{
    internal abstract class Factory
    {
        #region Fields
        abstract public string Name { get; }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public abstract TIntNumber FactoryMethod(string val);
        #endregion
    }
}
