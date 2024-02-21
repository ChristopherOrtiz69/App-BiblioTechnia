    using System;
    using System.Collections.Generic;
    using System.Drawing; // Agrega este using para System.Drawing.Image
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using Prueba_Libro;

    namespace Prueba_Libro
    {
        public class DocumentManager
        {
            public class Documento
            {
                public int Id { get; set; }
                public string Nombre { get; set; }
                public string Ruta { get; set; }
                public System.Drawing.Image Imagen { get; set; } // Usa System.Drawing.Image aquí

                public Documento(int id, string nombre, string ruta, System.Drawing.Image imagen) // Usa System.Drawing.Image aquí
                {
                    Id = id;
                    Nombre = nombre;
                    Ruta = ruta;
                    Imagen = imagen;                  
                }
            }

            private List<Documento> documentos;

            public DocumentManager()
            {
                documentos = new List<Documento>    
                {
                    new Documento(0, "Renácér","DocumentosPDF/9789562828055.epub", Properties.Resources._5R6d_0001_1707415664p),
                    new Documento(1, "El Pecado de la Carne", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(2, "Guía de Aves", "DocumentosPDF/QSK7-0001-1707415664.pdf", Properties.Resources.QSK7_0001_1707415664p),
                    new Documento(3, "Encomio de Arte en la Medicina", "DocumentosPDF/sl1p-0001-1707415665.pdf", Properties.Resources.sl1p_0001_1707415665p),
                    new Documento(4, "Texto o titulo largisisisisimo de prueba lalalalalalalaala ","DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(5, "gato ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(6, "sucio ","DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(7, "Presentación ","DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(8, "Pirámide","DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(9, "VASO ","DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(10, "pc ","DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(11, "christopher ","DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(12, "Alejandra ","DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(13, "Órdenes ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(14, "Suiza ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(15, "Pulsera ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(16, "Hígado ", "DocumentosPDF/KhLH-0001-1707415666.pdf"  , Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(17, "Ejército ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(18, "Ubicación ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(19, "Ridículo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(20, "Tarántula ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(21, "Cabeza ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(22, "Técnicas", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(23, "Intestino", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(24, "América", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(25, "Agrícola Oriental ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(26, "Memoria RAM ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(27, "México Magico", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(28, "DEDOS ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(29, "Muñeca ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(30, "Huesos", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(31, "Uñas", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(32, "Musculo", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(33, "Musculos", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(34,"Proteina", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(35, "Drogas", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(36, "Educacion", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(37, "Educacion1", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(38, "Educacion2", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(39, "Higado", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(40, "Cartilago", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(41, "Pollo", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(42, "Carne", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(43, "Ventilador", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(44, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(45, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(46, "hola mundo ", "DocumentosPDF/KhLH-0001-1707415666.pdf", Properties.Resources.KhLH_0001_1707415666p),
          
                };
        }
        public List<Documento> ObtenerDocumentos()
        {
            return documentos;
        }

        public List<Documento> ObtenerDocumentosOrdenadosPorId()
        {
            return documentos.OrderBy(doc => doc.Id).ToList();
        }

        public List<string> ObtenerNombresDocumentos()
        {
            return documentos.Select(doc => doc.Nombre).ToList();
        }

        public List<System.Drawing.Image> ObtenerImagenesDocumentos() // Usa System.Drawing.Image aquí
        {
            return documentos.Select(doc => doc.Imagen).ToList();
        }

        public void CifrarDocumento(int documentoId, string userPassword, string ownerPassword, bool permitirImpresion)
        {
            // Encontrar el documento en la lista
            Documento documento = documentos.Find(doc => doc.Id == documentoId);
            if (documento == null)
            {
                MessageBox.Show("Documento no encontrado.");
                return;
            }

            // Combinar la ruta del ejecutable con la ruta del documento PDF
            string pdfFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, documento.Ruta);

            // Generar el nombre del archivo cifrado
            string encryptedPdfFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Encrypted_{documento.Nombre}.pdf");

            // Cifrar el documento PDF con los permisos adecuados
            EncryptPdf(pdfFilePath, encryptedPdfFilePath, userPassword, ownerPassword);

            MessageBox.Show($"Documento cifrado guardado en: {encryptedPdfFilePath}");
        }

        private void EncryptPdf(string inputFilePath, string outputFilePath, string userPassword, string ownerPassword)
        {
            using (var reader = new PdfReader(inputFilePath))
            {
                using (var outputStream = new FileStream(outputFilePath, FileMode.Create))
                {
                    // Crea un escritor de PDF
                    var stamper = new PdfStamper(reader, outputStream);

                    // Configura las contraseñas de usuario y propietario
                    stamper.SetEncryption(Encoding.ASCII.GetBytes(userPassword), Encoding.ASCII.GetBytes(ownerPassword),
                        PdfWriter.ALLOW_COPY, PdfWriter.ENCRYPTION_AES_128);

                    // Configura todos los permisos a false
                    stamper.SetEncryption(null, null, 0, PdfWriter.ENCRYPTION_AES_128);

                    // Cierra el lector y el escritor
                    stamper.Close();
                    reader.Close();
                }
            }
        }


    }
}

