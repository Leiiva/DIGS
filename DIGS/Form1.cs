﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Drawing.Text;
using System.Text.RegularExpressions;
using static DIGS.Tokens;
using static DIGS.Archivo;
using static DIGS.Coordenada;
using static DIGS.Variables;
using static DIGS.Obstaculos;
using static DIGS.Analizador;
using FastColoredTextBoxNS;

namespace DIGS
{
    public partial class Form1 : Form
    {
        Archivo archivo = new Archivo();
        Analizador analizador = new Analizador();
        Sintactico sint = new Sintactico();
        List<Tokens> lTokens;
        List<ErrorSintactico> lerrores = new List<ErrorSintactico>();
        List<int> rango = new List<int>();
        List<Variables> lVariable = new List<Variables>();
        List<Coordenada> lCoordenadas = new List<Coordenada>();
        List<Obstaculos> lObstaculos = new List<Obstaculos>();
        int puntero;
        int puntero2;
        int auxcount;
        string nombrevariable;
        int valorvariable;
        bool suma = false;
        bool resta = false;
        bool multiplicacion = false;
        bool division = false;
        int total = 0;
        //IContainer components;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        string path;

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog OFDialog = new OpenFileDialog() { Filter = @"Project Documents ""*.digs"" | *.digs", ValidateNames = true, Multiselect = false })
            {
                if (OFDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (StreamReader sr = new StreamReader(OFDialog.FileName))
                        {
                            path = OFDialog.FileName;
                            Task<string> text = sr.ReadToEndAsync();
                            rtbT.Text = text.Result;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private async void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(path))
            {
                using (SaveFileDialog SFDialog = new SaveFileDialog() { Filter = @"Project Documents ""*.digs"" | *.digs", ValidateNames = true })
                {
                    if (SFDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            path = SFDialog.FileName;
                            using (StreamWriter sw = new StreamWriter(SFDialog.FileName))
                            {
                                await sw.WriteLineAsync(rtbT.Text);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(path))
                    {
                        await sw.WriteLineAsync(rtbT.Text);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void guardarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog SFDialog = new SaveFileDialog() { Filter = @"Project Documents ""*.digs"" | *.digs", ValidateNames = true })
            {
                if (SFDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(SFDialog.FileName))
                        {
                            await sw.WriteLineAsync(rtbT.Text);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void manualTecnicoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String filename = Application.StartupPath + "\\Manual Tecnico.pdf";
            if (File.Exists(filename))
            {
                Process.Start(filename);
            }

        }

        private void manualDeUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String filename = Application.StartupPath + "\\Manual de Usuario.pdf";
            if (File.Exists(filename))
            {
                Process.Start(filename);
            }

        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //AcercaDe af = new AcercaDe();
            //af.ShowDialog();
        }
        void coloreareditor(List<Tokens> li)
        {
            puntero = 0;
            puntero2 = 0;
            foreach (Tokens t in li)
            {
                puntero2 = t.getTamaño();
                rtbT.SelectionStart = puntero;
                rtbT.SelectionLength = puntero2;
                rtbT.SelectionColor = t.colorcito;
                puntero = puntero2;

            }
        }

        private void analizarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((rtbT.Text != ""))
            {
                lTokens = analizador.escnear(rtbT.Text);
                coloreareditor(lTokens);
                analizador.imprimirLista(lTokens);



                string mis_documentos = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string archivo = "Tokens";
                string archivo2 = "Errores";
                string archivoHTML = mis_documentos + @"\LFP_Proyecto2\" + archivo + ".html";
                string errorHTML = mis_documentos + @"\LFP_Proyecto2\" + archivo2 + ".html";
                StreamWriter escritor;
                FileInfo info = new FileInfo(archivoHTML);
                FileInfo info2 = new FileInfo(errorHTML);
                string tab = "";
                int conteyo = 0;
                string tablaerrores = "";
                System.Windows.Forms.MessageBox.Show("Listado de Tokens en HTML Creado exitosamente");

                for (int i = 0; i <= lTokens.Count - 1; i += 1)
                {
                    tab = tab + "<Tr><Td>" + i + 1 + "<Td>" + lTokens[i].getTexto() + "<Td>" + lTokens[i].getTipoString() + "<Td>" + lTokens[i].getFila().ToString() + "<Td>" + lTokens[i].getColumna().ToString() + "" + Environment.NewLine;
                }

                File.Delete(archivoHTML);
                escritor = File.AppendText(archivoHTML);
                escritor.WriteLine("<html>");
                escritor.WriteLine("<meta charset=\"UTF-8\">");
                escritor.WriteLine("<head>");
                escritor.WriteLine("<title>[LFP] Analizador Lexico </title>");
                escritor.WriteLine("</head>");

                escritor.WriteLine("<body bgcolor=\"#2E2EFE\"><font face=\"Arial Black\">");
                escritor.WriteLine("<center>");
                escritor.WriteLine("<h1><font face=\"Algerian\">Listado de Tokens</font></h1>");
                escritor.WriteLine("</center>");
                escritor.WriteLine("<h2>");
                escritor.WriteLine("<center>");
                escritor.WriteLine("<Table Border>");
                escritor.WriteLine("<Tr>");
                escritor.WriteLine("<Td> # token </td>");
                escritor.WriteLine("<Td> Lexema </Td>");
                escritor.WriteLine("<Td> Token </Td>");
                escritor.WriteLine("<Td> Fila </Td>");
                escritor.WriteLine("<Td> Columna </Td>");
                escritor.WriteLine("</Tr>");
                escritor.WriteLine(tab);
                escritor.WriteLine("</Table");

                escritor.WriteLine("</center");
                escritor.WriteLine("</h2>");
                escritor.WriteLine("</font></body>");
                escritor.WriteLine("</html>");
                escritor.Flush();
                escritor.Close();

                if ((analizador.getError() == false))
                {
                    tablaerrores = tablaerrores + "<Tr><Td>" + "------" + "<Td>" + "------" + "<Td>" + "------" + "<Td>" + "------" + "<Td>" + "------" + "" + Environment.NewLine;
                }

                else
                {
                    System.Windows.Forms.MessageBox.Show("El codigo ingresado contiene errores");
                    for (int j = 0; j <= lTokens.Count - 1; j += 1)
                    {
                        if ((lTokens[j].getTipoString() == "Error Caracter Desconocido"))
                        {
                            tablaerrores = tablaerrores + "<Tr><Td>" + conteyo + 1 + "<Td>" + lTokens[j].getTexto() + "<Td>" + "Caracter Desconocido" + "<Td>" + lTokens[j].getFila().ToString() + "<Td>" + lTokens[j].getColumna().ToString() + "" + Environment.NewLine;
                            conteyo = conteyo + 1;
                        }
                    }
                }


                File.Delete(errorHTML);
                escritor = File.AppendText(errorHTML);
                escritor.WriteLine("<html>");
                escritor.WriteLine("<meta charset=\"UTF-8\">");
                escritor.WriteLine("<head>");
                escritor.WriteLine("<title> [LFP] Errores Lexicos </title>");
                escritor.WriteLine("</head>");

                escritor.WriteLine("<body bgcolor=\"#DF3A01\"><font face=\"Arial Black\">");
                escritor.WriteLine("<center>");
                escritor.WriteLine("<h1><font face=\"Algerian\">Listado de Errores </font></h1>");
                escritor.WriteLine("</center>");
                escritor.WriteLine("<h2>");
                escritor.WriteLine("<center>");
                escritor.WriteLine("<Table Border>");
                escritor.WriteLine("<Tr>");
                escritor.WriteLine("<Td> # error </td>");
                escritor.WriteLine("<Td> Error </Td>");
                escritor.WriteLine("<Td> Descripción </Td>");
                escritor.WriteLine("<Td> Fila </Td>");
                escritor.WriteLine("<Td> Columna </Td>");
                escritor.WriteLine("</Tr>");
                escritor.WriteLine(tablaerrores);
                escritor.WriteLine("</Table>");

                escritor.WriteLine("</center>");
                escritor.WriteLine("</h2>");
                escritor.WriteLine("</font></body>");
                escritor.WriteLine("</html>");
                escritor.Flush();
                escritor.Close();
            }
            else
                System.Windows.Forms.MessageBox.Show("El campo esta vacio");

        }

        int operar(int val1, Tokens l, int val2)
        {
            int resultado = 0;
            if ((l.getTipo() == Tipo.MAS))
            {
                resultado = val1 + val2;
                return resultado;
            }
            else if ((l.getTipo() == Tipo.MENOS))
            {
                resultado = val1 - val2;
                return resultado;
            }
            else if ((l.getTipo() == Tipo.POR))
            {
                resultado = val1 * val2;
                return resultado;
            }
            else if (l.getTipo() == Tipo.DIVIDIR && val2 != 0)
            {
                resultado = val1 / val2;
                return resultado;
            }
            else
                return 0;
        }

        int buscarvariable(Tokens t)
        {
            int var = 0;
            for (int i = 0; i <= lVariable.Count - 1; i += 1)
            {
                if ((lVariable[i].getNombre().Equals(t.getTexto())))
                    Int32.TryParse(lVariable[i].getValor(), out var);
            }
            return var;
        }

        private void crearJuegoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lVariable.Clear();

            for (int i = 0; i <= lTokens.Count - 1; i += 1)
            {

                // Agregar Variables
                int a, b, c, d = 0;
                int bv = 0;

                if ((lTokens[i].getTipo() == Tokens.Tipo.VARIABLE))
                {
                    auxcount = i;
                    while ((lTokens[auxcount].getTipo() != Tokens.Tipo.PUNTO_Y_COMA))
                    {
                        if ((lTokens[auxcount].getTipo() == Tokens.Tipo.IDENTIFICADOR))
                        {
                            nombrevariable = lTokens[auxcount].getTexto();
                            lVariable.Add(new Variables(nombrevariable, "0"));
                        }
                        auxcount = auxcount + 1;
                    }
                }
                int valor = 0;
                // Agregar Valor a las Variables
                if ((lTokens[i].getTipo() == Tokens.Tipo.IDENTIFICADOR))
                {
                    if ((lTokens[i + 1].getTipo() == Tokens.Tipo.DOS_PUNTOS))
                    {
                        if ((lTokens[i + 2].getTipo() == Tokens.Tipo.IGUAL))
                        {
                            if ((lTokens[i + 3].getTipo() == Tokens.Tipo.NUMERO_ENTERO))
                            {
                                if ((lTokens[i + 4].getTipo() == Tokens.Tipo.PUNTO_Y_COMA))
                                {
                                    for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                    {
                                        if (lVariable[v].getNombre().Equals(lTokens[i].getTexto().ToString()))
                                        {
                                            Int32.TryParse(lTokens[i + 3].getTexto(), out a);
                                            lVariable[v].setValor(a);
                                        }
                                    }
                                }
                                else if (lTokens[i + 4].getTipo() == Tokens.Tipo.MAS | lTokens[i + 4].getTipo() == Tokens.Tipo.MENOS | lTokens[i + 4].getTipo() == Tokens.Tipo.POR | lTokens[i + 4].getTipo() == Tokens.Tipo.DIVIDIR)
                                {
                                    if ((lTokens[i + 5].getTipo() == Tipo.NUMERO_ENTERO))
                                    {
                                        for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                        {
                                            if (lVariable[v].getNombre().Equals(lTokens[i].getTexto().ToString()))
                                            {

                                                Int32.TryParse(lTokens[i + 3].getTexto(), out a);
                                                Int32.TryParse(lTokens[i + 5].getTexto(), out b);
                                                lVariable[v].setValor(operar(a, lTokens[i + 4], b));
                                            }

                                        }
                                    }
                                    else if ((lTokens[i + 5].getTipo() == Tipo.IDENTIFICADOR))
                                    {
                                        for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                        {
                                            if (lVariable[v].getNombre().Equals(lTokens[i].getTexto().ToString()))
                                            {
                                                Int32.TryParse(lTokens[i + 3].getTexto(), out a);
                                                lVariable[v].setValor(operar(a, lTokens[i + 4], buscarvariable(lTokens[i + 5])));
                                            }

                                        }
                                    }
                                }
                            }
                            else if (lTokens[i + 3].getTipo() == Tokens.Tipo.IDENTIFICADOR)
                            {
                                if (lTokens[i + 4].getTipo() == Tokens.Tipo.PUNTO_Y_COMA)
                                {
                                    for (int w = 0; w <= lVariable.Count - 1; w += 1)
                                    {
                                        if (lVariable[w].getNombre().Equals(lTokens[i + 3].getTexto()))
                                        {
                                            Int32.TryParse(lVariable[w].getValor(), out valor);
                                        }

                                    }
                                    for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                    {
                                        if (lVariable[v].getNombre().Equals(lTokens[i].getTexto().ToString()))
                                        {
                                            lVariable[v].setValor(valor);
                                        }

                                    }
                                }
                                else if (lTokens[i + 4].getTipo() == Tokens.Tipo.MAS | lTokens[i + 4].getTipo() == Tokens.Tipo.MENOS | lTokens[i + 4].getTipo() == Tokens.Tipo.POR | lTokens[i + 4].getTipo() == Tokens.Tipo.DIVIDIR)
                                {
                                    if (lTokens[i + 5].getTipo() == Tipo.NUMERO_ENTERO)
                                    {
                                        for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                        {
                                            if (lVariable[v].getNombre().Equals(lTokens[i].getTexto().ToString()))
                                            {
                                                Int32.TryParse(lTokens[i + 5].getTexto(), out b);
                                                lVariable[v].setValor(operar(buscarvariable(lTokens[i + 3]), lTokens[i + 4], b));
                                            }
                                        }
                                    }
                                    else if (lTokens[i + 5].getTipo() == Tipo.IDENTIFICADOR)
                                    {
                                        for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                        {
                                            if (lVariable[v].getNombre().Equals(lTokens[i].getTexto().ToString()))
                                            {
                                                Int32.TryParse(lTokens[i + 3].getTexto(), out a);
                                                lVariable[v].setValor(operar(a, lTokens[i + 4], buscarvariable(lTokens[i + 5])));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                int x = 0;
                int y = 0;
                // Verificar Paso
                if (lTokens[i].getTipo() == Tokens.Tipo.PASO)
                {
                    if (lTokens[i + 1].getTipo() == Tokens.Tipo.CORCHETE_CERRADO)
                    {
                        if (lTokens[i + 2].getTipo() == Tokens.Tipo.DOS_PUNTOS)
                        {
                            if (lTokens[i + 3].getTipo() == Tokens.Tipo.PARENTESIS_ABIERTO)
                            {
                                // numero,numero
                                if (lTokens[i + 4].getTipo() == Tokens.Tipo.NUMERO_ENTERO & lTokens[i + 6].getTipo() == Tokens.Tipo.NUMERO_ENTERO)
                                {
                                    Int32.TryParse(lTokens[i + 4].getTexto(), out a);
                                    Int32.TryParse(lTokens[i + 6].getTexto(), out b);
                                    lCoordenadas.Add(new Coordenada(a, b));
                                }

                                else if (lTokens[i + 4].getTipo() == Tokens.Tipo.NUMERO_ENTERO & lTokens[i + 6].getTipo() == Tokens.Tipo.IDENTIFICADOR)
                                {

                                    for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                    {
                                        if (lVariable[v].getNombre().Equals(lTokens[i + 6].getTexto()))
                                        {
                                            Int32.TryParse(lVariable[v].getValor(), out y);
                                        }
                                    }
                                    Int32.TryParse(lTokens[i + 4].getTexto(), out x);
                                    lCoordenadas.Add(new Coordenada(x, y));
                                }
                                else if ((lTokens[i + 4].getTipo() == Tokens.Tipo.IDENTIFICADOR & lTokens[i + 6].getTipo() == Tokens.Tipo.NUMERO_ENTERO))
                                {
                                    for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                    {
                                        if (lVariable[v].getNombre().Equals(lTokens[i + 4].getTexto()))
                                            Int32.TryParse(lVariable[v].getValor(), out x);
                                    }
                                    Int32.TryParse(lTokens[i + 6].getTexto(), out y);
                                    lCoordenadas.Add(new Coordenada(x, y));
                                }
                                else if (lTokens[i + 4].getTipo() == Tokens.Tipo.IDENTIFICADOR & lTokens[i + 6].getTipo() == Tokens.Tipo.IDENTIFICADOR)
                                {
                                    for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                    {
                                        if (lVariable[v].getNombre().Equals(lTokens[i + 4].getTexto()))
                                            Int32.TryParse(lVariable[v].getValor(), out x);
                                    }
                                    for (int w = 0; w <= lVariable.Count - 1; w += 1)
                                    {
                                        if (lVariable[w].getNombre().Equals(lTokens[i + 6].getTexto()))
                                            Int32.TryParse(lVariable[w].getValor(), out y);
                                    }
                                    lCoordenadas.Add(new Coordenada(x, y));
                                }

                                else
                                    System.Windows.Forms.MessageBox.Show("Error de sintaxis en [Paso]:();");
                            }
                        }
                    }

                    // Verificar Intervalo ]:(1000);
                    if (lTokens[i].getTipo() == Tokens.Tipo.INTERVALO)
                    {
                        if (lTokens[i + 1].getTipo() == Tokens.Tipo.CORCHETE_CERRADO)
                        {
                            if (lTokens[i + 2].getTipo() == Tokens.Tipo.DOS_PUNTOS)
                            {
                                if (lTokens[i + 3].getTipo() == Tokens.Tipo.PARENTESIS_ABIERTO)
                                {
                                    if (lTokens[i + 4].getTipo() == Tokens.Tipo.NUMERO_ENTERO)
                                    {
                                        if (lTokens[i + 5].getTipo() == Tokens.Tipo.PARENTESIS_CERRADO)
                                        {
                                            if (lTokens[i + 6].getTipo() == Tokens.Tipo.PUNTO_Y_COMA)
                                            {
                                                Int32.TryParse(lTokens[i + 4].getTexto(), out Module1.intervalo);
                                            }
                                            else
                                                System.Windows.Forms.MessageBox.Show("Error de sintaxis en [Intervalo]:();");
                                        }
                                        else
                                            System.Windows.Forms.MessageBox.Show("Error de sintaxis en [Intervalo]:();");
                                    }
                                    else
                                        System.Windows.Forms.MessageBox.Show("Error de sintaxis en [Intervalo]:();");
                                }
                                else
                                    System.Windows.Forms.MessageBox.Show("Error de sintaxis en [Intervalo]:();");
                            }
                            else
                                System.Windows.Forms.MessageBox.Show("Error de sintaxis en [Intervalo]:();");
                        }
                        else
                            System.Windows.Forms.MessageBox.Show("Error de sintaxis en [Intervalo]:();");
                    }
                    int pasito;
                    // Verificar Caminata
                    if (lTokens[i].getTipo() == Tokens.Tipo.CAMINATA)
                    {
                        if ((lTokens[i + 1].getTipo() == Tokens.Tipo.CORCHETE_CERRADO))
                        {
                            if ((lTokens[i + 2].getTipo() == Tokens.Tipo.DOS_PUNTOS))
                            {
                                if ((lTokens[i + 3].getTipo() == Tokens.Tipo.PARENTESIS_ABIERTO))
                                {
                                    // numero, numero..numero
                                    if ((lTokens[i + 4].getTipo() == Tokens.Tipo.NUMERO_ENTERO & lTokens[i + 5].getTipo() == Tokens.Tipo.COMA & lTokens[i + 6].getTipo() == Tokens.Tipo.NUMERO_ENTERO & lTokens[i + 9].getTipo() == Tokens.Tipo.NUMERO_ENTERO))
                                    {
                                        Int32.TryParse(lTokens[i + 6].getTexto(), out a);
                                        Int32.TryParse(lTokens[i + 9].getTexto(), out b);
                                        if (a < b)
                                        {
                                            pasito = 1;
                                        }
                                        else
                                        {
                                            pasito = -1;
                                        }
                                        for (int cy = a; cy <= b; cy += pasito)
                                        {
                                            Int32.TryParse(lTokens[i + 4].getTexto(), out d);
                                            lCoordenadas.Add(new Coordenada(d, cy));
                                        }
                                    }
                                    else if (lTokens[i + 4].getTipo() == Tokens.Tipo.IDENTIFICADOR & lTokens[i + 5].getTipo() == Tokens.Tipo.COMA & lTokens[i + 6].getTipo() == Tokens.Tipo.NUMERO_ENTERO & lTokens[i + 9].getTipo() == Tokens.Tipo.NUMERO_ENTERO)
                                    {
                                        for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                        {
                                            if (lVariable[v].getNombre().Equals(lTokens[i + 4].getTexto()))
                                            {
                                                Int32.TryParse(lVariable[v].getValor(), out x);
                                            }
                                        }
                                        Int32.TryParse(lTokens[i + 6].getTexto(), out a);
                                        Int32.TryParse(lTokens[i + 9].getTexto(), out b);
                                        if (a < b)
                                        {
                                            pasito = 1;
                                        }
                                        else
                                        {
                                            pasito = -1;
                                        }
                                        for (int cy = a; cy <= b; cy += pasito)
                                        {
                                            lCoordenadas.Add(new Coordenada(x, cy));
                                        }
                                    }
                                    else if (lTokens[i + 4].getTipo() == Tokens.Tipo.NUMERO_ENTERO & lTokens[i + 7].getTipo() == Tokens.Tipo.NUMERO_ENTERO & lTokens[i + 8].getTipo() == Tokens.Tipo.COMA & lTokens[i + 9].getTipo() == Tokens.Tipo.IDENTIFICADOR)
                                    {
                                        for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                        {
                                            if (lVariable[v].getNombre().Equals(lTokens[i + 9].getTexto()))
                                                Int32.TryParse(lVariable[v].getValor(), out y);
                                        }
                                        Int32.TryParse(lTokens[i + 4].getTexto(), out a);
                                        Int32.TryParse(lTokens[i + 7].getTexto(), out b);
                                        if (a < b)
                                            pasito = 1;
                                        else
                                            pasito = -1;
                                        for (int cx = a; cx <= b; cx += pasito)
                                            lCoordenadas.Add(new Coordenada(cx, y));
                                    }
                                    else if (lTokens[i + 4].getTipo() == Tokens.Tipo.NUMERO_ENTERO & lTokens[i + 7].getTipo() == Tokens.Tipo.NUMERO_ENTERO & lTokens[i + 8].getTipo() == Tokens.Tipo.COMA & lTokens[i + 9].getTipo() == Tokens.Tipo.NUMERO_ENTERO)
                                    {
                                        Int32.TryParse(lTokens[i + 4].getTexto(), out a);
                                        Int32.TryParse(lTokens[i + 7].getTexto(), out b);
                                        if (a < b)
                                            pasito = 1;
                                        else
                                            pasito = -1;
                                        for (int cx = a; cx <= b; cx += pasito)
                                        {
                                            Int32.TryParse(lTokens[i + 9].getTexto(), out y);
                                            lCoordenadas.Add(new Coordenada(cx, y));
                                        }
                                    }
                                    else if (lTokens[i + 4].getTipo() == Tokens.Tipo.NUMERO_ENTERO & lTokens[i + 5].getTipo() == Tokens.Tipo.COMA & lTokens[i + 6].getTipo() == Tokens.Tipo.IDENTIFICADOR & lTokens[i + 9].getTipo() == Tokens.Tipo.VARIABLE)
                                    {
                                        if (buscarvariable(lTokens[i + 6]) < buscarvariable(lTokens[i + 9]))
                                            pasito = 1;
                                        else
                                            pasito = -1;
                                        for (int cy = buscarvariable(lTokens[i + 6]); cy <= buscarvariable(lTokens[i + 9]); cy += pasito)
                                        {
                                            Int32.TryParse(lTokens[i + 4].getTexto(), out x);
                                            lCoordenadas.Add(new Coordenada(x, cy));
                                        }
                                    }
                                    else if (lTokens[i + 4].getTipo() == Tokens.Tipo.NUMERO_ENTERO & lTokens[i + 5].getTipo() == Tokens.Tipo.COMA & lTokens[i + 6].getTipo() == Tokens.Tipo.IDENTIFICADOR & lTokens[i + 9].getTipo() == Tokens.Tipo.NUMERO_ENTERO)
                                    {
                                        Int32.TryParse(lTokens[i + 6].getTexto(), out a);
                                        Int32.TryParse(lTokens[i + 9].getTexto(), out b);

                                        if (buscarvariable(lTokens[i + 6]) < b)
                                            pasito = 1;
                                        else
                                            pasito = -1;
                                        for (int cy = a; cy <= b; cy += pasito)
                                        {
                                            Int32.TryParse(lTokens[i + 4].getTexto(), out x);
                                            lCoordenadas.Add(new Coordenada(x, cy));
                                        }
                                    }
                                    else if (lTokens[i + 4].getTipo() == Tokens.Tipo.NUMERO_ENTERO & lTokens[i + 7].getTipo() == Tokens.Tipo.IDENTIFICADOR & lTokens[i + 8].getTipo() == Tokens.Tipo.COMA & lTokens[i + 9].getTipo() == Tokens.Tipo.IDENTIFICADOR)
                                    {
                                        Int32.TryParse(lTokens[i + 4].getTexto(), out a);
                                        Int32.TryParse(lTokens[i + 7].getTexto(), out b);
                                        if (a < buscarvariable(lTokens[i + 7]))
                                            pasito = 1;
                                        else
                                            pasito = -1;
                                        for (int cx = a; cx <= buscarvariable(lTokens[i + 7]); cx += pasito)
                                        {
                                            Int32.TryParse(lTokens[i + 9].getTexto(), out y);
                                            lCoordenadas.Add(new Coordenada(cx, y));
                                        }
                                    }
                                    else if (lTokens[i + 4].getTipo() == Tokens.Tipo.IDENTIFICADOR & lTokens[i + 7].getTipo() == Tokens.Tipo.IDENTIFICADOR & lTokens[i + 8].getTipo() == Tokens.Tipo.COMA & lTokens[i + 9].getTipo() == Tokens.Tipo.IDENTIFICADOR)
                                    {
                                        if (buscarvariable(lTokens[i + 4]) < buscarvariable(lTokens[i + 7]))
                                            pasito = 1;
                                        else
                                            pasito = -1;
                                        for (int cx = buscarvariable(lTokens[i + 4]); cx <= buscarvariable(lTokens[i + 7]); cx += pasito)
                                        {
                                            lCoordenadas.Add(new Coordenada(cx, buscarvariable(lTokens[i + 9])));
                                        }
                                    }
                                    else if (lTokens[i + 4].getTipo() == Tokens.Tipo.IDENTIFICADOR & lTokens[i + 5].getTipo() == Tokens.Tipo.COMA & lTokens[i + 6].getTipo() == Tokens.Tipo.IDENTIFICADOR & lTokens[i + 9].getTipo() == Tokens.Tipo.IDENTIFICADOR)
                                    {
                                        if (buscarvariable(lTokens[i + 6]) < buscarvariable(lTokens[i + 9]))
                                            pasito = 1;
                                        else
                                            pasito = -1;
                                        for (int cy = buscarvariable(lTokens[i + 6]); cy <= buscarvariable(lTokens[i + 9]); cy += pasito)
                                            lCoordenadas.Add(new Coordenada(buscarvariable(lTokens[i + 4]), cy));
                                    }
                                }
                            }
                        }
                    }

                    // Verificar Ubicación Personaje
                    if (lTokens[i].getTipo() == Tokens.Tipo.UBICACION_PERSONAJE)
                    {
                        if (lTokens[i + 1].getTipo() == Tokens.Tipo.CORCHETE_CERRADO)
                        {
                            if (lTokens[i + 2].getTipo() == Tokens.Tipo.DOS_PUNTOS)
                            {
                                if (lTokens[i + 3].getTipo() == Tokens.Tipo.PARENTESIS_ABIERTO)
                                {
                                    if (lTokens[i + 4].getTipo() == Tokens.Tipo.NUMERO_ENTERO)
                                    {
                                        if (lTokens[i + 5].getTipo() == Tokens.Tipo.COMA)
                                        {
                                            if (lTokens[i + 6].getTipo() == Tokens.Tipo.NUMERO_ENTERO)
                                            {
                                                if (lTokens[i + 7].getTipo() == Tokens.Tipo.PARENTESIS_CERRADO)
                                                {
                                                    if (lTokens[i + 8].getTipo() == Tokens.Tipo.PUNTO_Y_COMA)
                                                    {
                                                        Int32.TryParse(lTokens[i + 4].getTexto(), out Module1.x0personaje);
                                                        Int32.TryParse(lTokens[i + 6].getTexto(), out Module1.y0personaje);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    // Verificar Ubicación Tesoro
                    if (lTokens[i].getTipo() == Tokens.Tipo.UBICACION_TESORO)
                    {
                        if (lTokens[i + 1].getTipo() == Tokens.Tipo.CORCHETE_CERRADO)
                        {
                            if (lTokens[i + 2].getTipo() == Tokens.Tipo.DOS_PUNTOS)
                            {
                                if (lTokens[i + 3].getTipo() == Tokens.Tipo.PARENTESIS_ABIERTO)
                                {
                                    if (lTokens[i + 4].getTipo() == Tokens.Tipo.NUMERO_ENTERO)
                                    {
                                        if (lTokens[i + 5].getTipo() == Tokens.Tipo.COMA)
                                        {
                                            if (lTokens[i + 6].getTipo() == Tokens.Tipo.NUMERO_ENTERO)
                                            {
                                                if (lTokens[i + 7].getTipo() == Tokens.Tipo.PARENTESIS_CERRADO)
                                                {
                                                    if (lTokens[i + 8].getTipo() == Tokens.Tipo.PUNTO_Y_COMA)
                                                    {
                                                        Int32.TryParse(lTokens[i + 4].getTexto(), out Module1.x0personaje);
                                                        Int32.TryParse(lTokens[i + 6].getTexto(), out Module1.y0personaje);
                                                        Console.WriteLine("LA PUTA MADRE LASKDFJALSKDFJALSDKFJADF " + lTokens[i + 4].getTexto() + "Y" + lTokens[i + 6].getTexto());
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // Verificar Dimensiones
                    if (lTokens[i].getTipo() == Tokens.Tipo.DIMENSIONES)
                    {
                        if (lTokens[i + 1].getTipo() == Tokens.Tipo.CORCHETE_CERRADO)
                        {
                            if (lTokens[i + 2].getTipo() == Tokens.Tipo.DOS_PUNTOS)
                            {
                                if (lTokens[i + 3].getTipo() == Tokens.Tipo.PARENTESIS_ABIERTO)
                                {
                                    if (lTokens[i + 4].getTipo() == Tokens.Tipo.NUMERO_ENTERO)
                                    {
                                        if (lTokens[i + 5].getTipo() == Tokens.Tipo.COMA)
                                        {
                                            if (lTokens[i + 6].getTipo() == Tokens.Tipo.NUMERO_ENTERO)
                                            {
                                                if (lTokens[i + 7].getTipo() == Tokens.Tipo.PARENTESIS_CERRADO)
                                                {
                                                    if (lTokens[i + 8].getTipo() == Tokens.Tipo.PUNTO_Y_COMA)
                                                    {
                                                        Int32.TryParse(lTokens[i + 4].getTexto(), out Module1.x0personaje);
                                                        Int32.TryParse(lTokens[i + 6].getTexto(), out Module1.y0personaje);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // Verificar Obstaculos CASILLA
                    if (lTokens[i].getTipo() == Tokens.Tipo.CASILLA)
                    {
                        if (lTokens[i + 1].getTipo() == Tokens.Tipo.CORCHETE_CERRADO)
                        {
                            if (lTokens[i + 2].getTipo() == Tokens.Tipo.DOS_PUNTOS)
                            {
                                if (lTokens[i + 3].getTipo() == Tokens.Tipo.PARENTESIS_ABIERTO)
                                {
                                    if (lTokens[i + 4].getTipo() == Tokens.Tipo.NUMERO_ENTERO & lTokens[i + 6].getTipo() == Tokens.Tipo.NUMERO_ENTERO)
                                    {
                                        Int32.TryParse(lTokens[i + 4].getTexto(), out a);
                                        Int32.TryParse(lTokens[i + 6].getTexto(), out b);

                                        lObstaculos.Add(new Obstaculos(a, b, 1));
                                    }
                                    else if (lTokens[i + 4].getTipo() == Tokens.Tipo.NUMERO_ENTERO & lTokens[i + 6].getTipo() == Tokens.Tipo.IDENTIFICADOR)
                                    {
                                        for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                        {
                                            if (lVariable[v].getNombre().Equals(lTokens[i + 6].getTexto()))
                                            {
                                                Int32.TryParse(lVariable[v].getValor(), out y);
                                            }
                                        }
                                        Int32.TryParse(lTokens[i + 4].getTexto(), out a);
                                        lObstaculos.Add(new Obstaculos(a, y, 1));
                                    }
                                    else if (lTokens[i + 4].getTipo() == Tokens.Tipo.IDENTIFICADOR & lTokens[i + 6].getTipo() == Tokens.Tipo.NUMERO_ENTERO)
                                    {
                                        for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                        {
                                            if (lVariable[v].getNombre().Equals(lTokens[i + 4].getTexto()))
                                                Int32.TryParse(lVariable[v].getValor(), out x);
                                        }
                                        Int32.TryParse(lTokens[i + 6].getTexto(), out d);
                                        lObstaculos.Add(new Obstaculos(x, d, 1));
                                    }
                                    else if (lTokens[i + 4].getTipo() == Tokens.Tipo.IDENTIFICADOR & lTokens[i + 6].getTipo() == Tokens.Tipo.IDENTIFICADOR)
                                    {
                                        for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                        {
                                            if (lVariable[v].getNombre().Equals(lTokens[i + 4].getTexto()))
                                                Int32.TryParse(lVariable[v].getValor(), out x);
                                        }
                                        for (int w = 0; w <= lVariable.Count - 1; w += 1)
                                        {
                                            if (lVariable[w].getNombre().Equals(lTokens[i + 6].getTexto()))
                                                Int32.TryParse(lVariable[w].getValor(), out x);
                                        }
                                        lObstaculos.Add(new Obstaculos(x, y, 1));
                                    }
                                }
                            }
                        }
                    }
                    int vv = 0;
                    // Verificar Rango_Casillas
                    if (lTokens[i].getTipo() == Tokens.Tipo.RANGO_CASILLA)
                    {
                        if (lTokens[i + 1].getTipo() == Tokens.Tipo.CORCHETE_CERRADO)
                        {
                            if (lTokens[i + 2].getTipo() == Tokens.Tipo.DOS_PUNTOS)
                            {
                                if (lTokens[i + 3].getTipo() == Tokens.Tipo.PARENTESIS_ABIERTO)
                                {
                                    // numero, numero..numero
                                    if (lTokens[i + 4].getTipo() == Tokens.Tipo.NUMERO_ENTERO & lTokens[i + 5].getTipo() == Tokens.Tipo.COMA & lTokens[i + 6].getTipo() == Tokens.Tipo.NUMERO_ENTERO & lTokens[i + 9].getTipo() == Tokens.Tipo.NUMERO_ENTERO)
                                    {
                                        Int32.TryParse(lTokens[i + 6].getTexto(), out a);
                                        Int32.TryParse(lTokens[i + 9].getTexto(), out b);
                                        if (a < b)
                                            pasito = 1;
                                        else
                                            pasito = -1;
                                        for (int cm = a; cm <= b; cm += pasito)
                                        {
                                            Int32.TryParse(lTokens[i + 4].getTexto(), out d);
                                            lObstaculos.Add(new Obstaculos(d, cm, 1));
                                        }
                                    }
                                    else if (lTokens[i + 4].getTipo() == Tokens.Tipo.IDENTIFICADOR & lTokens[i + 5].getTipo() == Tokens.Tipo.COMA & lTokens[i + 6].getTipo() == Tokens.Tipo.NUMERO_ENTERO & lTokens[i + 9].getTipo() == Tokens.Tipo.NUMERO_ENTERO)
                                    {
                                        Int32.TryParse(lTokens[i + 6].getTexto(), out a);
                                        Int32.TryParse(lTokens[i + 9].getTexto(), out b);
                                        if (a < b)
                                            pasito = 1;
                                        else
                                            pasito = -1;
                                        for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                        {
                                            if (lVariable[v].getNombre().Equals(lTokens[i + 4].getTexto()))
                                                Int32.TryParse(lVariable[v].getValor(), out x);
                                        }
                                        for (int cm = a; cm <= b; cm += pasito)
                                            lObstaculos.Add(new Obstaculos(x, cm, 1));
                                    }
                                    else if (lTokens[i + 4].getTipo() == Tokens.Tipo.NUMERO_ENTERO & lTokens[i + 7].getTipo() == Tokens.Tipo.NUMERO_ENTERO & lTokens[i + 8].getTipo() == Tokens.Tipo.COMA & lTokens[i + 9].getTipo() == Tokens.Tipo.IDENTIFICADOR)
                                    {
                                        Int32.TryParse(lTokens[i + 4].getTexto(), out a);
                                        Int32.TryParse(lTokens[i + 7].getTexto(), out b);
                                        if (a < b)
                                            pasito = 1;
                                        else
                                            pasito = -1;
                                        for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                        {
                                            if (lVariable[v].getNombre().Equals(lTokens[i + 9].getTexto()))
                                                Int32.TryParse(lVariable[v].getValor(), out y);
                                        }
                                        for (int cx = a; cx <= b; cx += pasito)
                                            lObstaculos.Add(new Obstaculos(cx, y, 1));
                                    }
                                    else if (lTokens[i + 4].getTipo() == Tokens.Tipo.IDENTIFICADOR & lTokens[i + 7].getTipo() == Tokens.Tipo.NUMERO_ENTERO & lTokens[i + 8].getTipo() == Tokens.Tipo.COMA & lTokens[i + 9].getTipo() == Tokens.Tipo.IDENTIFICADOR)
                                    {
                                        Int32.TryParse(lTokens[i + 4].getTexto(), out a);
                                        Int32.TryParse(lTokens[i + 7].getTexto(), out b);
                                        if (a < b)
                                            pasito = 1;
                                        else
                                            pasito = -1;
                                        for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                        {
                                            if (lVariable[v].getNombre().Equals(lTokens[i + 4].getTexto()))
                                                Int32.TryParse(lVariable[v].getValor(), out d);
                                        }
                                        for (int w = 0; w <= lVariable.Count - 1; w += 1)
                                        {
                                            if (lVariable[w].getNombre().Equals(lTokens[i + 9].getTexto()))
                                                Int32.TryParse(lVariable[w].getValor(), out y);
                                        }
                                        for (int cx = d; cx <= b; cx += pasito)
                                            lObstaculos.Add(new Obstaculos(cx, y, 1));
                                    }
                                    else if (lTokens[i + 4].getTipo() == Tokens.Tipo.IDENTIFICADOR & lTokens[i + 7].getTipo() == Tokens.Tipo.NUMERO_ENTERO & lTokens[i + 8].getTipo() == Tokens.Tipo.COMA & lTokens[i + 9].getTipo() == Tokens.Tipo.NUMERO_ENTERO)
                                    {
                                        Int32.TryParse(lTokens[i + 7].getTexto(), out b);
                                        if (buscarvariable(lTokens[i + 4]) < b)
                                            pasito = 1;
                                        else
                                            pasito = -1;
                                        for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                        {
                                            if (lVariable[v].getNombre().Equals(lTokens[i + 4].getTexto()))
                                                Int32.TryParse(lVariable[v].getValor(), out d);
                                        }
                                        for (int cx = d; cx <= b; cx += pasito)
                                        {
                                            Int32.TryParse(lTokens[i + 9].getTexto(), out y);
                                            lObstaculos.Add(new Obstaculos(cx, y, 1));
                                        }
                                    }
                                    else if (lTokens[i + 4].getTipo() == Tokens.Tipo.NUMERO_ENTERO & lTokens[i + 5].getTipo() == Tokens.Tipo.COMA & lTokens[i + 6].getTipo() == Tokens.Tipo.NUMERO_ENTERO & lTokens[i + 9].getTipo() == Tokens.Tipo.IDENTIFICADOR)
                                    {
                                        if (Int32.Parse(lTokens[i + 6].getTexto()) < buscarvariable(lTokens[i + 9]))
                                            pasito = 1;
                                        else
                                            pasito = -1;

                                        for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                        {

                                            if (lVariable[v].getNombre().Equals(lTokens[i + 9].getTexto()))
                                            {
                                                Int32.TryParse(lVariable[v].getValor(), out vv);
                                            }
                                        }

                                        for (Int32.TryParse(lTokens[i + 6].getTexto(), out c); c <= vv; c += pasito)
                                        {
                                            lObstaculos.Add(new Obstaculos(Int32.Parse(lTokens[i + 4].getTexto()), c, 1));
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
                imprimirvariables(lVariable);
                Module1.lObstaculos = this.lObstaculos;
                Module1.lCoordenadas = this.lCoordenadas;
                Form2 F = new Form2();
                F.Show();
            }
        }

        void imprimirvariables(List<Variables> l)
        {
            foreach (Variables t in l)
            Console.WriteLine(t.getNombre().ToString() + "->" + t.getValor().ToString());
        }

        List<Coordenada> ObtenerLista()
        {
            return lCoordenadas;
        }

        Style BlueStyle = new TextStyle(Brushes.Blue, null, System.Drawing.FontStyle.Italic);
        Style RedStyle = new TextStyle(System.Drawing.Brushes.Red, null, System.Drawing.FontStyle.Regular);
        Style YellowStyle = new TextStyle(System.Drawing.Brushes.Yellow, null, System.Drawing.FontStyle.Bold);
        Style PinkStyle = new TextStyle(System.Drawing.Brushes.Pink, null, System.Drawing.FontStyle.Bold);
        Style SkyBlueStyle = new TextStyle(System.Drawing.Brushes.SkyBlue, null, System.Drawing.FontStyle.Bold);
        Style GreenStyle = new TextStyle(System.Drawing.Brushes.Green, null, System.Drawing.FontStyle.Italic);
        Style BrownStyle = new TextStyle(System.Drawing.Brushes.Brown, null, System.Drawing.FontStyle.Bold);
        Style VioletStyle = new TextStyle(System.Drawing.Brushes.Violet, null, System.Drawing.FontStyle.Bold);
        Style OrangeStyle = new TextStyle(System.Drawing.Brushes.Orange, null, System.Drawing.FontStyle.Bold);

        private void rtbT_TextChanged(object sender, EventArgs e)
        {
                Regex reservadas = new Regex(@"\b((P|p)rincipal|(I|i)ntevalo|(N|n)ivel|(D|d)imensiones|(I|i)nicio_(P|p)ersonaje|(U|u)bicacion_(S|s)alida|(P|p)ared|(E|e)nemigo|(C|c)aminata|(C|c)asilla|(V|v)arias_(C|c)asillas|(P|p)ersonaje|(P|p)aso|(V|v)ariable|(R|r)ango_(C|c)asillas)\b");
                Regex numeros = new Regex(@"\b\d+\b");
                Regex Dospuntos = new Regex(@":");
                Regex Llaves = new Regex(@"(\{|\})");
                Regex Corchetes = new Regex(@"(\[|\])");
                Regex Identificadores = new Regex(@"([a-z]|[A-Z])(\d+)");
                Regex Parentesis = new Regex(@"(\(|\))");
                Regex Varios = new Regex(@"(\.|\,|\;)");
                Regex Aritmetica = new Regex(@"(\+|\-|\*|\/|\=)");

                Range range = (sender as FastColoredTextBox).Range;

                range.ClearStyle(BlueStyle);
                range.SetStyle(BlueStyle, reservadas.ToString(), RegexOptions.IgnoreCase);

                range.ClearStyle(RedStyle);
                range.SetStyle(RedStyle, numeros.ToString(), RegexOptions.IgnoreCase);

                range.ClearStyle(YellowStyle);
                range.SetStyle(YellowStyle, Dospuntos.ToString(), RegexOptions.IgnoreCase);

                range.ClearStyle(PinkStyle);
                range.SetStyle(PinkStyle, Llaves.ToString(), RegexOptions.IgnoreCase);

                range.ClearStyle(SkyBlueStyle);
                range.SetStyle(SkyBlueStyle, Corchetes.ToString(), RegexOptions.IgnoreCase);

                range.ClearStyle(GreenStyle);
                range.SetStyle(GreenStyle, Identificadores.ToString(), RegexOptions.IgnoreCase);

                range.ClearStyle(BrownStyle);
                range.SetStyle(BrownStyle, Parentesis.ToString(), RegexOptions.IgnoreCase);

                range.ClearStyle(VioletStyle);
                range.SetStyle(VioletStyle, Varios.ToString(), RegexOptions.IgnoreCase);

                range.ClearStyle(OrangeStyle);
                range.SetStyle(OrangeStyle, Aritmetica.ToString(), RegexOptions.IgnoreCase);
        
        }
    }
}