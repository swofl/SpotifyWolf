using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NAudio.Lame;
using NAudio.Wave;
using SpotifyWolf.API;
using TagLib;

namespace SpotifyWolf
{
    public class DataPipeBase
    {
        private LameMP3FileWriter _mp3writer = null;
        private WaveFormat _mp3format = null;
        private const int _bitRate = 320000;

        private bool _isAborting = false;
        private List<byte> _output = new List<byte>();
        private bool _complete = false;

        public bool IsAborting
        {
            get { return this._isAborting; }
            set { this._isAborting = value; }
        }

        public bool Complete
        {
            get { return this._complete; }
            set { this._complete = value; }
        }

        public void Write(byte[] buffer, int start, int length)
        {
            for (int i = start; i < length; i++)
                _output.Add(buffer[i]);
        }

        public long BytesLoaded
        {
            get { return this._output.Count; }
        }

        public bool SaveToFile(string filename)
        {
            if (!this._complete)
            {
                Logging.WriteLine("Download has not completed yet!");
                return false;
            }

            try
            {
                //get these details from Session.music_delivery
                StartMP3Encoding(filename, new WaveFormat(44100, 2));

                EnqueueSamples(2, this._output.ToArray());

                StopMP3Encoding();

                return true;
            }
            catch (Exception _Exception)
            {
                // Error
                Logging.WriteLine(String.Format("Exception caught in process: {0}",
                                  _Exception.ToString()));
            }

            return false;
        }

        public void SetID3Tags(string filename, Track track)
        {
            TagLib.File file = TagLib.File.Create(filename);

            if (file != null)
            {
                if (track.Album != null && !String.IsNullOrEmpty(track.Album.Name))
                    file.Tag.Album = track.Album.Name;

                if (track.Artists != null && track.Artists.Length > 0)
                    file.Tag.Performers = track.Artists;

                if (!String.IsNullOrEmpty(track.Name))
                    file.Tag.Title = track.Name;

                if (track.TrackNumber > 0)
                    file.Tag.TrackCount = (uint)track.TrackNumber;
            }

            file.Save();
        }

        private void StartMP3Encoding(string filename, WaveFormat format)
        {
            if (this._mp3writer != null)
                this.StopMP3Encoding();

            this._mp3format = format;
            this._mp3writer = new LameMP3FileWriter(filename, format, _bitRate);
        }

        private void StopMP3Encoding()
        {
            if (this._mp3writer != null)
            {
                this._mp3writer.Dispose();
                this._mp3writer = null;
            }
        }

        private void EnqueueSamples(int channels, byte[] samples)
        {
            if (this._mp3writer != null && this._mp3format.Channels == channels)
                this._mp3writer.Write(samples, 0, samples.Length);
        }
    }
}
