﻿using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace Prueba_Libro
{
    public partial class PdfForm : Form
    {

        private string pdfFilePath;
        private WebView2 webView;
        private bool webViewInitialized;

        public PdfForm(string pdfFileName)
        {
            InitializeComponent();

            // Combinar la ruta del ejecutable con el nombre del archivo PDF
            pdfFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pdfFileName);

            // Crear y configurar el control WebView2
            webView = new WebView2();
            webView.Dock = DockStyle.Fill;
            Controls.Add(webView);

            // Suscribirse al evento CoreWebView2InitializationCompleted para cargar el PDF después de que se complete la inicialización del WebView2
            webView.CoreWebView2InitializationCompleted += WebView_CoreWebView2InitializationCompleted;

            // Inicializar el control WebView2
            webView.EnsureCoreWebView2Async(null);

        }


        private void WebView_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            // Marcar que el WebView2 está inicializado
            webViewInitialized = true;

            // Navegar al archivo PDF una vez que el control WebView2 esté inicializado
            webView.CoreWebView2.Navigate(pdfFilePath);

        }




    }

}