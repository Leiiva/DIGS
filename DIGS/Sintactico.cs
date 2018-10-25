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

namespace DIGS
{
    class Sintactico
    {
        private int numPreanalisis;
        private Tokens preanalisis;
        private Tokens postanalisis;
        private List<Tokens> listaTokens;
        public List<ErrorSintactico> lerrores = new List<ErrorSintactico>();
        private List<Variables> listavariable = new List<Variables>();

        private void BloqueLab()
        {
            this.match(Tokens.Tipo.CORCHETE_ABIERTO);
            this.match(Tokens.Tipo.BLOQUE_LABERINTO);
            this.match(Tokens.Tipo.CORCHETE_CERRADO);
            this.match(Tokens.Tipo.DOS_PUNTOS);
            this.match(Tokens.Tipo.LLAVE_ABIERTA);
            this.InsLaberinto();
            this.match(Tokens.Tipo.LLAVE_CERRADA);
        }

        private void BloqueRuta()
        {
            this.match(Tokens.Tipo.CORCHETE_ABIERTO);
            this.match(Tokens.Tipo.RUTA);
            this.match(Tokens.Tipo.CORCHETE_CERRADO);
            this.match(Tokens.Tipo.DOS_PUNTOS);
            this.match(Tokens.Tipo.LLAVE_ABIERTA);
            this.InsRuta();
            this.match(Tokens.Tipo.LLAVE_CERRADA);
        }

        private void caminata()
        {
            this.match(Tokens.Tipo.CORCHETE_ABIERTO);
            this.match(Tokens.Tipo.CAMINATA);
            this.match(Tokens.Tipo.CORCHETE_CERRADO);
            this.match(Tokens.Tipo.DOS_PUNTOS);
            this.match(Tokens.Tipo.PARENTESIS_ABIERTO);
            this.val_ar();
            this.rang_c();
            this.match(Tokens.Tipo.PARENTESIS_CERRADO);
            this.match(Tokens.Tipo.PUNTO_Y_COMA);
        }

        private void Casilla()
        {
            this.match(Tokens.Tipo.CORCHETE_ABIERTO);
            this.match(Tokens.Tipo.CASILLA);
            this.match(Tokens.Tipo.CORCHETE_CERRADO);
            this.match(Tokens.Tipo.DOS_PUNTOS);
            this.match(Tokens.Tipo.PARENTESIS_ABIERTO);
            this.val_ar();
            this.match(Tokens.Tipo.COMA);
            this.val_ar();
            this.match(Tokens.Tipo.PARENTESIS_CERRADO);
            this.match(Tokens.Tipo.PUNTO_Y_COMA);
        }

        private void Dimensiones()
        {
            this.match(Tokens.Tipo.CORCHETE_ABIERTO);
            this.match(Tokens.Tipo.DIMENSIONES);
            this.match(Tokens.Tipo.CORCHETE_CERRADO);
            this.match(Tokens.Tipo.DOS_PUNTOS);
            this.match(Tokens.Tipo.PARENTESIS_ABIERTO);
            this.match(Tokens.Tipo.NUMERO_ENTERO);
            this.match(Tokens.Tipo.COMA);
            this.match(Tokens.Tipo.NUMERO_ENTERO);
            this.match(Tokens.Tipo.PARENTESIS_CERRADO);
            this.match(Tokens.Tipo.PUNTO_Y_COMA);
        }

        private void exp_ar()
        {
            this.val_ar();
            if (this.preanalisis.tipoToken() == Tokens.Tipo.MAS)
            {
                this.match(Tokens.Tipo.MAS);
            }
            else if (this.preanalisis.getTipo() == Tokens.Tipo.MENOS)
            {
                this.match(Tokens.Tipo.MENOS);
            }
            else if (this.preanalisis.getTipo() == Tokens.Tipo.POR)
            {
                this.match(Tokens.Tipo.POR);
            }
            else if (this.preanalisis.getTipo() == Tokens.Tipo.DIVIDIR)
            {
                this.match(Tokens.Tipo.DIVIDIR);
            }
            else
            {
                this.match(Tokens.Tipo.POTENCIA);
            }
            this.val_ar();
        }

