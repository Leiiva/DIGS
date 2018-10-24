using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DIGS.Token;
using static DIGS.Error;

namespace DIGS
{
    class AnalizadorLexico
    {
        private int comodin;
        private int contador;
        public static AnalizadorLexico k;

        public static AnalizadorLexico devolver()
        {
            if ((k == null))
            {
                k = new AnalizadorLexico();
            }

            return k;
        }

        public List<Token> salida;
        public List<Error> salida2;
        private int estado;
        private string auxLex;
        private string auxEr;
        private int columna;
        private int aux;
        private bool hay_error = false;

        public void addToken(Tipo tipo, int fila)
        {

            salida.Add(new Token(tipo, auxLex, fila, columna));

        }

        public void addError(Tipos tipo, int fila)
        {
            salida2.Add(new Error(tipo, auxEr, fila, columna));
            auxEr = "";
            estado = 0;
        }

        public List<Token> analizar(string entrada, int Fila)
        {
            entrada = entrada + "#";
            salida = new List<Token>();
            estado = 0;
            columna = 0;
            auxLex = "";
            comodin = 0;
            int inicio = 0;
            char c;

            for (int PosColumna = 0; PosColumna <= (entrada.Length - 1); PosColumna++)
            {
                inicio = PosColumna;
                char[] cadena = entrada.ToCharArray();
                c = cadena[PosColumna];
                switch (estado)
                {
                    case 0:
                        if (char.IsDigit(c))
                        {
                            estado = 2;
                            auxLex = auxLex + c;
                        }
                        else if (char.IsLetter(c))
                        {
                            estado = 1;
                            auxLex = auxLex + c;
                        }
                        else if (c.Equals('+'))
                        {
                            estado = 3;
                            auxLex = auxLex + c;
                            addToken(Tipo.ARITEMETICOSUMA, Fila);
                        }
                        else if (c.Equals('-'))
                        {
                            estado = 3;
                            auxLex = auxLex + c;
                            addToken(Tipo.ARITMETICORESTA, Fila);
                        }
                        else if (c.Equals('*'))
                        {
                            estado = 3;
                            auxLex = auxLex + c;
                            addToken(Tipo.ARITMETICOMULTIPLICACION, Fila);
                        }
                        else if (c.Equals('/'))
                        {
                            estado = 3;
                            auxLex = auxLex + c;
                            addToken(Tipo.ARITMETICODIVISION, Fila);
                        }
                        else if (c.Equals('='))
                        {
                            estado = 3;
                            auxLex = auxLex + c;
                            addToken(Tipo.ARITMETICOIGUAL, Fila);
                        }
                        else if (c.Equals('{'))
                        {
                            estado = 3;
                            auxLex = auxLex + c;
                            addToken(Tipo.LLAVEAPERTURA, Fila);
                        }
                        else if (c.Equals('}'))
                        {
                            estado = 3;
                            auxLex = auxLex + c;
                            addToken(Tipo.LLAVECIERRE, Fila);
                        }
                        else if (c.Equals('['))
                        {
                            estado = 3;
                            auxLex = auxLex + c;
                            addToken(Tipo.CORCHETEAPERTURA, Fila);
                        }
                        else if (c.Equals(']'))
                        {
                            estado = 3;
                            auxLex = auxLex + c;
                            addToken(Tipo.CORCHETECIERRE, Fila);
                        }
                        else if (c.Equals('('))
                        {
                            estado = 3;
                            auxLex = auxLex + c;
                            addToken(Tipo.PARENTESISAPERTURA, Fila);
                        }
                        else if (c.Equals(')'))
                        {
                            estado = 3;
                            auxLex = auxLex + c;
                            addToken(Tipo.PARENTESISCIERRE, Fila);
                        }
                        else if (c.Equals(';'))
                        {
                            estado = 3;
                            auxLex = auxLex + c;
                            addToken(Tipo.PUNTOYCOMA, Fila);
                        }
                        else if (c.Equals(','))
                        {
                            estado = 3;
                            auxLex = auxLex + c;
                            addToken(Tipo.COMA, Fila);
                        }
                        else if (c.Equals('.'))
                        {
                            estado = 3;
                            auxLex = auxLex + c;
                            addToken(Tipo.PUNTO, Fila);
                        }
                        else if (c.Equals(':'))
                        {
                            estado = 3;
                            auxLex = auxLex + c;
                            addToken(Tipo.DOSPUNTOS, Fila);
                        }
                        else
                        {
                            hay_error = true;
                            auxEr = auxEr + c;
                            //addError(Tipos.ERROR_LEXICO, Fila);
                        }
                        break;

                    case 1:
                        if (char.IsLetter(c))
                        {
                            estado = 1;
                            auxLex = auxLex + c;
                        }
                        else if (char.IsDigit(c))
                        {
                            estado = 4;
                            auxLex = auxLex + c;
                        }
                        else if (c.Equals('_'))
                        {
                            estado = 5;
                            auxLex = auxLex + c;
                            addToken(Tipo.OTROS, Fila);
                        }
                        else if (c.Equals('#'))
                        {
                            estado = 0;
                            addToken(Tipo.RESERVADA, Fila);
                            // Fila++;
                        }
                        else if (c.Equals(' '))
                        {
                            estado = 1;
                        }
                        else
                        {
                            hay_error = true;
                            auxEr = auxEr + c;
                            //addError(Tipos.ERROR_LEXICO, Fila);
                        }
                        break;

                    case 2:
                        if (char.IsDigit(c))
                        {
                            estado = 2;
                            auxLex = auxLex + c;
                        }
                        else if (c.Equals('#'))
                        {
                            estado = 0;
                            addToken(Tipo.NUMEROS, Fila);
                            // Fila++;
                        }
                        else
                        {
                            hay_error = true;
                            auxEr = auxEr + c;
                            //addError(Tipos.ERROR_LEXICO, Fila);
                        }
                        break;

                    case 3:
                        if (c.Equals('#'))
                        {
                            estado = 0;
                            auxLex = "";
                        }
                        else
                        {
                            estado = 0;
                            auxLex = "";
                        }
                        break;

                    case 4:
                        if (char.IsLetterOrDigit(c))
                        {
                            estado = 4;
                            auxLex = auxLex + c;
                        }
                        else if (c.Equals('#'))
                        {
                            estado = 0;
                            addToken(Tipo.IDENTIFICADOR, Fila);
                            // Fila++;
                        }
                        else
                        {
                            hay_error = true;
                            auxEr = auxEr + c;
                            //addError(Tipos.ERROR_LEXICO, Fila);
                        }
                        break;

                    case 5:
                        if (char.IsLetter(c))
                        {
                            estado = 6;
                            auxLex = auxLex + c;
                        }
                        else
                        {
                            hay_error = true;
                            auxEr = auxEr + c;
                            //addError(Tipos.ERROR_LEXICO, Fila);
                        }
                        break;

                    case 6:
                        if (char.IsLetter(c))
                        {
                            estado = 6;
                            auxLex = auxLex + c;
                        }
                        else if (c.Equals('#'))
                        {
                            estado = 0;
                            addToken(Tipo.RESERVADA, Fila);
                            // Fila++;
                        }
                        break;

                }
                columna += 1;
            }
            hay_error = false;
            return salida;
        }

        public List<Error> escanearError(string entrada, int Fila)
        {
            entrada = (entrada + '#');
            salida2 = new List<Error>();
            estado = 0;
            columna = 4;
            auxEr = "";
            int contar = 0;
            char c;

            for (int i = 5; i <= (entrada.Length - 1); i++)
            {

                char[] cadena = entrada.ToCharArray();
                c = cadena[i];
                if (c.Equals('\"'))
                {
                    contar += 1;
                }
                if (contar == 1)
                {
                    if (Char.IsWhiteSpace(c) || Char.IsLetter(c) || c.Equals("//"))
                    {

                    }
                    else
                    {
                        auxEr = c.ToString();
                        addError(Tipos.ERROR_LEXICO, Fila);
                    }
                }
                else if (contar == 2)
                {
                    contar = 0;
                }
                else
                {
                    if (Char.IsWhiteSpace(c) || Char.IsLetter(c) || c.Equals("//"))
                    {

                    }
                    else if (c.Equals('#') && (i == entrada.Length - 1))
                    {

                    }
                    else
                    {
                        auxEr = c.ToString();
                        addError(Tipos.ERROR_LEXICO, Fila);
                    }
                }
                columna += 1;
            }
            return salida2;
        }
    }
}
