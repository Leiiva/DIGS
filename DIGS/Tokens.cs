using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace DIGS
{
    class Tokens
    {
        private Tipo TipoToken;
        private string Texto;
        private int fila;
        private int columna;
        public int Tamaño;
        public Color colorcito;

        public Tokens(Tipo tipo, string auxiliar, int filasa, int columnasa, Color coloraso, int tamañaso)
        {
            this.TipoToken = tipo;
            this.Texto = auxiliar;
            this.fila = filasa;
            this.columna = columnasa;
            this.colorcito = coloraso;
            this.Tamaño = tamañaso;
        }

        public Tokens(Tipo tipo, string auxLex, int fila, int columna)
        {
            this.fila = fila;
            this.columna = columna;
        }

        public Color getColor() =>
            this.colorcito;

        public int getColumna() =>
            this.columna;

        public int getFila() =>
            this.fila;

        public string getTamaño() =>
            Convert.ToString(this.Tamaño);

        public string getTexto() =>
            this.Texto;

        public Tipo getTipo() =>
            this.TipoToken;

        public string getTipoString()
        {
            string str;
            switch (this.TipoToken)
            {
                case Tipo.BLOQUE_LABERINTO:
                    str = "Bloque de Laberinto";
                    break;

                case Tipo.BLOQUE_RUTA:
                    str = "Bloque de Ruta";
                    break;

                case Tipo.CAMINATA:
                    str = "Caminata";
                    break;

                case Tipo.CASILLA:
                    str = "Casilla";
                    break;

                case Tipo.COMA:
                    str = "Coma";
                    break;

                case Tipo.CORCHETE_ABIERTO:
                    str = "Corchete Abierto";
                    break;

                case Tipo.CORCHETE_CERRADO:
                    str = "Corchete Cerrado";
                    break;

                case Tipo.DIMENSIONES:
                    str = "Dimensiones";
                    break;

                case Tipo.DIVIDIR:
                    str = "Signo Dividir";
                    break;

                case Tipo.DOS_PUNTOS:
                    str = "Dos puntos";
                    break;

                case Tipo.ERR:
                    str = "Error Caracter Desconocido";
                    break;

                case Tipo.IDENTIFICADOR:
                    str = "Identificador";
                    break;

                case Tipo.IGUAL:
                    str = "Igual";
                    break;

                case Tipo.INTERVALO:
                    str = "Intervalo";
                    break;

                case Tipo.LLAVE_ABIERTA:
                    str = "Llave Abierta";
                    break;

                case Tipo.LLAVE_CERRADA:
                    str = "Llave Cerrada";
                    break;

                case Tipo.MAS:
                    str = "Mas";
                    break;

                case Tipo.MENOS:
                    str = "Menos";
                    break;

                case Tipo.NUMERO_ENTERO:
                    str = "Numero Entero";
                    break;

                case Tipo.OBSTACULOS:
                    str = "Obstaculos";
                    break;

                case Tipo.PARENTESIS_ABIERTO:
                    str = "Parentesis Abierto";
                    break;

                case Tipo.PARENTESIS_CERRADO:
                    str = "Parentesis Cerrado";
                    break;

                case Tipo.PASO:
                    str = "Paso";
                    break;

                case Tipo.POTENCIA:
                    str = "Signo de Potencia";
                    break;

                case Tipo.POR:
                    str = "Signo Por";
                    break;

                case Tipo.PRINCIPAL:
                    str = "Principal";
                    break;

                case Tipo.PUNTO:
                    str = "Punto";
                    break;

                case Tipo.PUNTO_Y_COMA:
                    str = "Punto y Coma";
                    break;

                case Tipo.RANGO_CASILLA:
                    str = "Rango de Casilla";
                    break;

                case Tipo.RUTA:
                    str = "Ruta";
                    break;

                case Tipo.UBICACION_PERSONAJE:
                    str = "Ubicaci\x00f3n Personaje";
                    break;

                case Tipo.UBICACION_TESORO:
                    str = "Ubicaci\x00f3n Tesoro";
                    break;

                case Tipo.ULTIMO:
                    str = "Ultimo";
                    break;

                case Tipo.VARIABLE:
                    str = "Variable";
                    break;

                default:
                    str = "Error Caracter Desconocido";
                    break;
            }
            return str;
        }

        public enum Tipo
        {
            BLOQUE_LABERINTO,
            BLOQUE_RUTA,
            CAMINATA,
            CASILLA,
            COMA,
            CORCHETE_ABIERTO,
            CORCHETE_CERRADO,
            DIMENSIONES,
            DIVIDIR,
            DOS_PUNTOS,
            ERR,
            IDENTIFICADOR,
            IGUAL,
            INTERVALO,
            LLAVE_ABIERTA,
            LLAVE_CERRADA,
            MAS,
            MENOS,
            NUMERO_ENTERO,
            OBSTACULOS,
            PARENTESIS_ABIERTO,
            PARENTESIS_CERRADO,
            PASO,
            POTENCIA,
            POR,
            PRINCIPAL,
            PUNTO,
            PUNTO_Y_COMA,
            RANGO_CASILLA,
            RUTA,
            UBICACION_PERSONAJE,
            UBICACION_TESORO,
            ULTIMO,
            VARIABLE,
            ARITEMETICOSUMA,
            ARITMETICORESTA,
            ARITMETICOMULTIPLICACION,
            ARITMETICODIVISION
        }
    }
}
