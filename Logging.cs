using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SpotifyWolf
{
    public static class Logging
    {
        private static bool _console = false;
        private static bool _timestamps = false;
        private static bool _enabled = true;

        public static void EnableConsole()
        {
            if (!_console)
            {
                _console = true;

                TraceListener t = new ConsoleTraceListener();

                Trace.Listeners.Add(t);
            }
        }

        public static void EnableText(string filename)
        {
            TextWriterTraceListener t = new TextWriterTraceListener(filename);

            Trace.Listeners.Add(t);

            if (AutoFlush == false)
                AutoFlush = true;
        }

        public static void Write(string text)
        {
            if (_enabled)
            {
                if (_timestamps)
                    text = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + ": " + text;

                Trace.Write(text);
            }
        }

        public static void WriteLine(string message)
        {
            Write(message + Environment.NewLine);
        }

        public static bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public static bool UseTimestamps
        {
            get { return _timestamps; }
            set { _timestamps = value; }
        }

        public static bool AutoFlush
        {
            get { return Trace.AutoFlush; }
            set { Trace.AutoFlush = value; }
        }
    }
}
