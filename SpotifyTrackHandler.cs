///*-
// * Copyright (c) 2014 Software Development Solutions, Inc.
// * All rights reserved.
// *
// * Redistribution and use in source and binary forms, with or without
// * modification, are permitted provided that the following conditions
// * are met:
// * 1. Redistributions of source code must retain the above copyright
// *    notice, this list of conditions and the following disclaimer.
// * 2. Redistributions in binary form must reproduce the above copyright
// *    notice, this list of conditions and the following disclaimer in the
// *    documentation and/or other materials provided with the distribution.
// *
// * THIS SOFTWARE IS PROVIDED BY THE AUTHOR AND CONTRIBUTORS ``AS IS'' AND
// * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
// * ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR OR CONTRIBUTORS BE LIABLE
// * FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS
// * OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
// * HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
// * LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY
// * OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF
// * SUCH DAMAGE.
// */

//using System;

//using SpotifyWolf.API;

//namespace SpotifyWolf
//{
//    public class SpotifyTrackHandler : MediaRequestHandler
//    {
//        private IntPtr _trackPtr;

//        public override RequestInitializationResult Initialize()
//        {
//            AudioRequestInitializationResult result = new AudioRequestInitializationResult();
//            if (!Spotify.Login())
//            {
//                result.CanProceed = false;
//                return result;
//            }

//            try
//            {
//                var track = new Track(Context.Data[0]);
//                _trackPtr = track.TrackPtr;
//            }
//            catch
//            {
//                throw new MediaNotFoundException("Track not found.");
//            }

//            result.InputProperties = new AudioStreamProperties(MediaFormats.LPCM);
//            result.CanProceed = true;
//            result.IsConversion = this.Context.Format != MediaFormats.LPCM;
//            result.SupportsSeeking = false;

//            return result;
//        }

//        public override DataPipeBase RetrieveMedia()
//        {
//            return new SpotifyTrackDataPipe(_trackPtr);
//        }
//    }
//}