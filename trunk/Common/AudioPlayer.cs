using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.AudioVideoPlayback;

namespace Remwave.Client
{
    class AudioPlayer
    {
        Audio mAudioPlayer = null;
        internal void Play(string fileName)
        {
            if (mAudioPlayer != null) mAudioPlayer.Dispose();
            mAudioPlayer = new Audio(fileName, true);
        }

        internal void Stop()
        {
            if (mAudioPlayer != null) mAudioPlayer.Dispose();
        }

        internal double CurrentPosition
        {
            get{ return mAudioPlayer.CurrentPosition;}    
            set{ mAudioPlayer.CurrentPosition = value;}    
        }

        internal double Duration
        {
            get { return mAudioPlayer.Duration; }
        }
         internal bool Disposed
        {
            get { return mAudioPlayer == null; }
        }
        
    }
}
