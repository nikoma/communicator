using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;

namespace Remwave.Client.Events
{
    public enum ClientEvent
    {
        IncomingInstantMessage,
        IncomingVoiceMailMessage,
        IncomingCallStartRinging,
        IncomingCallStopRinging,
        IncomingCallNocking,
        IncomingCallStop,
        IncomingNudge
    }

    public enum EventNotificationType
    {
        Sound,
        Flash,
        RingingStart,
        RingingStop,
        None
    }

    public struct EventNotification
    {
        public ClientEvent Event;
        public EventNotificationType NotificationType;
        public string NotificationData;
    }

    public class ClientEvents
    {
        //AudioPlayback
        private AudioPlayer mAudioPlayer = null;
        private AudioPlayer mRingTonePlayer = null;

        //Events notifications collection
        private Hashtable eventNotifications = new Hashtable();

        private Timer mEventTimer = null;


        public ClientEvents()
        {
            mEventTimer = new Timer(1000);
            mEventTimer.Tick += new Timer.TimeElapsed(mEventTimer_Tick);
        }

        protected void Dispose()
        {
            mEventTimer.Stop();
            mEventTimer = null;
        }

        void mEventTimer_Tick()
        {
            if (mRingTonePlayer != null && !mRingTonePlayer.Disposed)
            {
                if (mRingTonePlayer.CurrentPosition >= mRingTonePlayer.Duration) mRingTonePlayer.CurrentPosition = 0;
            }
        }

        public class Timer
        {
            public delegate void TimeElapsed();
            public event TimeElapsed Tick;
            private int interval;
            private bool isRunning = false;
            public Timer(int msec)
            {
                setInterval(msec);
            }

            public void setInterval(int msec)
            {
                this.interval = msec;
            }

            public void Start()
            {
                if (!isRunning)
                {
                    isRunning = true;
                    new System.Threading.Thread(ini).Start();
                }
            }

            public void Stop()
            {
                isRunning = false;
            }

            private void ini()
            {
                while (isRunning)
                {
                    System.Threading.Thread.Sleep(interval);
                    Tick.Invoke();
                }
            }

            protected void Dispose()
            {
                isRunning = false;
            }

        }


        public void RaiseEvent(ClientEvent clientEvent)
        {
            try
            {
                if (eventNotifications[clientEvent] != null)
                {
                    EventNotification eventNotification = (EventNotification)eventNotifications[clientEvent];

                    switch (eventNotification.NotificationType)
                    {
                        case EventNotificationType.Sound:
                            PlayAudioFile(eventNotification.NotificationData);
                            break;
                        case EventNotificationType.Flash:
                            break;
                        case EventNotificationType.None:
                            break;
                        case EventNotificationType.RingingStart:
                            PlayRingTone(eventNotification.NotificationData);
                            break;
                        case EventNotificationType.RingingStop:
                            StopPlayingRingTone();
                            break;
                        default:
                            break;
                    }
                }


            }
            catch (Exception ex)
            {
#if (TRACE)
                Console.WriteLine("RaiseEvent Exception : " + ex.Message);
#endif
            }
        }



        public void AddEventNotification(EventNotification eventNotification)
        {
            if (eventNotifications.ContainsKey(eventNotification.Event)) eventNotifications.Remove(eventNotification.Event);
            eventNotifications.Add(eventNotification.Event, eventNotification);
        }

        private void PlayRingTone(string fileName)
        {

            if (mRingTonePlayer == null)
            {
                mRingTonePlayer = new AudioPlayer();

            }
            else
            {
                mRingTonePlayer.Stop();
            }
            if (!File.Exists(fileName)) return;
            mRingTonePlayer.Play(fileName);
            mEventTimer.Start();
        }


        private void StopPlayingRingTone()
        {
            mEventTimer.Stop();
            if (mRingTonePlayer != null) mRingTonePlayer.Stop();
        }

        private void PlayAudioFile(string fileName)
        {
            if (mAudioPlayer == null)
            {
                mAudioPlayer = new AudioPlayer();
            }
            else
            {
                mAudioPlayer.Stop();
            }
            if (!File.Exists(fileName)) return;
            mAudioPlayer.Play(fileName);

        }


        internal void StopEvents()
        {
            mEventTimer.Stop();
        }
    }
}
