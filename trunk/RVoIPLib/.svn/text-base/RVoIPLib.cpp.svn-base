// This is the main DLL file.

#include "stdafx.h"
#include "RVoIPLib.h"
#include "IpAddress.h"
#include "C:\Program Files\LanScape\VOIP Media Engine\5.12\Software Examples\Microcode\LanScapeVME.C"


#define MAX_PHONE_LINES_SUPPORTED		4
#define DEFAULT_PHONE_LINE_VOLUME		512
#define CALL_TERMINATE_TIMEOUT_MS		1000
#define MAX_REGISTRATION_TRIES			16
#define SIP_PORT						5060

namespace Remwave_RVoIPLib
{

	BYTE *pPersonalityMicrocode = LanScapeVME_F186495C_7AFF_4742_ADB7_87EA48A42633;
	SIPHANDLE hSipEngine = 0;
	START_SIP_TELEPHONY_PARAMS StartupParams;	
	IVRTXHANDLE hIvrTransmiters[MAX_PHONE_LINES_SUPPORTED];
	int SamplesPerTxIvrBuffer[MAX_PHONE_LINES_SUPPORTED];
	int BytesPerTxIvrBuffer[MAX_PHONE_LINES_SUPPORTED];
	HDTMFGENERATOR hDtmfGenerators[MAX_PHONE_LINES_SUPPORTED];
	CALL_FORWARD CallForwards[MAX_PHONE_LINES_SUPPORTED];
	DTMF_STATE DtmfState[MAX_PHONE_LINES_SUPPORTED];

	// Audio output related data.
	//
	AUDIOHANDLE hAudioOut = 0;						// the handle to the local audio output.

	int SamplesPerAudioOutBuffer = 0;				// the number of samples per audio output buffer.

	int BytesPerAudioOutBuffer = 0;					// the number of bytes per audio output buffer.


	//global sip configuration
	char *pConfigUsername;
	char *pConfigPassword;
	char *pConfigRealm;
	char *pConfigSipProxyAddress;
	char *pConfigUserAgent;
	char *pConfigLocalIP;
	char *pConfigLogFilePath;

	int validSipPort = 0;

	//Global Settings
	int SettingsRxVolume = 0;
	int SettingsTxVolume = 0;
	MEDIA_FORMAT_AUDIO SettingsMediaFormats[6];

	int SettingsNumMediaFormats=0;
	BOOL SettingEnableIncomingPhoneRing = TRUE;



	void __stdcall  DTMFCallbackProc(DTMF_GEN_DATA *pGeneratorData)
	{
		TELEPHONY_RETURN_VALUE status;
		status = 	TransmitInCallIvrData(hIvrTransmiters[0],pGeneratorData->pSampleBuffer);
		//		Console::WriteLine("SIP TransmitInCallIvrData : "+gcnew String(GetTelephonyStatusString(status)));
		status = WriteAudioOutData(hAudioOut,pGeneratorData->pSampleBuffer);
		//		Console::WriteLine("SIP WriteAudioOutData : "+gcnew String(GetTelephonyStatusString(status)));
	};

	void __stdcall receiveAudioCallback(IVR_RECOGNITION_DATA *pIvrRecognitionData)
	{
		const int id = pIvrRecognitionData->PhoneLine;
	}

	static void __stdcall receiveRtpCallback(RAW_RTP_DATA *pRawRtpData){

	static bool lineDtmfDown[MAX_PHONE_LINES_SUPPORTED] = {false};
	static unsigned int lineDtmfSendTimeStamp[MAX_PHONE_LINES_SUPPORTED] = {false};

	//Once we get some rtp off a line, make stopure it is marked as such
	//Some clients send RTCP header from different RTP port, should detect this, for now don't set here
	//((Engine*)pRawRtpData->pUserData)->mConnections[pRawRtpData->PhoneLine].setRtpConnectionChanged();
	int line = pRawRtpData->PhoneLine;

	//Make sure these corespond, or it is a crazy packet
	if(pRawRtpData->RtpPacketLengthInBytes != (pRawRtpData->RtpHeaderLengthInBytes + pRawRtpData->SampleBufferLengthInBytes))
		pRawRtpData->ProcessRtpPacket = FALSE;

	if(pRawRtpData->TransmittingPacket == TRUE)
	{
	
		if(DtmfState[line].Active)
		{
			DtmfState[line].Active = false;
			Console::WriteLine("SIP : receiveRtpCallback" );
			if(DtmfState[line].Released) {
				Console::WriteLine("SIP : receiveRtpCallback : Up | Line " + pRawRtpData->PhoneLine + " | Digit " + DtmfState[line].Digit);
			}
			else
			{
				Console::WriteLine("SIP : receiveRtpCallback : Down | Line " + pRawRtpData->PhoneLine + " | Digit " + DtmfState[line].Digit);
			}
			//Manipulate RTP Data:			
			if(DtmfState[line].Duration == 320)	//Only send one timestamp for whole duration
				lineDtmfSendTimeStamp[pRawRtpData->PhoneLine] = pRawRtpData->pRtpHeader->TimeStamp;

			pRawRtpData->pRtpHeader->TimeStamp = lineDtmfSendTimeStamp[pRawRtpData->PhoneLine];
			pRawRtpData->pRtpHeader->PayloadType = 101;	
			((unsigned char*)pRawRtpData->pSampleBuffer)[0] = DtmfState[line].Digit;
			((unsigned char*)pRawRtpData->pSampleBuffer)[1] = (DtmfState[line].Released ? 0x80 : 0) | 0x0A; /*volume*/
			//Little endian to big endian - x8086 specific
			((unsigned char*)pRawRtpData->pSampleBuffer)[2] = DtmfState[line].Duration >> 8; //Get top byte
			((unsigned char*)pRawRtpData->pSampleBuffer)[3] = DtmfState[line].Duration & 0x00FF; //only bottom byte

			pRawRtpData->NewRtpBufferLengthInBytes = (pRawRtpData->RtpPacketLengthInBytes - pRawRtpData->SampleBufferLengthInBytes) + 4;
			DtmfState[line].Released = false;
		}
	}
	}