        public string getTipoParaError(Tokens.Tipo p)
        {
            string str;
            switch (p)
            {
                case Tokens.Tipo.BLOQUE_LABERINTO:
                    str = "Bloque de Laberinto";
                    break;

                case Tokens.Tipo.BLOQUE_RUTA:
                    str = "Bloque de Ruta";
                    break;

                case Tokens.Tipo.CAMINATA:
                    str = "Caminata";
                    break;

                case Tokens.Tipo.CASILLA:
                    str = "Casilla";
                    break;

                case Tokens.Tipo.COMA:
                    str = "Coma";
                    break;

                case Tokens.Tipo.CORCHETE_ABIERTO:
                    str = "Corchete Abierto";
                    break;

                case Tokens.Tipo.CORCHETE_CERRADO:
                    str = "Corchete Cerrado";
                    break;

                case Tokens.Tipo.DIMENSIONES:
                    str = "Dimensiones";
                    break;

                case Tokens.Tipo.DIVIDIR:
                    str = "Signo Dividir";
                    break;

                case Tokens.Tipo.DOS_PUNTOS:
                    str = "Dos puntos";
                    break;

                case Tokens.Tipo.ERR:
                    str = "Error Caracter Desconocido";
                    break;

                case Tokens.Tipo.IDENTIFICADOR:
                    str = "Identificador";
                    break;

                case Tokens.Tipo.IGUAL:
                    str = "Igual";
                    break;

                case Tokens.Tipo.INTERVALO:
                    str = "Intervalo";
                    break;

                case Tokens.Tipo.LLAVE_ABIERTA:
                    str = "Llave Abierta";
                    break;

                case Tokens.Tipo.LLAVE_CERRADA:
                    str = "Llave Cerrada";
                    break;

                case Tokens.Tipo.MAS:
                    str = "Mas";
                    break;

                case Tokens.Tipo.MENOS:
                    str = "Menos";
                    break;

                case Tokens.Tipo.NUMERO_ENTERO:
                    str = "Numero Entero";
                    break;

                case Tokens.Tipo.OBSTACULOS:
                    str = "Obstaculos";
                    break;

                case Tokens.Tipo.PARENTESIS_ABIERTO:
                    str = "Parentesis Abierto";
                    break;

                case Tokens.Tipo.PARENTESIS_CERRADO:
                    str = "Parentesis Cerrado";
                    break;

                case Tokens.Tipo.PASO:
                    str = "Paso";
                    break;

                case Tokens.Tipo.POTENCIA:
                    str = "Signo de Potencia";
                    break;

                case Tokens.Tipo.POR:
                    str = "Signo Por";
                    break;

                case Tokens.Tipo.PRINCIPAL:
                    str = "Principal";
                    break;

                case Tokens.Tipo.PUNTO:
                    str = "Punto";
                    break;

                case Tokens.Tipo.PUNTO_Y_COMA:
                    str = "Punto y Coma";
                    break;

                case Tokens.Tipo.RANGO_CASILLA:
                    str = "Rango de Casilla";
                    break;

                case Tokens.Tipo.RUTA:
                    str = "Ruta";
                    break;

                case Tokens.Tipo.UBICACION_PERSONAJE:
                    str = "Ubicaci\x00f3n Personaje";
                    break;

                case Tokens.Tipo.UBICACION_TESORO:
                    str = "Ubicaci\x00f3n Tesoro";
                    break;

                case Tokens.Tipo.ULTIMO:
                    str = "Ultimo";
                    break;

                case Tokens.Tipo.VARIABLE:
                    str = "Variable";
                    break;

                default:
                    str = "Error Caracter Desconocido";
                    break;
            }
            return str;
        }

        private void id()
        {
            this.match(Tokens.Tipo.IDENTIFICADOR);
            this.match(Tokens.Tipo.DOS_PUNTOS);
            this.match(Tokens.Tipo.IGUAL);
            this.Valor();
            this.match(Tokens.Tipo.PUNTO_Y_COMA);
        }

