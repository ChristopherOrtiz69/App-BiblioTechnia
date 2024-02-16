using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace Prueba_Libro
{
    public partial class PdfForm : Form
    {
        private string epubFilePath;
        private WebView2 webView;

        public PdfForm(string epubFileName)
        {
            InitializeComponent();

            // Combinar la ruta del ejecutable con el nombre del archivo ePub
            epubFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, epubFileName);

            // Crear y configurar el control WebView2
            webView = new WebView2();
            webView.Dock = DockStyle.Fill;
            Controls.Add(webView);

            // Suscribirse al evento CoreWebView2InitializationCompleted para cargar el contenido ePub después de que se complete la inicialización del WebView2
            webView.CoreWebView2InitializationCompleted += WebView_CoreWebView2InitializationCompleted;

            // Inicializar el control WebView2
            webView.EnsureCoreWebView2Async(null);
        }

        private void WebView_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            // Navegar al contenido del archivo ePub una vez que el control WebView2 esté inicializado
            webView.CoreWebView2.Navigate(epubFilePath);
        }
    }
}
