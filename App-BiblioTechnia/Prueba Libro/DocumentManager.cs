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
                    new Documento(0, "Texto largooooooo","DocumentosPDF/Libro.pdf", Properties.Resources.LibroCerrado),
                    new Documento(1, "LibroconUnNombresoteMUYMUYMUYLARGOlargisisisisissisimo", "DocumentosPDF/Prueba_1.pdf", Properties.Resources.LibroCerrado),
                    new Documento(2, "HolaMundo", "DocumentosPDF/Libro.pdf", Properties.Resources.LibroCerrado),
                    new Documento(3, "Libro", "DocumentosPDF/Libro.pdf", Properties.Resources.LibroCerrado),
                    new Documento(4, "perro ","DocumentosPDF/Libro.pdf", Properties.Resources.LibroCerrado),
                    new Documento(5, "gato ", "DocumentosPDF/Libro.pdf", Properties.Resources.LibroCerrado),
                    new Documento(6, "sucio ","DocumentosPDF/Libro.pdf", Properties.Resources.LibroCerrado),
                    new Documento(7, "teclado ","DocumentosPDF/Libro.pdf", Properties.Resources.LibroCerrado),
                    new Documento(8, "MONITOR","DocumentosPDF/Libro.pdf", Properties.Resources.LibroCerrado),
                    new Documento(9, "VASO ","DocumentosPDF/Libro.pdf", Properties.Resources.LibroCerrado),
                    new Documento(10, "pc ","DocumentosPDF/Libro.pdf", Properties.Resources.LibroCerrado),
                    new Documento(11, "christopher ", "Prueba_1.pdf", Properties.Resources.LibroCerrado),
                    new Documento(12, "Alejandra ", "Prueba_1.pdf", Properties.Resources.LibroCerrado),
                    new Documento(13, "Alejandro ", "Prueba_1.pdf", Properties.Resources.LibroCerrado),
                    new Documento(14, "Suiza ", "Prueba_1.pdf", Properties.Resources.LibroCerrado),
                    new Documento(15, "Pulsera ", "Prueba_1.pdf", Properties.Resources.LibroCerrado),
                    new Documento(16, "Memoria ", "Prueba_1.pdf", Properties.Resources.LibroCerrado),
                    new Documento(17, "Reloj ", "Prueba_1.pdf", Properties.Resources.LibroCerrado),
                    new Documento(18, "Mano ", "Prueba_1.pdf", Properties.Resources.LibroCerrado),
                    new Documento(19, "Brazo ", "Prueba_1.pdf", Properties.Resources.LibroCerrado),
                    new Documento(20, "pierna ", "Prueba_1.pdf", Properties.Resources.LibroCerrado),
                    new Documento(21, "Cabeza ", "Prueba_1.pdf", Properties.Resources.LibroCerrado),
                    new Documento(22, "Higado", "Prueba_1.pdf", Properties.Resources.LibroCerrado),
                    new Documento(23, "Intestino", "Prueba_1.pdf", Properties.Resources.LibroCerrado),
                    new Documento(24, "Cerebro", "Prueba_1.pdf", Properties.Resources.LibroCerrado),
                    new Documento(25, "Procesador ", "Prueba_1.pdf", Properties.Resources.LibroCerrado),
                    new Documento(26, "Memoria RAM ", "Prueba_1.pdf", Properties.Resources.LibroCerrado),
                    new Documento(27, "Dedos", "Prueba_1.pdf", Properties.Resources.LibroCerrado),
                    new Documento(28, "DEDOS ", "Prueba_1.pdf", Properties.Resources.LibroCerrado),
                    new Documento(29, "Muñeca ", "Prueba_1.pdf", Properties.Resources.LibroCerrado),
                    new Documento(30, "Huesos", "Prueba_1.pdf", Properties.Resources.LibroCerrado),
                    new Documento(31, "Uñas", "Prueba_1.pdf", Properties.Resources.LibroCerrado),
                    new Documento(32, "Musculo", "Prueba_1.pdf", Properties.Resources.LibroCerrado),
                    new Documento(33, "Musculos", "Prueba_1.pdf", Properties.Resources.LibroCerrado),
                    new Documento(34,"Proteina", "Prueba_1.pdf", Properties.Resources.LibroCerrado),
                    new Documento(35, "Drogas", "Prueba_1.pdf", Properties.Resources.LibroCerrado),
                    new Documento(36, "Educacion", "Prueba_1.pdf", Properties.Resources.LibroCerrado),
                    new Documento(37, "Educacion1", "Prueba_1.pdf", Properties.Resources.LibroCerrado),
                    new Documento(38, "Educacion2", "Prueba_1.pdf", Properties.Resources.LibroCerrado),
                    new Documento(39, "Higado", "Prueba_1.pdf", Properties.Resources.LibroCerrado),
                    new Documento(40, "Cartilago", "Prueba_1.pdf", Properties.Resources.LibroCerrado),
                    new Documento(41, "Pollo", "Prueba_1.pdf", Properties.Resources.LibroCerrado),
                    new Documento(42, "Carne", "Prueba_1.pdf", Properties.Resources.LibroCerrado),
                    new Documento(43, "Ventilador", "Prueba_1.pdf", Properties.Resources.LibroCerrado),
                    new Documento(44, "hola mundo ", "Prueba_1.pdf", Properties.Resources.LibroCerrado),
                    new Documento(45, "hola mundo ", "Prueba_1.pdf", Properties.Resources.LibroCerrado),
                    new Documento(46, "hola mundo ", "Prueba_1.pdf", Properties.Resources.LibroCerrado),
          
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