        private void InsLaberinto()
        {
            this.postanalisis = this.listaTokens[this.numPreanalisis + 1];
            if ((this.preanalisis.getTipo() == Tokens.Tipo.CORCHETE_ABIERTO) & (this.postanalisis.getTipo() == Tokens.Tipo.DIMENSIONES))
            {
                this.Dimensiones();
                this.InsLaberinto();
            }
            else if ((this.preanalisis.getTipo() == Tokens.Tipo.CORCHETE_ABIERTO) & (this.postanalisis.getTipo() == Tokens.Tipo.UBICACION_PERSONAJE))
            {
                this.UbicacionPer();
                this.InsLaberinto();
            }
            else if ((this.preanalisis.getTipo() == Tokens.Tipo.CORCHETE_ABIERTO) & (this.postanalisis.getTipo() == Tokens.Tipo.UBICACION_TESORO))
            {
                this.UbicacionTes();
                this.InsLaberinto();
            }
            else if ((this.preanalisis.getTipo() == Tokens.Tipo.CORCHETE_ABIERTO) & (this.postanalisis.getTipo() == Tokens.Tipo.OBSTACULOS))
            {
                this.Obstaculos();
                this.InsLaberinto();
            }
        }

        private void InsObs()
        {
            this.postanalisis = this.listaTokens[this.numPreanalisis + 1];
            if ((this.preanalisis.getTipo() == Tokens.Tipo.CORCHETE_ABIERTO) & (this.postanalisis.getTipo() == Tokens.Tipo.CASILLA))
            {
                this.Casilla();
                this.InsObs();
            }
            else if ((this.preanalisis.getTipo() == Tokens.Tipo.CORCHETE_ABIERTO) & (this.postanalisis.getTipo() == Tokens.Tipo.RANGO_CASILLA))
            {
                this.Rango();
                this.InsObs();
            }
            else if ((this.preanalisis.getTipo() == Tokens.Tipo.CORCHETE_ABIERTO) & (this.postanalisis.getTipo() == Tokens.Tipo.VARIABLE))
            {
                this.variable();
                this.InsObs();
            }
            else if (this.preanalisis.getTipo() == Tokens.Tipo.IDENTIFICADOR)
            {
                this.id();
                this.InsObs();
            }
        }

        private void InsRuta()
        {
            this.postanalisis = this.listaTokens[this.numPreanalisis + 1];
            if ((this.preanalisis.getTipo() == Tokens.Tipo.CORCHETE_ABIERTO) & (this.postanalisis.getTipo() == Tokens.Tipo.PASO))
            {
                this.paso();
                this.InsRuta();
            }
            else if ((this.preanalisis.getTipo() == Tokens.Tipo.CORCHETE_ABIERTO) & (this.postanalisis.getTipo() == Tokens.Tipo.INTERVALO))
            {
                this.intervalo();
                this.InsRuta();
            }
            else if ((this.preanalisis.getTipo() == Tokens.Tipo.CORCHETE_ABIERTO) & (this.postanalisis.getTipo() == Tokens.Tipo.CAMINATA))
            {
                this.caminata();
                this.InsRuta();
            }
            else if ((this.preanalisis.getTipo() == Tokens.Tipo.CORCHETE_ABIERTO) & (this.postanalisis.getTipo() == Tokens.Tipo.VARIABLE))
            {
                this.variable();
                this.InsRuta();
            }
            else if (this.preanalisis.getTipo() == Tokens.Tipo.IDENTIFICADOR)
            {
                this.id();
                this.InsRuta();
            }
        }

        private void intervalo()
        {
            this.match(Tokens.Tipo.CORCHETE_ABIERTO);
            this.match(Tokens.Tipo.INTERVALO);
            this.match(Tokens.Tipo.CORCHETE_CERRADO);
            this.match(Tokens.Tipo.DOS_PUNTOS);
            this.match(Tokens.Tipo.PARENTESIS_ABIERTO);
            this.val_ar();
            this.match(Tokens.Tipo.PARENTESIS_CERRADO);
            this.match(Tokens.Tipo.PUNTO_Y_COMA);
        }

