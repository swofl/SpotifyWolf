using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using libspotifydotnet;
using SpotifyWolf.API;
using NAudio.Lame;

namespace SpotifyWolf
{
    class Program
    {
        static byte[] appKey = {
            0x01, 0x3C, 0xC6, 0xDC, 0x53, 0x45, 0xD6, 0xE6, 0x95, 0x00, 0x9E, 0xF4, 0x70, 0xBA, 0xCC, 0xD6,
            0x8E, 0x27, 0x70, 0x14, 0xD4, 0x3E, 0xBF, 0xE3, 0x69, 0x5F, 0x92, 0xB7, 0xE7, 0xD8, 0x6E, 0xE9,
            0x36, 0xB2, 0x46, 0xE1, 0x7C, 0xB2, 0x0C, 0x62, 0x63, 0x6E, 0x55, 0x8D, 0x08, 0x7D, 0x8A, 0x78,
            0xEF, 0x6B, 0x6C, 0xA2, 0xA9, 0x03, 0x26, 0x23, 0xE6, 0x76, 0xB9, 0x50, 0x40, 0x71, 0x97, 0x78,
            0x93, 0x4B, 0xBE, 0xE8, 0x11, 0x6E, 0x02, 0xE6, 0xA6, 0x71, 0x97, 0xEC, 0x55, 0x71, 0x98, 0x0E,
            0xEC, 0xBA, 0x51, 0x5B, 0x14, 0xD7, 0x3B, 0x75, 0xE3, 0x51, 0x70, 0x8C, 0x1E, 0x39, 0xF5, 0x3B,
            0xC4, 0xD1, 0x5B, 0x96, 0xB3, 0x1F, 0x5A, 0x4D, 0x14, 0x90, 0xE8, 0xAE, 0x76, 0xB5, 0x0B, 0x9A,
            0xC9, 0x0E, 0xA1, 0xBA, 0xCD, 0x50, 0xF0, 0x99, 0xA5, 0xE7, 0xF0, 0xDE, 0xF6, 0x24, 0x40, 0xE1,
            0x11, 0x27, 0x65, 0xAD, 0xB5, 0x84, 0x6D, 0xE3, 0x91, 0x77, 0xB7, 0xD4, 0x33, 0xDF, 0x3A, 0x84,
            0xBD, 0x7F, 0xD6, 0xDD, 0x06, 0xB4, 0xB0, 0x14, 0xC7, 0x2E, 0xDD, 0xD9, 0xFD, 0xC2, 0x8E, 0xE7,
            0x5C, 0x3E, 0x89, 0x22, 0x46, 0x29, 0xF1, 0xB7, 0x87, 0xEF, 0x83, 0x3B, 0x44, 0x0E, 0xDF, 0x8E,
            0xB0, 0xEB, 0xB5, 0xF9, 0x55, 0x1C, 0x78, 0xD4, 0x54, 0x23, 0xA8, 0x2D, 0x8B, 0x7D, 0x97, 0x81,
            0xFA, 0xCE, 0xA9, 0x38, 0x6C, 0x05, 0xC7, 0x5F, 0xD5, 0x5B, 0x49, 0x60, 0x13, 0x9E, 0xF3, 0x5C,
            0xF3, 0x3E, 0x76, 0xEC, 0x53, 0xEC, 0xB9, 0x06, 0x58, 0xC9, 0x0D, 0x80, 0x22, 0x99, 0xC7, 0x38,
            0xE2, 0xD4, 0xEE, 0x3D, 0x3B, 0x40, 0xF2, 0x0C, 0xB1, 0x71, 0x74, 0x29, 0x53, 0xB7, 0x69, 0x2E,
            0x25, 0xA7, 0xDF, 0xA6, 0x8E, 0x46, 0xE5, 0x96, 0x41, 0x80, 0x90, 0x64, 0xEB, 0x19, 0x11, 0x9E,
            0x06, 0x7B, 0xDF, 0xAA, 0x42, 0x81, 0x5A, 0x73, 0x77, 0x6D, 0x45, 0x15, 0x94, 0xA3, 0x1D, 0x4C,
            0xBC, 0xA0, 0x18, 0x70, 0x0B, 0xA0, 0xCA, 0x9A, 0x90, 0xA4, 0x19, 0xF4, 0xD0, 0x34, 0xDF, 0x94,
            0xCB, 0x40, 0x66, 0xAA, 0xBF, 0x77, 0x41, 0xB9, 0x71, 0xD9, 0x54, 0xE4, 0x96, 0xD4, 0x7A, 0x9F,
            0x19, 0x7B, 0xC7, 0x95, 0xA4, 0xAE, 0x8B, 0x43, 0x65, 0xB2, 0x2A, 0x87, 0x0E, 0xCD, 0x40, 0x6B,
            0x82
        };

