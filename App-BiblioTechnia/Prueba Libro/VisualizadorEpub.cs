using System;
using System.IO;
using System.Text;

using VersOne.Epub;

namespace EpubViewer
{
    public class VisualizadorEpub
    {
        private EpubBook _book;

        public VisualizadorEpub(string filePath)
        {
            try
            {
                _book = EpubReader.ReadBook(filePath);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al abrir el archivo ePub: " + ex.Message);
            }
        }

        public string ObtenerContenidoHtml()
        {
            if (_book == null)
                return string.Empty;

            StringBuilder htmlBuilder = new StringBuilder();
            foreach (EpubContentFile contentFile in _book.ReadingOrder)
            {
                htmlBuilder.AppendLine(contentFile.ContentMimeType);
            }

            return htmlBuilder.ToString();
        }
    }
}
