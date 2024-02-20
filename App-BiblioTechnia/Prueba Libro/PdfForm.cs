using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace Prueba_Libro
{
    public partial class PdfForm : Form
    {
        private string pdfFilePath;
        private WebView2 webView;
        private Button btnSubir;
        private Button btnBajar;

        public PdfForm(string pdfFileName)
        {
            InitializeComponent();

            // Combinar la ruta del ejecutable con el nombre del archivo PDF
            pdfFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pdfFileName);

            // Crear botones de subir y bajar
            btnSubir = new Button();
            btnSubir.Text = "Subir";
            btnSubir.Location = new System.Drawing.Point(10, 10);
            btnSubir.Click += BtnSubir_Click;
            Controls.Add(btnSubir);

            btnBajar = new Button();
            btnBajar.Text = "Bajar";
            btnBajar.Location = new System.Drawing.Point(10, 40);
            btnBajar.Click += BtnBajar_Click;
            Controls.Add(btnBajar);

            // Crear y configurar el control WebView2
            webView = new WebView2();
            webView.Dock = DockStyle.Fill;
            Controls.Add(webView);

            // Suscribirse al evento CoreWebView2InitializationCompleted para cargar el PDF después de que se complete la inicialización del WebView2
            webView.CoreWebView2InitializationCompleted += WebView_CoreWebView2InitializationCompleted;

            // Inicializar el control WebView2
            webView.EnsureCoreWebView2Async(null);

            // Deshabilitar todos los controles excepto los botones de subir y bajar
            DisableAllControlsExceptButtons();
        }

        private async void BtnSubir_Click(object sender, EventArgs e)
        {
            // Desplazar hacia arriba dentro del control WebView2
            await webView.ExecuteScriptAsync("window.scrollTo({ top: document.documentElement.scrollTop - 100, behavior: 'smooth' });");
        }

        private async void BtnBajar_Click(object sender, EventArgs e)
        {
            // Desplazar hacia abajo dentro del control WebView2
            await webView.ExecuteScriptAsync("window.scrollTo({ top: document.documentElement.scrollTop + 100, behavior: 'smooth' });");
        }

        private void WebView_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            // Navegar al archivo PDF una vez que el control WebView2 esté inicializado
            webView.CoreWebView2.Navigate(pdfFilePath);
        }

        private void DisableAllControlsExceptButtons()
        {
            foreach (Control control in Controls)
            {
                // Excluir los botones de subir y bajar de la desactivación
                if (control != btnSubir && control != btnBajar)
                {
                    DisableControl(control);
                }
            }
        }

        private void DisableControl(Control control)
        {
            // Deshabilitar el control y todos sus controles secundarios recursivamente
            control.Enabled = false;
            foreach (Control childControl in control.Controls)
            {
                DisableControl(childControl);
            }
        }
    }
}
