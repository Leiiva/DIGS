using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIGS
{
    class Error
    {
        public enum Tipos
        {
            ERROR_LEXICO
        }

        public Tipos tipoError;
        private string valor;
        private int fila;
        private int columna;

        public Error(Tipos tipo, string auxEr, int fila, int columna)
        {
            this.tipoError = tipo;
            this.valor = auxEr;
            this.fila = fila;
            this.columna = columna;
        }

        public int getFila()
        {
            return fila;
        }

        public int getColumna()
        {
            return columna;
        }

        public string getValor()
        {
            return valor;
        }

        public string getTipoEnString()
        {
            switch (tipoError)
            {
                case Tipos.ERROR_LEXICO:
                    return "Elemento Lexico Desconocido ";
                default:
                    return "Desconocido   ";
            }
        }
    }
}
