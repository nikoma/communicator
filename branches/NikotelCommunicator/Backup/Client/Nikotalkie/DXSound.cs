using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.DirectX.DirectSound;

using System.Windows.Forms;

namespace Remwave.Client
{
    public class DXSound
    {

        //player
        SecondaryBuffer mSecondaryBuffer;
        WaveFormat mWaveFormat = new WaveFormat();
        Device mDevice = new Device();

        //recording
        CaptureBuffer mCaptureBuffer;

        public MemoryStream CapturedStream
        {
            get {
                  MemoryStream capturedStream = null;
                  if (mCaptureBuffer != null)
                  {
              capturedStream = new MemoryStream();
                int currentCapturePosition = 0;
                int currentReadPosition = 0;
                mCaptureBuffer.GetCurrentPosition(out currentCapturePosition, out currentReadPosition);
                mCaptureBuffer.Read(0, capturedStream, currentCapturePosition, LockFlag.None);
                }
                return capturedStream;
            }
           
        }

        public  DXSound(UserControl owner)
        {
            mDevice.SetCooperativeLevel(owner, CooperativeLevel.Normal);

            mWaveFormat.BitsPerSample = 16;
            mWaveFormat.Channels = 1;
            mWaveFormat.BlockAlign = 2;

            mWaveFormat.FormatTag = WaveFormatTag.Pcm;
            mWaveFormat.SamplesPerSecond = 8000; //sampling frequency of your data;   
            mWaveFormat.AverageBytesPerSecond = mWaveFormat.SamplesPerSecond * mWaveFormat.BlockAlign;
        }

        public void StartPlaying(MemoryStream buffer)
        {
            if (buffer == null) return;
            buffer.Position = 0;

            if (mSecondaryBuffer != null) mSecondaryBuffer.Dispose();

            // buffer description         
            BufferDescription bufferDescription = new BufferDescription(mWaveFormat);
            bufferDescription.DeferLocation = true;
            bufferDescription.BufferBytes = Convert.ToInt32(buffer.Length);
            mSecondaryBuffer = new SecondaryBuffer(bufferDescription, mDevice);

            //load audio samples to secondary buffer
            mSecondaryBuffer.Write(0, buffer, Convert.ToInt32( buffer.Length), LockFlag.EntireBuffer);

            //play audio buffer			
            mSecondaryBuffer.Play(0, BufferPlayFlags.Default);
        }

        public void StopPlaying()
        {
            if (mSecondaryBuffer == null) return;
            if (mSecondaryBuffer.Status.Playing)
            {
                mSecondaryBuffer.Stop();
            }
        }


        public void StartRecording(int deviceIndex)
        {
            if (mCaptureBuffer != null)
            {
                if (mCaptureBuffer.Capturing)
                {
                    mCaptureBuffer.Stop();
                }

                mCaptureBuffer.Dispose();
                mCaptureBuffer = null;
            }

            CaptureDevicesCollection audioDevices = new CaptureDevicesCollection();
            if (deviceIndex != -1 && deviceIndex <  audioDevices.Count-1)
            {
                // initialize the capture buffer and start the animation thread
                Capture capture = new Capture(audioDevices[deviceIndex].DriverGuid);
                CaptureBufferDescription captureBufferDescription = new CaptureBufferDescription();
                WaveFormat waveFormat = new WaveFormat();
                waveFormat.BitsPerSample = 16;
                waveFormat.SamplesPerSecond = 8000;
                waveFormat.Channels = 1;
                waveFormat.BlockAlign = (short)(waveFormat.Channels * waveFormat.BitsPerSample / 8);
                waveFormat.AverageBytesPerSecond = waveFormat.BlockAlign * waveFormat.SamplesPerSecond;
                waveFormat.FormatTag = WaveFormatTag.Pcm;

                captureBufferDescription.Format = waveFormat;
                captureBufferDescription.BufferBytes = waveFormat.SamplesPerSecond * 120;

                mCaptureBuffer = new Microsoft.DirectX.DirectSound.CaptureBuffer(captureBufferDescription, capture);
                mCaptureBuffer.Start(true);

            }
        }

        public void StopRecording()
        {
            if (mCaptureBuffer != null)
            {
                if (mCaptureBuffer.Capturing)
                {
                    mCaptureBuffer.Stop();
                }
            }
        }

    }
}
