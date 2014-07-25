using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpotifyAPI.SpotifyLocalAPI;
using System.Threading;
using SpotifyEventHandler = SpotifyAPI.SpotifyLocalAPI.SpotifyEventHandler;



namespace mvSpotify
{
    public partial class Form1 : Form
    {
        SpotifyLocalAPIClass spotify;
        SpotifyMusicHandler mh;
        SpotifyEventHandler eh;

        public Form1()
        {
            InitializeComponent();
            spotify = new SpotifyLocalAPIClass();
            if (!SpotifyLocalAPIClass.IsSpotifyRunning())
            {
                spotify.RunSpotify();
                Thread.Sleep(5000);
            }

            if (!SpotifyLocalAPIClass.IsSpotifyWebHelperRunning())
            {
                spotify.RunSpotifyWebHelper();
                Thread.Sleep(4000);
            }

            if (!spotify.Connect())
            {
                Boolean retry = true;
                while (retry)
                {
                    if (MessageBox.Show("SpotifyLocal no se pudo cargar!", "Error", MessageBoxButtons.RetryCancel) == System.Windows.Forms.DialogResult.Retry)
                    {
                        if (spotify.Connect())
                            retry = false;
                        else
                            retry = true;
                    }
                    else
                    {
                        this.Close();
                        return;
                    }
                }
            }
            mh = spotify.GetMusicHandler();
            eh = spotify.GetEventHandler();
        }

        //load sincrono con el spotify
        private async void Form1_Load(object sender, EventArgs e)
        {
            spotify.Update();
            progressBar1.Maximum = (int)mh.GetCurrentTrack().GetLength() * 100;
            pictureBox1.Image = await spotify.GetMusicHandler().GetCurrentTrack().GetAlbumArtAsync(AlbumArtSize.SIZE_160);
            pictureBox2.Image = await spotify.GetMusicHandler().GetCurrentTrack().GetAlbumArtAsync(AlbumArtSize.SIZE_640);

            linkLabel1.Text = mh.GetCurrentTrack().GetTrackName();
            linkLabel1.LinkClicked += (senderTwo, args) => Process.Start(mh.GetCurrentTrack().GetTrackURI());
            linkLabel2.Text = mh.GetCurrentTrack().GetArtistName();
            linkLabel2.LinkClicked += (senderTwo, args) => Process.Start(mh.GetCurrentTrack().GetArtistURI());
            linkLabel3.Text = mh.GetCurrentTrack().GetAlbumName();
            linkLabel3.LinkClicked += (senderTwo, args) => Process.Start(mh.GetCurrentTrack().GetAlbumURI());

            label9.Text = mh.IsPlaying().ToString();
            label11.Text = ((int)(mh.GetVolume() * 100)).ToString();
            label7.Text = mh.IsAdRunning().ToString();

            eh.OnTrackChange += new SpotifyEventHandler.TrackChangeEventHandler(trackchange);
            eh.OnTrackTimeChange += new SpotifyEventHandler.TrackTimeChangeEventHandler(timechange);
            eh.OnPlayStateChange += new SpotifyEventHandler.PlayStateEventHandler(playstatechange);
            eh.OnVolumeChange += new SpotifyEventHandler.VolumeChangeEventHandler(volumechange);
            eh.SetSynchronizingObject(this);
            eh.ListenForEvents(true);
        }

        //metodo que muestra el nivel del volumen
        private void volumechange(VolumeChangeEventArgs e)
        {
            label11.Text = ((int)(mh.GetVolume() * 100)).ToString();
        }

        //metodo para mostrar el estado de la cancion (True si es que esta en curso y false si esta detenida)
        private void playstatechange(PlayStateEventArgs e)
        {
            label9.Text = e.playing.ToString();
        }

        //metodo para reasignar los atributos de la cancion(track, artista, album)
        private async void trackchange(TrackChangeEventArgs e)
        {
            progressBar1.Maximum = (int)mh.GetCurrentTrack().GetLength() * 100;
            linkLabel1.Text = e.new_track.GetTrackName();
            linkLabel2.Text = e.new_track.GetArtistName();
            linkLabel3.Text = e.new_track.GetAlbumName();
            pictureBox1.Image = await e.new_track.GetAlbumArtAsync(AlbumArtSize.SIZE_160);
            pictureBox2.Image = await e.new_track.GetAlbumArtAsync(AlbumArtSize.SIZE_640);
            label7.Text = mh.IsAdRunning().ToString();
        }

        //metodo para asignar el valor al progressbar segun el tiempo de reproduccion de la cancion
        private void timechange(TrackTimeChangeEventArgs e)
        {
            label4.Text = formatTime(e.track_time) + "/" + formatTime(mh.GetCurrentTrack().GetLength());
            progressBar1.Value = (int)e.track_time * 100;
        }

        //metodo para obtener el tiempo de reproduccion de la cancion
        private String formatTime(double sec)
        {
            TimeSpan span = TimeSpan.FromSeconds(sec);
            String secs = span.Seconds.ToString(), mins = span.Minutes.ToString();
            if (secs.Length < 2)
                secs = "0" + secs;
            return mins + ":" + secs;
        }

        #region BOTONES
        
        //boton play
        private void button1_Click(object sender, EventArgs e)
        {
            mh.Play();
        }

        //boton pause
        private void button2_Click(object sender, EventArgs e)
        {
            mh.Pause();
        }

        //boton anterior
        private void button3_Click(object sender, EventArgs e)
        {
            mh.Previous();
        }

        //boton siguiente
        private void button4_Click(object sender, EventArgs e)
        {
            mh.Skip();
        }

        //boton buscar 
        private void button5_Click(object sender, EventArgs e)
        {
            mh.PlayURL(textBox1.Text);
        }

        #endregion

        //Accion para dejar en mute la cancion
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                mh.Mute();
            }
            else
            {
                mh.UnMute();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int i;
            for (i = 0; i <= 1; i++)
            {
                label11.Text = ((int)(mh.GetVolume() * 100) + i).ToString();
            }
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            label11.Text = ((int)(mh.GetVolume() * 100) - 1).ToString();
        }

    }
}