	void _stdcall TelephonyEngineCallback(SIPHANDLE hStateMachine, int NotifyType, int PhoneLineID, int TelephonyEvent, void *pUserDefinedData, void *pEventData)
	{
		try	{
			String ^EventMessage;
			TELEPHONY_RETURN_VALUE status;
			SIP_OUTGOING_CALL_ERROR_INFO OutgoingCallErrorInfo;
			SIP_OUTGOING_CALL_INFO OutgoingCallInfo;
			SIP_INCOMING_CALL_INFO IncomingCallInfo;

			//Debug Messages

			if(NotifyType==0)
			{
				switch (TelephonyEvent)
				{
				case TELEPHONY_RETURN_VALUE::SipCallRecordComplete:
					EventMessage = gcnew String((char *)pEventData);
					break;
				case TELEPHONY_RETURN_VALUE::SipModifySipMessage:
					SIP_MESSAGE_IMMEDIATE_DATA *pSipMessageImmediateData;
					char *pOldSipMessage;
					char *pNewSipMessage;

					// By processing this immediate event, application software
					// can add or remove information from the raw SIP message.
					// The SIP message data passed via this event is simple ascii
					// text. It is the application's responsibility to parse the SIP
					// message properly and modify it as appropriate.
					//

					// access the event data.
					pSipMessageImmediateData = (SIP_MESSAGE_IMMEDIATE_DATA *)pEventData;
					pOldSipMessage = *(pSipMessageImmediateData->ppSipMsg);
					String ^sOldSipMessage = gcnew String(pOldSipMessage);



					String ^sCallId = "";
					for	(int i=0;i<MAX_PHONE_LINES_SUPPORTED;i++)
					{
						sCallId = gcnew String(CallForwards[i].pCallId);
						if ( sCallId != "" && sOldSipMessage->Contains(sCallId) )
						{
							PhoneLineID = i;
							break;
						}
					}

					if(pSipMessageImmediateData->Received)
					{
						//	Console::WriteLine("-->>----Received---------------");
						if (PhoneLineID>=0 && sOldSipMessage->Contains("SIP/2.0 302 Moved Temporarily"))
						{   
							//302 Moved Temporarily Detected
							pSipMessageImmediateData->IgnoreSipMessage=1;	

							//Last Contact
							String ^sContact = gcnew String(CallForwards[PhoneLineID].pContact);
							//Current Contact
							String ^sContactTag = sOldSipMessage->Substring(sOldSipMessage->IndexOf("Contact:")+14);
							sContactTag = sContactTag->Substring(0,sContactTag->IndexOf("@"));			


							if (( CallForwards[PhoneLineID].NextCallForward==FALSE && CallForwards[PhoneLineID].ForwardAcitve==FALSE ) || (sContactTag!=sContact))
							{
								//Set Forwarding Flag and Original CallID
								CallForwards[PhoneLineID].NextCallForward = TRUE;																													
								CallForwards[PhoneLineID].pContact =  (char*)(void*)Marshal::StringToHGlobalAnsi(sContactTag);
								SIPPhone::m_CallBack(PhoneLineID,2,(int)TELEPHONY_RETURN_VALUE::SipReceived302MovedTemporarily,sContactTag);									
							}
							return;		
						}
					}
					else
					{
						//	Console::WriteLine("--<<----Sending----------------");
						String ^sNewUserAgent = gcnew String(pConfigUserAgent);

						//Change Agent
						sOldSipMessage=sOldSipMessage->Replace(
							"User-Agent: LanScape VOIP Media Engine/5.12.8.1 (www.LanScapeCorp.com)",
							sNewUserAgent);

						sOldSipMessage=sOldSipMessage->Replace(
							"User-Agent: LanScape VOIP Media Engine/5.12.7.21 (www.LanScapeCorp.com)",
							sNewUserAgent);

						pNewSipMessage = (char*)(void*)Marshal::StringToHGlobalAnsi(sOldSipMessage);

						// now modify the SIP message the Media Engine will process.
						status = ModifySipMessage(
							hStateMachine,							// handle to the Media Engine.
							pSipMessageImmediateData->ppSipMsg,		// the original SIP message.
							pNewSipMessage					// the new SIP message
							);

						if(status != TELEPHONY_RETURN_VALUE::SipSuccess)
						{
							// handle the error.
						}
					}

					break;
				}
			}

			if(NotifyType == 2)
			{

				switch(TelephonyEvent)
				{
				case TELEPHONY_RETURN_VALUE::SipFarEndError:
					GetOutgoingCallErrorInfo(hSipEngine,PhoneLineID,&OutgoingCallErrorInfo);
					//				Console::Write("SIP Outgoing Call Error "+PhoneLineID.ToString()+": ("+OutgoingCallErrorInfo.ResponseCode+") "+gcnew String(OutgoingCallErrorInfo.pResponseReasonPhrase));
					EventMessage = OutgoingCallErrorInfo.ResponseCode.ToString()+" - "+gcnew String(OutgoingCallErrorInfo.pResponseReasonPhrase);
					//TerminateCall(hSipEngine,PhoneLineID,FALSE,CALL_TERMINATE_TIMEOUT_MS);
					break;
				case TELEPHONY_RETURN_VALUE::SipInCall:  //TELEPHONY_RETURN_VALUE::SipInCall;
					if(OpenTxIvrChannel(hSipEngine,PhoneLineID,0,&(hIvrTransmiters[PhoneLineID])) == TELEPHONY_RETURN_VALUE::SipSuccess)
					{
						if(SetTxIvrDataType(hIvrTransmiters[PhoneLineID],AUDIO_BANDWIDTH::AUDIO_BW_PCM_8K) == TELEPHONY_RETURN_VALUE::SipSuccess)
						{
							/*
							// get the number of samples in an ivr buffer.
							GetTxIvrSampleBlockSize(
							hIvrTransmiters[PhoneLineID],
							AUDIO_BANDWIDTH::AUDIO_BW_PCM_8K,
							&(SamplesPerTxIvrBuffer[PhoneLineID]),
							&(BytesPerTxIvrBuffer[PhoneLineID])
							);
							status = GetNumAudioOutBuffers(hAudioOut,&NumAudioOutBuffers);
							*/
						}
					}
					break;
				case TELEPHONY_RETURN_VALUE::SipCallComplete: //TELEPHONY_RETURN_VALUE::SipCallComplete;
					status = CloseTxIvrChannel(hIvrTransmiters[PhoneLineID]);
					//				Console::Write("SIP "+PhoneLineID.ToString()+": CloseTxIvrChannel - ");
					//				Console::WriteLine(gcnew String(GetTelephonyStatusString(status)));
					break;


					//set call id on incoming and outgoing call events
					//Outgoing
				case TELEPHONY_RETURN_VALUE::SipOutgoingCallStart :
				case TELEPHONY_RETURN_VALUE::SipTransferingCall :

					status =  GetOutgoingCallInfo(hSipEngine,PhoneLineID, &OutgoingCallInfo);
					CallForwards[PhoneLineID].pCallId = (char*)(void*)Marshal::StringToHGlobalAnsi(gcnew String(OutgoingCallInfo.pCallId));
					break;
					//
					//Incoming
				case TELEPHONY_RETURN_VALUE::SipIncomingCallStart:

					status =   GetIncomingCallInfo(hSipEngine,PhoneLineID,&IncomingCallInfo);
					CallForwards[PhoneLineID].pCallId = (char*)(void*)Marshal::StringToHGlobalAnsi(gcnew String(IncomingCallInfo.pCallId));
					break;

				case TELEPHONY_RETURN_VALUE::SipOnHook:
					CallForwards[PhoneLineID].ForwardAcitve = FALSE;
					break;
				}
			}
			SIPPhone::m_CallBack(PhoneLineID,NotifyType,TelephonyEvent,EventMessage);		
		}	
		catch(...){
			return;
		}

	}


