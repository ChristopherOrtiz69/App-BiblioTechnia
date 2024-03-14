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
        private bool modoNoche = false;
        Image playImage = Image.FromFile("Images/play-pause.png");
        Image pauseImage = Image.FromFile("Images/flecha-izquierda.png");


        public ReproductorAudio(byte[] audioBytes)
        {
            InitializeComponent();
            this.audioStream = new MemoryStream(audioBytes);
            trackBar.Scroll += trackBar_Scroll;

            // Agrega el PictureBox al formulario
            pictureBox = new PictureBox();
            pictureBox.Image = Properties.Resources.Logo_bibliotechnia_SinFondo;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.Location = new Point(30, -150);
            pictureBox.Size = new Size(400, 400);
            Controls.Add(pictureBox);

            // Agrega el botón de play/pause
            // Crear el botón playPauseButton
            playPauseButton = new Button();
            playPauseButton.Size = new Size(100, 50);
            playPauseButton.Location = new Point(287, 250);
            playPauseButton.FlatStyle = FlatStyle.Flat;
            playPauseButton.FlatAppearance.BorderSize = 0;
            playPauseButton.Margin = new Padding(0);
            playPauseButton.ImageAlign = ContentAlignment.MiddleCenter;

            // Cargar la imagen y redimensionarla
            Image image = Image.FromFile("Images/play-pause.png");
            int newWidth = 50; // Ancho deseado
            int newHeight = 50; // Alto deseado
            Image resizedImage = new Bitmap(image, new Size(newWidth, newHeight));
            playPauseButton.Image = resizedImage;

            // Agregar el controlador de eventos al evento Click
            playPauseButton.Click += PlayPauseButton_Click;

            // Agregar el botón al formulario
            Controls.Add(playPauseButton);


            restartButton = new Button();
            restartButton.Size = new Size(100, 50);
            restartButton.Location = new Point(500, 250);
            restartButton.FlatStyle = FlatStyle.Flat;
            restartButton.FlatAppearance.BorderSize = 0;
            restartButton.Margin = new Padding(0);
            Image image1 = Image.FromFile("Images/repeticion.png");
            // Redimensionar la imagen a un tamaño más pequeño
            int newWidth1 = 50; // Ancho deseado
            int newHeight1 = 50; // Alto deseado
            Image resizedImage1 = new Bitmap(image1, new Size(newWidth1, newHeight1));
            restartButton.Image = resizedImage1;
            restartButton.ImageAlign = ContentAlignment.MiddleCenter;
            restartButton.Click += RestartButton_Click;
            Controls.Add(restartButton);

            volumeTrackBar = new TrackBar();
            volumeTrackBar.Location = new Point(650, 60);
            volumeTrackBar.Size = new Size(40, 100); // Cambiar el tamaño para que sea vertical
            volumeTrackBar.Orientation = Orientation.Vertical; // Cambiar la orientación a vertical
            volumeTrackBar.Minimum = 0; // Volumen mínimo
            volumeTrackBar.Maximum = 100; // Volumen máximo
            volumeTrackBar.TickFrequency = 10; // Incremento de volumen
            volumeTrackBar.Value = 50; // Valor inicial del volumen (por ejemplo, 50%)
            volumeTrackBar.Scroll += VolumeTrackBar_Scroll;
            Controls.Add(volumeTrackBar);

            PictureBox volumePictureBox = new PictureBox();
            volumePictureBox.Image = Properties.Resources.volumen;
            volumePictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            volumePictureBox.Size = new Size(40, 40);
            volumePictureBox.Location = new Point(volumeTrackBar.Left, volumeTrackBar.Top - volumePictureBox.Height - 5);
            Controls.Add(volumePictureBox);

           /* Button modoNocheButton = new Button();
            modoNocheButton.Size = new Size(100, 50);
            modoNocheButton.Location = new Point(600, 250);
            modoNocheButton.Click += ModoNocheButton_Click;
            modoNocheButton.FlatStyle = FlatStyle.Flat;
            modoNocheButton.FlatAppearance.BorderSize = 0;
            modoNocheButton.Margin = new Padding(0);
            Image modoNocheImage = Image.FromFile("Images/modo-oscuro.png"); 
            int newWidth2 = 50; // Ancho deseado
            int newHeight2 = 50; // Alto deseado
            Image resizedImage2 = new Bitmap(modoNocheImage, new Size(newWidth2, newHeight2));
            modoNocheButton.Image = resizedImage2;
            modoNocheButton.ImageAlign = ContentAlignment.MiddleCenter;
            Controls.Add(modoNocheButton);*/

            Color colorFondo = this.BackColor; // Guarda el color de fondo del formulario

          

            Button retrocederButton = new Button();
            retrocederButton.Size = new Size(50, 50); // Ajusta el tamaño según el tamaño de tu imagen
            retrocederButton.Location = new Point(225, 250); // Ajusta la posición según la ubicación deseada
            retrocederButton.BackgroundImage = Properties.Resources.flecha_izquierda; // Reemplaza "ImagenRetroceder" con el nombre de tu imagen retroceder en los recursos
            retrocederButton.BackgroundImageLayout = ImageLayout.Stretch; // Ajusta el diseño de la imagen según tus necesidades
            retrocederButton.FlatStyle = FlatStyle.Flat; // Establece el estilo del botón como Flat
            retrocederButton.FlatAppearance.BorderSize = 1; // Establece el ancho del borde a 1
            retrocederButton.FlatAppearance.BorderColor = colorFondo; // Establece el color del borde como el color del fondo del formulario
            retrocederButton.BackColor = colorFondo; // Establece el color de fondo del botón como el color del fondo del formulario
            retrocederButton.Click += RetrocederButton_Click; // Asigna el evento de clic
            Controls.Add(retrocederButton);

            // Carga la imagen de Avanzar desde los recursos
            Bitmap imagenAvanzar = Properties.Resources.flecha_izquierda;

            // Rota la imagen 180 grados
            imagenAvanzar.RotateFlip(RotateFlipType.Rotate180FlipNone);

            // Crea el botón "Avanzar" con la imagen rotada
            Button avanzarButton = new Button();
            avanzarButton.Size = new Size(50, 50); // Ajusta el tamaño según el tamaño de tu imagen
            avanzarButton.Location = new Point(400, 250); // Ajusta la posición según la ubicación deseada
            avanzarButton.BackgroundImage = imagenAvanzar; // Usa la imagen rotada como fondo del botón
            avanzarButton.BackgroundImageLayout = ImageLayout.Stretch; // Ajusta el diseño de la imagen según tus necesidades
            avanzarButton.FlatStyle = FlatStyle.Flat; // Establece el estilo del botón como Flat
            avanzarButton.FlatAppearance.BorderSize = 1; // Establece el ancho del borde a 1
            avanzarButton.FlatAppearance.BorderColor = colorFondo; // Establece el color del borde como el color del fondo del formulario
            avanzarButton.BackColor = colorFondo; // Establece el color de fondo del botón como el color del fondo del formulario
            avanzarButton.Click += AvanzarButton_Click; // Asigna el evento de clic
            Controls.Add(avanzarButton);




            try
            {
                // Crea un lector de archivos MP3
                mp3FileReader = new Mp3FileReader(this.audioStream);

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

      

        private void AvanzarButton_Click(object sender, EventArgs e)
        {
            if (waveOutDevice != null && mp3FileReader != null)
            {
                // Pausa la reproducción mientras se ajusta la posición
                waveOutDevice.Pause();

                // Avanza 30 segundos
                TimeSpan nuevaPosicion = mp3FileReader.CurrentTime.Add(TimeSpan.FromSeconds(30));

                // Establece la nueva posición de reproducción
                mp3FileReader.CurrentTime = nuevaPosicion;

                // Continúa la reproducción
                waveOutDevice.Play();
            }
        }

        private void RetrocederButton_Click(object sender, EventArgs e)
        {
            if (waveOutDevice != null && mp3FileReader != null)
            {
                // Pausa la reproducción mientras se ajusta la posición
                waveOutDevice.Pause();

                // Retrocede 10 segundos
                TimeSpan nuevaPosicion = mp3FileReader.CurrentTime.Subtract(TimeSpan.FromSeconds(10));

                // Asegúrate de que la nueva posición no sea negativa
                if (nuevaPosicion.TotalSeconds < 0)
                    nuevaPosicion = TimeSpan.Zero;

                // Establece la nueva posición de reproducción
                mp3FileReader.CurrentTime = nuevaPosicion;

                // Continúa la reproducción
                waveOutDevice.Play();
            }
        }


        private void ModoNocheButton_Click(object sender, EventArgs e)
        {
            // Cambiar el estado del modo noche
            modoNoche = !modoNoche;

            // Cambiar el color de fondo de la ventana
            if (modoNoche)
            {
                this.BackColor = Color.Gray; // Cambiar el color a gris
                pictureBox.Image = Properties.Resources.Icono_SinFondo;
                pictureBox.Size = new Size(180,180);
                pictureBox.Location = new Point(20, -40);
            }
            else
            {
                this.BackColor = SystemColors.Control; // Cambiar el color al estado natural
                pictureBox.Image = Properties.Resources.Logo_bibliotechnia_SinFondo; 
                pictureBox.Size = new Size(400, 400);
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox.Location = new Point(30, -150);
               
            }
        }
        private void VolumeTrackBar_Scroll(object sender, EventArgs e)
        {
            // Obtén el valor del control TrackBar para el volumen
            int volume = volumeTrackBar.Value;

           
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
