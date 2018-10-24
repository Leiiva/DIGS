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
using static DIGS.AnalizadorLexico;
using static DIGS.Token;
using FastColoredTextBoxNS;
using System.Drawing;

namespace DIGS
{
    public partial class Form1 : Form
    {
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
            using (OpenFileDialog OFDialog = new OpenFileDialog() { Filter = @"Project Documents ""*.omg"" | *.omg", ValidateNames = true, Multiselect = false })
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
                using (SaveFileDialog SFDialog = new SaveFileDialog() { Filter = @"Project Documents ""*.omg"" | *.omg", ValidateNames = true })
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
            using (SaveFileDialog SFDialog = new SaveFileDialog() { Filter = @"Project Documents ""*.omg"" | *.omg", ValidateNames = true })
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

        private void analizarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.WriteLine("si paso");
            String[] lines = rtbT.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            AnalizadorLexico anlex = new AnalizadorLexico();
            for (int i = 0; i < lines.Length; i++)
            {
                anlex.analizar(lines[i], i);
            }
            for (int i = 0; i < anlex.salida.Count; i++)
            {
                Console.WriteLine(anlex.salida[i].valor);
            }
        }

        private void crearJuegoToolStripMenuItem_Click(object sender, EventArgs e)
        {

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
