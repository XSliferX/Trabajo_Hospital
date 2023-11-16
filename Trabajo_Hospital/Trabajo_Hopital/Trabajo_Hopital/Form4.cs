using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Trabajo_Hopital
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            MostrarEstadisticas();

        }

        private void MostrarEstadisticas()
        {
            // Datos de ejemplo (reemplaza esto con tus propios datos)
            List<Estadistica> estadisticas = new List<Estadistica>
            {
                new Estadistica { Categoria = "Categoría 1", Valor = 25 },
                new Estadistica { Categoria = "Categoría 2", Valor = 50 },
                new Estadistica { Categoria = "Categoría 3", Valor = 30 }
            };

            // Enlazar los datos al DataGridView
            dataGridView1.DataSource = estadisticas;
        }
        // Clase de ejemplo para representar datos de estadísticas
        public class Estadistica
        {
            public string Categoria { get; set; }
            public int Valor { get; set; }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
