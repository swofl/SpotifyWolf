using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyWolf
{
    public class SpotifyHandler
    {
        public static bool Initialize()
        {
            try
            {
                Spotify.Initialize();
            }
            catch (Exception ex)
            {
                Logging.WriteLine("Could not initialize the Spotify interface. " + ex);
                return false;
            }

            Logging.WriteLine("Spotify plugin initialized successfully.");
            return true;
        }

        public static bool Login()
        {
            try
            {
                if (!Spotify.Login())
                    throw new Exception("Channel requires Spotify Premium subscription. Please log in using Jamcast Server Manager.");
                if (!Spotify.IsLoggedIn || !Spotify.IsRunning)
                    throw new ApplicationException("Spotify is not running.");
            }
            catch (Exception e)
            {
                Logging.WriteLine(e.Message);
                return false;
            }

            return true;
        }

        public static void Shutdown()
        {
            Spotify.ShutDown();
            Logging.WriteLine("Spotify was shut down successfully");
        }
    }
}
