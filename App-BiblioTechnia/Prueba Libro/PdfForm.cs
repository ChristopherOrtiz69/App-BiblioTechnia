using System;
using System.Drawing;
using System.Windows.Forms;
using Ghostscript.NET;
using Ghostscript.NET.Viewer;
using iText.Kernel.Pdf;
using System.IO;

namespace Prueba_Libro
{
    public partial class PdfForm : Form
    {
        private GhostscriptViewer _viewer;
        private GhostscriptVersionInfo _gsVersion = GhostscriptVersionInfo.GetLastInstalledVersion();
        private bool _shiftActivated = false;
        private bool _imprActivated = false;
        private float _zoomFactor = 1.0f; // Factor de zoom inicial
        private int _zoomInCount = 0;
        private int _zoomOutCount = 0;
        private const int MaxZoomCount = 10;
        private Label lblPageCounter;

        public PdfForm(byte[] pdfContent)
        {
            InitializeComponent();
            InitializeViewer(pdfContent);
            InitializeNavigationButtons();
            InitializeZoomButtons();

            // Obtener el número total de páginas del documento PDF
            using (MemoryStream ms = new MemoryStream(pdfContent))
            {
                using (var pdfDocument = new PdfDocument(new PdfReader(ms)))
                {
                    int totalPages = pdfDocument.GetNumberOfPages();
                    InitializePageCounter(totalPages);
                }
            }

            // Suscribirse al evento MouseWheel
            this.MouseWheel += PdfForm_MouseWheel;
        }


        private void InitializeViewer(byte[] pdfContent)
        {
            _viewer = new GhostscriptViewer();
            _viewer.DisplaySize += _viewer_DisplaySize;
            _viewer.DisplayUpdate += _viewer_DisplayUpdate;
            _viewer.DisplayPage += _viewer_DisplayPage;

            this.KeyPreview = true;
            this.KeyDown += PdfForm_KeyDown;
            this.KeyUp += PdfForm_KeyUp;

            using (MemoryStream ms = new MemoryStream(pdfContent))
            {
                _viewer.Open(ms, _gsVersion, false);
            }
        }

        private void InitializeNavigationButtons()
        {
            // Botón Siguiente
            Button btnNextPage = new Button();
            btnNextPage.Text = "Siguiente";
            btnNextPage.Location = new Point(1300, 250);
            btnNextPage.Size = new Size(400, 500); // Tamaño más grande       
            btnNextPage.Click += btnNextPage_Click;
            this.Controls.Add(btnNextPage);

            // Botón Anterior
            Button btnPreviousPage = new Button();
            btnPreviousPage.Text = "Anterior";
            btnPreviousPage.Location = new Point(10, 250);
            btnPreviousPage.Size = new Size(400, 500); // Tamaño más grande         
            btnPreviousPage.Click += btnPreviousPage_Click;
            this.Controls.Add(btnPreviousPage);

            // Botón Primera Página
            Button btnFirstPage = new Button();
            btnFirstPage.Text = "Primera";
            btnFirstPage.Location = new Point(50, 10);
            btnFirstPage.Size = new Size(150, 50); // Tamaño más grande    
            btnFirstPage.Click += btnFirstPage_Click;
            this.Controls.Add(btnFirstPage);

            // Botón Última Página
            Button btnLastPage = new Button();
            btnLastPage.Text = "Última";
            btnLastPage.Location = new Point(250, 10);
            btnLastPage.Size = new Size(150, 50); // Tamaño más grande         
            btnLastPage.Click += btnLastPage_Click;
            this.Controls.Add(btnLastPage);
        }

        private void InitializeZoomButtons()
        {
            Button btnZoomIn = new Button();
            btnZoomIn.Text = "+";
            btnZoomIn.Size = new Size(100, 50);
            btnZoomIn.Location = new Point(1300, 10);
            btnZoomIn.Font = new Font("barlow", 20, FontStyle.Regular);
            btnZoomIn.Click += btnZoomIn_Click;
            this.Controls.Add(btnZoomIn);

            Button btnZoomOut = new Button();
            btnZoomOut.Text = "-";
            btnZoomOut.Size = new Size(100, 50);
            btnZoomOut.Location = new Point(1400, 10);
            btnZoomOut.Font = new Font("barlow", 20, FontStyle.Regular);
            btnZoomOut.Click += btnZoomOut_Click;
            this.Controls.Add(btnZoomOut);
        }
        private void InitializePageCounter(int totalPages)
        {
            lblPageCounter = new Label();
            lblPageCounter.Text = $"Página 1 de {totalPages}"; // Inicialmente en la página 1
            lblPageCounter.AutoSize = true;
            lblPageCounter.Location = new Point(900, this.ClientSize.Height - lblPageCounter.Height - 40); // Ubicación del contador de páginas en la parte inferior
            this.Controls.Add(lblPageCounter);

            // Suscribirse al evento MouseWheel del formulario
            this.MouseWheel += PdfForm_MouseWheel;
        }

        private void PdfForm_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                _viewer.ShowPreviousPage(); // Scroll hacia arriba
            }
            else
            {
                _viewer.ShowNextPage(); // Scroll hacia abajo
            }
            // Manejar el evento MouseWheel para evitar el desplazamiento de la ventana
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private void _viewer_DisplaySize(object sender, GhostscriptViewerViewEventArgs e)
        {
            pictureBox1.Image = e.Image;
            ApplyZoom();
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
            if (e.KeyCode == (Keys.ShiftKey))
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                MessageBox.Show("Prohibido tomar capturas de pantalla por derechos de autor.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            if (e.KeyCode == Keys.PrintScreen)
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

      

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            if (_zoomInCount < MaxZoomCount)
            {
                _zoomFactor += 0.01f; // Aumentar el factor de zoom
                ApplyZoom(true); // Aplicar estiramiento en los lados
                _zoomInCount++;
                _zoomOutCount = 0; // Reiniciar contador de reducción de zoom
            }
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            if (_zoomOutCount < MaxZoomCount)
            {
                _zoomFactor -= 0.01f; // Reducir el factor de zoom
                ApplyZoom(true); // Aplicar estiramiento en los lados
                _zoomOutCount++;
                _zoomInCount = 0; // Reiniciar contador de aumento de zoom
            }
        }

        private void ApplyZoom(bool stretchSides = false)
        {
            // Obtener el tamaño de la imagen ajustado con el factor de zoom
            int newWidth = (int)(pictureBox1.Image.Width * _zoomFactor);
            int newHeight = (int)(pictureBox1.Image.Height * _zoomFactor);

            if (stretchSides)
            {
                newWidth = (int)(newWidth * 1.5f); // Factor de escala para el ancho
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }

            // Ajustar el tamaño del PictureBox para mostrar la imagen con el zoom aplicado
            pictureBox1.Size = new Size(newWidth, newHeight);
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
