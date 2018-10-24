using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIGS
{
    class Token
    {
        public enum Tipo
        {
            RESERVADA,
            NUMEROS,
            DOSPUNTOS,
            LLAVEAPERTURA, 
            LLAVECIERRE,
            CORCHETEAPERTURA,
            CORCHETECIERRE,
            IDENTIFICADOR,
            PARENTESISAPERTURA,
            PARENTESISCIERRE,
            PUNTOYCOMA,
            PUNTO,
            COMA,
            ARITEMETICOSUMA,
            ARITMETICORESTA,
            ARITMETICODIVISION,
            ARITMETICOMULTIPLICACION,
            OTROS,
            ARITMETICOIGUAL,
        }

        public Tipo tipoToken;
        public string valor;
        private int fila;
        private int columna;

        public Token(Tipo tipo, string auxLex, int fila, int columna)
        {
            this.tipoToken = tipo;
            this.valor = auxLex;
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

        public string TipoString()
        {
            switch (tipoToken)
            {
                case Tipo.RESERVADA:
                    return "Reservada";
                case Tipo.NUMEROS:
                    return "Numeros";
                case Tipo.DOSPUNTOS:
                    return "Dos Puntos";
                case Tipo.LLAVEAPERTURA:
                    return "Llave Apertura";
                case Tipo.LLAVECIERRE:
                    return "Llave Cierre";
                case Tipo.CORCHETEAPERTURA:
                    return "Corchete Apertura";
                case Tipo.CORCHETECIERRE:
                    return "Corchete Cierre";
                case Tipo.IDENTIFICADOR:
                    return "Identificadores";
                case Tipo.PARENTESISAPERTURA:
                    return "Parentesis Apertura";
                case Tipo.PARENTESISCIERRE:
                    return "Parentesis Cierre";
                case Tipo.PUNTOYCOMA:
                    return "Punto y Coma";
                case Tipo.PUNTO:
                    return "Punto";
                case Tipo.COMA:
                    return "Coma";
                case Tipo.ARITEMETICOSUMA:
                    return "Simbolo Aritmetico de Suma";
                case Tipo.ARITMETICODIVISION:
                    return "Simbolo Aritmetico de Division";
                case Tipo.ARITMETICORESTA:
                    return "Simbolo Aritmetico de Resta";
                case Tipo.ARITMETICOMULTIPLICACION:
                    return "Simbolo Aritmetico de Multiplicacion";
                case Tipo.ARITMETICOIGUAL:
                    return "Simbolo Aritmetico de Igual";
                case Tipo.OTROS:
                    return "Otros";
                default:
                    return "Desconocido";
            }

        }
    }
}
