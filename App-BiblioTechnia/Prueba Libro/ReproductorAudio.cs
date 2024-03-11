using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using NAudio.Wave;

namespace Prueba_Libro
{
    public partial class ReproductorAudio : Form
    {
        // Declara un objeto para la reproducción de audio
        private IWavePlayer waveOutDevice;
        private MemoryStream audioStream;
        private Mp3FileReader mp3FileReader;
        private Timer timer;
        private PictureBox pictureBox;
        private Button playPauseButton;
        private Button restartButton;
        private TrackBar volumeTrackBar;

        public ReproductorAudio()
        {
            InitializeComponent();
            trackBar.Scroll += trackBar_Scroll;

            // Agrega el PictureBox al formulario
            pictureBox = new PictureBox();
            pictureBox.Image = Properties.Resources.Logo_BiblioTechnia; // Cambia "NombreDeTuImagen" al nombre de tu imagen
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom; // Ajusta el tamaño del PictureBox al tamaño de la imagen
            pictureBox.Location = new Point(30, -150);
            pictureBox.Size = new Size(400, 400);
            Controls.Add(pictureBox); 

            // Agrega el botón de play/pause
            playPauseButton = new Button();
            playPauseButton.Size = new Size(100, 50);
            playPauseButton.Location = new Point(375, 250);
            playPauseButton.Click += PlayPauseButton_Click;
            playPauseButton.FlatStyle =FlatStyle.Flat;
            playPauseButton.FlatAppearance.BorderSize = 0;
            playPauseButton.Margin = new Padding(0);
            Image image = Image.FromFile("Images/pausa.png");
            // Redimensionar la imagen a un tamaño más pequeño
            int newWidth = 50; // Ancho deseado
            int newHeight = 50; // Alto deseado
            Image resizedImage = new Bitmap(image, new Size(newWidth, newHeight));
            playPauseButton.Image = resizedImage;
            playPauseButton.ImageAlign = ContentAlignment.MiddleCenter;
            Controls.Add(playPauseButton);
            
            restartButton = new Button();
            restartButton.Size = new Size(100, 50);
            restartButton.Location = new Point(287, 250);
            restartButton.FlatStyle = FlatStyle.Flat;
            restartButton.FlatAppearance.BorderSize = 0;
            restartButton.Margin = new Padding(0);
            Image image1 = Image.FromFile("Images/reiniciar.png");
            // Redimensionar la imagen a un tamaño más pequeño
            int newWidth1 = 50; // Ancho deseado
            int newHeight1 = 50; // Alto deseado
            Image resizedImage1 = new Bitmap(image1, new Size(newWidth1, newHeight1));
            restartButton.Image = resizedImage1;
            restartButton.ImageAlign = ContentAlignment.MiddleCenter;
            restartButton.Click += RestartButton_Click;
            Controls.Add(restartButton);

            volumeTrackBar = new TrackBar();
            volumeTrackBar.Location = new Point(50, 260);
            volumeTrackBar.Size = new Size(150, 40);
            volumeTrackBar.Minimum = 0; // Volumen mínimo
            volumeTrackBar.Maximum = 100; // Volumen máximo
            volumeTrackBar.TickFrequency = 10; // Incremento de volumen
            volumeTrackBar.Value = 50; // Valor inicial del volumen (por ejemplo, 50%)
            volumeTrackBar.Scroll += VolumeTrackBar_Scroll;
            Controls.Add(volumeTrackBar);

            try
            {
                // Convierte los bytes del recurso de audio en un MemoryStream
                audioStream = new MemoryStream(Properties.Resources.PruebaAudio2);

                // Crea un lector de archivos MP3
                mp3FileReader = new Mp3FileReader(audioStream);

                // Crea un dispositivo de salida de audio
                waveOutDevice = new WaveOutEvent();

                // Asigna el lector de archivos MP3 al dispositivo de salida
                waveOutDevice.Init(mp3FileReader);

                // Asigna el máximo valor de la barra de desplazamiento al tiempo total del audio
                trackBar.Maximum = (int)mp3FileReader.TotalTime.TotalSeconds;

                // Reproduce el audio
                waveOutDevice.Play();

                // Inicializa el temporizador para actualizar la posición del TrackBar y la etiqueta de tiempo
                timer = new Timer();
                timer.Interval = 1000; // Actualiza cada segundo
                timer.Tick += Timer_Tick;
                timer.Start();
            }
            catch (Exception ex)
            {
                // Muestra un mensaje de error si ocurre alguna excepción
                MessageBox.Show("Error al cargar o reproducir el archivo de audio: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void VolumeTrackBar_Scroll(object sender, EventArgs e)
        {
            // Obtén el valor del control TrackBar para el volumen
            int volume = volumeTrackBar.Value;

            // Convierte el valor del volumen al rango aceptado por NAudio (0.0 a 1.0)
            float volumeNormalized = volume / 100f;

            // Ajusta el volumen del dispositivo de salida de audio
            if (waveOutDevice != null)
            {
                waveOutDevice.Volume = volumeNormalized;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Actualiza la posición del TrackBar al tiempo actual de reproducción
            if (mp3FileReader != null && waveOutDevice != null)
            {
                // Actualiza el valor del TrackBar
                trackBar.Value = (int)mp3FileReader.CurrentTime.TotalSeconds;

                // Actualiza la etiqueta de tiempo
                labelTiempo.Text = TimeSpan.FromSeconds(trackBar.Value).ToString(@"hh\:mm\:ss");
            }
        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {
            // Cuando el usuario mueve la barra de desplazamiento, establece la posición de reproducción del audio
            if (waveOutDevice != null && mp3FileReader != null)
            {
                waveOutDevice.Pause(); // Pausa la reproducción mientras se cambia la posición
                mp3FileReader.CurrentTime = TimeSpan.FromSeconds(trackBar.Value);
                waveOutDevice.Play(); // Continúa la reproducción desde la nueva posición
            }
        }

        private void PlayPauseButton_Click(object sender, EventArgs e)
        {
            // Si la reproducción está en pausa, reanuda; si no, pausa
            if (waveOutDevice.PlaybackState == PlaybackState.Paused)
            {
                waveOutDevice.Play();

            }
            else
            {
                waveOutDevice.Pause();
               
            }
        }
        private void RestartButton_Click(object sender, EventArgs e)
        {
            // Detiene la reproducción del audio
            waveOutDevice.Stop();
            waveOutDevice.Dispose();
            audioStream.Position = 0; // Reinicia el audio al segundo 0
            mp3FileReader.Position = 0; // Reinicia el audio al segundo 0
            waveOutDevice.Init(mp3FileReader);
            waveOutDevice.Play(); // Reproduce el audio desde el principio
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Detiene la reproducción del audio al cerrar el formulario
            if (waveOutDevice != null)
            {
                waveOutDevice.Stop();
                waveOutDevice.Dispose();
            }

            if (audioStream != null)
            {
                audioStream.Dispose();
            }

            if (mp3FileReader != null)
            {
                mp3FileReader.Dispose();
            }

            // Detiene el temporizador al cerrar el formulario
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }

            base.OnFormClosing(e);
        }
    }
}
