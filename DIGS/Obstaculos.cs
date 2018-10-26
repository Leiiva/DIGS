using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIGS
{
    class Obstaculos
    {
        public int coordenadax;
        public int coordenaday;
        public int obs;

        public Obstaculos(int x, int y, int o)
        {
            coordenadax = x;
            coordenaday = y;
            obs = o;
        }

        public int getX()
        {
            return coordenadax;
        }
        public int getY()
        {
            return coordenaday;
        }
        public int getObs()
        {
            return obs;
        }
    }

}