        static void Main(string[] args)
        {
            Logging.EnableConsole();
            Logging.Enabled = true;

            Configuration.Instance.ApplicationKey = appKey;

            if (!Directory.Exists(Configuration.Instance.OutputFolder))
                Directory.CreateDirectory(Configuration.Instance.OutputFolder);
            
            SpotifyHandler.Initialize();

            while (SpotifyHandler.Login() == false)
                Thread.Sleep(100);

            Logging.WriteLine("waiting for Spotify to finish background initialization...");
            Thread.Sleep(30000);
            Logging.WriteLine("that's enough. our turn.");

            List<Playlist> playlists = new List<Playlist>();

            try
            {
                Playlist starred = Spotify.GetStarredPlaylist();
                starred.SetName("Starred");
                playlists.Add(starred);
            }
            catch (Exception e)
            {
                Logging.WriteLine("starred playlist error " + e.Message);
            }

            foreach (PlaylistContainer.PlaylistInfo playlist in Spotify.GetPlaylists())
            {
                if ((int)playlist.Pointer == 0)
                    continue;

                try
                {
                    string playlistLink = Spotify.GetPlaylistLink(playlist.Pointer);

                    Playlist pl = Spotify.GetPlaylist(playlistLink, true);

                    while (!pl.IsLoaded)
                        Thread.Sleep(100);

                    playlists.Add(pl);
                }
                catch (Exception e)
                {
                    Logging.WriteLine("error " + e.Message);
                }
            }

            PrintOptions(playlists);

            string input = Console.ReadLine();

            while (input != "q")
            {
                int entry = -1;

                if (Int32.TryParse(input, out entry) && entry < playlists.Count)
                {
                    DownloadPlaylist(playlists[entry]);
                    PrintOptions(playlists);
                }
                else
                {
                    Console.WriteLine("invalid input");
                }

                input = Console.ReadLine();
            }

            Logging.WriteLine("All done, quitting..");

            SpotifyHandler.Shutdown();
        }

        static void PrintOptions(List<Playlist> playlists)
        {
            Logging.WriteLine(" ");
            Logging.WriteLine("Welche Playlist soll heruntergeladen werden?");
            Logging.WriteLine(" ");

            for (int i = 0; i < playlists.Count; i++)
            {
                Logging.WriteLine(String.Format("[{0}] {1}", i, String.IsNullOrEmpty(playlists[i].Name) ? "UNKNOWN" : playlists[i].Name));
            }

            Logging.WriteLine("[q] - quit");
        }

        static void DownloadPlaylist(Playlist pl)
        {
            string playlistPath = Path.Combine(Configuration.Instance.OutputFolder, String.IsNullOrEmpty(pl.Name) ? "UNKNOWN" : pl.Name);

            if (!Directory.Exists(playlistPath))
            {
                Logging.WriteLine(String.Format("Creating playlist folder {0}", String.IsNullOrEmpty(pl.Name) ? "UNKNOWN" : pl.Name));
                Directory.CreateDirectory(playlistPath);
            }

            Logging.WriteLine(String.Format("Scanning playlist {0}", String.IsNullOrEmpty(pl.Name) ? "UNKNOWN" : pl.Name));
            List<Track> tracks = pl.GetTracks();

            for (int i = 1; i <= tracks.Count; i++)
            {
                Track t = tracks[i - 1];
                
                string filename = PathSanitizer.SanitizeFilename(String.Format("{0} - {1}.mp3", t.Artists[0], t.Name), '_').Replace("_", "");
                string filepath = Path.Combine(playlistPath, filename);

                Logging.WriteLine(String.Format("Downloading Song {2}/{3}: {0} - {1}",
                    t.Artists.Length > 0 ? t.Artists[0] : "UNKNOWN",
                    String.IsNullOrEmpty(t.Name) ? t.TrackPtr.ToString() : t.Name,
                    i,
                    tracks.Count));

                if (File.Exists(filepath))
                {
                    Logging.WriteLine("already exists, skipping");
                    continue;
                }

                Logging.Enabled = false;

                try
                {
                    SpotifyTrackDataPipe download = new SpotifyTrackDataPipe(t.TrackPtr);

                    DataPipeWatch watch = new DataPipeWatch(download, t.Seconds);

                    Thread watcher = new Thread(new ThreadStart(watch.Report));
                    watcher.Start();

                    download.FetchData();

                    watcher.Join();

                    Logging.Enabled = true;
                    Logging.WriteLine(" ");

                    if (download.Complete)
                    {
                        download.SaveToFile(filepath);
                        download.SetID3Tags(filepath, t);
                    }
                }
                catch (Exception e)
                {
                    Logging.Enabled = true;

                    Logging.WriteLine("Download error " + e.Message);
                }
            }
        }
    }
}
