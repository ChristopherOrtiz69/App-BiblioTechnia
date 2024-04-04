using System;
using System.Windows.Forms;
using VersOne.Epub;
using System.IO;
using System.Text;
using VersOne.Epub.Schema;
using System.Threading.Tasks;

namespace Prueba_Libro
{
    public partial class pruebaEPUB : Form
    {
        public pruebaEPUB()
        {
            InitializeComponent();
            MostrarContenidoEPUB(); // Llama al método para mostrar el contenido del EPUB al iniciar el formulario
        }

        private async Task MostrarContenidoEPUB()
        {
            // Abre el cuadro de diálogo para seleccionar el archivo EPUB
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos EPUB|*.epub";
            openFileDialog.Title = "Seleccionar archivo EPUB";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string epubFilePath = openFileDialog.FileName;

                try
                {
                    // Carga el libro EPUB
                    EpubBook book = EpubReader.ReadBook(epubFilePath);

                    // Leer metadatos   
                    string title = book.Title;

                    // Mostrar metadatos en los controles del formulario
                    lblTitle.Text = "Título: " + title;

                    // Convertir contenido a HTML
                    StringBuilder htmlBuilder = new StringBuilder();
                    /*foreach (EpubContentFileRef contentFile in book.ReadingOrder)
                    {
                        // Leer contenido del archivo
                        string content = await contentFile.ReadContentAsTextAsync();

                        // Agregar el contenido al HTML
                        htmlBuilder.AppendLine($"<h1>{contentFile.FileName}</h1>");
                        htmlBuilder.AppendLine(content);
                    }
                    string htmlContent = htmlBuilder.ToString();

                    // Mostrar contenido HTML en el WebBrowser
                    webBrowser.DocumentText = htmlContent;*/
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al abrir el archivo EPUB: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

    }
