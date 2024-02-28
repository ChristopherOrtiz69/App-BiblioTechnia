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


        public PdfForm(byte[] pdfContent)
        {
            InitializeComponent();
            InitializeViewer(pdfContent);
            InitializeNavigationButtons();
            InitializeZoomButtons();
            InitializeDarkModeButton();
            InitializeGlossaryPanel();

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
            btnDarkMode.Location = new Point(1220, 10);
            btnDarkMode.Size = new Size(100, 50);

            // Quitar bordes y margen
            btnDarkMode.FlatStyle = FlatStyle.Flat;
            btnDarkMode.FlatAppearance.BorderSize = 0;
            btnDarkMode.Margin = new Padding(0);

            // Cargar la imagen desde un archivo
            Image image = Image.FromFile("Images/mode.png");
            // Redimensionar la imagen a un tamaño más pequeño
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
                SetControlTextColors(this, Color.White); // Establecer el color del texto en blanco para todos los controles
                SetControlTextColors(glossaryPanel, Color.Black); // Establecer el color del texto en negro solo para el panel de glosario
            }
            else
            {
                this.BackColor = Color.White;
                SetControlTextColors(this, Color.Black); // Establecer el color del texto en negro para todos los controles
                SetControlTextColors(glossaryPanel, Color.Black); // Establecer el color del texto en negro solo para el panel de glosario
            }
        }


        // Método recursivo para establecer el color del texto en todos los controles y sus hijos
        private void SetControlTextColors(Control control, Color color)
        {
            control.ForeColor = color; // Establecer el color del texto para el control actual

            // Recorrer todos los controles secundarios de forma recursiva
            foreach (Control childControl in control.Controls)
            {
                SetControlTextColors(childControl, color); // Llamar recursivamente al método para los controles secundarios
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
            Button btnFirstPage = new Button();
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
            this.Controls.Add(btnLastPage);
        }

        private void InitializeZoomButtons()
        {
            Button btnZoomIn = new Button();
            btnZoomIn.Size = new Size(100, 50);
            btnZoomIn.Location = new Point(1000, 10);

            // Cargar la imagen desde un archivo
            Image image = Image.FromFile("Images/plus.png"); // Reemplaza "ruta_de_la_imagen.png" con la ruta de tu imagen

            // Define el ancho y alto deseados para la imagen dentro del botón
            int newWidth = 40; // Ancho deseado
            int newHeight = 40; // Alto deseado
            btnZoomIn.FlatStyle = FlatStyle.Flat;
            btnZoomIn.FlatAppearance.BorderSize = 0;
            btnZoomIn.Margin = new Padding(0);

            // Redimensiona la imagen a las dimensiones deseadas
            Image resizedImage = new Bitmap(image, new Size(newWidth, newHeight));
            btnZoomIn.Image = resizedImage;

            // Ajustar el tamaño de la imagen para que se ajuste al tamaño del botón
            btnZoomIn.ImageAlign = ContentAlignment.MiddleCenter;
            btnZoomIn.TextImageRelation = TextImageRelation.ImageBeforeText;

            btnZoomIn.Click += btnZoomIn_Click;
            this.Controls.Add(btnZoomIn);


            Button btnZoomOut = new Button();
            btnZoomOut.Size = new Size(100, 50);
            btnZoomOut.Location = new Point(1100, 10);
            btnZoomOut.FlatStyle = FlatStyle.Flat;
            btnZoomOut.FlatAppearance.BorderSize = 0;
            btnZoomOut.Margin = new Padding(0);

            // Cargar la imagen desde un archivo
            Image image1 = Image.FromFile("Images/delete.png"); // Reemplaza "ruta_de_la_imagen.png" con la ruta de tu imagen

            // Define el ancho y alto deseados para la imagen dentro del botón
            int newWidth1 = 40; // Ancho deseado
            int newHeight1 = 40; // Alto deseado

            // Redimensiona la imagen a las dimensiones deseadas
            Image resizedImage1 = new Bitmap(image1, new Size(newWidth1, newHeight1)); // Aquí se debe usar image1 en lugar de image
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
            lblPageCounter = new Label();
            lblPageCounter.Text = $"Página 1 de {totalPages}"; // Inicialmente en la página 1
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
                _zoomInCount = 0; // Reiniciar contador de aumento de zoom
            }
        }
        private void InitializeGlossaryPanel()
        {
            // Crear el panel
            glossaryPanel = new Panel();
            glossaryPanel.BackColor = Color.LightGoldenrodYellow;
            glossaryPanel.Size = new Size(500, 500);
            glossaryPanel.Location = new Point(650, 200); // Ajusta la ubicación según sea necesario
            glossaryPanel.BorderStyle = BorderStyle.FixedSingle;
            glossaryPanel.Visible = false; // Inicialmente oculto

            // Agregar imagen al panel
            PictureBox pictureBoxGlossary = new PictureBox();
            pictureBoxGlossary.Image = Image.FromFile("Images/mode.png"); // Reemplaza "Images/glossary_image.png" con la ruta de tu imagen
            pictureBoxGlossary.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxGlossary.Size = new Size(50, 50);
            pictureBoxGlossary.Location = new Point(15, 35);
            glossaryPanel.Controls.Add(pictureBoxGlossary);
            
            
            PictureBox pictureBoxGlossary1 = new PictureBox();
            pictureBoxGlossary1.Image = Image.FromFile("Images/plus.png"); // Reemplaza "Images/glossary_image.png" con la ruta de tu imagen
            pictureBoxGlossary1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxGlossary1.Size = new Size(50, 50);
            pictureBoxGlossary1.Location = new Point(16, 95);
            glossaryPanel.Controls.Add(pictureBoxGlossary1);  
            
            PictureBox pictureBoxGlossary2 = new PictureBox();
            pictureBoxGlossary2.Image = Image.FromFile("Images/delete.png"); // Reemplaza "Images/glossary_image.png" con la ruta de tu imagen
            pictureBoxGlossary2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxGlossary2.Size = new Size(50, 50);
            pictureBoxGlossary2.Location = new Point(16, 150);
            glossaryPanel.Controls.Add(pictureBoxGlossary2); 
            
            PictureBox pictureBoxGlossary3 = new PictureBox();
            pictureBoxGlossary3.Image = Image.FromFile("Images/FlechaRetroceso.png"); // Reemplaza "Images/glossary_image.png" con la ruta de tu imagen
            pictureBoxGlossary3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxGlossary3.Size = new Size(50, 50);
            pictureBoxGlossary3.Location = new Point(5, 210);
            glossaryPanel.Controls.Add(pictureBoxGlossary3);  
            
            PictureBox pictureBoxGlossary4 = new PictureBox();
            pictureBoxGlossary4.Image = Image.FromFile("Images/FlechaAvance.png"); // Reemplaza "Images/glossary_image.png" con la ruta de tu imagen
            pictureBoxGlossary4.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxGlossary4.Size = new Size(50, 50);
            pictureBoxGlossary4.Location = new Point(47, 210);
            glossaryPanel.Controls.Add(pictureBoxGlossary4);



            // Agregar texto al panel
            Label labelGlossary = new Label();
            labelGlossary.Text = "Modo lectura Dia/Noche"; // Reemplaza con tu texto
            labelGlossary.AutoSize = true;
            labelGlossary.Font = new Font("barlow", 15, FontStyle.Regular);
            labelGlossary.Location = new Point(100, 50);
            glossaryPanel.Controls.Add(labelGlossary); 
            
            Label labelGlossary1 = new Label();
            labelGlossary1.Text = "Aumentar zoom"; // Reemplaza con tu texto
            labelGlossary1.AutoSize = true;
            labelGlossary1.Font = new Font("barlow", 15, FontStyle.Regular);
            labelGlossary1.Location = new Point(100, 100);
            glossaryPanel.Controls.Add(labelGlossary1);
            
            Label labelGlossary2 = new Label();
            labelGlossary2.Text = "Disminuir zoom"; // Reemplaza con tu texto
            labelGlossary2.AutoSize = true;
            labelGlossary2.Font = new Font("barlow", 15, FontStyle.Regular);
            labelGlossary2.Location = new Point(100, 160);
            glossaryPanel.Controls.Add(labelGlossary2);
            
            Label labelGlossary3 = new Label();
            labelGlossary3.Text = "Avanzar/Retroceder de página"; // Reemplaza con tu texto
            labelGlossary3.AutoSize = true;
            labelGlossary3.Font = new Font("barlow", 15, FontStyle.Regular);
            labelGlossary3.Location = new Point(100, 220);
            glossaryPanel.Controls.Add(labelGlossary3);

            // Agregar el panel al formulario
            this.Controls.Add(glossaryPanel);

            // Crear el botón del glosario
            btnGlossary = new Button();
            btnGlossary.Text = "Glosario";
            btnGlossary.Location = new Point(10, 10); // Ajusta la ubicación según sea necesario
            btnGlossary.Size = new Size(80, 30);
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
