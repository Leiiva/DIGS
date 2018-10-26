using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using static DIGS.Tokens;
using System.Drawing;
using DIGS;

public class Analizador
{
    // Variable que representa la lista de tokens
    List<Tokens> salida;
    // Variable que representa el estado actual
    int estado;
    // Variable que representa el lexema que actualmente se esta acumulando
    string auxLex;
    // Variable que representa la fila que se esta analizando
    int fila;
    // Variable que representa la columna en la que se encuentra el caracter
    int columna;
    // Variable para calcular el tamaño del token y colorear
    int tamaño = 0;
    // Variable para verificar errores
    bool Errorlexico = false;
    public bool getError()
    {
        return Errorlexico;
    }
    private void addToken(Tipo tipo, int fila, int columna, Color color)
    {
        salida.Add(new Tokens(tipo, auxLex, fila, columna, color, tamaño));
        auxLex = "";
        estado = 0;
        tamaño = 0;
    }
    void imprimirLista(List<Tokens> l)
    {
        foreach (Tokens t in l)
            Console.WriteLine(t.getTexto() + "<-->" + t.getTipoString() + "<-->" + t.getFila() + "<-->" + t.getColumna());
    }

    List<Tokens> escnear(string entrada)
    {
        // Le agrego caracter de fin de cadena porque hay lexemas que aceptan con 
        // el primer caracter del siguiente lexema y si este caracter no existe entonces
        // perdemos el lexema

        entrada = entrada + "#";
        salida = new List<Tokens>();
        estado = 0;
        auxLex = "";
        fila = 1;
        columna = 0;
        tamaño = 0;
        char c;
        // Ciclo que recorre de izquierda a derecha caracter por caracter la cadena de entrada
        for (int i = 0; i <= entrada.Length - 1; i += 1)
        {
            c = entrada[i];
            // Select en el que cada caso representa cada uno de los estados del conjunto de estados
            switch (estado)
            {
                case 0:
                    {
                        tamaño += 1;
                        // Para cada caso (o estado) hay un if elseif elseif ... else que representan el conjunto de transiciones que 
                        // salen de dicho estado, por ejemplo, estando en el estado 0 si el caracter reconocido es un dígito entonces, 
                        // pasamos al estado 1 y acumulamos el caracter reconocido en auxLex, que es el auxiliar de lexemas.
                        if ((c.Equals('[')))
                        {
                            auxLex += c;
                            columna = columna + 1;
                            addToken(Tipo.CORCHETE_ABIERTO, fila, columna, Color.SkyBlue);
                        }
                        else if ((c.Equals(']')))
                        {
                            auxLex += c;
                            columna = columna + 1;
                            addToken(Tipo.CORCHETE_CERRADO, fila, columna, Color.SkyBlue);
                        }
                        else if (c.Equals(':'))
                        {
                            auxLex += c;
                            columna = columna + 1;
                            addToken(Tipo.DOS_PUNTOS, fila, columna, Color.Orange);
                        }
                        else if (c.Equals('{'))
                        {
                            auxLex += c;
                            columna = columna + 1;
                            addToken(Tipo.LLAVE_ABIERTA, fila, columna, Color.Pink);
                        }
                        else if (c.Equals('}'))
                        {
                            auxLex += c;
                            columna = columna + 1;
                            addToken(Tipo.LLAVE_CERRADA, fila, columna, Color.Pink);
                        }
                        else if (c.Equals(';'))
                        {
                            auxLex += c;
                            columna = columna + 1;
                            addToken(Tipo.PUNTO_Y_COMA, fila, columna, Color.Purple);
                        }
                        else if (c.Equals(','))
                        {
                            auxLex += c;
                            columna = columna + 1;
                            addToken(Tipo.COMA, fila, columna, Color.Orange);
                        }
                        else if (c.Equals('.'))
                        {
                            auxLex += c;
                            columna = columna + 1;
                            addToken(Tipo.PUNTO, fila, columna, Color.OrangeRed);
                        }
                        else if (c.Equals('('))
                        {
                            auxLex += c;
                            columna = columna + 1;
                            addToken(Tipo.PARENTESIS_ABIERTO, fila, columna, Color.Green);
                        }
                        else if (c.Equals(')'))
                        {
                            auxLex += c;
                            columna = columna + 1;
                            addToken(Tipo.PARENTESIS_CERRADO, fila, columna, Color.Green);
                        }
                        else if (c.Equals('='))
                        {
                            auxLex += c;
                            columna = columna + 1;
                            addToken(Tipo.IGUAL, fila, columna, Color.Orange);
                        }
                        else if (c.Equals('+'))
                        {
                            auxLex += c;
                            columna = columna + 1;
                            addToken(Tipo.MAS, fila, columna, Color.Orange);
                        }
                        else if (c.Equals('-'))
                        {
                            auxLex += c;
                            columna = columna + 1;
                            addToken(Tipo.MENOS, fila, columna, Color.Orange);
                        }
                        else if (c.Equals('/'))
                        {
                            auxLex += c;
                            columna = columna + 1;
                            addToken(Tipo.DIVIDIR, fila, columna, Color.Orange);
                        }
                        else if (c.Equals('*'))
                        {
                            auxLex += c;
                            columna = columna + 1;
                            addToken(Tipo.POR, fila, columna, Color.Orange);
                        }
                        else if ((char.IsDigit(c)))
                        {
                            estado = 4;
                            auxLex += c;
                        }
                        else if (char.IsLetter(c))
                        {
                            auxLex += c;
                            estado = 1;
                        }
                        else if (c.Equals('\r') || c.Equals('\n'))
                        {
                            fila = fila + 1;
                            columna = 0;
                        }
                        else if (c.Equals(' ') | char.IsSeparator(c) | char.IsWhiteSpace(c))
                            estado = 0;
                        else if (c.Equals('#') & i == entrada.Length - 1)
                            // Hemos concluido el análisis léxico.
                            Console.WriteLine("Hemos concluido el análisis léxico satisfactoriamente");
                        else
                        {
                            columna = columna + 1;
                            auxLex += c;
                            addToken(Tipo.ERR, fila, columna, Color.Black);
                            Console.WriteLine("Error léxico con: " + c);
                            Errorlexico = true;
                            estado = 0;
                        }

                        break;
                    }

                case 1:
                    {
                        tamaño += 1;
                        if (char.IsLetter(c))
                        {
                            estado = 1;
                            auxLex += c;
                        }
                        else if (c.Equals('_'))
                        {
                            estado = 2;
                            auxLex += c;
                        }
                        else if (char.IsDigit(c))
                        {
                            estado = 3;
                            auxLex += c;
                        }
                        else if (auxLex == "Principal")
                        {
                            tamaño = tamaño - 1;
                            columna = columna + 1;
                            addToken(Tipo.PRINCIPAL, fila, columna, Color.Blue);
                            i -= 1;
                        }
                        else if (auxLex == "Laberinto")
                        {
                            tamaño = tamaño - 1;
                            columna = columna + 1;
                            addToken(Tipo.BLOQUE_LABERINTO, fila, columna, Color.Blue);
                            i -= 1;
                        }
                        else if (auxLex == "Dimensiones")
                        {
                            tamaño = tamaño - 1;
                            columna = columna + 1;
                            addToken(Tipo.DIMENSIONES, fila, columna, Color.Blue);
                            i -= 1;
                        }
                        else if (auxLex == "Ubicación_personaje")
                        {
                            tamaño = tamaño - 1;
                            columna = columna + 1;
                            addToken(Tipo.UBICACION_PERSONAJE, fila, columna, Color.Blue);
                            i -= 1;
                        }
                        else if (auxLex == "Ubicación_tesoro")
                        {
                            tamaño = tamaño - 1;
                            columna = columna + 1;
                            addToken(Tipo.UBICACION_TESORO, fila, columna, Color.Blue);
                            i -= 1;
                        }
                        else if (auxLex == "Obstáculos")
                        {
                            tamaño = tamaño - 1;
                            columna = columna + 1;
                            addToken(Tipo.OBSTACULOS, fila, columna, Color.Blue);
                            i -= 1;
                        }
                        else if (auxLex == "Casilla")
                        {
                            tamaño = tamaño - 1;
                            columna = columna + 1;
                            addToken(Tipo.CASILLA, fila, columna, Color.Blue);
                            i -= 1;
                        }
                        else if (auxLex == "Rango_Casillas")
                        {
                            tamaño = tamaño - 1;
                            columna = columna + 1;
                            addToken(Tipo.RANGO_CASILLA, fila, columna, Color.Blue);
                            i -= 1;
                        }
                        else if (auxLex == "Ruta")
                        {
                            tamaño = tamaño - 1;
                            columna = columna + 1;
                            addToken(Tipo.RUTA, fila, columna, Color.Blue);
                            i -= 1;
                        }
                        else if (auxLex == "Intervalo")
                        {
                            tamaño = tamaño - 1;
                            columna = columna + 1;
                            addToken(Tipo.INTERVALO, fila, columna, Color.Blue);
                            i -= 1;
                        }
                        else if (auxLex == "Paso")
                        {
                            tamaño = tamaño - 1;
                            columna = columna + 1;
                            addToken(Tipo.PASO, fila, columna, Color.Blue);
                            i -= 1;
                        }
                        else if (auxLex == "Caminata")
                        {
                            tamaño = tamaño - 1;
                            columna = columna + 1;
                            addToken(Tipo.CAMINATA, fila, columna, Color.Blue);
                            i -= 1;
                        }
                        else if (auxLex == "Variable")
                        {
                            tamaño = tamaño - 1;
                            columna = columna + 1;
                            addToken(Tipo.VARIABLE, fila, columna, Color.Blue);
                            i -= 1;
                        }
                        else
                        {
                            tamaño = tamaño - 1;
                            columna = columna + 1;
                            // columna = columna - auxLex.Length + 1
                            addToken(Tipo.IDENTIFICADOR, fila, columna, Color.Blue);
                            i -= 1;
                        }

                        break;
                    }

                case 2:
                    {
                        tamaño += 1;
                        if ((char.IsLetter(c) | c.Equals('_')))
                        {
                            estado = 2;
                            auxLex += c;
                        }
                        else if (auxLex == "Principal")
                        {
                            tamaño = tamaño - 1;
                            columna = columna + 1;
                            addToken(Tipo.PRINCIPAL, fila, columna, Color.Blue);
                            i -= 1;
                        }
                        else if (auxLex == "Laberinto")
                        {
                            tamaño = tamaño - 1;
                            columna = columna + 1;
                            addToken(Tipo.BLOQUE_LABERINTO, fila, columna, Color.Blue);
                            i -= 1;
                        }
                        else if (auxLex == "Dimensiones")
                        {
                            tamaño = tamaño - 1;
                            columna = columna + 1;
                            addToken(Tipo.DIMENSIONES, fila, columna, Color.Blue);
                            i -= 1;
                        }
                        else if (auxLex == "Ubicación_personaje")
                        {
                            tamaño = tamaño - 1;
                            columna = columna + 1;
                            addToken(Tipo.UBICACION_PERSONAJE, fila, columna, Color.Blue);
                            i -= 1;
                        }
                        else if (auxLex == "Ubicación_tesoro")
                        {
                            tamaño = tamaño - 1;
                            columna = columna + 1;
                            addToken(Tipo.UBICACION_TESORO, fila, columna, Color.Blue);
                            i -= 1;
                        }
                        else if (auxLex == "Obstáculos")
                        {
                            tamaño = tamaño - 1;
                            columna = columna + 1;
                            addToken(Tipo.OBSTACULOS, fila, columna, Color.Blue);
                            i -= 1;
                        }
                        else if (auxLex == "Casilla")
                        {
                            tamaño = tamaño - 1;
                            columna = columna + 1;
                            addToken(Tipo.CASILLA, fila, columna, Color.Blue);
                            i -= 1;
                        }
                        else if (auxLex == "Rango_Casillas")
                        {
                            tamaño = tamaño - 1;
                            columna = columna + 1;
                            addToken(Tipo.RANGO_CASILLA, fila, columna, Color.Blue);
                            i -= 1;
                        }
                        else if (auxLex == "Ruta")
                        {
                            tamaño = tamaño - 1;
                            columna = columna + 1;
                            addToken(Tipo.RUTA, fila, columna, Color.Blue);
                            i -= 1;
                        }
                        else if (auxLex == "Intervalo")
                        {
                            tamaño = tamaño - 1;
                            columna = columna + 1;
                            addToken(Tipo.INTERVALO, fila, columna, Color.Blue);
                            i -= 1;
                        }
                        else if (auxLex == "Paso")
                        {
                            tamaño = tamaño - 1;
                            columna = columna + 1;
                            addToken(Tipo.PASO, fila, columna, Color.Blue);
                            i -= 1;
                        }
                        else if (auxLex == "Caminata")
                        {
                            tamaño = tamaño - 1;
                            columna = columna + 1;
                            addToken(Tipo.CAMINATA, fila, columna, Color.Blue);
                            i -= 1;
                        }
                        else if (auxLex == "Variable")
                        {
                            tamaño = tamaño - 1;
                            columna = columna + 1;
                            addToken(Tipo.VARIABLE, fila, columna, Color.Blue);
                            i -= 1;
                        }
                        else
                        {
                            tamaño = tamaño - 1;
                            columna = columna + 1;
                            // columna = columna - auxLex.Length + 1
                            addToken(Tipo.IDENTIFICADOR, fila, columna, Color.Blue);
                            i -= 1;
                        }

                        break;
                    }

                case 3:
                    {
                        tamaño += 1;
                        if ((char.IsLetter(c) | char.IsDigit(c)))
                        {
                            // columna = columna + 1
                            estado = 3;
                            auxLex += c;
                        }
                        else
                        {
                            tamaño = tamaño - 1;
                            columna = columna + 1;
                            // columna = columna - auxLex.Length + 1
                            addToken(Tipo.IDENTIFICADOR, fila, columna, Color.Blue);
                            i -= 1;
                        }

                        break;
                    }

                case 4:
                    {
                        tamaño += 1;
                        if ((char.IsDigit(c)))
                        {
                            columna = columna + 1;
                            estado = 4;
                            auxLex += c;
                        }
                        else
                        {
                            columna = columna + 1;
                            columna = columna - auxLex.Length + 1;
                            tamaño = tamaño - 1;
                            addToken(Tipo.NUMERO_ENTERO, fila, columna, Color.Red);
                            i -= 1;
                        }

                        break;
                    }
            }
        }
        return salida;
    }
}
