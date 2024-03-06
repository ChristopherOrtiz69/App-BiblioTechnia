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
        private Panel glossaryPanel;
        private Button btnGlossary;
        private int _currentPage = 1;
        private int _totalPages;


        public PdfForm(byte[] pdfContent)
        {
            InitializeComponent();
            InitializeViewer(pdfContent);
            InitializeNavigationButtons();
            InitializeZoomButtons();
            InitializeDarkModeButton();
            InitializeGlossaryPanel();

            // Obtener el número total de páginas del documento PDF
            int totalPages = GetTotalPages(pdfContent);
            InitializePageCounter(totalPages);

            // Suscribirse al evento MouseWheel
            this.MouseWheel += PdfForm_MouseWheel;
        }
        private int GetTotalPages(byte[] pdfContent)
        {
            using (MemoryStream ms = new MemoryStream(pdfContent))
            {
                using (var pdfDocument = new PdfDocument(new PdfReader(ms)))
                {
                    return pdfDocument.GetNumberOfPages();
                }
            }
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
            btnDarkMode.Location = new Point(1150, 10);
            btnDarkMode.Size = new Size(100, 50);

            // Quitar bordes y margen
            btnDarkMode.FlatStyle = FlatStyle.Flat;
            btnDarkMode.FlatAppearance.BorderSize = 0;
            btnDarkMode.Margin = new Padding(0);

            // Cargar la imagen desde un archivo
            Image image = Image.FromFile("Images/modo-oscuro.png");
          
            int newWidth = 49; // Ancho deseado
            int newHeight = 49; // Alto deseado
            Image resizedImage = new Bitmap(image, new Size(newWidth, newHeight));
            // Establecer la imagen redimensionada en el botón
            btnDarkMode.Image = resizedImage;
            btnDarkMode.Click += btnDarkMode_Click;

            this.Controls.Add(btnDarkMode);

        }

        private void btnDarkMode_Click(object sender, EventArgs e)
        {
            _darkMode = !_darkMode; // Alternar entre modo oscuro y modo claro

            if (_darkMode)
            {
                this.BackColor = Color.Gray;
                SetControlTextColors(this, Color.White);
                SetControlTextColors(glossaryPanel, Color.Black); 
            }
            else
            {
                this.BackColor = Color.White;
                SetControlTextColors(this, Color.Black);
                SetControlTextColors(glossaryPanel, Color.Black); 
            }
        }


        private void SetControlTextColors(Control control, Color color)
        {
            control.ForeColor = color; 

           
            foreach (Control childControl in control.Controls)
            {
                SetControlTextColors(childControl, color);
            }
        }



        private void InitializeNavigationButtons()
        {
            Button btnNextPage = new Button();
            btnNextPage.Location = new Point(1400, 100);
            btnNextPage.Size = new Size(500, 800); // Tamaño más grande       

            // Quitar bordes y margen
            btnNextPage.FlatStyle = FlatStyle.Flat;
            btnNextPage.FlatAppearance.BorderSize = 0;
            btnNextPage.Margin = new Padding(0);

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
            btnPreviousPage.FlatStyle= FlatStyle.Flat;
            btnPreviousPage.FlatAppearance.BorderSize=0;
            btnPreviousPage.Margin = new Padding(0);
            Image image1 = Image.FromFile("Images/FlechaRetroceso.png");
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
           /* Button btnFirstPage = new Button();
            btnFirstPage.Text = "Primera";
            btnFirstPage.Font = new Font("barlow", 15, FontStyle.Regular);
            btnFirstPage.Location = new Point(750, 10);
            btnFirstPage.Size = new Size(100, 35); // Tamaño más grande    
            btnFirstPage.Click += btnFirstPage_Click;
            this.Controls.Add(btnFirstPage);

            // Botón Última Página
            Button btnLastPage = new Button();
            btnLastPage.Text = "Última";
            btnLastPage.Font = new Font("barlow", 15, FontStyle.Regular);
            btnLastPage.Location = new Point(850, 10);
            btnLastPage.Size = new Size(100, 35); // Tamaño más grande         
            btnLastPage.Click += btnLastPage_Click;
            this.Controls.Add(btnLastPage);*/
        }

        private void InitializeZoomButtons()
        {
            Button btnZoomIn = new Button();
            btnZoomIn.Size = new Size(100, 50);
            btnZoomIn.Location = new Point(850, 10);

            // Cargar la imagen desde un archivo
            Image image = Image.FromFile("Images/zoom.png");

            // Define el ancho y alto deseados para la imagen dentro del botón
            int newWidth = 40; // Ancho deseado
            int newHeight = 40; // Alto deseado
            btnZoomIn.FlatStyle = FlatStyle.Flat;
            btnZoomIn.FlatAppearance.BorderSize = 0;
            btnZoomIn.Margin = new Padding(0);

           
            Image resizedImage = new Bitmap(image, new Size(newWidth, newHeight));
            btnZoomIn.Image = resizedImage;

            // Ajustar el tamaño de la imagen para que se ajuste al tamaño del botón
            btnZoomIn.ImageAlign = ContentAlignment.MiddleCenter;
            btnZoomIn.TextImageRelation = TextImageRelation.ImageBeforeText;

            btnZoomIn.Click += btnZoomIn_Click;
            this.Controls.Add(btnZoomIn);


            Button btnZoomOut = new Button();
            btnZoomOut.Size = new Size(100, 50);
            btnZoomOut.Location = new Point(950, 10);
            btnZoomOut.FlatStyle = FlatStyle.Flat;
            btnZoomOut.FlatAppearance.BorderSize = 0;
            btnZoomOut.Margin = new Padding(0);

            // Cargar la imagen desde un archivo
            Image image1 = Image.FromFile("Images/zoomenos.png"); // Reemplaza "ruta_de_la_imagen.png" con la ruta de tu imagen

            // Define el ancho y alto deseados para la imagen dentro del botón
            int newWidth1 = 40; // Ancho deseado
            int newHeight1 = 40; // Alto deseado

            // Redimensiona la imagen a las dimensiones deseadas
            Image resizedImage1 = new Bitmap(image1, new Size(newWidth1, newHeight1));
            btnZoomOut.Image = resizedImage1;

            // Ajustar el tamaño de la imagen para que se ajuste al tamaño del botón
            btnZoomOut.ImageAlign = ContentAlignment.MiddleCenter;
            btnZoomOut.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnZoomOut.Text = ""; // Establecer el texto del botón como una cadena vacía

            btnZoomOut.Click += btnZoomOut_Click;
            this.Controls.Add(btnZoomOut);


        }
        private void InitializePageCounter(int totalPages)
        {
            _totalPages = totalPages;
            lblPageCounter = new Label();
            UpdatePageCounter(_currentPage, _totalPages); // Inicialmente en la página 1
            lblPageCounter.Font = new Font("barlow", 20, FontStyle.Regular);
            lblPageCounter.AutoSize = true;
            lblPageCounter.Location = new Point(150, this.ClientSize.Height - lblPageCounter.Height - -10); // Ubicación del contador de páginas en la parte inferior
            this.Controls.Add(lblPageCounter);

            // Suscribirse al evento MouseWheel del formulario
            this.MouseWheel += PdfForm_MouseWheel;
        }

        private void PdfForm_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!_scrolling) // Verificar si no se está realizando ya un cambio de página por scroll
            {
                _scrolling = true; // Marcar que se está realizando un cambio de página por scroll

                if (e.Delta > 0) // Scroll hacia arriba
                {
                    if (_currentPage > 1)
                    {
                        _currentPage--;
                        _viewer.ShowPreviousPage();
                        UpdatePageCounter(_currentPage, _totalPages);
                    }
                }
                else // Scroll hacia abajo
                {
                    if (_currentPage < _totalPages)
                    {
                        _currentPage++;
                        _viewer.ShowNextPage();
                        UpdatePageCounter(_currentPage, _totalPages);
                    }
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
                _zoomFactor += 0.02f; // Aumentar el factor de zoom
                ApplyZoom(true); // Aplicar estiramiento en los lados
                _zoomInCount++;
                _zoomOutCount = 0; // Reiniciar contador de reducción de zoom
            }
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            if (_zoomOutCount < MaxZoomCount)
            {
                _zoomFactor -= 0.02f; // Reducir el factor de zoom
                ApplyZoom(true); // Aplicar estiramiento en los lados
                _zoomOutCount++;
                _zoomInCount = 0; 
            }
        }
        private void InitializeGlossaryPanel()
        {
            // Crear el panel
            glossaryPanel = new Panel();
            glossaryPanel.BackColor = Color.AliceBlue;
            glossaryPanel.Size = new Size(500, 400);
            glossaryPanel.Location = new Point(60, 10); 
            glossaryPanel.BorderStyle = BorderStyle.FixedSingle;
            glossaryPanel.Visible = false; // Inicialmente oculto

            // Agregar imagen al panel
            PictureBox pictureBoxGlossary = new PictureBox();
            pictureBoxGlossary.Image = Image.FromFile("Images/modo-oscuro.png"); 
            pictureBoxGlossary.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxGlossary.Size = new Size(50, 50);
            pictureBoxGlossary.Location = new Point(15, 35);
            glossaryPanel.Controls.Add(pictureBoxGlossary);
            
            
            PictureBox pictureBoxGlossary1 = new PictureBox();
            pictureBoxGlossary1.Image = Image.FromFile("Images/zoom.png"); 
            pictureBoxGlossary1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxGlossary1.Size = new Size(50, 50);
            pictureBoxGlossary1.Location = new Point(16, 95);
            glossaryPanel.Controls.Add(pictureBoxGlossary1);  
            
            PictureBox pictureBoxGlossary2 = new PictureBox();
            pictureBoxGlossary2.Image = Image.FromFile("Images/zoomenos.png"); 
            pictureBoxGlossary2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxGlossary2.Size = new Size(50, 50);
            pictureBoxGlossary2.Location = new Point(16, 150);
            glossaryPanel.Controls.Add(pictureBoxGlossary2); 
            
            PictureBox pictureBoxGlossary3 = new PictureBox();
            pictureBoxGlossary3.Image = Image.FromFile("Images/FlechaRetroceso.png"); 
            pictureBoxGlossary3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxGlossary3.Size = new Size(50, 50);
            pictureBoxGlossary3.Location = new Point(5, 210);
            glossaryPanel.Controls.Add(pictureBoxGlossary3);  
            
            PictureBox pictureBoxGlossary4 = new PictureBox();
            pictureBoxGlossary4.Image = Image.FromFile("Images/FlechaAvance.png"); 
            pictureBoxGlossary4.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxGlossary4.Size = new Size(50, 50);
            pictureBoxGlossary4.Location = new Point(47, 210);
            glossaryPanel.Controls.Add(pictureBoxGlossary4);



            // Agregar texto al panel
            Label labelGlossary = new Label();
            labelGlossary.Text = "Modo lectura Dia/Noche"; 
            labelGlossary.AutoSize = true;
            labelGlossary.Font = new Font("barlow", 15, FontStyle.Regular);
            labelGlossary.Location = new Point(100, 50);
            glossaryPanel.Controls.Add(labelGlossary); 
            
            Label labelGlossary1 = new Label();
            labelGlossary1.Text = "Aumentar zoom";
            labelGlossary1.AutoSize = true;
            labelGlossary1.Font = new Font("barlow", 15, FontStyle.Regular);
            labelGlossary1.Location = new Point(100, 100);
            glossaryPanel.Controls.Add(labelGlossary1);
            
            Label labelGlossary2 = new Label();
            labelGlossary2.Text = "Disminuir zoom";
            labelGlossary2.AutoSize = true;
            labelGlossary2.Font = new Font("barlow", 15, FontStyle.Regular);
            labelGlossary2.Location = new Point(100, 160);
            glossaryPanel.Controls.Add(labelGlossary2);
            
            Label labelGlossary3 = new Label();
            labelGlossary3.Text = "Avanzar/Retroceder de página";
            labelGlossary3.AutoSize = true;
            labelGlossary3.Font = new Font("barlow", 15, FontStyle.Regular);
            labelGlossary3.Location = new Point(100, 220);
            glossaryPanel.Controls.Add(labelGlossary3);

            // Agregar el panel al formulario
            this.Controls.Add(glossaryPanel);

            // Crear el botón del glosario
            // Carga la imagen desde el archivo
            Image image = Image.FromFile("Images/signo-de-interrogacion.png");

            // Define el tamaño deseado para la imagen
            int desiredWidth = 50;
            int desiredHeight = 50;

            // Escala la imagen al tamaño deseado
            Image scaledImage = new Bitmap(image, new Size(desiredWidth, desiredHeight));

            // Crea el botón y asigna la imagen escalada
            btnGlossary = new Button();
            btnGlossary.Image = scaledImage;
            btnGlossary.ImageAlign = ContentAlignment.MiddleCenter; // Centra la imagen dentro del botón
            btnGlossary.BackgroundImageLayout = ImageLayout.Stretch; // Estira la imagen para que se ajuste al tamaño del botón
            btnGlossary.FlatStyle = FlatStyle.Flat; // Establece el estilo de borde plano
            btnGlossary.FlatAppearance.BorderSize = 0; // Establece el grosor del borde a cero
            btnGlossary.Location = new Point(650, 10);
            btnGlossary.Size = new Size(50, 50);
            btnGlossary.Click += BtnGlossary_Click;
            this.Controls.Add(btnGlossary);


        }

        private void BtnGlossary_Click(object sender, EventArgs e)
        {
            // Alternar la visibilidad del panel del glosario
            glossaryPanel.Visible = !glossaryPanel.Visible;
            glossaryPanel.BringToFront();
        }


        private void ApplyZoom(bool stretchSides = false)
        {
            // Obtener el tamaño de la imagen ajustado con el factor de zoom
            int newWidth = (int)(pictureBox1.Image.Width * _zoomFactor);
            int newHeight = (int)(pictureBox1.Image.Height * _zoomFactor);

            if (stretchSides)
            {
                newWidth = (int)(newWidth * 1.2f); // Factor de escala para el ancho
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
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                _viewer.ShowNextPage();
                UpdatePageCounter(_currentPage, _totalPages);
            }
        }

        private void btnPreviousPage_Click(object sender, EventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                _viewer.ShowPreviousPage();
                UpdatePageCounter(_currentPage, _totalPages);
            }
        }


        private void UpdatePageCounter(int currentPage, int totalPages)
        {
            lblPageCounter.Text = $"Página {currentPage} de {totalPages}";
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
