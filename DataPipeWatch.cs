using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyWolf
{
    public class DataPipeWatch
    {
        private DataPipeBase _datapipe;
        private int _updateDelay = 500;
        private int _bitRateLossless;
        private decimal _songSeconds;

        public DataPipeWatch(DataPipeBase datapipe, decimal songSeconds)
        {
            this._datapipe = datapipe;
            this._songSeconds = songSeconds;
            this._bitRateLossless = 44100 * 16 * 2; //sampling rate (Hz) * Bits per sample (16/24) * channelsv
        }

        public void Report()
        {
            DateTime startTime = DateTime.Now;

            while (!this._datapipe.Complete && !this._datapipe.IsAborting)
            {
                Thread.Sleep(this._updateDelay);

                decimal percentageDone = ((decimal)(this._datapipe.BytesLoaded * 8.0 / _bitRateLossless) / this._songSeconds) * 100;
                string output = String.Format("\rstreamed {0} kb ({1}%)", Math.Floor(_datapipe.BytesLoaded / 1024.0), Math.Round(percentageDone, 1));

                Logging.Enabled = true;
                Logging.Write(output);
                Logging.Enabled = false;
            }

            double difference = Math.Round((DateTime.Now - startTime).TotalSeconds);

            Logging.Enabled = true;
            Logging.Write(String.Format(" - took {0}s", difference));
            Logging.Enabled = false;
        }
    }
}
