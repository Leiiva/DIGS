using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace DIGS
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        List<Tokens> lTokens;
        List<Coordenada> lPasos = new List<Coordenada>();
        List<Obstaculos> lObstaculos = new List<Obstaculos>();
        // Variable que almacena la posición de la casilla en x
        int personajex = Module1.x0personaje;
        // Variable que almacena la posición de la casilla en y
        int personajey = Module1.y0personaje;
        // Variable que almacena la posición del tesoro en x
        int tesorox = Module1.x0tesoro;
        // Variable que almacena la posición del tesoro en y
        int tesoroy = Module1.y0tesoro;
        // Variable que almacena la dimension en x del mapa
        int dimensionx = Module1.dimensionx;
        // variable que almacena la dimensión en y del mapa
        int dimensiony = Module1.dimensiony;

        int[,] obstaculos = new int[Module1.dimensionx, Module1.dimensiony];

        void imprimirpasos(List<Coordenada> l)
        {
            foreach (Coordenada t in l)
                Console.WriteLine(t.getX().ToString() + "->" + t.getY().ToString());
        }
        void imprimirobstaculos(List<Obstaculos> l)
        {
            foreach (Obstaculos t in l)
                Console.WriteLine(t.getX().ToString() + "->" + t.getY().ToString());
        }

        private void btn_Click(object sender, EventArgs e)
        {
            lPasos = Module1.lCoordenadas;
            imprimirpasos(lPasos);
            imprimirobstaculos(lObstaculos);
            animacion(lPasos);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true; // Seteamos doble buffer para que las imagenes no parpadeen al refrescar el Form
            this.Size = new System.Drawing.Size(51 * dimensionx, 55 * dimensiony); // Cambiamos el tamaño para que quepa la imagen de fondo que mide 500 x 500
                                                                                     // Llenar la matriz de 0
            for (var i = 0; i <= obstaculos.GetUpperBound(0); i++)
            {
                for (var j = 0; j <= obstaculos.GetUpperBound(1); j++)
                    obstaculos[i,j] = 0;
            }
            // Llenar la matriz de 1
            lObstaculos = Module1.lObstaculos;
            for (int d = 0; d <= lObstaculos.Count - 1; d += 1)
                obstaculos[lObstaculos[d].getX(), lObstaculos[d].getY()] = lObstaculos[d].getObs();
        }

        private void Form2_Paint(object sender, PaintEventArgs pe)
        {
            // Cada vez que llamemos al método refresh del Form, se ejecutará este método, que es el del evento paint
            // Definimos un objeto Graphics que se usará para dibujar sobre el formulario
            Graphics g = pe.Graphics;

            // Dibujamos el fondo
            Bitmap imagen = new Bitmap("obstaculos.jpg");
            imagen.SetResolution(51 * dimensionx, 55 * dimensiony);
            g.DrawImage(imagen, 0, 0);


            // Dibujamos los obstaculos
            imagen = new Bitmap("obstaculos.jpg");
            for (var i = 0; i <= obstaculos.GetUpperBound(0); i++)
            {
                for (var j = 0; j <= obstaculos.GetUpperBound(1); j++)
                {
                    if (obstaculos[i, j] == 1)
                        g.DrawImage(imagen, 50 * i, 50 * j);// Lo multiplico por 50 porque la imagen obstaculo es de 50*50
                }
            }
            // Dibujamos el tesoro
            imagen = new Bitmap("tesoro.jpg");
            g.DrawImage(imagen, tesorox * 50, tesoroy * 50);
            // Dibujamos la casilla según el lugar en el que esté
            imagen = new Bitmap("personaje.jpg");
            g.DrawImage(imagen, 50 * personajex, 50 * personajey); // Lo multiplico por 50 porque la imagen casilla es de 50*50
        }

        private void animacion(List<Coordenada> lista)
        {
            int a, b = 0;
            foreach (var c in lista)
            {
                // Actualizamos la posición en x segun la coordenada del elemento de la lista
                Int32.TryParse(c.getX(),out a);
                Int32.TryParse(c.getY(), out b);
                personajex = a;
                // Actualizamos la posición en y segun la coordenada del elemento de la lista
                personajey = b;
                // Esperamos 200 milisegundos
                Thread.Sleep(Module1.intervalo);
                // Refrescamos el formulario para que se repinte el fondo y la casilla en sus nuevas coordenadas
                this.Refresh();
            }
            if (personajex == tesorox & personajey == tesoroy)
                System.Windows.Forms.MessageBox.Show("Has Ganado");
            else
                System.Windows.Forms.MessageBox.Show("Has Perdido");
        }

    }
}


