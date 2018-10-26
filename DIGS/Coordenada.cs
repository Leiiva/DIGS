using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIGS
{
    class Coordenada
    {
        public int x;
        public int y;

        public SurroundingClass(int c_x, int c_y)
        {
            x = c_x;
            y = c_y;
        }

        public string getX()
        {
            return x;
        }
        public string getY()
        {
            return y;
        }
    }
}
