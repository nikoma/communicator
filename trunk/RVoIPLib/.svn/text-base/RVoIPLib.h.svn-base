// RVoIPLib.h


#pragma once

#include "stdafx.h"
#using <mscorlib.dll>


using namespace System::Runtime::InteropServices;
using namespace System;

namespace Remwave_RVoIPLib {

	typedef struct  CALL_FORWARD
	{
		char *pContact;
		BOOL NextCallForward;
		BOOL ForwardAcitve;
		char *pCallId;
	}CALL_FORWARD;

	typedef struct  DTMF_STATE
	{
		BOOL Active;
		BOOL Released;
		int Digit;
		int Duration;

	}DTMF_STATE;

	public delegate void ClientCallBack(int PhoneLine, int NotificationType, int TelephonyEvent, String ^EventMessage);

	public ref class SIPPhone
	{
		// TODO: Add your methods for this class here.
	public:
		SIPPhone();
			TELEPHONY_RETURN_VALUE SIPPhone::InitEngine(ClientCallBack ^pfn, String ^sUsername, String ^sPassword, String ^sRealm, String ^sSipProxyAddress,  String ^sLocalIP, String ^sInitData, String ^sUserAgent, BOOL bEnableSipLog, String ^sLogFilePath );

		TELEPHONY_RETURN_VALUE SIPPhone::ReStartSip(String ^LocalIP);
		TELEPHONY_RETURN_VALUE SIPPhone::StartSip();
		TELEPHONY_RETURN_VALUE SIPPhone::ConfigureSip();
		TELEPHONY_RETURN_VALUE CallOrAnswer(int PhoneLineID, String ^Destination);

		TELEPHONY_RETURN_VALUE CancelCall(int PhoneLineID);
		TELEPHONY_RETURN_VALUE ConferenceOnOffCall(int PhoneLineID);
		TELEPHONY_RETURN_VALUE HoldOnOffCall(int PhoneLineID);
		TELEPHONY_RETURN_VALUE XferCall(int PhoneLineID,String ^Destination);
		TELEPHONY_RETURN_VALUE RecOnOffCall(int PhoneLineID,String ^Directory);


		TELEPHONY_RETURN_VALUE GetPhoneLineStatus(int PhoneLineID, TELEPHONY_RETURN_VALUE LineState, CALL_DIRECTION CallDirection, bool RecordingActive );
		TELEPHONY_RETURN_VALUE GetPhoneLineState(int PhoneLineID);
		CALL_DIRECTION GetPhoneLineCallDirection(int PhoneLineID);
		int GetPhoneLineCallRecordingActive(int PhoneLineID);
		String^ GetIncomingCallDetails(int PhoneLineID);
		String^ GetOutgoingCallDetails (int PhoneLineID);
		TELEPHONY_RETURN_VALUE StartDTMF(int PhoneLineID,DTMF_TONE Tone);
		TELEPHONY_RETURN_VALUE StopDTMF(int PhoneLineID);
		TELEPHONY_RETURN_VALUE ShutdownEngine();


		TELEPHONY_RETURN_VALUE SIPPhone::SetIncomingPhoneRingEnable(BOOL bEnableState);
		TELEPHONY_RETURN_VALUE SIPPhone::SetAudioVolume(int iRxVolume, int iTxVolume);
		TELEPHONY_RETURN_VALUE SIPPhone::SetMediaFormats(int iNumMediaFormats, int iMediaFormat0, int iMediaFormat1, int iMediaFormat2, int iMediaFormat3);


		int GetFreePhoneLine();
		static ClientCallBack ^m_CallBack;
	private:
		void SIPPhone::InitializeAudioOutput(int ZerobasedAudioOutput, AUDIOHANDLE *phAudioOut);
		void SIPPhone::UnInitializeAudioOutput(AUDIOHANDLE *phAudioOut);
	
	};
}


