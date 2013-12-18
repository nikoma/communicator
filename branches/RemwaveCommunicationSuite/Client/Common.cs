using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Remwave.Client.Serializable;

namespace Remwave.Client
{





    class LINE_STATE
    {
        private decimal _CallDuration = 0;

        public decimal CallDuration
        {
            get { return _CallDuration; }
            set { _CallDuration = value; }
        }

        private string _LastErrorMessage = "";
        public string LastErrorMessage
        {
            get { return _LastErrorMessage; }
            set { _LastErrorMessage = value; }
        }

        private string _LastDialedNumber = "";
        public string LastDialedNumber
        {
            get { return _LastDialedNumber; }
            set { _LastDialedNumber = value; }
        }

        private TELEPHONY_RETURN_VALUE _State = TELEPHONY_RETURN_VALUE.SipOnHook;
        public TELEPHONY_RETURN_VALUE State
        {
            get { return _State; }
            set { _State = value; }
        }

        private CALL_DIRECTION _CallDirection = CALL_DIRECTION.CallDirectionNone;
        public CALL_DIRECTION CallDirection
        {
            get { return _CallDirection; }
            set { _CallDirection = value; }
        }

        private bool _CallActive = false;
        public bool CallActive
        {
            get { return _CallActive; }
            set { _CallActive = value; }
        }

        private bool _CallConferenceActive = false;
        public bool CallConferenceActive
        {
            get { return _CallConferenceActive; }
            set { _CallConferenceActive = value; }
        }

        private bool _CallHoldActive = false;
        public bool CallHoldActive
        {
            get { return _CallHoldActive; }
            set { _CallHoldActive = value; }
        }

        private bool _CallRecordingActive = false;
        public bool CallRecordingActive
        {
            get { return _CallRecordingActive; }
            set { _CallRecordingActive = value; }
        }

        private bool _CallTransferActive = false;
        public bool CallTransferActive
        {
            get { return _CallTransferActive; }
            set { _CallTransferActive = value; }
        }
        private bool _VideoConferenceActive = false;
        public bool VideoConferenceActive
        {
            get { return _VideoConferenceActive; }
            set { _VideoConferenceActive = value; }
        }
        public void Tick()
        {

            if (_CallActive)
            {
                _CallDuration++;
            }
        }

        public void OnHook(List<CallRecord> myCallHistoryRecords)
        {

            try
            {
                CallRecord tmpCallRecord ;
                switch (_CallDirection)
                {
                    case CALL_DIRECTION.CallDirectionIn:
                        if (_CallDuration > 0)
                        {
                          tmpCallRecord = new CallRecord(DateTime.Now, CallStatus.CallReceived, _LastDialedNumber, _CallDuration);
                        }
                        else
                        {
                            tmpCallRecord = new CallRecord(DateTime.Now, CallStatus.CallMissed, _LastDialedNumber, _CallDuration);
                        }
                        myCallHistoryRecords.Add(tmpCallRecord);
                        break;
                    case CALL_DIRECTION.CallDirectionOut:
                        tmpCallRecord = new CallRecord(DateTime.Now, CallStatus.CallDialed, _LastDialedNumber, _CallDuration);
                        myCallHistoryRecords.Add(tmpCallRecord);
                        break;
                }
                
            }
            catch (Exception)
            {

                //throw;
            }



            _CallActive = false;
            _CallConferenceActive = false;
            _CallDirection = CALL_DIRECTION.CallDirectionNone;
            _CallHoldActive = false;
            _CallRecordingActive = false;
            _CallTransferActive = false;
            _State = TELEPHONY_RETURN_VALUE.SipOnHook;
            _CallDuration = 0;
        }

    }
}
