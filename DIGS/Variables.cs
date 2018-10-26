using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIGS
{
    class Variables
    {
        private string Nombre;
        private string Valor;

        public Variables(string Nombre, string Valor)
        {
            this.Nombre = Nombre;
            this.Valor = Valor;
        }

        public string getNombre()
        {
            return Nombre;
        }

        public string getValor()
        {
            return Valor;
        }

        public void setValor(string val)
        {
            this.Valor = val;
        }
        public void setNombre(string nom)
        {
            this.Nombre = nom;
        }
    }
}
