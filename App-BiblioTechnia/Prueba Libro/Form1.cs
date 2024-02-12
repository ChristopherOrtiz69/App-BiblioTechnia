using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Prueba_Libro
{
    public partial class Form1 : Form
    {
        private DocumentManager documentManager;
        private TextBox txtBusqueda;
        private List<string> nombresDocumentos;
        private List<Image> imagenesDocumentos;

        public Form1()
        {
            InitializeComponent();
            InitializeComponentsAndData();
            this.AutoScroll = true;
            //InicializarPDFEncriptado();
        }

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
            pictureBoxHeader.Image = Properties.Resources.biblioTechnia;
            pictureBoxHeader.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxHeader.Size = new Size(160, 170);
            pictureBoxHeader.Location = new Point(50, 0);
            Controls.Add(pictureBoxHeader);

            Label labelTitle = CreateLabel("----Catálogo----", new Font("Barlow", 20, FontStyle.Regular), new Size(400, 50), new Point((int)((ClientSize.Width - 400) / 2.2), 240));
            Controls.Add(labelTitle);

            // Aquí están almacenadas las variables para la disposición de las imágenes
            int xPos = 50;
            int yPos = 300;
            int pictureBoxWidth = 125;
            int pictureBoxHeight = 200;
            int spacingX = 40;
            int spacingY = 50;
            int maxColumns = 4;
            int columnCount = 0;

            for (int i = 0; i < documentos.Count; i++)
            {
                PictureBox pictureBox = CreatePictureBox(imagenesDocumentos[i], new Point(xPos, yPos), pictureBox_Click, pictureBox_MouseEnter, pictureBox_MouseLeave);
                pictureBox.Tag = documentos[i].Id; // Se asigna el ID del documento al Tag del PictureBox
                Controls.Add(pictureBox);

                Label labelSubtitle = CreateLabel(nombresDocumentos[i], new Font("Barlow", 10, FontStyle.Regular), new Size(pictureBoxWidth, 40), new Point(xPos, yPos + pictureBoxHeight));
                Controls.Add(labelSubtitle);

                // Se actualiza posición X e Y para el próximo PictureBox y Label
                xPos += pictureBoxWidth + spacingX;
                columnCount++;

                if (columnCount >= maxColumns)
                {
                    // Se reinicia X y ajustar Y para comenzar una nueva fila
                    xPos = 55;
                    yPos += pictureBoxHeight + spacingY;
                    columnCount = 0;
                }
            }
        }

        private void InitializeSearchBar()
        {
            // Crear la barra de búsqueda en la parte superior derecha del formulario
            txtBusqueda = new TextBox();
            txtBusqueda.Location = new Point(ClientSize.Width - 400, 100);
            txtBusqueda.Size = new Size(200, 40);
            Controls.Add(txtBusqueda);

            // Crear el botón de búsqueda
            Button btnBuscar = new Button();
            btnBuscar.Text = "Buscar";
            btnBuscar.Location = new Point(txtBusqueda.Location.X + txtBusqueda.Width + 10, 100);
            btnBuscar.Size = new Size(75, 20);
            btnBuscar.Click += BtnBuscar_Click;
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
                if (control != txtBusqueda && control != sender as Button)
                {
                    Controls.Remove(control);
                    control.Dispose();
                }
            }

            // Volver a inicializar la UI con los documentos filtrados
            nombresDocumentos = documentosFiltrados.Select(doc => doc.Nombre).ToList();
            imagenesDocumentos = documentosFiltrados.Select(doc => doc.Imagen).ToList();
            InitializeUI(documentosFiltrados);
        }

        private PictureBox CreatePictureBox(Image image, Point location, EventHandler clickEvent, EventHandler enterEvent, EventHandler leaveEvent)
        {
            PictureBox pictureBox = new PictureBox();
            pictureBox.Image = image;
            pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox.Location = location;
            pictureBox.Click += clickEvent;
            pictureBox.MouseEnter += enterEvent;
            pictureBox.MouseLeave += leaveEvent;
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

                // Solicitas  contraseña al usuario (puedes implementar tu propia lógica para esto)
                string userPassword = "contraseña_del_usuario";
                string ownerPassword = "contraseña_del_propietario";



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


        private bool EsDocumentoEncriptado(DocumentManager.Documento documento)
        {
            // Comprobar si el documento está encriptado en función de su ruta
            return documento.Ruta.StartsWith("Encrypted_");
        }

        private void pictureBox_MouseEnter(object sender, EventArgs e)
        {
            // Se maneja el evento mouse enter en el PictureBox
            PictureBox pictureBox = sender as PictureBox;
            pictureBox.Image = Properties.Resources.libro_fondo_ejemplo;
        }

        private void pictureBox_MouseLeave(object sender, EventArgs e)
        {
            // Se maneja el evento mouse leave en el PictureBox
            PictureBox pictureBox = sender as PictureBox;
            pictureBox.Image = Properties.Resources.LibroCerrado;
        }

        private void ShowPdfForm(int documentId)
        {
            DocumentManager.Documento documento = documentManager.ObtenerDocumentos().FirstOrDefault(doc => doc.Id == documentId);

            if (documento != null)
            {
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