	SIPPhone::SIPPhone()
	{
		//Defaults
		SettingsMediaFormats[0]= MEDIA_FORMAT_AUDIO::Media_Format_G729;
		SettingsMediaFormats[1]= MEDIA_FORMAT_AUDIO::Media_Format_iLBC_30Ms;
		SettingsMediaFormats[2]= MEDIA_FORMAT_AUDIO::Media_Format_uLaw8k;
		SettingsMediaFormats[3]= MEDIA_FORMAT_AUDIO::Media_Format_aLaw8k;
		SettingsNumMediaFormats=4;

		SettingsRxVolume = DEFAULT_PHONE_LINE_VOLUME;
		SettingsTxVolume = DEFAULT_PHONE_LINE_VOLUME;

	}


	TELEPHONY_RETURN_VALUE SIPPhone::ReStartSip(String ^LocalIP)
	{
		TELEPHONY_RETURN_VALUE status;
		CIpAddress IpAddress;
		IpAddress.IsIpAddress((char*)(void*)Marshal::StringToHGlobalAnsi(LocalIP),StartupParams.IpAddressOfThisHost);
		status = ReStartSipTelephony(&StartupParams,&hSipEngine);
		if (status != TELEPHONY_RETURN_VALUE::SipSuccess) return status;

		status = SIPPhone::ConfigureSip();
		return status;
	}

	TELEPHONY_RETURN_VALUE SIPPhone::StartSip()
	{
		TELEPHONY_RETURN_VALUE status;
		status = StartSipTelephony(&StartupParams,&hSipEngine);
		if( status !=  TELEPHONY_RETURN_VALUE::SipSuccess) status = ReStartSipTelephony(&StartupParams,&hSipEngine);


		return status;
	}

