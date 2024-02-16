using System;
using System.IO;
using System.Windows.Forms;
using VersOne.Epub;

namespace Prueba_Libro
{
    public partial class pruebaEPUB : Form
    {
        public pruebaEPUB()
        {
            InitializeComponent();
            this.AutoScroll = true;
            MostrarContenidoEpub("DocumentosPDF/pruebaEpub.epub");
        }

        private void MostrarContenidoEpub(string filePath)
        {
            try
            {
                // Leer el archivo ePub
                EpubBook book = EpubReader.ReadBook(filePath);

                // Obtener el contenido HTML del primer capítulo
                string htmlContent = ObtenerContenidoHtml(book);

                // Mostrar el contenido en el control WebBrowser
                webBrowser1.DocumentText = htmlContent;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir el archivo ePub: " + ex.Message,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private string ObtenerContenidoHtml(EpubBook book)
        {
            // Construir el contenido HTML utilizando el contenido de los archivos del ePub
            // Aquí necesitarás procesar los archivos HTML, CSS y otros recursos del ePub
            // para construir un documento HTML completo que pueda ser renderizado por el WebBrowser
            // Puedes acceder a los archivos a través de las propiedades del objeto EpubBook
            // como ReadingOrder, Resources, etc.

            // Este es solo un ejemplo básico, necesitarás adaptarlo según tus necesidades específicas
            return "<html><body><h1>Contenido del ePub</h1><p>Este es un ejemplo básico de contenido.</p></body></html>";
        }
    }
}
