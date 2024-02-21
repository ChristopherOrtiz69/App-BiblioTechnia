    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using VersOne.Epub;


    namespace Prueba_Libro
    {
        public partial class Form1 : Form
        {
            private DocumentManager documentManager;
            private TextBox txtBusqueda;
            private List<string> nombresDocumentos;
            private List<Image> imagenesDocumentos;
            private Panel panelContenedor;
            private const int pictureBoxWidth = 500;
            private const int pictureBoxHeight = 800;
            //private System.Windows.Forms.WebBrowser webBrowser;
            public Form1()
            {
                InitializeComponent();  
                InitializeComponentsAndData();
                this.AutoScroll = true;
                //pruebaEPUB pruebaEPUBForm = new pruebaEPUB();
                //pruebaEPUBForm.Show();
                //InicializarPDFEncriptado();
               // webBrowser = new System.Windows.Forms.WebBrowser();
               // webBrowser.Dock = DockStyle.Fill; // Ajustar el control al tamaño del formulario
                //Controls.Add(webBrowser); // Agregar el control al formulario
                /*webBrowser.DocumentCompleted += (sender, e) =>
                {
                    if (webBrowser.Document != null && webBrowser.Document.Body != null)
                    {
                        // Acceder a los metadatos del libro ePub
                        EpubBook book = EpubReader.ReadBook("DocumentosPDF/pruebaEpub2.epub");

                        // Mostrar un mensaje de confirmación de detección de ePub
                        MessageBox.Show("Archivo ePub detectado", "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Continuar solo si el libro es un archivo ePub
                        if (book != null)
                        {
                            string title = book.Title ?? "Título no encontrado";
                            string author = book.Author ?? "Autor no encontrado";
                            string description = book.Description ?? "Descripción no encontrada";

                            // Crear el mensaje con los metadatos
                            string message = $"Título: {title}\n" +
                                             $"Autor: {author}\n" +
                                             $"Descripción: {description}\n";

                            // Mostrar el mensaje con los metadatos en una ventana emergente
                            MessageBox.Show(message, "Metadatos del ePub", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            // Si no se pudo cargar el libro, mostrar un mensaje de error
                            MessageBox.Show("No se pudo cargar el archivo ePub", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }*/

                }


               /* AbrirYMostrarLibroEpub();
            }
            private void AbrirYMostrarLibroEpub()
            {
                // Ruta al archivo ePub
                string filePath = "DocumentosPDF/pruebaEpub.epub";

                try
                {
                    // Leer el archivo ePub
                    EpubBook book = EpubReader.ReadBook(filePath);

                    // Obtener el contenido HTML del primer capítulo
                    string htmlContent = book.ReadingOrder.FirstOrDefault()?.Content ?? "No hay contenido disponible";

                    // Mostrar el contenido en el WebBrowser agregado dinámicamente
                   // webBrowser.DocumentText = htmlContent;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al abrir el archivo ePub: " + ex.Message,
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }*/

            private void InitializeComponentsAndData()
            {
                documentManager = new DocumentManager();
                var documentos = documentManager.ObtenerDocumentos(); // Obtener la lista de documentos
                nombresDocumentos = documentos.Select(doc => doc.Nombre).ToList(); // Obtener nombres de documentos
                imagenesDocumentos = documentos.Select(doc => doc.Imagen).ToList(); // Obtener imágenes de documentos
                InitializeUI(documentos); // Inicializar la interfaz de usuario
                InitializeSearchBar(); // Inicializar la barra de búsqueda
            }

            private void InitializeUI(List<DocumentManager.Documento> documentos)
            {
                // Aquí se obtienen los documentos ordenados
                var documentosOrdenados = documentManager.ObtenerDocumentos();

                // Crear encabezado del formulario que es la imagen de BiblioTechnia
                PictureBox pictureBoxHeader = new PictureBox();
                pictureBoxHeader.Image = Properties.Resources.Logo_BiblioTechnia;
                pictureBoxHeader.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBoxHeader.Size = new Size(350, 300);
                pictureBoxHeader.Location = new Point(20, -50);
                Controls.Add(pictureBoxHeader);

                Label labelTitle = CreateLabel("    Catálogo", new Font("monofonto", 30), new Size(400, 70), new Point((int)((ClientSize.Width - 400) / 2.2), 220));
                labelTitle.ForeColor = Color.DodgerBlue; // Cambiar el color del texto a azul
                Controls.Add(labelTitle);

                // Agregar contador de documentos
                Label labelCounter = new Label();
                labelCounter.Text = $"Libros: {documentos.Count}";
                labelCounter.Font = new Font("Barlow", 12, FontStyle.Regular);
                labelCounter.Location = new Point((int)((ClientSize.Width - 400) / 1.6), 290);
                labelCounter.ForeColor = Color.DodgerBlue;
                Controls.Add(labelCounter);

                // Crear el botón de borrar
                Button btnBorrar = new Button();
                btnBorrar.Text = "Regresar";
                btnBorrar.Location = new Point(ClientSize.Width - 125, 100);
                btnBorrar.Size = new Size(85, 30);
                btnBorrar.Click += BtnBorrar_Click;
                btnBorrar.ForeColor = Color.DodgerBlue;
                Controls.Add(btnBorrar);

                // Aquí están almacenadas las variables para la disposición de las imágenes
                int xPos = 20;
                int yPos = 300;
                int spacingX = 20;
                int spacingY = 20;
                int maxColumns = 4;
                int columnCount = 0;

                for (int i = 0; i < documentos.Count; i++)
                {
                    PictureBox pictureBox = CreatePictureBox(imagenesDocumentos[i], new Point(xPos, yPos), pictureBox_Click);
                    pictureBox.Size = new Size(pictureBoxWidth / 2, pictureBoxHeight / 2);

                    pictureBox.Tag = documentos[i].Id; // Se asigna el ID del documento al Tag del PictureBox
                    Controls.Add(pictureBox);

                    Label labelSubtitle = CreateLabel(nombresDocumentos[i], new Font("chris", 12, FontStyle.Italic), new Size(pictureBoxWidth / 2, 80), new Point(xPos, yPos - -380));

                    Controls.Add(labelSubtitle);

                    // Se actualiza posición X e Y para el próximo PictureBox y Label
                    xPos += pictureBoxWidth / 2 + spacingX;
                    columnCount++; 

                    if (columnCount >= maxColumns)
                    {
                        // Se reinicia X y ajustar Y para comenzar una nueva fila
                        xPos = 20;
                        yPos += pictureBoxHeight / 2 + spacingY;
                        columnCount = 0;
                    }
                }
            }
            private void InitializeSearchBar()
            {
                // Crear la barra de búsqueda en la parte superior derecha del formulario
                txtBusqueda = new TextBox();
                txtBusqueda.Location = new Point(ClientSize.Width - 565, 100);
                txtBusqueda.Size = new Size(350, 30); // Aumentar la altura de la TextBox
                txtBusqueda.Multiline = true;
                txtBusqueda.Font = new Font("Barlow", 12, FontStyle.Regular);
                Controls.Add(txtBusqueda);

                // Crear el botón de búsqueda
                Button btnBuscar = new Button();
                btnBuscar.Location = new Point(txtBusqueda.Location.X + txtBusqueda.Width + 3, 100);
                btnBuscar.Size = new Size(85, 30);
                btnBuscar.Click += BtnBuscar_Click;

                // Agregar imagen de lupa al botón
                ImageList imageList = new ImageList();
                imageList.Images.Add(Properties.Resources.Lupa); // Suponiendo que "lupa" es el nombre de la imagen en tus recursos
                btnBuscar.ImageList = imageList;
                btnBuscar.ImageIndex = 0; // El índice de la imagen dentro de ImageList

                Controls.Add(btnBuscar);
            }


            private void BtnBuscar_Click(object sender, EventArgs e)
            {
                // Obtener el texto de búsqueda
                string textoBusqueda = txtBusqueda.Text.ToLower();

                // Obtener todos los documentos
                var documentos = documentManager.ObtenerDocumentos();

                // Filtrar los documentos que contienen el texto de búsqueda en el nombre
                var documentosFiltrados = documentos.Where(doc => doc.Nombre.ToLower().Contains(textoBusqueda)).ToList();

                // Limpiar los controles excepto la barra de búsqueda y el botón de búsqueda
                foreach (Control control in Controls.Cast<Control>().ToList())
                {
                    if (control != txtBusqueda && control != sender as Button && control != Controls.OfType<Button>().FirstOrDefault(btn => btn.Text == "Borrar"))
                    {
                        Controls.Remove(control);
                        control.Dispose();
                    }
                }

                if (documentosFiltrados.Count == 0)
                {
                    MessageBox.Show("No se encontraron resultados de búsqueda, por favor escribir el nombre del título correctamente.", "Búsqueda", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Suscribir al evento FormClosed del formulario actual
                    InitializeUI(documentos);
                }
                else
                {
                    // Volver a inicializar la UI con los documentos filtrados
                    nombresDocumentos = documentosFiltrados.Select(doc => doc.Nombre).ToList();
                    imagenesDocumentos = documentosFiltrados.Select(doc => doc.Imagen).ToList();
                    InitializeUI(documentosFiltrados);
                }
            }


            private void BtnBorrar_Click(object sender, EventArgs e)
            {
                // Limpiar los controles excepto la barra de búsqueda y el botón de búsqueda
                foreach (Control control in Controls.Cast<Control>().ToList())
                {
                    if (control != txtBusqueda && control != Controls.OfType<Button>().FirstOrDefault(btn => btn.Text == "Buscar"))
                    {
                        Controls.Remove(control);
                        control.Dispose();
                    }
                }

                // Volver a agregar la barra de búsqueda y el botón de búsqueda
                InitializeSearchBar();

                // Volver a inicializar la UI con todos los documentos
                var documentos = documentManager.ObtenerDocumentos();
                nombresDocumentos = documentos.Select(doc => doc.Nombre).ToList();
                imagenesDocumentos = documentos.Select(doc => doc.Imagen).ToList();
                InitializeUI(documentos);
            }

            private PictureBox CreatePictureBox(Image image, Point location, EventHandler clickEvent)
            {
                PictureBox pictureBox = new PictureBox();
                pictureBox.Image = image;
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox.Location = location;
                pictureBox.Click += clickEvent;
                return pictureBox;
            }
            private Label CreateLabel(string text, Font font, Size size, Point location)
            {
                Label label = new Label();
                label.Text = text;
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.Font = font;
                label.Size = size;
                label.Location = location;

                return label;
            }
            private void InicializarPDFEncriptado()
            {
                // Obtener el primer documento en la lista de documentos
                var documentos = documentManager.ObtenerDocumentos();
                if (documentos.Count > 0)
                {
                    // Obtener el primer documento de la lista
                    var primerDocumento = documentos[0]; 
                    // Combinar la ruta del ejecutable con la ruta del documento PDF cifrado
                    string pdfFileName = $"Encrypted_{primerDocumento.Nombre}.pdf";
                    string pdfFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pdfFileName);

                    // Abrir el PDF encriptado en un nuevo formulario
                    using (var pdfForm = new PdfForm(pdfFilePath))
                    {
                        pdfForm.ShowDialog(); // Mostrar el formulario PDF de manera modal
                    }
                }
                else
                {
                    MessageBox.Show("No hay documentos disponibles.");
                }
            }

            private void pictureBox_Click(object sender, EventArgs e)
            {
                // Manejar el evento de clic en el PictureBox
                PictureBox pictureBox = sender as PictureBox;
                int selectedDocumentId = (int)pictureBox.Tag; // Obtener el ID del documento del Tag del PictureBox

                // Obtener el documento correspondiente al ID
                DocumentManager.Documento documento = documentManager.ObtenerDocumentos().FirstOrDefault(doc => doc.Id == selectedDocumentId);

                // Verificar si se encontró el documento
                if (documento != null)
                {
                    // Mostrar el formulario del PDF correspondiente
                    ShowPdfForm(selectedDocumentId);
                }
                else
                {
                    MessageBox.Show("Documento no encontrado.");
                }
            }

            private void pictureBox_MouseLeave(object sender, EventArgs e)
            {
                // Se maneja el evento mouse leave en el PictureBox
                PictureBox pictureBox = sender as PictureBox;
                pictureBox.Image = Properties.Resources.LibroCerrado;
            }

        private void ShowPdfForm(int documentId)
        {
            // Obtener el documento correspondiente al ID
            DocumentManager.Documento documento = documentManager.ObtenerDocumentos().FirstOrDefault(doc => doc.Id == documentId);

            if (documento != null)
            {
                // Crear una instancia del formulario de visor de PDF y mostrarlo
                PdfForm pdfForm = new PdfForm(documento.Ruta);
                pdfForm.Show();
            }
            else
            {
                MessageBox.Show("Documento no encontrado.");
            }
        }

    }
}
