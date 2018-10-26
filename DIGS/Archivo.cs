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
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace DIGS
{
    class Archivo
    {
        public string archivo_actual;

        public string Abrir()
        {
            string linea = "";
            OpenFileDialog ventana = new OpenFileDialog();
            ventana.Filter = "Archivo DIGS(*.digs)|*.digs";
            ventana.Multiselect = false;
            ventana.CheckFileExists = false;
            ventana.Title = "Selecciona un Archivo";
            ventana.ShowDialog();
            if (ventana.FileName != "")
            {
                archivo_actual = ventana.FileName;
                StreamReader Lector = new StreamReader(archivo_actual);
                while (Lector.Peek() >= 0)
                    linea += Lector.ReadLine() + Environment.NewLine;
                Lector.Close();
            }
            return linea;
        }
        public void Guardar_Como(string[] texto)
        {
            SaveFileDialog guardarcomo = new SaveFileDialog();
            {
                var withBlock = guardarcomo;
                withBlock.Title = "Guardar Como...";
                withBlock.Filter = "Archivo DIGS(*.digs)|*.digs";
                withBlock.ShowDialog();
                if (withBlock.FileName != "")
                {
                    archivo_actual = withBlock.FileName;
                    StreamWriter escritor = new StreamWriter(archivo_actual);
                    foreach (string linea in texto)
                        escritor.WriteLine(linea);
                    escritor.Close();
                }
            }
        }
        public void Guardar(string[] texto)
        {
            if ((File.Exists(archivo_actual)))
            {
                StreamWriter escritor = new StreamWriter(archivo_actual);
                foreach (string linea in texto)
                    escritor.WriteLine(linea);
                escritor.Close();
            }
            else
                Guardar_Como(texto);
        }
    }
}