        private void lista_var()
        {
            if (this.preanalisis.getTipo() == Tokens.Tipo.COMA)
            {
                this.match(Tokens.Tipo.COMA);
                this.lista_varI();
            }
        }

        private void lista_varI()
        {
            this.match(Tokens.Tipo.IDENTIFICADOR);
            this.lista_var();
        }

        private void match(Tokens.Tipo p)
        {
            if (p != this.preanalisis.getTipo())
            {
                Interaction.MsgBox("Se esperaba " + this.getTipoParaError(p).ToString() + " " + this.numPreanalisis.ToString(), MsgBoxStyle.ApplicationModal, null);
                this.lerrores.Add(new ErrorSintactico(this.getTipoParaError(p).ToString(), this.numPreanalisis.ToString()));
            }
            if (preanalisis.tipoToken != Tipo.ULTIMO)
            {
                numPreanalisis += 1;
                preanalisis = listaTokens[numPreanalisis];
            }
        }

        private void Obstaculos()
        {
            this.match(Tokens.Tipo.CORCHETE_ABIERTO);
            this.match(Tokens.Tipo.OBSTACULOS);
            this.match(Tokens.Tipo.CORCHETE_CERRADO);
            this.match(Tokens.Tipo.DOS_PUNTOS);
            this.match(Tokens.Tipo.LLAVE_ABIERTA);
            this.InsObs();
            this.match(Tokens.Tipo.LLAVE_CERRADA);
        }

        public List<ErrorSintactico> obtenererrores() =>
            this.lerrores;

        public void parsear(List<Tokens> l)
        {
            this.listaTokens = l;
            this.preanalisis = this.listaTokens[0];
            this.numPreanalisis = 0;
            this.S();
        }

        private void paso()
        {
            this.match(Tokens.Tipo.CORCHETE_ABIERTO);
            this.match(Tokens.Tipo.PASO);
            this.match(Tokens.Tipo.CORCHETE_CERRADO);
            this.match(Tokens.Tipo.DOS_PUNTOS);
            this.match(Tokens.Tipo.PARENTESIS_ABIERTO);
            this.val_ar();
            this.match(Tokens.Tipo.COMA);
            this.val_ar();
            this.match(Tokens.Tipo.PARENTESIS_CERRADO);
            this.match(Tokens.Tipo.PUNTO_Y_COMA);
        }

        private void Rang()
        {
            if (this.preanalisis.getTipo() == Tokens.Tipo.PUNTO)
            {
                this.match(Tokens.Tipo.PUNTO);
                this.match(Tokens.Tipo.PUNTO);
                this.val_ar();
                this.match(Tokens.Tipo.COMA);
                this.Rang_d();
            }
            else
            {
                this.match(Tokens.Tipo.COMA);
                this.val_ar();
                this.match(Tokens.Tipo.PUNTO);
                this.match(Tokens.Tipo.PUNTO);
                this.val_ar();
            }
        }

        private void rang_c()
        {
            if (this.preanalisis.getTipo() == Tokens.Tipo.PUNTO)
            {
                this.match(Tokens.Tipo.PUNTO);
                this.match(Tokens.Tipo.PUNTO);
                this.val_ar();
                this.match(Tokens.Tipo.COMA);
                this.val_ar();
            }
            else
            {
                this.match(Tokens.Tipo.COMA);
                this.val_ar();
                this.match(Tokens.Tipo.PUNTO);
                this.match(Tokens.Tipo.PUNTO);
                this.val_ar();
            }
        }

        private void Rang_d()
        {
            this.postanalisis = this.listaTokens[this.numPreanalisis + 1];
            if (this.postanalisis.getTipo() != Tokens.Tipo.PUNTO)
            {
                this.val_ar();
            }
            else
            {
                this.val_ar();
                this.match(Tokens.Tipo.PUNTO);
                this.match(Tokens.Tipo.PUNTO);
                this.val_ar();
            }
        }

