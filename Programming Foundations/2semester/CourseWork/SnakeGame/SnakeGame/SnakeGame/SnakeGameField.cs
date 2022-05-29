using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    internal class SnakeGameField
    {
        #region Fields
        private int[,] _field;
        public int[,] Field { get { return _field; } }  

        #endregion

        #region Constructors
        public SnakeGameField()
        {
            _field = new int[15,15];
            for (int i = 0; i < Field.GetLength(0); i++)
            {
                for (int j = 0; j < Field.GetLength(1); j++)
                {
                    Field[i, j] = 0;
                }
            }
        }
        #endregion

        #region Methods
        #endregion
    }
}
