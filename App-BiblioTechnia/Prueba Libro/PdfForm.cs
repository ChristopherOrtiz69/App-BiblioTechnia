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
        private bool _darkMode = false;
        private bool _scrolling = false;

        public PdfForm(byte[] pdfContent)
        {
            InitializeComponent();
            InitializeViewer(pdfContent);
            InitializeNavigationButtons();
            InitializeZoomButtons();
            InitializeDarkModeButton();

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
        private void InitializeDarkModeButton()
        {
            Button btnDarkMode = new Button();
            btnDarkMode.Text = "Modo Oscuro";
            btnDarkMode.Location = new Point(1550, 10);
            btnDarkMode.Size = new Size(200, 50);
            btnDarkMode.Click += btnDarkMode_Click;
            this.Controls.Add(btnDarkMode);
        }

        private void btnDarkMode_Click(object sender, EventArgs e)
        {
            _darkMode = !_darkMode; // Alternar entre modo oscuro y modo claro

            if (_darkMode)
            {
                this.BackColor = Color.DarkGray;
            }
            else
            {
                this.BackColor = Color.White;
            }
        }


        private void InitializeNavigationButtons()
        {
            // Botón Siguiente
            Button btnNextPage = new Button();
            btnNextPage.Location = new Point(1400, 100);
            btnNextPage.Size = new Size(500, 800); // Tamaño más grande       
            // Cargar la imagen desde un archivo
            Image image = Image.FromFile("Images/FlechaAvance.png");
            // Redimensionar la imagen a un tamaño más pequeño
            int newWidth = 50; // Ancho deseado
            int newHeight = 50; // Alto deseado
            Image resizedImage = new Bitmap(image, new Size(newWidth, newHeight));
            // Establecer la imagen redimensionada en el botón
            btnNextPage.Image = resizedImage;
            // Centrar la imagen dentro del botón
            btnNextPage.ImageAlign = ContentAlignment.MiddleCenter;
            btnNextPage.Click += btnNextPage_Click;
            this.Controls.Add(btnNextPage);

            // Botón Anterior
            Button btnPreviousPage = new Button();
            btnPreviousPage.Location = new Point(10, 100);
            btnPreviousPage.Size = new Size(500, 800); // Tamaño más grande         

            // Cargar la segunda imagen desde un archivo
            Image image1 = Image.FromFile("Images/FlechaRetroceso.png");

            // Redimensionar la segunda imagen a un tamaño más pequeño
            int newWidth1 = 50; // Ancho deseado
            int newHeight1 = 50; // Alto deseado
            Image resizedImage1 = new Bitmap(image1, new Size(newWidth1, newHeight1));

            // Establecer la imagen redimensionada en el botón
            btnPreviousPage.Image = resizedImage1;

            // Centrar la imagen dentro del botón
            btnPreviousPage.ImageAlign = ContentAlignment.MiddleCenter;
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
            btnZoomIn.Location = new Point(1550, 950);
            btnZoomIn.Font = new Font("barlow", 20, FontStyle.Regular);
            btnZoomIn.Click += btnZoomIn_Click;
            this.Controls.Add(btnZoomIn);

            Button btnZoomOut = new Button();
            btnZoomOut.Text = "-";
            btnZoomOut.Size = new Size(100, 50);
            btnZoomOut.Location = new Point(1650, 950);
            btnZoomOut.Font = new Font("barlow", 20, FontStyle.Regular);
            btnZoomOut.Click += btnZoomOut_Click;
            this.Controls.Add(btnZoomOut);
        }
        private void InitializePageCounter(int totalPages)
        {
            lblPageCounter = new Label();
            lblPageCounter.Text = $"Página 1 de {totalPages}"; // Inicialmente en la página 1
            lblPageCounter.AutoSize = true;
            lblPageCounter.Location = new Point(900, this.ClientSize.Height - lblPageCounter.Height - -10); // Ubicación del contador de páginas en la parte inferior
            this.Controls.Add(lblPageCounter);

            // Suscribirse al evento MouseWheel del formulario
            this.MouseWheel += PdfForm_MouseWheel;
        }

        private void PdfForm_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!_scrolling) // Verificar si no se está realizando ya un cambio de página por scroll
            {
                _scrolling = true; // Marcar que se está realizando un cambio de página por scroll

                if (e.Delta > 0)
                {
                    _viewer.ShowPreviousPage(); // Mostrar página anterior al hacer scroll hacia arriba
                }
                else
                {
                    _viewer.ShowNextPage(); // Mostrar página siguiente al hacer scroll hacia abajo
                }

                // Manejar el evento MouseWheel para evitar el desplazamiento de la ventana
                ((HandledMouseEventArgs)e).Handled = true;

                // Restablecer la variable _scrolling después de un breve tiempo para permitir el siguiente cambio de página por scroll
                Timer timer = new Timer();
                timer.Interval = 500; // Tiempo de espera en milisegundos
                timer.Tick += (senderObj, args) =>
                {
                    _scrolling = false;
                    timer.Stop();
                    timer.Dispose();
                };
                timer.Start();
            }
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
