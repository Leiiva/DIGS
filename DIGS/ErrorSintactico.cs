using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIGS
{
    class ErrorSintactico
    {
        public string tipo;
        public string numero;

        public ErrorSintactico(string c_x, string c_y)
        {
            tipo = c_x;
            numero = c_y;
        }
        public string getTipo()
        {
            return tipo;
        }
        public string getNumero()
        {
            return numero;
        }
    }
}