	TELEPHONY_RETURN_VALUE SIPPhone::ConfigureSip()
	{
		Console::WriteLine("Realm:" + gcnew String(pConfigRealm));
		Console::WriteLine("ProxyAddress:" + gcnew String(pConfigSipProxyAddress));
		Console::WriteLine("Username:" + gcnew String(pConfigUsername));
		Console::WriteLine("Password:" + gcnew String(pConfigPassword));








		TELEPHONY_RETURN_VALUE status;
		SetInCallProcessPriority(hSipEngine,IN_CALL_PROCESS_PRIORITY::PriorityRealtime);

		// enble noise discrimination.
		SetSilenceDecay(hSipEngine,300);
		SetNoiseThreshold(hSipEngine,200);
		SetNoiseDiscriminationEnableState(hSipEngine,TRUE);
		status = EnableIncomingPhoneRing(hSipEngine,SettingEnableIncomingPhoneRing);

		for(int PhoneLine=0;PhoneLine<MAX_PHONE_LINES_SUPPORTED;PhoneLine++)
		{


			AUDIO_BANDWIDTH TelephonyChannelBandwidth = AUDIO_BANDWIDTH::AUDIO_BW_UNDEFINED;
			// set values as appropriate.
			switch(TelephonyChannelBandwidth)
			{
			case AUDIO_BANDWIDTH::AUDIO_BW_ULAW_8K:
				// activate 8Khz uLaw channel support.
				SetAudioMediaFormat(hSipEngine,PhoneLine,MEDIA_FORMAT_AUDIO::Media_Format_uLaw8k);
				break;
			case AUDIO_BANDWIDTH::AUDIO_BW_ALAW_8K:
				// activate 8Khz aLaw channel support.
				SetAudioMediaFormat(hSipEngine,PhoneLine,MEDIA_FORMAT_AUDIO::Media_Format_aLaw8k);
				break;
			case AUDIO_BANDWIDTH::AUDIO_BW_G729:
				// activate G729 channel support.
				SetAudioMediaFormat(hSipEngine,PhoneLine,MEDIA_FORMAT_AUDIO::Media_Format_G729);
				break;
			case AUDIO_BANDWIDTH::AUDIO_BW_G729A:
				// activate G729A channel support.
				SetAudioMediaFormat(hSipEngine,PhoneLine,MEDIA_FORMAT_AUDIO::Media_Format_G729A);
				break;
			case AUDIO_BANDWIDTH::AUDIO_BW_ILBC_20MS:
				// activate iLBC 20Ms channel support.
				SetAudioMediaFormat(hSipEngine,PhoneLine,MEDIA_FORMAT_AUDIO::Media_Format_iLBC_20Ms);				
				break;
			case AUDIO_BANDWIDTH::AUDIO_BW_ILBC_30MS:
				// activate iLBC 30Ms channel support.
				SetAudioMediaFormat(hSipEngine,PhoneLine,MEDIA_FORMAT_AUDIO::Media_Format_iLBC_30Ms);				
				break;
			case AUDIO_BANDWIDTH::AUDIO_BW_PCM_8K:
				// activate 8Khz channel support.
				SetAudioMediaFormat(hSipEngine,PhoneLine,MEDIA_FORMAT_AUDIO::Media_Format_Pcm8000);				
				break;
			case AUDIO_BANDWIDTH::AUDIO_BW_PCM_11K:
				// activate 11Khz channel support.
				SetAudioMediaFormat(hSipEngine,PhoneLine,MEDIA_FORMAT_AUDIO::Media_Format_Pcm11050);				
				break;
			case AUDIO_BANDWIDTH::AUDIO_BW_PCM_22K:
				// activate 22Khz channel support.
				SetAudioMediaFormat(hSipEngine,PhoneLine,MEDIA_FORMAT_AUDIO::Media_Format_Pcm22050);				
				break;
			default:
				status= SetAudioMediaFormats(hSipEngine,PhoneLine,SettingsNumMediaFormats, SettingsMediaFormats);
				break;
			}
			status= SetPhoneLineVolume(hSipEngine,PhoneLine,DEFAULT_PHONE_LINE_VOLUME,DEFAULT_PHONE_LINE_VOLUME);
			status = EnableRawRtpPacketAccess(hSipEngine, PhoneLine, true, receiveRtpCallback, 0);
		}

		status = AddAuthorizationCredentials(hSipEngine,
			pConfigUsername,
			pConfigPassword,
			pConfigRealm
			);

		if (status != TELEPHONY_RETURN_VALUE::SipSuccess) return status;

		SIPPhone::InitializeAudioOutput(0,&hAudioOut);


		status = SipTelephonyEnable(hSipEngine);
		if (status != TELEPHONY_RETURN_VALUE::SipSuccess) return status;

		status = EnableSipDomain(hSipEngine, pConfigSipProxyAddress);
		if (status != TELEPHONY_RETURN_VALUE::SipSuccess) return status;

		status = EnableSipProxyServer(hSipEngine,pConfigSipProxyAddress,SIP_PORT);
		if (status != TELEPHONY_RETURN_VALUE::SipSuccess) return status;

		status = SendSipKeepAlive(hSipEngine,pConfigSipProxyAddress,SIP_PORT,TRUE,30);
		if (status != TELEPHONY_RETURN_VALUE::SipSuccess) return status;

		status = EnableSipRegisterServer(
			hSipEngine,
			pConfigUsername,
			FALSE,
			TRUE,
			pConfigSipProxyAddress,
			SIP_PORT,
			3600,
			3600,
			5000,
			FALSE
			);

		status = SetChallengeAuthenticationState(hSipEngine, TRUE);



		return status;
	}
	TELEPHONY_RETURN_VALUE SIPPhone::InitEngine(ClientCallBack ^pfn, String ^sUsername, String ^sPassword, String ^sRealm, String ^sSipProxyAddress,  String ^sLocalIP, String ^sInitData, String ^sUserAgent, BOOL bEnableSipLog, String ^sLogFilePath )
	{
		WSADATA wsaData;
		int result  = 0;
		int registrationTries =0;
		CIpAddress IpAddress;
		TELEPHONY_RETURN_VALUE status;

		m_CallBack = pfn;

		void *pData;


		pData = (void*)Marshal::StringToHGlobalAnsi(sInitData);

		pConfigRealm = "*";//(char*)(void*)Marshal::StringToHGlobalAnsi(sRealm);
		pConfigSipProxyAddress = (char*)(void*)Marshal::StringToHGlobalAnsi(sSipProxyAddress);

		pConfigUsername = (char*)(void*)Marshal::StringToHGlobalAnsi(sUsername);
		pConfigPassword = (char*)(void*)Marshal::StringToHGlobalAnsi(sPassword);

		pConfigUserAgent = (char*)(void*)Marshal::StringToHGlobalAnsi(sUserAgent);
		pConfigLocalIP = (char*)(void*)Marshal::StringToHGlobalAnsi(sLocalIP);

		pConfigLogFilePath = (char*)(void*)Marshal::StringToHGlobalAnsi(sLogFilePath);
			

		for(;true;){
			// initialize the LanScape VOIP Media Engine.

			status = InitializeMediaEngine(pData,0);
			if (status != TELEPHONY_RETURN_VALUE::SipSuccess && status != TELEPHONY_RETURN_VALUE::SipMediaEngineAlreadyInitialized) break;


			// Initialize the startup parameter block to all zero. It is a good
			// idea to perform this operation in case LanScape adds additional
			// members to the startup param structure and your application code
			// forgets to initialize the new members.
			memset(&StartupParams,0,sizeof(StartupParams));			
			// Initialize the telephony engine startup parameters.
			//
			StartupParams.pPersonalityMicrocode = pPersonalityMicrocode;
			StartupParams.NumPhoneLinesRequested = MAX_PHONE_LINES_SUPPORTED;
			StartupParams.LineMode =  LINE_MODE::PHONE_LINE;
			StartupParams.UserNotifyCallbackProc = (SIPCALLBACKPROC)TelephonyEngineCallback;
			StartupParams.pUserDefinedData = 0;

			// replace this IP address with the one your machine uses.

			IpAddress.IsIpAddress((char*)(void*)Marshal::StringToHGlobalAnsi(sLocalIP),StartupParams.IpAddressOfThisHost);

			registrationTries ++;
			if (registrationTries > MAX_REGISTRATION_TRIES) break;


			if(validSipPort>0)
			{
				StartupParams.SipPort = validSipPort;
			}
			else
			{
				int randPort = rand()%16383;
				StartupParams.SipPort = 49152+randPort;
			}


			//Test socket if not aleady in use.
			result = WSAStartup(MAKEWORD(2,2), &wsaData);
			if (result == NO_ERROR){
				SOCKET RecvSocket = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);
				if (RecvSocket != INVALID_SOCKET)
				{
					sockaddr_in RecvAddr;
					memset(&RecvAddr, 0, sizeof(RecvAddr));
					RecvAddr.sin_family = AF_INET;
					RecvAddr.sin_addr.s_addr = inet_addr(pConfigLocalIP);
					RecvAddr.sin_port = htons(StartupParams.SipPort);
					//Try to bind to socket
					result = bind(RecvSocket, (SOCKADDR *) &RecvAddr, sizeof(RecvAddr));
					//Close it
					closesocket(RecvSocket);		
				}
				WSACleanup();

				if (result == SOCKET_ERROR) {
					validSipPort = 0;
					continue;
				}}

			StartupParams.MaxSipMesageLength = 4196;
			StartupParams.pPhoneName = pConfigUsername;
			StartupParams.pPhoneDisplayName = pConfigUsername;
			StartupParams.CallConferenceEnabled = TRUE;
			StartupParams.FarEndCallTransferEnabled = TRUE;
			StartupParams.MinLocalRtpPort = 8000;
			StartupParams.MaxLocalRtpPort = 8899;
			StartupParams.ZeroBasedAudioInDeviceId = SIP_USE_PREFERED_AUDIO_DEVICE;
			StartupParams.ZeroBasedAudioOutDeviceId = SIP_USE_PREFERED_AUDIO_DEVICE;
			StartupParams.AudioRecordBandWidth = AUDIO_BANDWIDTH::AUDIO_BW_PCM_22K;
			StartupParams.AudioPlaybackBandWidth = AUDIO_BANDWIDTH::AUDIO_BW_PCM_22K;
			StartupParams.PlaybackBufferingDefault = 2;
			StartupParams.PlaybackBufferingDuringSounds = 4;
			StartupParams.PhoneLineTransmitBuffering = 2;
			StartupParams.LogSipMessages = bEnableSipLog;
			if(bEnableSipLog){
				StartupParams.pSipLogFileName = pConfigLogFilePath;
			}

			// enable this if you want "man readable" events to be sent to a
			// log server. Useful while debugging and to become familiar with
			// media egnie event behavior.
			StartupParams.EnableEventLogServers = FALSE;

			// enable this if you want to record phone calls. The buffering depth
			// can remain at 0 for most applications.
			StartupParams.EnablePhoneLineRecording = TRUE;
			StartupParams.PhoneLineRecordBuffering = 0;

			StartupParams.StartupFlags = 0;


			status = SIPPhone::StartSip();
			if (status == TELEPHONY_RETURN_VALUE::SipSuccess)
			{
				status = SIPPhone::ConfigureSip();
			}

			if(status == TELEPHONY_RETURN_VALUE::SipRegistrationTimeOut || 
				status == TELEPHONY_RETURN_VALUE::SipSocketOpenError ||
				status == TELEPHONY_RETURN_VALUE::SipSocketBindError	) 
			{


				ShutdownEngine();
				validSipPort = 0;
				continue;

			}
			if (status != TELEPHONY_RETURN_VALUE::SipSuccess) break;

			validSipPort = StartupParams.SipPort;
			return status;
		}
		validSipPort = 0;
		ShutdownEngine();
		return status;
	}

	TELEPHONY_RETURN_VALUE SIPPhone::CallOrAnswer(int PhoneLineID, System::String ^Destination)
	{
		TELEPHONY_RETURN_VALUE status;
		LINE_STATE LineState;

		if(PhoneLineID==-1)
		{
			//get free line
			PhoneLineID=GetFreePhoneLine();
		}

		status = GetLineStatus(hSipEngine,PhoneLineID,&LineState);


		if( LineState.State == TELEPHONY_RETURN_VALUE::SipOnHook)
		{
			if(CallForwards[PhoneLineID].NextCallForward)
			{
				status = EnableDialTone(hSipEngine,FALSE);
				status = EnableDtmfDigits(hSipEngine,FALSE);
				CallForwards[PhoneLineID].NextCallForward = FALSE;
				CallForwards[PhoneLineID].ForwardAcitve = TRUE;
			}
			else
			{
				status = EnableDialTone(hSipEngine,TRUE);
				status = EnableDtmfDigits(hSipEngine,TRUE);
				CallForwards[PhoneLineID].ForwardAcitve = FALSE;
			}

			char *pDestination = (char*)(void*)Marshal::StringToHGlobalAnsi(Destination);
			status = MakeCallUri(hSipEngine,pDestination,true,PhoneLineID,false,30000);

		}
		else if (LineState.CallDirection == CALL_DIRECTION::CallDirectionIn)
		{
			if(LineState.State == TELEPHONY_RETURN_VALUE::SipOkToAnswerCall)
			{
				GoOffHook(hSipEngine,PhoneLineID);
			}
		}

		return status;


	}


	TELEPHONY_RETURN_VALUE SIPPhone::CancelCall(int PhoneLineID){
		char *pAbortPhrase = "Busy here";
		LINE_STATE lineState;
		TELEPHONY_RETURN_VALUE status;
		status = GetLineStatus(hSipEngine,PhoneLineID,&lineState);
		if(lineState.State == TELEPHONY_RETURN_VALUE::SipOkToAnswerCall)
		{
			status = AbortIncomingCall(hSipEngine,PhoneLineID,486,pAbortPhrase);
		}
		else
		{
			status = TerminateCall(hSipEngine,PhoneLineID,FALSE,CALL_TERMINATE_TIMEOUT_MS);
		}
		return status;
	}

	TELEPHONY_RETURN_VALUE SIPPhone::ConferenceOnOffCall(int PhoneLineID){

		TELEPHONY_RETURN_VALUE status = TELEPHONY_RETURN_VALUE::SipCallFailure;
		LINE_STATE LineState;

		status = GetLineStatus(hSipEngine,PhoneLineID,&LineState);

		if(status == TELEPHONY_RETURN_VALUE::SipSuccess)
		{
			switch(LineState.State)
			{
			case TELEPHONY_RETURN_VALUE::SipInCall:
			case TELEPHONY_RETURN_VALUE::SipCallHold:

				// any "in call" or "on hold" call can be added to a conference session.
				// the LED status is handled by the notification callback.
				status = ConferenceLine(hSipEngine,PhoneLineID,TRUE);

				if(status != TELEPHONY_RETURN_VALUE::SipSuccess)
				{
					// handle the error.
				}

				break;

			case TELEPHONY_RETURN_VALUE::SipInConference:

				// remove the phone line from the conference. LED status is handled
				// by the notification callback.
				status = ConferenceLine(hSipEngine,PhoneLineID,FALSE);

				if(status != TELEPHONY_RETURN_VALUE::SipSuccess)
				{
					// handle the error.
				}

				break;

			default:
				// do nothing.
				break;
			}
		}
		return status;
	}
	TELEPHONY_RETURN_VALUE SIPPhone::HoldOnOffCall(int PhoneLineID)
	{
		TELEPHONY_RETURN_VALUE status = TELEPHONY_RETURN_VALUE::SipCallFailure;
		LINE_STATE LineState;

		status = GetLineStatus(hSipEngine,PhoneLineID,&LineState);

		if(status == TELEPHONY_RETURN_VALUE::SipSuccess)
		{
			switch(LineState.State)
			{
			case TELEPHONY_RETURN_VALUE::SipInCall:
			case TELEPHONY_RETURN_VALUE::SipInConference:

				// put the call on hold. LED status is handled
				// by the notification callback.
				status = HoldLine(hSipEngine,PhoneLineID,TRUE);

				if(status != TELEPHONY_RETURN_VALUE::SipSuccess)
				{
					// handle the error.
				}

				break;


			case TELEPHONY_RETURN_VALUE::SipCallHold:

				// take the call out of hold. LED status is handled
				// by the notification callback.
				status = HoldLine(hSipEngine,PhoneLineID,FALSE);

				if(status != TELEPHONY_RETURN_VALUE::SipSuccess)
				{
					// handle the error.
				}


			default:
				// do nothing.
				break;

			}
		}
		return status;
	}

	TELEPHONY_RETURN_VALUE SIPPhone::XferCall(int PhoneLineID,String ^Destination){
		TELEPHONY_RETURN_VALUE status = TELEPHONY_RETURN_VALUE::SipCallFailure;
		LINE_STATE LineState;

		char *pDestination = (char*)(void*)Marshal::StringToHGlobalAnsi(Destination);

		// get current line status.
		status = GetLineStatus(hSipEngine,PhoneLineID,&LineState);

		if(status == TELEPHONY_RETURN_VALUE::SipSuccess)
		{
			switch(LineState.State)
			{
			case TELEPHONY_RETURN_VALUE::SipInCall:
			case TELEPHONY_RETURN_VALUE::SipCallHold:

				// transfer the call to another phone.
				status = TransferLineUri(
					hSipEngine,									// handle of telephony engine.
					pDestination,	// destination SIP URI.
					true,				// send the call transfer through the configured SIP proxy.
					PhoneLineID									// phone line to transfer.
					);

				break;


			case TELEPHONY_RETURN_VALUE::SipOnHook:
				// do nothing.
				break;


			default:
				// do nothing.
				break;
			}
		}
		return status;
	}
	TELEPHONY_RETURN_VALUE SIPPhone::RecOnOffCall(int PhoneLineID,String ^Directory)
	{
		LINE_STATE LineState;
		TELEPHONY_RETURN_VALUE status = TELEPHONY_RETURN_VALUE::SipCallFailure;

		char *pDirectory = (char*)(void*)Marshal::StringToHGlobalAnsi(Directory);
		if(GetLineStatus(hSipEngine,PhoneLineID,&LineState) == TELEPHONY_RETURN_VALUE::SipSuccess)
		{
			//we only want to record active calls.
			if(LineState.State == TELEPHONY_RETURN_VALUE::SipInCall)
			{
				if(LineState.CallRecordingActive)
				{
					// turn off call recording.
					status = StopPhoneLineRecording(
						hSipEngine,
						PhoneLineID
						);
				}
				else
				{
					// turn on call recording.
					status = StartPhoneLineRecording(
						hSipEngine,
						PhoneLineID,
						TRUE,			// record to a file.
						FALSE,			// use wave file not raw samples.
						pDirectory,
						0,
						0
						);
				}

			}
		}
		return status;
	}

	TELEPHONY_RETURN_VALUE SIPPhone::GetPhoneLineStatus(int PhoneLineID, TELEPHONY_RETURN_VALUE LineState, CALL_DIRECTION CallDirection, bool RecordingActive ){
		LINE_STATE LineStatus;
		TELEPHONY_RETURN_VALUE SipStatus;

		// get the current line state so we know the direction of the call.
		SipStatus = GetLineStatus(hSipEngine,PhoneLineID,&LineStatus);
		CallDirection =LineStatus.CallDirection;
		LineState = LineStatus.State;
		RecordingActive = LineStatus.CallRecordingActive = 1?true:false;

		return SipStatus;

	}



	TELEPHONY_RETURN_VALUE SIPPhone::GetPhoneLineState(int PhoneLineID){

		LINE_STATE LineState;
		TELEPHONY_RETURN_VALUE SipStatus;

		// get the current line state so we know the direction of the call.
		SipStatus = GetLineStatus(hSipEngine,PhoneLineID,&LineState);
		return LineState.State;

	}
	CALL_DIRECTION SIPPhone::GetPhoneLineCallDirection(int PhoneLineID){

		LINE_STATE LineState;
		TELEPHONY_RETURN_VALUE SipStatus;

		// get the current line state so we know the direction of the call.
		SipStatus = GetLineStatus(hSipEngine,PhoneLineID,&LineState);
		return LineState.CallDirection;

	}
	int SIPPhone::GetPhoneLineCallRecordingActive(int PhoneLineID){

		LINE_STATE LineState;
		TELEPHONY_RETURN_VALUE SipStatus;

		// get the current line state so we know the direction of the call.
		SipStatus = GetLineStatus(hSipEngine,PhoneLineID,&LineState);
		return LineState.CallRecordingActive;

	}
	int SIPPhone::GetFreePhoneLine()
	{
		TELEPHONY_RETURN_VALUE status;
		LINE_STATE state;
		for(int i=0;i<MAX_PHONE_LINES_SUPPORTED;i++){
			status = GetLineStatus(hSipEngine,i,&state);
			if(state.State==TELEPHONY_RETURN_VALUE::SipOnHook)
			{
				return i;
			}
		}
		return -1;
	}



	String^ SIPPhone::GetIncomingCallDetails(int PhoneLineID)
	{
		TELEPHONY_RETURN_VALUE status;
		SIP_INCOMING_CALL_INFO myIncomingCallInfo;
		status = GetIncomingCallInfo(hSipEngine,PhoneLineID,&myIncomingCallInfo);
		return System::Runtime::InteropServices::Marshal::PtrToStringAnsi(static_cast<IntPtr>(myIncomingCallInfo.pSrcUserName));
	}

	String^ SIPPhone::GetOutgoingCallDetails (int PhoneLineID)
	{

		TELEPHONY_RETURN_VALUE status;
		SIP_OUTGOING_CALL_INFO myOutgoingCallInfo;
		status = GetOutgoingCallInfo(hSipEngine,PhoneLineID,&myOutgoingCallInfo);
		return System::Runtime::InteropServices::Marshal::PtrToStringAnsi(static_cast<IntPtr>(myOutgoingCallInfo.pDestUserName));
	}




	TELEPHONY_RETURN_VALUE SIPPhone::StartDTMF(int PhoneLineID,DTMF_TONE Tone)
	{

		if(DtmfState[PhoneLineID].Active || DtmfState[PhoneLineID].Released) return TELEPHONY_RETURN_VALUE::SipCallFailure;;

		LINE_STATE LineStatus;
		GetLineStatus(hSipEngine,PhoneLineID,&LineStatus);
		if(TELEPHONY_RETURN_VALUE::SipInCall == LineStatus.State)
		{
			hDtmfGenerators[PhoneLineID] = 0;
			CreateDtmfGenerator(hSipEngine,(DTMF_GENERATOR_CALLBACK_PROC)DTMFCallbackProc,&(hIvrTransmiters[PhoneLineID]),&(hDtmfGenerators[PhoneLineID]));
			if(!hDtmfGenerators[PhoneLineID])
			{
				//				Console::WriteLine("SIP : CreateDtmfGenerator Failed!");
			}
			else
			{
				
				DtmfState[PhoneLineID].Digit = (int)Tone;
				DtmfState[PhoneLineID].Duration = 320;
				DtmfState[PhoneLineID].Active = true;

				StartDtmfTone(hDtmfGenerators[PhoneLineID], Tone, 5000);
			}
			return TELEPHONY_RETURN_VALUE::SipSuccess;
		}
		return TELEPHONY_RETURN_VALUE::SipCallFailure;
	}
	TELEPHONY_RETURN_VALUE SIPPhone::StopDTMF(int PhoneLineID)
	{
		if(DtmfState[PhoneLineID].Released) return TELEPHONY_RETURN_VALUE::SipCallFailure;;

		LINE_STATE LineStatus;
		GetLineStatus(hSipEngine,PhoneLineID,&LineStatus);
		if(TELEPHONY_RETURN_VALUE::SipInCall == LineStatus.State)
		{
			if(hDtmfGenerators[PhoneLineID])
			{
				DtmfState[PhoneLineID].Released = true;
				DtmfState[PhoneLineID].Active = true;

				StopDtmfTone(hDtmfGenerators[PhoneLineID]);
				DestroyDtmfGenerator(hDtmfGenerators[PhoneLineID]);
				hDtmfGenerators[PhoneLineID] = 0;
				//				Console::WriteLine("SIP : Stop DTMF");
				return TELEPHONY_RETURN_VALUE::SipSuccess;
			}

		}
		return TELEPHONY_RETURN_VALUE::SipCallFailure;
	}

	TELEPHONY_RETURN_VALUE SIPPhone::ShutdownEngine()
	{
		TELEPHONY_RETURN_VALUE status;


		if(hSipEngine)
		{
			if(hAudioOut)
			{
				status = CloseAudioOutChannel(hAudioOut);

				if(status !=TELEPHONY_RETURN_VALUE::SipSuccess)
				{
					// handle the error.
				}
				else
				{
					hAudioOut = 0;
				}
			}
						
			SIPPhone::UnInitializeAudioOutput(&hAudioOut);
			status = DisableSipRegisterServer(hSipEngine);
			status = DisableSipProxyServer(hSipEngine);
			status = StopSipTelephony(hSipEngine);
		
		}
		status = UnInitializeMediaEngine();
		hSipEngine = 0;
		return status;
	}

	void SIPPhone::InitializeAudioOutput(int ZerobasedAudioOutput, AUDIOHANDLE *phAudioOut)
	{
		TELEPHONY_RETURN_VALUE status;

		// open an audio output channel so we can playback wave file data
		// when an incoming phone call is answered.
		status = OpenAudioOutChannel(
			hSipEngine,			// handle to the media engine.
			0,					// the audio output line to use.
			phAudioOut			// returned audio out handle.
			);

		if(status != TELEPHONY_RETURN_VALUE::SipSuccess)
		{
			// handle the error.
		}
		else
		{
			// specify the sample data type we will send to the audio
			// output.
			status = SetAudioOutDataType(*phAudioOut,AUDIO_BANDWIDTH::AUDIO_BW_PCM_8K);

			if(status !=TELEPHONY_RETURN_VALUE::SipSuccess)
			{
				// handle the error.
			}

			// get the amount of sample data we can send to the audio output
			// in a single sample block.
			BOOL SamplesInByteArray = false;
			status = GetAudioOutSampleBlockSize(*phAudioOut,&SamplesPerAudioOutBuffer,&BytesPerAudioOutBuffer,&SamplesInByteArray);


			if(status != TELEPHONY_RETURN_VALUE::SipSuccess)
			{
				// handle the error.
			}
		}
	}

	TELEPHONY_RETURN_VALUE SIPPhone::SetIncomingPhoneRingEnable(BOOL bEnableState)
	{
		TELEPHONY_RETURN_VALUE status = TELEPHONY_RETURN_VALUE::SipCallFailure;
		SettingEnableIncomingPhoneRing = bEnableState;

		status = EnableIncomingPhoneRing(hSipEngine,SettingEnableIncomingPhoneRing);
		return status;
	};

	TELEPHONY_RETURN_VALUE SIPPhone::SetAudioVolume(int iRxVolume, int iTxVolume)
	{
		TELEPHONY_RETURN_VALUE status = TELEPHONY_RETURN_VALUE::SipCallFailure;
		SettingsRxVolume = iRxVolume;
		SettingsTxVolume = iTxVolume;

		for(int PhoneLine=0;PhoneLine<MAX_PHONE_LINES_SUPPORTED;PhoneLine++)
		{
			status = SetPhoneLineVolume(hSipEngine,PhoneLine,SettingsRxVolume,SettingsTxVolume);
		}
		return status;
	};

	TELEPHONY_RETURN_VALUE SIPPhone::SetMediaFormats(int iNumMediaFormats, int iMediaFormat0, int iMediaFormat1, int iMediaFormat2, int iMediaFormat3)
	{
		TELEPHONY_RETURN_VALUE status = TELEPHONY_RETURN_VALUE::SipCallFailure;

		SettingsMediaFormats[0]= (MEDIA_FORMAT_AUDIO)iMediaFormat0;
		SettingsMediaFormats[1]= (MEDIA_FORMAT_AUDIO)iMediaFormat1;
		SettingsMediaFormats[2]= (MEDIA_FORMAT_AUDIO)iMediaFormat2;
		SettingsMediaFormats[3]= (MEDIA_FORMAT_AUDIO)iMediaFormat3;
		SettingsNumMediaFormats = iNumMediaFormats;
		if(iNumMediaFormats==0) {
			SettingsMediaFormats[0]=	MEDIA_FORMAT_AUDIO::Media_Format_Undefined;
			SettingsNumMediaFormats = 1;
		}
		for(int PhoneLine=0;PhoneLine<MAX_PHONE_LINES_SUPPORTED;PhoneLine++)
		{
			status = SetAudioMediaFormats(hSipEngine,PhoneLine,iNumMediaFormats, SettingsMediaFormats);
		}
		return status;
	};




	// close the audio output.
	void SIPPhone::UnInitializeAudioOutput(AUDIOHANDLE *phAudioOut)
	{
		TELEPHONY_RETURN_VALUE status;

		// close the audio output.
		if(hAudioOut)
		{
			status = CloseAudioOutChannel(hAudioOut);

			if(status != TELEPHONY_RETURN_VALUE::SipSuccess)
			{
				// handle the error.
			}
			else
			{
				hAudioOut = 0;
			}
		}
	}

	
}