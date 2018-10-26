using System;
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
        IContainer components;
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
            foreach(Tokens t in li)
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

        private void crearJuegoToolStripMenuItem_Click(object sender, EventArgs e)
        {
                lVariable.Clear();

                for (int i = 0; i <= lTokens.Count - 1; i += 1)
                {

                    // Agregar Variables
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
                    int valor;
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
                                            if ((lVariable[v].getNombre().Equals(lTokens[i].getTexto().ToString())))
                                                lVariable[v].setValor(lTokens[i + 3].getTexto().ToString());
                                        }
                                    }
                                    else if ((lTokens(i + 4).getTipo == Tokens.Tipo.MAS | lTokens(i + 4).getTipo == Tokens.Tipo.MENOS | lTokens(i + 4).getTipo == Tokens.Tipo.POR | lTokens(i + 4).getTipo == Tokens.Tipo.DIVIDIR))
                                    {
                                        if ((lTokens(i + 5).getTipo == Tipo.NUMERO_ENTERO))
                                        {
                                            for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                            {
                                                if ((lVariable(v).getNombre.Equals(lTokens(i).getTexto.ToString)))
                                                    lVariable(v).setValor(operar(Val(lTokens(i + 3).getTexto), lTokens(i + 4), Val(lTokens(i + 5).getTexto)).ToString);
                                            }
                                        }
                                        else if ((lTokens(i + 5).getTipo == Tipo.IDENTIFICADOR))
                                        {
                                            for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                            {
                                                if ((lVariable(v).getNombre.Equals(lTokens(i).getTexto.ToString)))
                                                    lVariable(v).setValor(operar(Val(lTokens(i + 3).getTexto), lTokens(i + 4), Val(buscarvariable(lTokens(i + 5)))).ToString);
                                            }
                                        }
                                    }
                                }
                                else if ((lTokens(i + 3).getTipo == Tokens.Tipo.IDENTIFICADOR))
                                {
                                    if ((lTokens(i + 4).getTipo == Tokens.Tipo.PUNTO_Y_COMA))
                                    {
                                        for (int w = 0; w <= lVariable.Count - 1; w += 1)
                                        {
                                            if ((lVariable(w).getNombre.Equals(lTokens(i + 3).getTexto)))
                                                valor = Val(lVariable(w).getValor);
                                        }
                                        for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                        {
                                            if ((lVariable(v).getNombre.Equals(lTokens(i).getTexto.ToString)))
                                                lVariable(v).setValor(valor);
                                        }
                                    }
                                    else if ((lTokens(i + 4).getTipo == Tokens.Tipo.MAS | lTokens(i + 4).getTipo == Tokens.Tipo.MENOS | lTokens(i + 4).getTipo == Tokens.Tipo.POR | lTokens(i + 4).getTipo == Tokens.Tipo.DIVIDIR))
                                    {
                                        if ((lTokens(i + 5).getTipo == Tipo.NUMERO_ENTERO))
                                        {
                                            for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                            {
                                                if ((lVariable(v).getNombre.Equals(lTokens(i).getTexto.ToString)))
                                                    lVariable(v).setValor(operar(Val(buscarvariable(lTokens(i + 3))), lTokens(i + 4), Val(lTokens(i + 5).getTexto)).ToString);
                                            }
                                        }
                                        else if ((lTokens(i + 5).getTipo == Tipo.IDENTIFICADOR))
                                        {
                                            for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                            {
                                                if ((lVariable(v).getNombre.Equals(lTokens(i).getTexto.ToString)))
                                                    lVariable(v).setValor(operar(Val(buscarvariable(lTokens(i + 3))), lTokens(i + 4), Val(buscarvariable(lTokens(i + 5)))).ToString);
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
                    if ((lTokens(i).getTipo == Tokens.Tipo.PASO))
                    {
                        if ((lTokens(i + 1).getTipo == Tokens.Tipo.CORCHETE_CERRADO))
                        {
                            if ((lTokens(i + 2).getTipo == Tokens.Tipo.DOS_PUNTOS))
                            {
                                if ((lTokens(i + 3).getTipo == Tokens.Tipo.PARENTESIS_ABIERTO))
                                {
                                    // numero,numero
                                    if ((lTokens(i + 4).getTipo == Tokens.Tipo.NUMERO_ENTERO & lTokens(i + 6).getTipo == Tokens.Tipo.NUMERO_ENTERO))
                                        lCoordenadas.Add(new Coordenada(Val(lTokens(i + 4).getTexto), Val(lTokens(i + 6).getTexto)));
                                    else if ((lTokens(i + 4).getTipo == Tokens.Tipo.NUMERO_ENTERO & lTokens(i + 6).getTipo == Tokens.Tipo.IDENTIFICADOR))
                                    {
                                        for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                        {
                                            if ((lVariable(v).getNombre.Equals(lTokens(i + 6).getTexto)))
                                                y = Val(lVariable(v).getValor);
                                        }
                                        lCoordenadas.Add(new Coordenada(Val(lTokens(i + 4).getTexto), y));
                                    }
                                    else if ((lTokens(i + 4).getTipo == Tokens.Tipo.IDENTIFICADOR & lTokens(i + 6).getTipo == Tokens.Tipo.NUMERO_ENTERO))
                                    {
                                        for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                        {
                                            if ((lVariable(v).getNombre.Equals(lTokens(i + 4).getTexto)))
                                                x = Val(lVariable(v).getValor);
                                        }
                                        lCoordenadas.Add(new Coordenada(x, Val(lTokens(i + 6).getTexto)));
                                    }
                                    else if ((lTokens(i + 4).getTipo == Tokens.Tipo.IDENTIFICADOR & lTokens(i + 6).getTipo == Tokens.Tipo.IDENTIFICADOR))
                                    {
                                        for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                        {
                                            if ((lVariable(v).getNombre.Equals(lTokens(i + 4).getTexto)))
                                                x = Val(lVariable(v).getValor);
                                        }
                                        for (int w = 0; w <= lVariable.Count - 1; w += 1)
                                        {
                                            if ((lVariable(w).getNombre.Equals(lTokens(i + 6).getTexto)))
                                                y = Val(lVariable(w).getValor);
                                        }
                                        lCoordenadas.Add(new Coordenada(x, y));
                                    }
                                }
                                else
                                    Interaction.MsgBox("Error de sintaxis en [Paso]:();");
                            }
                        }
                    }

                    // Verificar Intervalo ]:(1000);
                    if ((lTokens(i).getTipo == Tokens.Tipo.INTERVALO))
                    {
                        if ((lTokens(i + 1).getTipo == Tokens.Tipo.CORCHETE_CERRADO))
                        {
                            if ((lTokens(i + 2).getTipo == Tokens.Tipo.DOS_PUNTOS))
                            {
                                if ((lTokens(i + 3).getTipo == Tokens.Tipo.PARENTESIS_ABIERTO))
                                {
                                    if ((lTokens(i + 4).getTipo == Tokens.Tipo.NUMERO_ENTERO))
                                    {
                                        if ((lTokens(i + 5).getTipo == Tokens.Tipo.PARENTESIS_CERRADO))
                                        {
                                            if ((lTokens(i + 6).getTipo == Tokens.Tipo.PUNTO_Y_COMA))
                                                Module1.intervalo = Val(lTokens(i + 4).getTexto);
                                            else
                                                Interaction.MsgBox("Error de sintaxis en [Intervalo]:();");
                                        }
                                        else
                                            Interaction.MsgBox("Error de sintaxis en [Intervalo]:();");
                                    }
                                    else
                                        Interaction.MsgBox("Error de sintaxis en [Intervalo]:();");
                                }
                                else
                                    Interaction.MsgBox("Error de sintaxis en [Intervalo]:();");
                            }
                            else
                                Interaction.MsgBox("Error de sintaxis en [Intervalo]:();");
                        }
                        else
                            Interaction.MsgBox("Error de sintaxis en [Intervalo]:();");
                    }
                    int pasito;
                    // Verificar Caminata
                    if ((lTokens(i).getTipo == Tokens.Tipo.CAMINATA))
                    {
                        if ((lTokens(i + 1).getTipo == Tokens.Tipo.CORCHETE_CERRADO))
                        {
                            if ((lTokens(i + 2).getTipo == Tokens.Tipo.DOS_PUNTOS))
                            {
                                if ((lTokens(i + 3).getTipo == Tokens.Tipo.PARENTESIS_ABIERTO))
                                {
                                    // numero, numero..numero
                                    if ((lTokens(i + 4).getTipo == Tokens.Tipo.NUMERO_ENTERO & lTokens(i + 5).getTipo == Tokens.Tipo.COMA & lTokens(i + 6).getTipo == Tokens.Tipo.NUMERO_ENTERO & lTokens(i + 9).getTipo == Tokens.Tipo.NUMERO_ENTERO))
                                    {
                                        if ((Val(lTokens(i + 6).getTexto) < Val(lTokens(i + 9).getTexto)))
                                            pasito = 1;
                                        else
                                            pasito = -1;
                                        for (int c = Val(lTokens(i + 6).getTexto); c <= Val(lTokens(i + 9).getTexto); c += pasito)
                                            lCoordenadas.Add(new Coordenada(Val(lTokens(i + 4).getTexto), c));
                                    }
                                    else if ((lTokens(i + 4).getTipo == Tokens.Tipo.IDENTIFICADOR & lTokens(i + 5).getTipo == Tokens.Tipo.COMA & lTokens(i + 6).getTipo == Tokens.Tipo.NUMERO_ENTERO & lTokens(i + 9).getTipo == Tokens.Tipo.NUMERO_ENTERO))
                                    {
                                        for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                        {
                                            if ((lVariable(v).getNombre.Equals(lTokens(i + 4).getTexto)))
                                                x = Val(lVariable(v).getValor);
                                        }
                                        if ((Val(lTokens(i + 6).getTexto) < Val(lTokens(i + 9).getTexto)))
                                            pasito = 1;
                                        else
                                            pasito = -1;
                                        for (int c = Val(lTokens(i + 6).getTexto); c <= Val(lTokens(i + 9).getTexto); c += pasito)
                                            lCoordenadas.Add(new Coordenada(x, c));
                                    }
                                    else if ((lTokens(i + 4).getTipo == Tokens.Tipo.NUMERO_ENTERO & lTokens(i + 7).getTipo == Tokens.Tipo.NUMERO_ENTERO & lTokens(i + 8).getTipo == Tokens.Tipo.COMA & lTokens(i + 9).getTipo == Tokens.Tipo.IDENTIFICADOR))
                                    {
                                        for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                        {
                                            if ((lVariable(v).getNombre.Equals(lTokens(i + 9).getTexto)))
                                                y = Val(lVariable(v).getValor);
                                        }
                                        if ((Val(lTokens(i + 4).getTexto) < Val(lTokens(i + 7).getTexto)))
                                            pasito = 1;
                                        else
                                            pasito = -1;
                                        for (int c = Val(lTokens(i + 4).getTexto); c <= Val(lTokens(i + 7).getTexto); c += pasito)
                                            lCoordenadas.Add(new Coordenada(c, y));
                                    }
                                    else if ((lTokens(i + 4).getTipo == Tokens.Tipo.NUMERO_ENTERO & lTokens(i + 7).getTipo == Tokens.Tipo.NUMERO_ENTERO & lTokens(i + 8).getTipo == Tokens.Tipo.COMA & lTokens(i + 9).getTipo == Tokens.Tipo.NUMERO_ENTERO))
                                    {
                                        if ((Val(lTokens(i + 4).getTexto) < Val(lTokens(i + 7).getTexto)))
                                            pasito = 1;
                                        else
                                            pasito = -1;
                                        for (int c = Val(lTokens(i + 4).getTexto); c <= Val(lTokens(i + 7).getTexto); c += pasito)
                                            lCoordenadas.Add(new Coordenada(c, Val(lTokens(i + 9).getTexto)));
                                    }
                                    else if ((lTokens(i + 4).getTipo == Tokens.Tipo.NUMERO_ENTERO & lTokens(i + 5).getTipo == Tokens.Tipo.COMA & lTokens(i + 6).getTipo == Tokens.Tipo.IDENTIFICADOR & lTokens(i + 9).getTipo == Tokens.Tipo.VARIABLE))
                                    {
                                        if ((Val(buscarvariable(lTokens(i + 6))) < Val(buscarvariable(lTokens(i + 9)))))
                                            pasito = 1;
                                        else
                                            pasito = -1;
                                        for (int c = Val(buscarvariable(lTokens(i + 6))); c <= Val(buscarvariable(lTokens(i + 9))); c += pasito)
                                            lCoordenadas.Add(new Coordenada(Val(lTokens(i + 4).getTexto), c));
                                    }
                                    else if ((lTokens(i + 4).getTipo == Tokens.Tipo.NUMERO_ENTERO & lTokens(i + 5).getTipo == Tokens.Tipo.COMA & lTokens(i + 6).getTipo == Tokens.Tipo.IDENTIFICADOR & lTokens(i + 9).getTipo == Tokens.Tipo.NUMERO_ENTERO))
                                    {
                                        if ((Val(buscarvariable(lTokens(i + 6))) < Val(lTokens(i + 9).getTexto)))
                                            pasito = 1;
                                        else
                                            pasito = -1;
                                        for (int c = Val(buscarvariable(lTokens(i + 6))); c <= Val(lTokens(i + 9).getTexto); c += pasito)
                                            lCoordenadas.Add(new Coordenada(Val(lTokens(i + 4).getTexto), c));
                                    }
                                    else if ((lTokens(i + 4).getTipo == Tokens.Tipo.NUMERO_ENTERO & lTokens(i + 7).getTipo == Tokens.Tipo.IDENTIFICADOR & lTokens(i + 8).getTipo == Tokens.Tipo.COMA & lTokens(i + 9).getTipo == Tokens.Tipo.IDENTIFICADOR))
                                    {
                                        if ((Val(lTokens(i + 4).getTexto) < Val(buscarvariable(lTokens(i + 7)))))
                                            pasito = 1;
                                        else
                                            pasito = -1;
                                        for (int c = Val(lTokens(i + 4).getTexto); c <= Val(buscarvariable(lTokens(i + 7))); c += pasito)
                                            lCoordenadas.Add(new Coordenada(c, Val(buscarvariable(lTokens(i + 9)))));
                                    }
                                    else if ((lTokens(i + 4).getTipo == Tokens.Tipo.IDENTIFICADOR & lTokens(i + 7).getTipo == Tokens.Tipo.IDENTIFICADOR & lTokens(i + 8).getTipo == Tokens.Tipo.COMA & lTokens(i + 9).getTipo == Tokens.Tipo.IDENTIFICADOR))
                                    {
                                        if ((Val(buscarvariable(lTokens(i + 4))) < Val(buscarvariable(lTokens(i + 7)))))
                                            pasito = 1;
                                        else
                                            pasito = -1;
                                        for (int c = Val(buscarvariable(lTokens(i + 4))); c <= Val(buscarvariable(lTokens(i + 7))); c += pasito)
                                            lCoordenadas.Add(new Coordenada(c, Val(buscarvariable(lTokens(i + 9)))));
                                    }
                                    else if ((lTokens(i + 4).getTipo == Tokens.Tipo.IDENTIFICADOR & lTokens(i + 5).getTipo == Tokens.Tipo.COMA & lTokens(i + 6).getTipo == Tokens.Tipo.IDENTIFICADOR & lTokens(i + 9).getTipo == Tokens.Tipo.IDENTIFICADOR))
                                    {
                                        if ((Val(buscarvariable(lTokens(i + 6))) < Val(buscarvariable(lTokens(i + 9)))))
                                            pasito = 1;
                                        else
                                            pasito = -1;
                                        for (int c = Val(buscarvariable(lTokens(i + 6))); c <= Val(buscarvariable(lTokens(i + 9))); c += pasito)
                                            lCoordenadas.Add(new Coordenada(Val(buscarvariable(lTokens(i + 4))), c));
                                    }
                                }
                            }
                        }
                    }

                    // Verificar Ubicación Personaje
                    if ((lTokens(i).getTipo == Tokens.Tipo.UBICACION_PERSONAJE))
                    {
                        if ((lTokens(i + 1).getTipo == Tokens.Tipo.CORCHETE_CERRADO))
                        {
                            if ((lTokens(i + 2).getTipo == Tokens.Tipo.DOS_PUNTOS))
                            {
                                if ((lTokens(i + 3).getTipo == Tokens.Tipo.PARENTESIS_ABIERTO))
                                {
                                    if ((lTokens(i + 4).getTipo == Tokens.Tipo.NUMERO_ENTERO))
                                    {
                                        if ((lTokens(i + 5).getTipo == Tokens.Tipo.COMA))
                                        {
                                            if ((lTokens(i + 6).getTipo == Tokens.Tipo.NUMERO_ENTERO))
                                            {
                                                if ((lTokens(i + 7).getTipo == Tokens.Tipo.PARENTESIS_CERRADO))
                                                {
                                                    if ((lTokens(i + 8).getTipo == Tokens.Tipo.PUNTO_Y_COMA))
                                                    {
                                                        Module1.x0personaje = Val(lTokens(i + 4).getTexto);
                                                        Module1.y0personaje = Val(lTokens(i + 6).getTexto);
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
                    if ((lTokens(i).getTipo == Tokens.Tipo.UBICACION_TESORO))
                    {
                        if ((lTokens(i + 1).getTipo == Tokens.Tipo.CORCHETE_CERRADO))
                        {
                            if ((lTokens(i + 2).getTipo == Tokens.Tipo.DOS_PUNTOS))
                            {
                                if ((lTokens(i + 3).getTipo == Tokens.Tipo.PARENTESIS_ABIERTO))
                                {
                                    if ((lTokens(i + 4).getTipo == Tokens.Tipo.NUMERO_ENTERO))
                                    {
                                        if ((lTokens(i + 5).getTipo == Tokens.Tipo.COMA))
                                        {
                                            if ((lTokens(i + 6).getTipo == Tokens.Tipo.NUMERO_ENTERO))
                                            {
                                                if ((lTokens(i + 7).getTipo == Tokens.Tipo.PARENTESIS_CERRADO))
                                                {
                                                    if ((lTokens(i + 8).getTipo == Tokens.Tipo.PUNTO_Y_COMA))
                                                    {
                                                        Module1.x0tesoro = Val(lTokens(i + 4).getTexto);
                                                        Module1.y0tesoro = Val(lTokens(i + 6).getTexto);
                                                        Console.WriteLine("LA PUTA MADRE LASKDFJALSKDFJALSDKFJADF " + lTokens(i + 4).getTexto + "Y" + lTokens(i + 6).getTexto);
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
                    if ((lTokens(i).getTipo == Tokens.Tipo.DIMENSIONES))
                    {
                        if ((lTokens(i + 1).getTipo == Tokens.Tipo.CORCHETE_CERRADO))
                        {
                            if ((lTokens(i + 2).getTipo == Tokens.Tipo.DOS_PUNTOS))
                            {
                                if ((lTokens(i + 3).getTipo == Tokens.Tipo.PARENTESIS_ABIERTO))
                                {
                                    if ((lTokens(i + 4).getTipo == Tokens.Tipo.NUMERO_ENTERO))
                                    {
                                        if ((lTokens(i + 5).getTipo == Tokens.Tipo.COMA))
                                        {
                                            if ((lTokens(i + 6).getTipo == Tokens.Tipo.NUMERO_ENTERO))
                                            {
                                                if ((lTokens(i + 7).getTipo == Tokens.Tipo.PARENTESIS_CERRADO))
                                                {
                                                    if ((lTokens(i + 8).getTipo == Tokens.Tipo.PUNTO_Y_COMA))
                                                    {
                                                        Module1.dimensionx = Val(lTokens(i + 4).getTexto);
                                                        Module1.dimensiony = Val(lTokens(i + 6).getTexto);
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
                    if ((lTokens(i).getTipo == Tokens.Tipo.CASILLA))
                    {
                        if ((lTokens(i + 1).getTipo == Tokens.Tipo.CORCHETE_CERRADO))
                        {
                            if ((lTokens(i + 2).getTipo == Tokens.Tipo.DOS_PUNTOS))
                            {
                                if ((lTokens(i + 3).getTipo == Tokens.Tipo.PARENTESIS_ABIERTO))
                                {
                                    if ((lTokens(i + 4).getTipo == Tokens.Tipo.NUMERO_ENTERO & lTokens(i + 6).getTipo == Tokens.Tipo.NUMERO_ENTERO))
                                        lObstaculos.Add(new Obstaculos(Val(lTokens(i + 4).getTexto), Val(lTokens(i + 6).getTexto), 1));
                                    else if ((lTokens(i + 4).getTipo == Tokens.Tipo.NUMERO_ENTERO & lTokens(i + 6).getTipo == Tokens.Tipo.IDENTIFICADOR))
                                    {
                                        for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                        {
                                            if ((lVariable(v).getNombre.Equals(lTokens(i + 6).getTexto)))
                                                y = Val(lVariable(v).getValor);
                                        }
                                        lObstaculos.Add(new Obstaculos(Val(lTokens(i + 4).getTexto), y, 1));
                                    }
                                    else if ((lTokens(i + 4).getTipo == Tokens.Tipo.IDENTIFICADOR & lTokens(i + 6).getTipo == Tokens.Tipo.NUMERO_ENTERO))
                                    {
                                        for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                        {
                                            if ((lVariable(v).getNombre.Equals(lTokens(i + 4).getTexto)))
                                                x = Val(lVariable(v).getValor);
                                        }
                                        lObstaculos.Add(new Obstaculos(x, Val(lTokens(i + 6).getTexto), 1));
                                    }
                                    else if ((lTokens(i + 4).getTipo == Tokens.Tipo.IDENTIFICADOR & lTokens(i + 6).getTipo == Tokens.Tipo.IDENTIFICADOR))
                                    {
                                        for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                        {
                                            if ((lVariable(v).getNombre.Equals(lTokens(i + 4).getTexto)))
                                                x = Val(lVariable(v).getValor);
                                        }
                                        for (int w = 0; w <= lVariable.Count - 1; w += 1)
                                        {
                                            if ((lVariable(w).getNombre.Equals(lTokens(i + 6).getTexto)))
                                                y = Val(lVariable(w).getValor);
                                        }
                                        lObstaculos.Add(new Obstaculos(x, y, 1));
                                    }
                                }
                            }
                        }
                    }

                    int a;
                    int b;
                    // Verificar Rango_Casillas
                    if ((lTokens(i).getTipo == Tokens.Tipo.RANGO_CASILLA))
                    {
                        if ((lTokens(i + 1).getTipo == Tokens.Tipo.CORCHETE_CERRADO))
                        {
                            if ((lTokens(i + 2).getTipo == Tokens.Tipo.DOS_PUNTOS))
                            {
                                if ((lTokens(i + 3).getTipo == Tokens.Tipo.PARENTESIS_ABIERTO))
                                {
                                    // numero, numero..numero
                                    if ((lTokens(i + 4).getTipo == Tokens.Tipo.NUMERO_ENTERO & lTokens(i + 5).getTipo == Tokens.Tipo.COMA & lTokens(i + 6).getTipo == Tokens.Tipo.NUMERO_ENTERO & lTokens(i + 9).getTipo == Tokens.Tipo.NUMERO_ENTERO))
                                    {
                                        if ((Val(lTokens(i + 6).getTexto) < Val(lTokens(i + 9).getTexto)))
                                            pasito = 1;
                                        else
                                            pasito = -1;
                                        for (int c = Val(lTokens(i + 6).getTexto); c <= Val(lTokens(i + 9).getTexto); c += pasito)
                                            lObstaculos.Add(new Obstaculos(Val(lTokens(i + 4).getTexto), c, 1));
                                    }
                                    else if ((lTokens(i + 4).getTipo == Tokens.Tipo.IDENTIFICADOR & lTokens(i + 5).getTipo == Tokens.Tipo.COMA & lTokens(i + 6).getTipo == Tokens.Tipo.NUMERO_ENTERO & lTokens(i + 9).getTipo == Tokens.Tipo.NUMERO_ENTERO))
                                    {
                                        if ((Val(lTokens(i + 6).getTexto) < Val(lTokens(i + 9).getTexto)))
                                            pasito = 1;
                                        else
                                            pasito = -1;
                                        for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                        {
                                            if ((lVariable(v).getNombre.Equals(lTokens(i + 4).getTexto)))
                                                x = Val(lVariable(v).getValor);
                                        }
                                        for (int c = Val(lTokens(i + 6).getTexto); c <= Val(lTokens(i + 9).getTexto); c += pasito)
                                            lObstaculos.Add(new Obstaculos(x, c, 1));
                                    }
                                    else if ((lTokens(i + 4).getTipo == Tokens.Tipo.NUMERO_ENTERO & lTokens(i + 7).getTipo == Tokens.Tipo.NUMERO_ENTERO & lTokens(i + 8).getTipo == Tokens.Tipo.COMA & lTokens(i + 9).getTipo == Tokens.Tipo.IDENTIFICADOR))
                                    {
                                        if ((Val(lTokens(i + 4).getTexto) < Val(lTokens(i + 7).getTexto)))
                                            pasito = 1;
                                        else
                                            pasito = -1;
                                        for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                        {
                                            if ((lVariable(v).getNombre.Equals(lTokens(i + 9).getTexto)))
                                                y = Val(lVariable(v).getValor);
                                        }
                                        for (int c = Val(lTokens(i + 4).getTexto); c <= Val(lTokens(i + 7).getTexto); c += pasito)
                                            lObstaculos.Add(new Obstaculos(c, y, 1));
                                    }
                                    else if ((lTokens(i + 4).getTipo == Tokens.Tipo.IDENTIFICADOR & lTokens(i + 7).getTipo == Tokens.Tipo.NUMERO_ENTERO & lTokens(i + 8).getTipo == Tokens.Tipo.COMA & lTokens(i + 9).getTipo == Tokens.Tipo.IDENTIFICADOR))
                                    {
                                        if ((Val(buscarvariable(lTokens(i + 4))) < Val(lTokens(i + 7).getTexto)))
                                            pasito = 1;
                                        else
                                            pasito = -1;
                                        for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                        {
                                            if ((lVariable(v).getNombre.Equals(lTokens(i + 4).getTexto)))
                                                a = Val(lVariable(v).getValor);
                                        }
                                        for (int w = 0; w <= lVariable.Count - 1; w += 1)
                                        {
                                            if ((lVariable(w).getNombre.Equals(lTokens(i + 9).getTexto)))
                                                y = Val(lVariable(w).getValor);
                                        }
                                        for (int c = a; c <= Val(lTokens(i + 7).getTexto); c += pasito)
                                            lObstaculos.Add(new Obstaculos(c, y, 1));
                                    }
                                    else if ((lTokens(i + 4).getTipo == Tokens.Tipo.IDENTIFICADOR & lTokens(i + 7).getTipo == Tokens.Tipo.NUMERO_ENTERO & lTokens(i + 8).getTipo == Tokens.Tipo.COMA & lTokens(i + 9).getTipo == Tokens.Tipo.NUMERO_ENTERO))
                                    {
                                        if ((Val(buscarvariable(lTokens(i + 4))) < Val(lTokens(i + 7).getTexto)))
                                            pasito = 1;
                                        else
                                            pasito = -1;
                                        for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                        {
                                            if ((lVariable(v).getNombre.Equals(lTokens(i + 4).getTexto)))
                                                a = Val(lVariable(v).getValor);
                                        }
                                        for (int c = a; c <= Val(lTokens(i + 7).getTexto); c += pasito)
                                            lObstaculos.Add(new Obstaculos(c, Val(lTokens(i + 9).getTexto), 1));
                                    }
                                    else if ((lTokens(i + 4).getTipo == Tokens.Tipo.NUMERO_ENTERO & lTokens(i + 5).getTipo == Tokens.Tipo.COMA & lTokens(i + 6).getTipo == Tokens.Tipo.NUMERO_ENTERO & lTokens(i + 9).getTipo == Tokens.Tipo.IDENTIFICADOR))
                                    {
                                        if ((Val(lTokens(i + 6).getTexto) < Val(buscarvariable(lTokens(i + 9)))))
                                            pasito = 1;
                                        else
                                            pasito = -1;
                                        for (int v = 0; v <= lVariable.Count - 1; v += 1)
                                        {
                                            if ((lVariable(v).getNombre.Equals(lTokens(i + 9).getTexto)))
                                                b = Val(lVariable(v).getValor);
                                        }
                                        for (int c = Val(lTokens(i + 6).getTexto); c <= b; c += pasito)
                                            lObstaculos.Add(new Obstaculos(Val(lTokens(i + 4).getTexto), c, 1));
                                    }
                                }
                            }
                        }
                    }
                }
                imprimirvariables(lVariable);
                Module1.lObstaculos = this.lObstaculos;
                Module1.lCoordenadas = this.lCoordenadas;
                Form2.Show();
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

        private void rtbT_TextChanged(object sender, TextChangedEventArgs e)
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
