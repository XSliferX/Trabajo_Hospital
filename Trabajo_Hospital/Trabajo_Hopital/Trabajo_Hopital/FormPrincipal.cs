using System.Runtime.InteropServices;

namespace Trabajo_Hopital
{
    public partial class FormPrincipal : Form
    {
        public FormPrincipal()
        {
            InitializeComponent();
        }
        private void FormPrincipal_Load(object sender, EventArgs e)
        {

        }
        #region Funcionalidades del formulario
        //RESIZE METODO PARA REDIMENCIONAR/CAMBIAR TAMA�O A FORMULARIO EN TIEMPO DE EJECUCION ----------------------------------------------------------
        private int tolerance = 12;
        private const int WM_NCHITTEST = 132;
        private const int HTBOTTOMRIGHT = 17;
        private Rectangle sizeGripRectangle;
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    base.WndProc(ref m);
                    var hitPoint = this.PointToClient(new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16));
                    if (sizeGripRectangle.Contains(hitPoint))
                        m.Result = new IntPtr(HTBOTTOMRIGHT);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }
        //----------------DIBUJAR RECTANGULO / EXCLUIR ESQUINA PANEL 
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            var region = new Region(new Rectangle(0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height));
            sizeGripRectangle = new Rectangle(this.ClientRectangle.Width - tolerance, this.ClientRectangle.Height - tolerance, tolerance, tolerance);
            region.Exclude(sizeGripRectangle);
            this.panelContenedor.Region = region;
            this.Invalidate();
        }
        //----------------COLOR Y GRIP DE RECTANGULO INFERIOR
        protected override void OnPaint(PaintEventArgs e)
        {
            SolidBrush blueBrush = new SolidBrush(Color.FromArgb(244, 244, 244));
            e.Graphics.FillRectangle(blueBrush, sizeGripRectangle);
            base.OnPaint(e);
            ControlPaint.DrawSizeGrip(e.Graphics, Color.Transparent, sizeGripRectangle);
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //Capturar posicion y tama�o antes de maximizar para restaurar
        int lx, ly;
        int sw, sh;
        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            lx = this.Location.X;
            ly = this.Location.Y;
            sw = this.Size.Width;
            sh = this.Size.Height;
            btnMaximizar.Visible = false;
            btnRestaurar.Visible = true;
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            this.Location = Screen.PrimaryScreen.WorkingArea.Location;

        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            btnMaximizar.Visible = true;
            btnRestaurar.Visible = false;
            this.Size = new Size(sw, sh);
            this.Location = new Point(lx, ly);
        }
        // METODO PARA ARRASTRAR EL FORMULARIO
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void panelBarraTitulo_MouseMove(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AbrirFormulario<Form4>();
            button1.BackColor = Color.FromArgb(12, 61, 92);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AbrirFormulario<Form2>();
            button2.BackColor = Color.FromArgb(12, 61, 92);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AbrirFormulario<Form3>();
            button3.BackColor = Color.FromArgb(12, 61, 92);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            AbrirFormulario<Form1>();
            button4.BackColor = Color.FromArgb(12, 61, 92);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AbrirFormulario<Form5>();
            button5.BackColor = Color.FromArgb(12, 61, 92);

        }

        #endregion

        //METODO PARA ABRIR FORMULARIO DENTRO DEL PANEL
        private void AbrirFormulario<MiForm>() where MiForm : Form, new()
        {
            Form formulario;
            formulario = panelformularios.Controls.OfType<MiForm>().FirstOrDefault();//Busca en la colecci�n el formulario
            //si el formulario/instancia no existe
            if (formulario == null)
            {
                formulario = new MiForm();
                formulario.TopLevel = false;
                formulario.FormBorderStyle = FormBorderStyle.None;
                formulario.Dock = DockStyle.Fill;
                panelformularios.Controls.Add(formulario);
                panelformularios.Tag = formulario;
                formulario.Show();
                formulario.BringToFront();
                formulario.FormClosed += new FormClosedEventHandler(CloseForms);

                // Suscribirse al evento del formulario hijo (Form5 en este caso)
                if (formulario is Form5 form5)
                {
                    form5.AceptarClick += Form5_AceptarClick;
                }
                else if (formulario is Form3 form3)
                {
                    form3.FechaSeleccionada += Form3_FechaSeleccionada;
                }
            }
            //si el formulario existe
            else
            {
                formulario.BringToFront();
            }
        }
        private void Form3_FechaSeleccionada(object sender, DateTime fecha)
        {
            // Mostrar la fecha seleccionada en el lugar que desees
            MessageBox.Show($"Fecha seleccionada: {fecha.ToShortDateString()}");
        }
        // Manejar el evento del formulario hijo (Form5)
        private void Form5_AceptarClick(object sender, EventArgs e)
        {
            // Obtener datos del paciente desde Form5
            if (sender is Form5 form5)
            {
                string nombre = form5.Nombre;
                string apellidos = form5.Apellidos;
                string correo = form5.Correo;

                // Mostrar la informaci�n en los labels del panel principal
                label2.Text = $"Nombres: {nombre}";
                label3.Text = $"Apellidos: {apellidos}";
                label4.Text = $"Correo: {correo}";
            }
        }
        private void CloseForms(object sender, FormClosedEventArgs e)
        {
            if (Application.OpenForms["Form1"] == null)
                button1.BackColor = Color.FromArgb(4, 41, 68);
            if (Application.OpenForms["Form2"] == null)
                button2.BackColor = Color.FromArgb(4, 41, 68);
            if (Application.OpenForms["Form3"] == null)
                button3.BackColor = Color.FromArgb(4, 41, 68);
            if (Application.OpenForms["Form4"] == null)
                button4.BackColor = Color.FromArgb(4, 41, 68);
            if (Application.OpenForms["Form5"] == null)
                button5.BackColor = Color.FromArgb(4, 41, 68);
        }
        private void LimpiarLabelsFormPrincipal()
        {
            label2.Text = "Nombres: ";
            label3.Text = "Apellidos: ";
            label4.Text = "Correo: ";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Limpiar los datos en los labels del FormPrincipal
            LimpiarLabelsFormPrincipal();

            // Tambi�n puedes limpiar los datos en el Form5 si es necesario
            Form5 form5 = panelformularios.Controls.OfType<Form5>().FirstOrDefault();
            if (form5 != null)
            {
                form5.LimpiarDatos();
            }
        }

    }
}