        private void Rango()
        {
            this.match(Tokens.Tipo.CORCHETE_ABIERTO);
            this.match(Tokens.Tipo.RANGO_CASILLA);
            this.match(Tokens.Tipo.CORCHETE_CERRADO);
            this.match(Tokens.Tipo.DOS_PUNTOS);
            this.match(Tokens.Tipo.PARENTESIS_ABIERTO);
            this.val_ar();
            this.Rang();
            this.match(Tokens.Tipo.PARENTESIS_CERRADO);
            this.match(Tokens.Tipo.PUNTO_Y_COMA);
        }

        private void S()
        {
            this.match(Tokens.Tipo.CORCHETE_ABIERTO);
            this.match(Tokens.Tipo.PRINCIPAL);
            this.match(Tokens.Tipo.CORCHETE_CERRADO);
            this.match(Tokens.Tipo.DOS_PUNTOS);
            this.match(Tokens.Tipo.LLAVE_ABIERTA);
            this.BloqueLab();
            this.BloqueRuta();
            this.match(Tokens.Tipo.LLAVE_CERRADA);
        }

        private void UbicacionPer()
        {
            this.match(Tokens.Tipo.CORCHETE_ABIERTO);
            this.match(Tokens.Tipo.UBICACION_PERSONAJE);
            this.match(Tokens.Tipo.CORCHETE_CERRADO);
            this.match(Tokens.Tipo.DOS_PUNTOS);
            this.match(Tokens.Tipo.PARENTESIS_ABIERTO);
            this.match(Tokens.Tipo.NUMERO_ENTERO);
            this.match(Tokens.Tipo.COMA);
            this.match(Tokens.Tipo.NUMERO_ENTERO);
            this.match(Tokens.Tipo.PARENTESIS_CERRADO);
            this.match(Tokens.Tipo.PUNTO_Y_COMA);
        }

        private void UbicacionTes()
        {
            this.match(Tokens.Tipo.CORCHETE_ABIERTO);
            this.match(Tokens.Tipo.UBICACION_TESORO);
            this.match(Tokens.Tipo.CORCHETE_CERRADO);
            this.match(Tokens.Tipo.DOS_PUNTOS);
            this.match(Tokens.Tipo.PARENTESIS_ABIERTO);
            this.match(Tokens.Tipo.NUMERO_ENTERO);
            this.match(Tokens.Tipo.COMA);
            this.match(Tokens.Tipo.NUMERO_ENTERO);
            this.match(Tokens.Tipo.PARENTESIS_CERRADO);
            this.match(Tokens.Tipo.PUNTO_Y_COMA);
        }

        private void val_ar()
        {
            if (this.preanalisis.getTipo() == Tokens.Tipo.IDENTIFICADOR)
            {
                this.match(Tokens.Tipo.IDENTIFICADOR);
            }
            else
            {
                this.match(Tokens.Tipo.NUMERO_ENTERO);
            }
        }

        private void Valor()
        {
            this.postanalisis = this.listaTokens[this.numPreanalisis + 1];
            if ((this.preanalisis.getTipo() == Tokens.Tipo.NUMERO_ENTERO) | ((this.preanalisis.getTipo() == Tokens.Tipo.IDENTIFICADOR) & (this.postanalisis.getTipo() == Tokens.Tipo.PUNTO_Y_COMA)))
            {
                this.val_ar();
            }
            else
            {
                this.exp_ar();
            }
        }

        private void variable()
        {
            this.match(Tokens.Tipo.CORCHETE_ABIERTO);
            this.match(Tokens.Tipo.VARIABLE);
            this.match(Tokens.Tipo.CORCHETE_CERRADO);
            this.match(Tokens.Tipo.DOS_PUNTOS);
            this.lista_varI();
            this.match(Tokens.Tipo.PUNTO_Y_COMA);
        }
    }
}
