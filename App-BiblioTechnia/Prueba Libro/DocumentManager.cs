using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

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
            public byte[] Audio { get; set; }

            public Documento(int id, string nombre, byte[] contenido, System.Drawing.Image imagen, byte[] audio = null)
            {
                Id = id;
                Nombre = nombre;
                Contenido = contenido;
                Imagen = imagen;
                Audio = audio;
            }
        }

        private List<Documento> documentos;

        public DocumentManager()
        {
            documentos = new List<Documento>
            {
                // Ejemplo de documento con contenido y sin audio
                new Documento(0, "Renacer", Properties.Resources.perro1, Properties.Resources._5R6d_0001_1707415664p),
                new Documento(1, "Documento de audio", null,  Properties.Resources.KhLH_0001_1707415666p, Properties.Resources.PruebaAudio2),
                new Documento(2, "Documento de audio", null,  Properties.Resources.KhLH_0001_1707415666p, Properties.Resources.balnearios),
                new Documento(3, "No me quiero casar", null,  Properties.Resources.KhLH_0001_1707415666p, Properties.Resources.BadBunny),
                new Documento(4, "Encomio del arte de la medicina", Properties.Resources.sl1p_0001_1707415665, Properties.Resources.sl1p_0001_1707415665p),
                new Documento(5, "Guia de Aves", Properties.Resources.QSK7_0001_1707415664, Properties.Resources.QSK7_0001_1707415664p),

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

        public List<System.Drawing.Image> ObtenerImagenesDocumentos()
        {
            return documentos.Select(doc => doc.Imagen).ToList();
        }

        public List<byte[]> ObtenerAudiosDocumentos()
        {
            return documentos.Where(doc => doc.Audio != null).Select(doc => doc.Audio).ToList(); // Retorna la lista de audios
        }
    }
}
