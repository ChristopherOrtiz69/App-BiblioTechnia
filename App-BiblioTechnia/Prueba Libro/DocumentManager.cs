using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
namespace Prueba_Libro
    {
        public class DocumentManager
        {
            public class Documento
            {
                public int Id { get; set; }
                public string Nombre { get; set; }
               public byte[] Contenido { get; set; }
                public System.Drawing.Image Imagen { get; set; }

                public Documento(int id, string nombre, byte[] ruta, System.Drawing.Image imagen)
                {
                    Id = id;
                    Nombre = nombre;
                    Contenido = ruta;
                    Imagen = imagen;                   
                }
            }

            private List<Documento> documentos;

            public DocumentManager()
            {
                documentos = new List<Documento>
                {
                    new Documento(0,"Renacer", Properties.Resources.perro1                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        , Properties.Resources._5R6d_0001_1707415664p),
                    new Documento(1, "El Pecado de la Carne",Properties.Resources.sl1p_0001_1707415665, Properties.Resources.sl1p_0001_1707415665p),
                    new Documento(2, "Guía de Aves",Properties.Resources.QSK7_0001_1707415664, Properties.Resources.QSK7_0001_1707415664p),
                    new Documento(3, "Encomio de Arte en la Medicina", Properties.Resources.KhLH_0001_1707415666, Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(4, "Texto o titulo largisisisisimo de prueba lalalalalalalaala ",Properties.Resources.perro1, Properties.Resources.sl1p_0001_1707415665p),
                    new Documento(5, "gato ",Properties.Resources.KhLH_0001_1707415666, Properties.Resources.KhLH_0001_1707415666p),
                    new Documento(6, "sucio ",Properties.Resources.perro1, Properties.Resources.sl1p_0001_1707415665p),
                    new Documento(7, "Presentación ",Properties.Resources.perro1, Properties.Resources.sl1p_0001_1707415665p),
                    new Documento(8, "Pirámide",Properties.Resources.perro1, Properties.Resources.sl1p_0001_1707415665p),
                    new Documento(9, "VASO ",Properties.Resources.perro1, Properties.Resources.sl1p_0001_1707415665p),
                    new Documento(10, "pc ",Properties.Resources.perro1, Properties.Resources.sl1p_0001_1707415665p),
                    new Documento(11, "christopher ",Properties.Resources.perro1, Properties.Resources.sl1p_0001_1707415665p),
                    new Documento(12, "Alejandra ",Properties.Resources.perro1, Properties.Resources.sl1p_0001_1707415665p),
                    new Documento(13, "Órdenes ", Properties.Resources.perro1, Properties.Resources.sl1p_0001_1707415665p),
                    new Documento(14, "Suiza ", Properties.Resources.perro1, Properties.Resources.sl1p_0001_1707415665p),
                    new Documento(15, "Pulsera ", Properties.Resources.perro1, Properties.Resources.sl1p_0001_1707415665p),
                    new Documento(16, "Hígado ", Properties.Resources.perro1  , Properties.Resources.sl1p_0001_1707415665p),
                    new Documento(17, "Ejército ", Properties.Resources.perro1, Properties.Resources.sl1p_0001_1707415665p),
                    new Documento(18, "Ubicación ", Properties.Resources.perro1, Properties.Resources.sl1p_0001_1707415665p),
                    new Documento(19, "Ridículo ", Properties.Resources.perro1, Properties.Resources.sl1p_0001_1707415665p),
                    new Documento(20, "Tarántula ", Properties.Resources.perro1, Properties.Resources.sl1p_0001_1707415665p),
                    new Documento(21, "Cabeza ", Properties.Resources.perro1, Properties.Resources.sl1p_0001_1707415665p),
                    new Documento(22, "Técnicas", Properties.Resources.perro1, Properties.Resources.sl1p_0001_1707415665p),
                    new Documento(23, "Intestino", Properties.Resources.perro1, Properties.Resources.sl1p_0001_1707415665p),
                    new Documento(24, "América", Properties.Resources.perro1, Properties.Resources.sl1p_0001_1707415665p),
                    new Documento(25, "Agrícola Oriental ", Properties.Resources.perro1, Properties.Resources.sl1p_0001_1707415665p),
                    new Documento(26, "Memoria RAM ", Properties.Resources.perro1, Properties.Resources.sl1p_0001_1707415665p),
                    new Documento(27, "México Magico", Properties.Resources.perro1, Properties.Resources.sl1p_0001_1707415665p),
                    new Documento(28, "DEDOS ", Properties.Resources.perro1, Properties.Resources.sl1p_0001_1707415665p),
                    new Documento(29, "Muñeca ", Properties.Resources.perro1, Properties.Resources.sl1p_0001_1707415665p),
                    new Documento(30, "Huesos", Properties.Resources.perro1, Properties.Resources.sl1p_0001_1707415665p),
                    new Documento(31, "Uñas", Properties.Resources.perro1, Properties.Resources.sl1p_0001_1707415665p),
                    new Documento(32, "Musculo", Properties.Resources.perro1, Properties.Resources.sl1p_0001_1707415665p),
                    new Documento(33, "Musculos", Properties.Resources.perro1, Properties.Resources.sl1p_0001_1707415665p),
                    new Documento(34,"Proteina", Properties.Resources.perro1, Properties.Resources.sl1p_0001_1707415665p),
                  
          
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

       /* public void CifrarDocumento(int documentoId, string userPassword, string ownerPassword, bool permitirImpresion)
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
        }*/


    }
}

