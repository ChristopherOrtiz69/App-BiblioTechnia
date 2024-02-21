using System;
using System.Windows.Forms;
using Ghostscript.NET;
using Ghostscript.NET.Viewer;

namespace Prueba_Libro
{
    public partial class PdfForm : Form
    {
        private GhostscriptViewer _viewer;
        private GhostscriptVersionInfo _gsVersion = GhostscriptVersionInfo.GetLastInstalledVersion();
        private bool _shiftActivated = false;
        private bool _imprActivated = false;

        public PdfForm(string filePath)
        {
            InitializeComponent();
            InitializeViewer(filePath);
            InitializeNavigationButtons();

            // Inicializar el evento de teclado
            this.KeyPreview = true;
            this.KeyDown += PdfForm_KeyDown;
            this.KeyUp += PdfForm_KeyUp;
           
        }

        private void InitializeViewer(string filePath)
        {
            _viewer = new GhostscriptViewer();
            _viewer.DisplaySize += _viewer_DisplaySize;
            _viewer.DisplayUpdate += _viewer_DisplayUpdate;
            _viewer.DisplayPage += _viewer_DisplayPage;

            _viewer.Open(filePath, _gsVersion, false);
        }

        private void InitializeNavigationButtons()
        {
            Button btnNextPage = new Button();
            btnNextPage.Text = "Siguiente";
            btnNextPage.Location = new System.Drawing.Point(1050, 500);
            btnNextPage.Click += btnNextPage_Click;
            this.Controls.Add(btnNextPage);

            Button btnPreviousPage = new Button();
            btnPreviousPage.Text = "Anterior";
            btnPreviousPage.Location = new System.Drawing.Point(350, 500);
            btnPreviousPage.Click += btnPreviousPage_Click;
            this.Controls.Add(btnPreviousPage);

            Button btnFirstPage = new Button();
            btnFirstPage.Text = "Primera";
            btnFirstPage.Location = new System.Drawing.Point(190, 10);
            btnFirstPage.Click += btnFirstPage_Click;
            this.Controls.Add(btnFirstPage);

            Button btnLastPage = new Button();
            btnLastPage.Text = "Última";
            btnLastPage.Location = new System.Drawing.Point(280, 10);
            btnLastPage.Click += btnLastPage_Click;
            this.Controls.Add(btnLastPage);
        }

        private void _viewer_DisplaySize(object sender, GhostscriptViewerViewEventArgs e)
        {
            pictureBox1.Image = e.Image;
        }

        private void _viewer_DisplayUpdate(object sender, GhostscriptViewerViewEventArgs e)
        {
            pictureBox1.Invalidate();
            pictureBox1.Update();
        }

        private void _viewer_DisplayPage(object sender, GhostscriptViewerViewEventArgs e)
        {
            pictureBox1.Invalidate();
            pictureBox1.Update();
        }

        private void PdfForm_KeyDown(object sender, KeyEventArgs e)
        {
            // Verificar si se presionó la tecla Shift
            if (e.KeyCode == Keys.ShiftKey)
            {
                _shiftActivated = true;
            }
            if ( e.KeyCode == (Keys.ShiftKey))
             {
                e.Handled = true;
                e.SuppressKeyPress = true;
                MessageBox.Show("Detecte shift", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
               
            }
            if( e.KeyCode == Keys.PrintScreen)
            {
                _imprActivated = true;
            }
            if (e.KeyCode == Keys.PrintScreen)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                MessageBox.Show("Prohibido tomar capturas de pantalla por derechos de autor.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Verificar si se presionó la combinación Windows + Shift + S
            if (_shiftActivated && e.KeyCode == Keys.S && e.Modifiers == (Keys.Shift | Keys.Control))
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                MessageBox.Show("Prohibido tomar capturas de pantalla. Se detectó la combinación de teclas: Windows + Shift + S", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Bloquear combinación Alt + P
            if (e.KeyCode == Keys.P && e.Modifiers == Keys.Control)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                MessageBox.Show("Prohibido tomar capturas de pantalla. Se detectó la combinación de teclas: Alt + P", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void PdfForm_KeyUp(object sender, KeyEventArgs e)
        {
            // Verificar si se levantó la tecla Shift
            if (e.KeyCode == Keys.ShiftKey)
            {
                _shiftActivated = false;
              
            }
            if (e.KeyCode == Keys.PrintScreen)
            {
                _imprActivated = false;
            }
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            _viewer.ShowNextPage();
        }

        private void btnPreviousPage_Click(object sender, EventArgs e)
        {
            _viewer.ShowPreviousPage();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            _viewer.ShowFirstPage();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            _viewer.ShowLastPage();
        }
    }
}
