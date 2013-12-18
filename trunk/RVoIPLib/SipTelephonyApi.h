
//////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 1987-2007 LanScape Corporation.
// All Rights Reserved.
//
// Permission is hereby granted to use and develop additional software
// applications using the software contained within this source code module.
// Redistribution of this source code in whole or in part is strictly
// prohibited.
//
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// IN NO EVENT SHALL LANSCAPE BE LIABLE TO ANY PARTY FOR DIRECT, INDIRECT,
// SPECIAL, INCIDENTAL, OR CONSEQUENTIAL DAMAGES, INCLUDING LOST PROFITS,
// ARISING OUT OF THE USE OF THIS SOFTWARE AND ITS DOCUMENTATION, EVEN IF
// LANSCAPE HAS BEEN ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
// LANSCAPE SPECIFICALLY DISCLAIMS ANY WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
// PARTICULAR PURPOSE. THE SOFTWARE AND ACCOMPANYING DOCUMENTATION, IF
// ANY, PROVIDED HEREUNDER IS PROVIDED "AS IS". LANSCAPE HAS NO OBLIGATION
// TO PROVIDE MAINTENANCE, SUPPORT, UPDATES, ENHANCEMENTS, OR MODIFICATIONS.
//
//////////////////////////////////////////////////////////////////////////

#ifndef _SipTelephonyApi_h_
#define _SipTelephonyApi_h_



#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000


#include <Windows.h>
#include <Mmsystem.h>		// If you don't already have this include file,
							// look for it on the Microsoft Platform SDK: Windows Multimedia.




#define VOIP_API			__stdcall


#define NUM_BYTES_IP_ADDRESS						4
#define DEFAULT_SIP_MESSAGE_LENGTH					1500
#define DEFAULT_SIP_MESSAGE_RECEIVE_FIFO_LENGTH		8192

#if SUPPORT_MIXER_LINE_RESAMPLING
#define DEFAULT_RTP_PACKET_LENGTH					930
#else
#define DEFAULT_RTP_PACKET_LENGTH					926
#endif // #if SUPPORT_MIXER_LINE_RESAMPLING

// values that are specified for the flags parameter when
// creating a new call engine.
//
#define ENABLE_START_AUDIO_SPLASH		0x00000001
#define ENABLE_STOP_AUDIO_SPLASH		0x00000002



// codes that can be returned by the API or the
// notification mechanism.
//
public enum class  TELEPHONY_RETURN_VALUE
{
	// Phone line return values and Notifications.
	//

	// success.
	SipSuccess = 0,									// API return value

	// errors.
	SipCallFailure,									// API return value
	SipCallTimeOut,									// API return value
	SipPhoneLineAccessError,						// API return value
	SipUnknownHost,									// API return value
	SipCallAlreadyInProgress,						// API return value
	SipBadPhoneLine,								// API return value
	SipInvalidHandle,								// API return value
	SipCallStateHistoryMustBeRead,					// API return value
	SipCallInstanceDataNoExist,						// API return value
	SipNotEnabled,									// API return value
	SipLineBusyOut,									// API return value
	SipAlreadyOffHook,								// API return value
	SipAlreadyOnHook,								// API return value
	SipThreadCreationError,							// API return value, Event Notification
	SipBandwidthError,								// API return value
	SipMemoryError,									// API return value, Event Notification
	SipEventLogServerParameterError,				// API return value
	SipDirectoryDoesNotExist,						// API return value
	SipStartRtpPortError,							// API return value
	SipRtpAndSipPortOverlapError,					// API return value
	SipBadPhoneName,								// API return value
	SipBadDisplayName,								// API return value
	SipBadParameter,								// API return value
	SipBadNumberOfPhoneLines,						// API return value
	SipAudioOutFailure,								// API return value
	SipAudioInFailure,								// API return value
	SipRtpReceiverEventError,						// API return value
	SipRtpReceiverThreadStartFailure,				// API return value
	SipPlaybackMixerThreadStartError,				// API return value
	SipCallThreadStartEventFailure,					// API return value
	SipCallThreadStartFailure,						// API return value
	SipEventError,									// API return value
	SipUdpTxBufferSizeError,						// API return value
	SipUdpRxBufferSizeError,						// API return value
	SipSocketOpenError,								// API return value
	SipSocketBindError,								// API return value
	SipInvalidInstance,								// API return value
	SipApiProcNotAllowed,							// API return value
	SipNetworkInitializeError,						// API return value
	SipProxyNotEnabled,								// API return value
	SipRtpSocketOpenError,							// Event Notification
	SipRtpSocketBindError,							// Event Notification
	SipSessionReceiveOverrun,						// Event Notification
	SipReceiveBufferTooSmall,						// Event Notification
	SipRtpReceiveBufferTooSmall,					// Event Notification
	SipRtpFatalStartError,							// Event Notification
	SipCallThreadTerminateTimeout,					// Event Notification

	SipMediaEngineNotInitialized,					// API return value
	SipMediaEngineAlreadyInitialized,				// API return value
	SipInitializeError,								// API return value

	SipLineInitialized,								// Event Notification
	SipLineCapacityReached,							// Event Notification
	SipCallStartFailed,								// API return value, Event Notification
	SipInviteSendFailed,							// Event Notification
	SipInviteAckSendFailed,							// Event Notification
	SipCallCanceled,								// Event Notification

	SipFarEndIsBusy,								// API return value, Event Notification
	SipFarEndError,									// API return value, Event Notification

	SipAnswerTimeout,								// Event Notification
									
	SipNoCallActive,								// API return value
	SipCallNotOnHold,								// API return value
	SipConferenceActive,							// API return value
	SipDomainError,									// API return value
	SipBadUserName,									// API return value
	SipTransfrerNotAllowed,							// API return value
	SipDomainNameNotDefined,						// API return value

	// hook states.
	SipOffHook,										// Event Notification
	SipOnHook,										// Event Notification

	// outbound call related.
	SipBadSipUri,									// API return value
	SipOutgoingCallStateError,						// API return value
	SipOutgoingCallInitializing,					// Event Notification
	SipOutgoingTransferInitializing,				// Event Notification
	SipOutgoingCallStart,							// Event Notification
	SipDialTone,									// Event Notification
	SipDialing,										// Event Notification
	SipSendInvite,									// Event Notification
	SipStartOutgoingRing,							// Event Notification
	SipReceivedProvisionalResponse,					// Event Notification
	SipReceived100Trying,							// Event Notification
	SipReceived180Ringing,							// Event Notification
	SipReceived181CallBeingForwarded,				// Event Notification
	SipReceived182Queued,							// Event Notification
	SipReceived183SessionProgress,					// Event Notification
	SipReceivedUnsupportedProvisionalResponse,		// Event Notification
	SipWaitForInviteOk,								// Event Notification
	SipInviteOkReceived,							// Event Notification
	SipSendInviteAck,								// Event Notification
	SipOutgoingCallConnected,						// Event Notification

	// inbound call related.
	SipIncomingCallStart,							// Event Notification
	SipIncomingCallAssignPhoneLine,					// Event Notification
	SipIncomingCallInitialized,						// Event Notification
	SipTransferExecuting,							// Event Notification
	SipStartIncomingRing,							// Event Notification
	SipSendTrying,									// Event Notification
	SipSendRinging,									// Event Notification
	SipSendSessionProgress,							// Event Notification
	SipOkToAnswerCall,								// Event Notification
	SipSend200Ok,									// Event Notification
	SipInviteAckNotReceived,						// Event Notification
	SipReceivedInviteAck,							// Event Notification
	SipAnsweringCall,								// Event Notification
	SipIncomingCallConnected,						// Event Notification
	SipIncomingCallAborted,							// Event Notification

	// call control related.
	SipInCall,										// Event Notification
	SipCallHold,									// Event Notification
	SipBusyOut,										// Event Notification
	SipInConference,								// Event Notification

	SipFarEndHoldOn,								// API return value, Event Notification
									
	SipFarEndHoldOff,								// Event Notification
	SipTransferingCall,								// Event Notification

	// state transition notifications.
	SipCallHoldOn,									// Event Notification
	SipCallHoldOff,									// Event Notification
	SipBusyOutOn,									// Event Notification
	SipBusyOutOff,									// Event Notification
	SipInConferenceOn,								// Event Notification
	SipInConferenceOff,								// Event Notification

	// bye transactions.
	SipByeReceived,									// Event Notification
	SipSendByeAck,									// Event Notification
	SipSendBye,										// Event Notification
	SipReceivedByeAck,								// Event Notification
	SipReceivedByeErrorAck,							// Event Notification
	SipByeAckNotReceived,							// Event Notification

	SipCallComplete,								// Event Notification

	// IVR and Audio Out
	SipIvrBadChannel,								// API return value
	SipIvrAlreadyOpened,							// API return value
	SipIvrTransmitterFull,							// API return value

	SipAudioOutAlreadyOpened,						// API return value
	SipBadAudioOutLine,								// API return value
	SipNoAudioOutType,								// API return value
	SipAudioOutputFull,								// API return value
	SipBadAudioDataType,							// API return value


	// Global return values and Notifications.
	// (i.e. not specific to a particular phone line )
	//

	// Call engine status.
	SipCallEngineReady,								// Event Notification
	SipCallEngineTerminated,						// Event Notification

	// server registration.
	SipRegisterReceived,							// Event Notification
	SipRegisterTrying,								// Event Notification
	SipRegisterSuccess,								// Event Notification
	SipRegistrationIntervalError,					// API return value, Event Notification
	SipRegistrationTimeOut,							// Event Notification
	SipRegisterErrorBadCredentials,					// API return value. Event Notification.
	SipRegisterBadNameList,							// API return value.
	SipRegisterError,								// API return value. Event Notification.

	// Subscribe/Notify status.
	SipSubscriptionAlreadyExists,					// API return value
	SipEventNotifyResponseTimeOut,					// API return value
	SipEventNotifyNotAccepted,						// API return value
	SipSubscriptionMemoryError,						// Event Notification
	SipSubscriptionTrying,							// Event Notification
	SipSubscriptionTimeOut,							// Event Notification
	SipSubscriptionNotAccepted,						// Event Notification
	SipSubscriptionRequiresAuthentication,			// Event Notification
	SipSubscriptionSuccess,							// Event Notification
	SipSubscriptionReceived,						// Event Notification
	SipEventNotifyReceived,							// Event Notification

	// Wan IP address detection.
	SipWanIpAddressChange,							// Event Notification

	// Wan IP address config errors.
	SipWanIpAddressConfigError,						// Event Notification

	// port translation errors.
	SipPortTranslationError,						// Event Notification

	// Network errors.
	SipRegisterNetworkError,						// API return value. Event Notification.
	SipCallNetworkError,							// API return value. Event Notification.
	SipSubscriptionNetworkError,					// Event Notification.
	SipNotifyNetworkError,							// API return value.
	SipNetworkTransmitNotAllBytesSent,				// API return value. Event Notification.
	SipNetworkTransmitWouldBlock,					// API return value. Event Notification.
	SipNetworkTransmitFatalError,					// API return value. Event Notification.

	// Authentication - Incoming.
	SipIncomingAuthentication,						// Event Notification

	// Authentication - Outgoing.
	SipRegisterAuthorizationError,					// API return value. Event Notification.
	SipOutgoingCallDigestAuthenticationRequired,	// Event Notification
	SipOutgoingCallBasicAuthenticationRequired,		// Event Notification
	SipOutgoingCallUnsupportedAuthentication,		// Event Notification
	SipComputeAuthenticationCredentialsFailed,		// Event Notification

	// Modification and inspection of SIP messages.
	SipModifySipMessage,							// Event Notification
	SipReceivedSipMessageParseError,				// Event Notification

	// Phone line call recording.
	SipCallRecordNotEnabled,						// API return value
	SipCallRecordAlreadyStarted,					// API return value
	SipCallRecordAlreadyStopped,					// API return value
	SipCallRecordFilenameNotCreated,				// API return value. Event Notification.
	SipCallRecordSampleFileNotCreated,				// API return value. Event Notification.
	SipCallRecordFileWriteError,					// Event Notification
	SipCallRecordActive,							// Event Notification
	SipCallRecordComplete,							// Event Notification

	// RTP media releated.
	SipReceivedRtpMediaConflict,					// Event Notification


	// Not Used.
	SipUndefined,

	SipReceived302MovedTemporarily = 500					// Event Notification

};



typedef void *			SIPHANDLE;
typedef unsigned int	IVRRXHANDLE;
typedef unsigned int *	IVRTXHANDLE;
typedef char *			AUDIOHANDLE;
typedef void *			EVENT_SUBSCRIBE_HANDLE;
typedef void *			NOTIFY_HANDLE;
typedef void *			CHALLENGE_HANDLE;
typedef int				FORMAT_RATE_CONVERT_HANDLE;



// Call hold types supported.
//
public enum class  CALL_HOLD_TYPE
{
	CALL_HOLD_RFC2543 = 0,		// RFC2543 type (c=0.0.0.0)
	CALL_HOLD_RFC3261,			// RFC3261(sendonly/recvonly)
	CALL_HOLD_DEFAULT			// support for both RFC2543 and RFC3261.
	
};



// Audio data types.
//
public enum class  AUDIO_BANDWIDTH
{
	AUDIO_BW_ULAW_8K = 0,
	AUDIO_BW_ALAW_8K,
	AUDIO_BW_G729,
	AUDIO_BW_G729A,
	AUDIO_BW_ILBC_20MS,
	AUDIO_BW_ILBC_30MS,
	AUDIO_BW_PCM_8K,
	AUDIO_BW_PCM_11K,
	AUDIO_BW_PCM_22K,
	AUDIO_BW_UNDEFINED

};


// Rates and formats of streaming RTP audio media.
//
public enum class  MEDIA_FORMAT_AUDIO
{
	Media_Format_uLaw8k = 0,
	Media_Format_aLaw8k,
	Media_Format_G729,
	Media_Format_G729A,
	Media_Format_iLBC_20Ms,
	Media_Format_iLBC_30Ms,
	Media_Format_Pcm8000,
	Media_Format_Pcm11050,
	Media_Format_Pcm22050,
	Media_Format_Undefined

};


// Rates and formats of streaming RTP video media.
//
public enum class  MEDIA_FORMAT_VIDEO
{
	Media_Format_Video_Undefined = 0

};



// Return values for format and rate conversion functions.
//
public enum class  FORMAT_RATE_STATUS
{
	FORMAT_RATE_SUCCESS = 0,
	FORMAT_RATE_BAD_HANDLE,
	FORMAT_RATE_MEMORY_ERROR,
	FORMAT_RATE_BAD_DATA_TYPE,
	FORMAT_RATE_UNKNOWN_ERROR

};




#define SIP_USE_PREFERED_AUDIO_DEVICE			-1		// Allow system settings to select the audio device.

#define SIP_AUDIO_DEVICE_NOT_USED				-2		// indicates that the telephony engine will not
														// manage an audio device.


#define MAX_VOLUME								2048	// this represents 0% to 400% volume:
														//
														//		0		-	0% volume
														//		512		-	100% volume
														//		1024	-	200% volume
														//		2048	-	400% volume
														//

#define MAX_NOISE_THRESHOLD						0x7fff	// This is the same value as that returned by
														// the GetMaxNoiseThreshold API procedure.


// information associated with an inbound phone call.
//
typedef struct SIP_INCOMING_CALL_INFO
{
	// Source information
	//
	char *pFromHeader;			// the raw SIP "From:" header.
	char *pSrcUrl;				// the SIP url associated with the originator of the phone call.
								// equivalent to information in an INVITE "From:" header.
	
	char *pSrcDisplayName;		// the display name name of the originator of the phone call.
	char *pSrcUserName;			// user name of the originator of the phone call. same as the user name in the SIP "From:" header.
	char *pSrcContactUserName;	// user name of the originating contact of the phone call. same as the user name in the SIP "Contact:" header.
	char *pSrcHost;				// host name of the originator of the phone call.
	DWORD SrcPort;				// the far end's network port used for the inbound call.


	char *pReceivedIpAddress;	// the detected ip address the far end used to originate the phone call.
	DWORD ReceivedPort;			// the detected port the far end used to originate the phone call.


	// Destination information
	//
	char *pToHeader;			// the raw SIP "To:" header.
	char *pDestUrl;				// the SIP url of the phone call's destination. If you are developing
								// a PSTN gateway, you can use this member to get the PSTN phone
								// number.

	char *pDestUserName;		// user name of the destination of the phone call.
	char *pDestHost;			// host of the destination of the phone call.


	char *pCallId;				// the call ID for the call.


	// Other information
	//
	char *pOriginatingEngineId;	// if the originator of the call is another LanScape VOIP Media Engine,
								// this is the unique ID of that media engine.

}SIP_INCOMING_CALL_INFO;



// information associated with who we are calling.
//
typedef struct SIP_OUTGOING_CALL_INFO
{
	char *pDestUrl;				// the SIP url associated with the destination of the phone call.
	char *pDestUserName;		// user name of the destination of the phone call.
	char *pDestHost;			// host name of the destination of the phone call.
	char *pDestIpAddress;		// ip address of the destination of the phone call.
	DWORD DestPort;				// the destination network port.

	char *pCallId;				// the call ID for the call.

}SIP_OUTGOING_CALL_INFO;


// extended error information associated with who we are calling.
//
typedef struct SIP_OUTGOING_CALL_ERROR_INFO
{
	int ResponseCode;				// the SIP error response code from the far end device.
	char *pResponseReasonPhrase;	// the "man readable" error description from the far end device.
	char *pRawSipMessage;			// the received raw error SIP message from the far end device.

}SIP_OUTGOING_CALL_ERROR_INFO;


// information about the active call.
//
typedef struct SIP_ACTIVE_CALL_INFO
{
	// url of far end.
	char *pSipUrlFarEnd;

	// the call ID for the call.
	char *pCallId;

	// raw SIP messages used to set up the call.
	char *pInviteSipMesage;
	char *pResponse2XXSipMesage;

	// audio.
	int LocalAudioRtpPort;
	int FarEndAudioRtpPort;
	MEDIA_FORMAT_AUDIO AudioFormat;
	int AudioFormatRtp;

	// video.
	int LocalVideoRtpPort;
	int FarEndVideoRtpPort;
	MEDIA_FORMAT_VIDEO VideoFormat;
	int VideoFormatRtp;

	// call hold.
	BOOL LocalHoldActive;
	BOOL FarEndHoldActive;

}SIP_ACTIVE_CALL_INFO;


// values used to keep track of the direction
// of an active call.
//
public enum class  CALL_DIRECTION
{
	CallDirectionNone,
	CallDirectionOut,
	CallDirectionIn

};



typedef struct LINE_STATE
{
	TELEPHONY_RETURN_VALUE State;		// phone line states.

	CALL_DIRECTION CallDirection;		// if a call is active on a phone line
										// this indicates who originated the call.

	BOOL CallRecordingActive;			// non zero if call recording has been activated 
										// for the phone line.

}LINE_STATE;


public enum class  SIP_NOTIFY_TYPE
{
	IMMEDIATE_NOTIFICATION,
	GLOBAL_NOTIFICATION,
	PHONE_LINE_NOTIFICATION

};


public enum class  IN_CALL_PROCESS_PRIORITY
{
	PriorityNormal,
	PriorityHigh,
	PriorityRealtime

};


// data struct passed to a user registered speech recognition callback proc.
// Used for local speech recognition.
//
typedef struct SPEECH_RECOGNITION_DATA
{
	AUDIO_BANDWIDTH AudioBandwidth;			// represents the format and rate of the sampled data.

	void *pSampleBuffer;					// address of the sample buffer.

	unsigned long BufferLengthInBytes;		// the number of bytes in the sample buffer.

	void *pUserData;						// the user supplied callback data. this is the same value
											// that was specified when the speech callback was registered.

}SPEECH_RECOGNITION_DATA;



// data struct passed to a user registered IVR callback procedure. Used to perform speech
// recognition of received phone line audio data.
//
typedef struct IVR_RECOGNITION_DATA
{
	int PhoneLine;							// the zero based phone line the data was received on.

	MEDIA_FORMAT_AUDIO MediaFormat;			// represents the format and rate of the sampled data the phone line
											// is using.

	AUDIO_BANDWIDTH RequestedFormat;		// the audio format the application requested when the Rx IVR
											// channel was opened.

	void *pSampleBuffer;					// address of the sample buffer. the data in the block buffer
											// has been converted from the phone line rate/format to the
											// rate/format the application requested.

	unsigned long BufferLengthInBytes;		// the number of bytes in the sample buffer.

	BOOL SamplesInByteArray;				// when set to TRUE, the data pointed to by the 
											// pSampleBuffer member points to a byte (8 bits per element)
											// buffer. If false, then pSampleBuffer points to a
											// short (16 bits per element) buffer.

	void *pUserData;						// the user supplied callback data. this is the same value
											// that was specified when the speech callback was registered.

}IVR_RECOGNITION_DATA;


// data struct passed to a user registered phone line call record callback procedure.
// Used to allow the application to gain real time access to phone line recorded data.
//
typedef struct PHONE_LINE_RECORD_DATA
{
	int PhoneLine;							// the zero based phone line the data was recorded from.

	void *pSampleBuffer;					// address of the record sample buffer.

	unsigned long BufferLengthInBytes;		// the number of bytes in the sample buffer.

	void *pUserData;						// the user supplied callback data. this is the same value
											// that was specified when the phone line record callback
											// was registered.

}PHONE_LINE_RECORD_DATA;



// data struct passed to a user registered phone line VU meter callback procedure.
// Used to allow the application to gain real time access to digitally mixed phone
// line sample data. Useful for driving VU meter displays in your application.
//
// Note:
//
// VU meter functionality shares the phone line's digital mixer that is used
// during phone call recording. You must configure the media engine to enable call
// recording if you want VU meter sample data. This does not mean that call 
// recording has to be active during any call in order to get VU meter sampled data.
// It simply means the media enigne's phone line record digital mixers must be created
// to support VU meter callback functionality.
//
typedef struct PHONE_LINE_VU_METER_DATA
{
	int PhoneLine;							// the zero based phone line the data was recorded from.

	void *pSampleBuffer;					// address of the record sample buffer.

	unsigned long BufferLengthInBytes;		// the number of bytes in the sample buffer.

	void *pUserData;						// the user supplied callback data. this is the same value
											// that was specified when the phone line VU meter callback
											// was registered.

}PHONE_LINE_VU_METER_DATA;



// RTP packet header.
// For complete details see:
//
//   RFC 1889 - RTP: A Transport Protocol for Real-Time Applications
//
#ifndef RTP_HEADER_DEFINED
#define RTP_HEADER_DEFINED
typedef struct RTP_HEADER
{
	//-------------------------------------------------------------------------------------------------------

	// Byte 0,1,2,3:
	unsigned long CsrcCount			: 4;	// The CSRC count contains the number of CSRC identifiers that
											// follow the fixed header.

	unsigned long Extension			: 1;	// If the extension bit is set, this fixed header is followed by
											// exactly one header extension, with a format defined in Section
											// 5.3.1.

	unsigned long Padding			: 1;	// If the padding bit is set, the packet contains one or more
											// additional padding octets at the end which are not part of the
											// payload. The last octet of the padding contains a count of how
											// many padding octets should be ignored. Padding may be needed by
											// some encryption algorithms with fixed block sizes or for
											// carrying several RTP packets in a lower-layer protocol data
											// unit.

	unsigned long Version			: 2;	// The version of RTP. must be set to 2.

	//-------------------------------------------------------------------------------------------------------

	unsigned long PayloadType		: 7;	// This field identifies the format of the RTP payload and
											// determines its interpretation by the application.

	unsigned long Marker			: 1;	// The interpretation of the marker is defined by a profile. It is
											// intended to allow significant events such as frame boundaries to
											// be marked in the packet stream. A profile may define additional
											// marker bits or specify that there is no marker bit by changing
											// the number of bits in the payload type field (see Section 5.3).

	//-------------------------------------------------------------------------------------------------------

	unsigned long Sequence			: 16;	// The sequence number increments by one for each RTP data packet
											// sent, and may be used by the receiver to detect packet loss and
											// to restore packet sequence. The initial value of the sequence
											// number is random (unpredictable) to make known-plaintext attacks
											// on encryption more difficult, even if the source itself does not
											// encrypt, because the packets may flow through a translator that
											// does. Techniques for choosing unpredictable numbers are
											// discussed in [7].

	//-------------------------------------------------------------------------------------------------------

	// Byte 4,5,6,7:
    unsigned long TimeStamp;				// The timestamp reflects the sampling instant of the first octet
											// in the RTP data packet.

	//-------------------------------------------------------------------------------------------------------

	// Byte 8,9,10,11:
    unsigned long SynchronizationSourceId;	// The SSRC field identifies the synchronization source. This
											// identifier is chosen randomly, with the intent that no two
											// synchronization sources within the same RTP session will have
											// the same SSRC identifier. 

	//-------------------------------------------------------------------------------------------------------

}RTP_HEADER;
#endif // #ifndef RTP_HEADER_DEFINED




// data struct passed to an application that has enabled raw RTP packet access.
//
typedef struct RAW_RTP_DATA
{
	// these values are passed from the media engine to the application.
	//
	int PhoneLine;								// the zero based phone line the RTP data is associated with.

	BOOL TransmittingPacket;					// non zero if the RTP packet is being transmitted.
												// zero if the RTP packet is being received.

	RTP_HEADER *pRtpHeader;						// address of the start of the raw RTP packet.The RTP header
												// resides at this location.

	unsigned long RtpHeaderLengthInBytes;		// the number of bytes in the RTP header.

	void *pSampleBuffer;						// The address of the RTP payload samples. An application
												// can modify these bytes directly as needed to perform media 
												// stream encryption.

	unsigned long SampleBufferLengthInBytes;	// the number of bytes in the sample buffer.

	unsigned long RtpPacketLengthInBytes;		// the total number of bytes in the RTP packet.

	unsigned long MaxRtpPacketLength;			// the max number of bytes of storage allocated for the RTP packet.
												// the value of this member is the same as the value specified
												// for the MaxRtpPacketLength startup parameter.

	void *pUserData;							// the user supplied callback data. this is the same value
												// that was specified when the RTP callback was registered.


	unsigned long NewRtpBufferLengthInBytes;	// the number of bytes in the "application updated" RTP buffer.
												// the application mus set this value if it modifies the RTP
												// packet for transmission. This value is not used when
												// receiving an RTP packet.

	BOOL ProcessRtpPacket;						// by default this value is set to non zero by the media
												// engine. an application can set this value to zero to
												// allow the media engine to ignore the RTP packet.
												//
												// This is useful if:
												//
												//	The application wants to filter incoming RTP packets
												//	for some reason. For example, detecting out of band
												//	DTMF payload RTP packets.
												//
												//	The application wants to perform a simple media mute
												//	function. In this case, the application would set this
												//	value to zero for all received and transmitted RTP
												//	packets for the duration of the mute.
												//

}RAW_RTP_DATA;


// data struct passed to an application when the media engine detects that RTP
// media for the call is not coming from the far end call endpoint as negotiated
// by SIP call setup. This data can be sent to the application if the media engine 
// application is communicating with SIP user agents and the UAs are behind thier own
// NATs.
//
typedef struct RECEIVED_RTP_MEDIA_CONFLICT
{
	int PhoneLine;								// the zero based phone line the RTP media
												// conflict is associated with.

	RTP_HEADER *pRtpHeader;						// address of the start of the raw RTP packet.The RTP header
												// resides at this location.

	unsigned long RtpHeaderLengthInBytes;		// the number of bytes in the RTP header.

	void *pSampleBuffer;						// The address of the RTP payload samples. An application
												// should not modify these bytes directly.

	unsigned long SampleBufferLengthInBytes;	// the number of bytes in the sample buffer.

	unsigned long RtpPacketLengthInBytes;		// the total number of bytes in the RTP packet.


	BYTE NegotiatedFarEndMediaIpAddress[NUM_BYTES_IP_ADDRESS];
	int NegotiatedFarEndMediaPort;

	BYTE DetectedFarEndMediaIpAddress[NUM_BYTES_IP_ADDRESS];
	int DetectedFarEndMediaPort;


	// values passed back to the media engine. when the app sets either
	// of these values, media will be sent to the combined far end ip:port.
	BYTE NewFarEndMediaIpAddress[NUM_BYTES_IP_ADDRESS];
	int NewFarEndMediaPort;

}RECEIVED_RTP_MEDIA_CONFLICT;


public enum class  LINE_MODE
{
	SWITCH_LINE					// the telephony engine does not apply telephony
								// line logic to any of the phone lines. all phone lines
								// act independently of one another.
								,PHONE_LINE
};



// structure passed back to application space when the media engine's
// event subscription mechanism is active. When applications receive this information,
// it represents the results of event subscriptions that were sent to targets
// you previously specified. In other words, we tried to subscribe to events offered by
// other devices/targets and this information gives us the result of the subscribe operation.
//
typedef struct SUBSCRIBE_RESULTS
{
	EVENT_SUBSCRIBE_HANDLE EventSubscribeHandle;	// the event subscription handle.

	char *pLocalPhoneName;				// the name of your local phone.

	char *pNameOfEventServer;			// the name of the server device offering events. if event subscriptions
										// are successful, this telephony device will send us events
										// of the specified name/type.

	char *pDestinationAddress;			// the address of the event server target.

	int DestinationPort;				// the network server port of the event server target.

	char *pEventName;					// the name of the event we want to subscribe to.

	char *pEventParameter;				// the optional event subscription parameter.

	DWORD SubscriptionIntervalSeconds;	// the number of seconds the event subscription will be active.
										// if unsubscribing from events, this value will be zero.

	TELEPHONY_RETURN_VALUE SipStatus;	// staus code describing the nature of this data. The value
										// can be any of the SipSubscriptionXXXXX values.

	int SipResponseCode;				// the actual SIP response code returned from the
										// destination of the subscribe operation.

}SUBSCRIBE_RESULTS;



// structure passed back to applications when the media engine receives
// event subscription requests.
//
typedef struct SUBSCRIBE_REQUEST
{
	char *pSrcUserName;			// the name of the user sending the event subscription. this
								// is the same as the username specified in the SIP "From:"
								// header.

	char *pContactUserName;		// the alternate name of the user sending the event subscription. this
								// is the same as the username specified in the SIP "Contact:"
								// header.

	char *pSrcHost;				// the host address of the sender.

	DWORD SrcPort;				// the port address of the sender.


	char *pEventName;			// the null terminated name of the event the "other end" wants
								// to subscribe to.

	char *pEventParameter;		// the null terminated event parameter string.

	DWORD ExpiresSeconds;		// the time duration the other device wants to receive event notifications.

	int AcceptRequest;			// application code should set this to a non zero value if
								// the event subscribe request is going to be accepted. if the
								// application does not want to accept the subscribe request, this
								// value must be set to zero.

	NOTIFY_HANDLE NotifyHandle;	// if your application decides to accept an event subscription, use
								// this handle to send your notifications. applications must close
								// this handle by calling CloseNotifyHandle when the subscription
								// is terminated by the far end or when the app terminates.

}SUBSCRIBE_REQUEST;



// Allows the media engine to specify the type of notify
// request being received.
//
public enum class  NOTIFY_TYPE
{
	SUBSCRIPTION_NOTIFY = 0,
	UNSOLICITED_NOTIFY

};


// structure passed back to applications when the media engine receives
// event notifications.
//
typedef struct NOTIFY_REQUEST
{
	NOTIFY_TYPE NotifyType;			// the notify request is the result of a subscription
									// or it is an unsolicited notify.

	char *pSrcUserName;				// the name of the device sending the event.

	char *pSrcHost;					// the host adddress of the sender.

	DWORD SrcPort;					// the port adddress of the sender.

	char *pEventName;				// the null terminated name of the event we are being notified about.

	char *pEventParameter;			// Depending on the sender of this event, the sender may chose to
									// supply this additional "user specified" event parameter string.
	
	char *pDestUserName;			// the name of the device receiving the event.

	char *pDestHost;				// the host address of the destination.

	DWORD DestPort;					// the port address of the destination.

	char *pSipMsgStr;				// the receive SIP message.

	int UnsolicitedNotifyResponse;	// Used only when receiving unsolicited NOTIFY requests.
									// Application software can set this value to a valid SIP 
									// response code. If the applications wants to inform
									// the sender that the NOTIFY was accepted, it should be set to 200.
									// If an error response is desired, it should be set in the range 
									// of 400-699 depending on the specific SIP error response desired.
									// Generally a 400 (Invalid Request) error response is acceptable.

}NOTIFY_REQUEST;


// structure passed back to applications when the media engine detects that
// its internal Wan IP address has been modified.
//
typedef struct WAN_IP_NOTIFICATION
{
	char *pWanIpAddress;		// the current Wan IP address in "x.x.x.x" format.
								// if the Wan IP address is being disabled, the IP
								// address returned will be an empty string.

}WAN_IP_NOTIFICATION;



// this structure is passed to the applications when the media engine receives
// an incoming call. The media engine uses this structure to communicate 
// with the application in order to assign the incoming call to an
// available phone line. For further information, see the SipIncomingCallAssignPhoneLine
// immediate event.
//
typedef struct ASSIGN_INCOMING_PHONE_LINE
{
	char *pReceivedSipInvite;	// the SIP INVITE message received. The application should
								// not attempt to modify this buffer.


	// Source information
	//
	char *pFromHeader;			// the raw SIP "From:" header.
	char *pSrcUrl;				// the SIP url associated with the originator of the phone call.
								// equivalent to information in an INVITE "From:" header.
	
	char *pSrcDisplayName;		// the display name name of the originator of the phone call.
	char *pSrcUserName;			// user name of the originator of the phone call. same as the user name in the SIP "From:" header.
	char *pSrcContactUserName;	// user name of the originating contact of the phone call. same as the user name in the SIP "Contact:" header.
	char *pSrcHost;				// host name of the originator of the phone call.
	DWORD SrcPort;				// the far end's network port used for the inbound call.


	char *pReceivedIpAddress;	// the detected ip address the far end used to originate the phone call.
	DWORD ReceivedPort;			// the detected port the far end used to originate the phone call.


	// Destination information
	//
	char *pToHeader;			// the raw SIP "To:" header.
	char *pDestUrl;				// the SIP url of the phone call's destination. If you are developing
								// a PSTN gateway, you can use this member to get the PSTN phone
								// number.

	char *pDestUserName;		// user name of the destination of the phone call.
	char *pDestHost;			// host of the destination of the phone call.


	char *pCallId;				// the call ID for the call.

	// Other information
	//
	char *pOriginatingEngineId;	// if the originator of the call is another LanScape VOIP Media Engine,
								// this is the unique ID of that media engine.



	// data returned to the media engine.
	//

	int AssignedPhoneLine;		// A -1 value is passed to the application. if the app
								// wants to tell the media engine what phone line to assign
								// the call to, the app can set this to the zero based index
								// of the phone line to use.

	BOOL TerminateIncomingCall;	// If the application sets this valuse to non zero, the incoming
								// call will be terminated immediately and the far end of the call
								// will be sent the response as specified by the SipResponseCode
								// member value.


	int SipResponseCode;		// a user specified response code that will be sent to the
								// originating side of the call. The following values can
								// be specified:
								//		
								//		400		-	"Invalid Request"
								//		402		-	"Payment Required"
								//		403		-	"Forbidden"
								//		404		-	"Not Found"
								//		405		-	"Method Not Allowed"
								//		406		-	"Not Acceptable"
								//		409		-	"Conflict"
								//		410		-	"Gone"
								//		420		-	"Bad Extension"
								//		481		-	"Transaction Does Not Exist
								//		485		-	"Ambiguous"
								//		486		-	"Busy Here"
								//		488		-	"Not Acceptable Here"
								//

}ASSIGN_INCOMING_PHONE_LINE;


// structure passed to the applications when the media engine receives
// an incoming call and the incoming call setup initialization has been
// performed. For further information, see the SipIncomingCallInitializing
// immediate event.
//
typedef struct INCOMING_CALL_INITIALIZED_DATA
{
	char *pReceivedSipInvite;	// the SIP INVITE message received. The application should
								// not attempt to modify this buffer.


	// data returned to the media engine.
	//
	BOOL TerminateIncomingCall;	// If the application sets this valuse to non zero, the incoming
								// call will be terminated immediately and the far end of the call
								// will be sent the response as specified by the SipResponseCode
								// member value.


	int SipResponseCode;		// a user specified response code that will be sent to the
								// originating side of the call. The following values can
								// be specified:
								//		
								//		400		-	"Invalid Request"
								//		402		-	"Payment Required"
								//		403		-	"Forbidden"
								//		404		-	"Not Found"
								//		405		-	"Method Not Allowed"
								//		406		-	"Not Acceptable"
								//		409		-	"Conflict"
								//		410		-	"Gone"
								//		420		-	"Bad Extension"
								//		481		-	"Transaction Does Not Exist
								//		485		-	"Ambiguous"
								//		486		-	"Busy Here"
								//		488		-	"Not Acceptable Here"
								//


}INCOMING_CALL_INITIALIZED_DATA;




#define MAX_SIP_IMMEDIATE_DESTINATION		255

// structure passed to the applications when the media engine receives
// or is ready to transmit a SIP mesage. The application can use the 
// information in this structure to modify or ignore the received/transmiting
// SIP message when the application processes the SipModifySipmessage event.
//
typedef struct SIP_MESSAGE_IMMEDIATE_DATA
{
	char **ppSipMsg;			// the address of the SIP message buffer pointer.
								// not attempt to modify this buffer directly.

	BOOL Received;				// if non zero, the SIP message is being received by the 
								// Media Engine. if zero, the SIP message is ready to be
								// transmitted by the Media Engine.


	// the following values can be set by application software
	//

	BOOL IgnoreSipMessage;		// Must be set by application software. if set to non zero,
								// the received/transmitted SIP message
								// will be ignored (thrown away).

	char NewDestination[MAX_SIP_IMMEDIATE_DESTINATION + 1];		// for SIP messages that are ready to be transmitted, application
																// software can set this value to a host name or IP address
																// of where the SIP message should be sent.

	int NewDestinationPort;		// for SIP messages that are ready to be transmitted, application
								// software can set this value to the destination port
								// of where the SIP message should be sent. This value is ignored if
								// the NewDestination member is not set by the application.




}SIP_MESSAGE_IMMEDIATE_DATA;



// structure passed to the applications when the media engine detects
// a parse error associated with a received SIP message. Passed to
// application software via the SipReceivedSipMessageParseError immediate
// event.
//
typedef struct SIP_MESSAGE_PARSE_IMMEDIATE_DATA
{
	char *pSipMsg;					// the receive SIP mesage that caused the parse error.

	char *pIpAddressOfSender;		// the source IP address where the SIP message originated.
									// this is a string containing a dotted decimal IP address
									// such as x.x.x.x

	DWORD PortOfSender;				// the source UDP port where the SIP message originated.


}SIP_MESSAGE_PARSE_IMMEDIATE_DATA;



#define MAX_CHALLENGE_USER_NAME_LENGTH				63
#define MAX_CHALLENGE_REALM_NAME_LENGTH				127
#define MAX_CHALLENGE_ERROR_MSG_LENGTH				63


// Authentication operations the media engine tells your app about.
//
public enum class  AUTHENTICATE_OPERATION
{
	AUTHENTICATE_UNDEFINED = 0,

	AUTHENTICATE_INCOMING_MESSAGE,
	AUTHENTICATE_VERIFY_CREDENTIALS,
	AUTHENTICATE_BAD_CREDENTIALS_RECEIVED
	

};


// SIP mesage type your application can chose to challenge.
//
public enum class  AUTHENTICATE_MESSAGE_TYPE
{
	MESSAGE_UNDEFINED = 0,

	MESSAGE_REGISTER,
	MESSAGE_INVITE,
	MESSAGE_BYE,
	MESSAGE_SUBSCRIBE,
	MESSAGE_NOTIFY


};


public enum class  CHALLENGE_MODE
{
	CHALLENGE_MODE_NONE = 0,				// do not perform a message challenge.

	CHALLENGE_MODE_WWW_AUTHENTICATE,		// use this for most application related challenges.
											// i.e. your application is a soft phone, voicemail
											// server, etc.

	CHALLENGE_MODE_PROXY_AUTHENTICATE		// use this if your intended application type is a
											// media bridge/media proxy or other similar
											// type application.

};


public enum class  CHALLENGE_TYPE
{
	CHALLENGE_TYPE_UNDEFINED = 0,

	CHALLENGE_TYPE_DIGEST					// inform the far end that your app wants to perform
											// a Digest authentication.

};


public enum class  CHALLENGE_ALGORITHM
{
	CHALLENGE_ALGORITHM_UNDEFINED = 0,

	CHALLENGE_ALGORITHM_MD5					// MD5 Digest authentication. used by all SIP
											// enabled devices.

};



typedef struct CHALLENGE_AUTHENTICATION
{
	AUTHENTICATE_OPERATION Operation;		// Direction: Application <== Media Engine.
											//
											// This value is set by the media engine to
											// inform your app of one of the the following:
											//
											//		An incoming SIP mesage was received and the media
											//		engine wants to know if your app wants to challenge
											//		the message. In this case, the value of this member
											//		will be set to AUTHENTICATE_INCOMING_MESSAGE and the
											//		message type received will be specified by the MesageType
											//		member. If your app wants to challenge the SIP message,
											//		it should set the ChallengeMode and ChallengeType members to
											//		the appropriate values for your application. If your app does
											//		not want to challenge the SIP message, it should set the
											//		ChallengeMode to either WWW or proxy authentication,
											//		ChallengeType to CHALLENGE_TYPE_NONE. If your app challenges
											//		the SIP message, it also must specify a challenge algoritm
											//		that will be used by the media engine. It is recommended
											//		that the Algorithm member be set to CHALLENGE_ALGORITHM_MD5.
											//		The far end also requires a realm to challenge so your app
											//		must also set the ChallengeRealm member.
											//
											//		- OR -
											//
											//		An incoming SIP mesage was received that contains
											//		authorization credentials and the media engine is
											//		asking your application to validate the credentials.
											//		In this case, the value of this member will be set
											//		to AUTHENTICATE_VERIFY_CREDENTIALS. The ChallengeRealm
											//		member will contain the authorization realm the far end
											//		device specified and the hChallenge member will be a handle
											//		to internal challenge information the far end specified.
											//


	AUTHENTICATE_MESSAGE_TYPE MesageType;	// Direction: Application <== Media Engine.
											//
											//		the media engine uses this value to inform your app of
											//		the message type being received.

	CHALLENGE_MODE ChallengeMode;			// Direction: Application ==> Media Engine.
											//
											//		Allows the application to specify either WWW or Proxy challenge.
											//

	CHALLENGE_TYPE ChallengeType;			// Direction: Application ==> Media Engine.
											//
											//		Allows the application to specify the challenge type to
											//		perform. Crrently only Digest authentication is supported.
											//
											//



	CHALLENGE_ALGORITHM Algorithm;			// Direction: Application ==> Media Engine.
											//
											//		Allows the application to specify the challenge algoritm
											//		to use for the challenge.
											//

	char ChallengeUserName[MAX_CHALLENGE_USER_NAME_LENGTH + 1];

											// Direction: Application <== Media Engine.
											//
											//		The media engine uses this member to inform the application
											//		about the user name of the challenge. Each incoming SIP message
											//		that contains challenge credentials has a user name specified.
											//		The application can use this value to look up the appropriate
											//		authorization password for the specified user. This way
											//		an application can support multiple authenticated users.
											//

	char ChallengeRealm[MAX_CHALLENGE_REALM_NAME_LENGTH + 1];

											// Direction: Application <== Media Engine.
											//
											//		The media engine uses this member to inform the application
											//		about the realm of the challenge. Each incoming SIP message
											//		that contains challenge credentials has a realm specified.
											//		The application can use this value to look up the appropriate
											//		authorization password for the specified realm. This way
											//		an application can support multiple authentication realms.
											//

	SIPHANDLE hStateMachine;				// Direction: Application <== Media Engine.
											//
											//		The handle to the telephony engine.
											//

	CHALLENGE_HANDLE hChallenge;			// Direction: Application <== Media Engine.
											//
											//		The application should use this handle when verifying
											//		challenge response credentials. The application can pass to the
											//		telephony API procedure VerifyChallengeResponse() this handle,
											//		the realm given by the ChallengeRealm member and the
											//		authentication password that the app maintains for the
											//		realm.
											//

	BOOL AuthorizationGranted;				// Direction: Application ==> Media Engine.
											//
											//		When the application make the determination that an
											//		authorization can be granted, it should set this member
											//		to a non zero value to allow the media engine to accept
											//		the challenge response from the far end SIP device.
											//
											//

	char ErrorMessage[MAX_CHALLENGE_ERROR_MSG_LENGTH + 1];

											// Direction: Application <== Media Engine.
											//
											//		The media engine uses this member to inform the application
											//		about received authentication credential errors. When the Operation
											//		member is set to AUTHENTICATE_BAD_CREDENTIALS_RECEIVED, this
											//		text buffer will contain an appropriate error message describing
											//		why the received credentials were identified as having an error.
											//


}CHALLENGE_AUTHENTICATION;


// structure used to pass back challenge error information to the app.
//
typedef struct CHALLENGE_ERROR_DATA
{
	char *pRealm;				// the realm of the far end that is challenging us.

	char *pChallengeType;		// set to an asciiz string that describes the challenge
								// type the far end is requesting. This is normally
								// set to type "Digest".
	
}CHALLENGE_ERROR_DATA;


// data passed to applications when receiving registration requests.
//
typedef struct REGISTER_DETAILS
{
	char *pRegistrationDomain;		// the domain name for the registration.

	char *pToName;					// the name of the user agent to register
	char *pToHost;					// the name or ip address of the user agent to register
	char *pToPort;					// the port of the user agent to register

	char *pContactName;				// the contact name being registered as specified by the far end UA.
	char *pContactHost;				// the contact host being registered as specified by the far end UA.
	char *pContactPort;				// the contact port being registered as specified by the far end UA.

	char *pReceivedFromIpAddress;	// the actual ip address the register request was received from.
	char *pReceivedFromPort;		// the actual port the register request was received from.

	char *pExpiresTimeSeconds;		// the time in seconds of the registration interval.
	
	char *pRawSipMesage;			// a pointer to the received sip message.

	BOOL AcceptRegistration;		// the app should set this member to TRUE to accept the
									// registration.

}REGISTER_DETAILS;



typedef struct TELPHONY_ENGINE_VERSION_INFO
{
	char *pMediaEngineVersion;
	char *pDspMicroCodeVersion;
	char *pAudioEngineVersion;	
	char *pAudioMixerVersion;	
	char *pIvrStreamingMediaVersion;	
	char *pSipStackVersion;	
	char *pRtpStackVersion;	

}TELPHONY_ENGINE_VERSION_INFO;



// In-band DTMF encoder/decoder
//

typedef void *			HDTMFDECODER;
typedef void *			HDTMFGENERATOR;
typedef void *			HWAVEFILE;

#define MAX_DTMF_DIGITS							16
#define MAX_DTMF_AMPLITUDE						32767
#define MAX_DOUBLE								1.7E+308


// Defines the supported tones of the generator and decoder.
//
public enum class  DTMF_TONE
{
	DtmfTone1 = 0,
	DtmfTone2,
	DtmfTone3,
	DtmfToneA,
	DtmfTone4,
	DtmfTone5,
	DtmfTone6,
	DtmfToneB,
	DtmfTone7,
	DtmfTone8,
	DtmfTone9,
	DtmfToneC,
	DtmfToneAsterisk,
	DtmfTone0,
	DtmfTonePound,
	DtmfToneD,
	DtmfToneUndefined

};


typedef struct DTMF_DETECT_DATA
{
	void *pUserSpecifiedData;
	DTMF_TONE DtmfTone;
	BOOL StateOn;

}DTMF_DETECT_DATA;


typedef struct DTMF_GEN_DATA
{
	void *pUserSpecifiedData;

	BYTE *pSampleBuffer;

	int LengthInBytes;

}DTMF_GEN_DATA;



// Frequency ratio table for inclusion into source code.
// Used to tune the DTMF detector.
//
typedef struct FREQUENCY_RATIO_TABLE
{
	int LowFreqRatio;
	int HighFreqRatio;

}FREQUENCY_RATIO_TABLE;



// Magnitude envelope table for inclusion into source code.
// Used to tune the DTMF detector.
//
typedef struct FREQUENCY_MAGNITUDE_TABLE
{
	int MinMagFundamentaFreq1;
	int MaxMagFundamentaFreq1;
	int MinMagFundamentaFreq2;
	int MaxMagFundamentaFreq2;
	int MinMag2ndHarmonicFreq1;
	int MaxMag2ndHarmonicFreq1;
	int MinMag2ndHarmonicFreq2;
	int MaxMag2ndHarmonicFreq2;

}FREQUENCY_MAGNITUDE_TABLE;


typedef struct FREQUENCY_RATIOS
{
	BOOL LowTriggered;
	BOOL HighTriggered;

	double LowFreqRatio;	// The ratio of freq 1 fundamental to the 2nd harmonic.

	double HighFreqRatio;	// The ratio of freq 2 fundamental to the 2nd harmonic.

}FREQUENCY_RATIOS;


typedef struct DTMF_MAGNITUDES
{
	double MagFundamentalFreq1;

	double MagFundamentalFreq2;

	double MagSecondHarmonicFreq1;

	double MagSecondHarmonicFreq2;

}DTMF_MAGNITUDES;



// Callback procedures.
//
typedef void (VOIP_API *SIPCALLBACKPROC)(SIPHANDLE hStateMachine, SIP_NOTIFY_TYPE NotifyType, int PhoneLine, TELEPHONY_RETURN_VALUE TelephonyEvent, void *pUserDefinedData, void *pEventData);
typedef void (VOIP_API *IVRCALLBACKPROC)(IVR_RECOGNITION_DATA *pIvrRecognitionData);
typedef void (VOIP_API *SPEECH_RECOGNITION_CALLBACK_PROC)(SPEECH_RECOGNITION_DATA *pSpeechRecognitionData);
typedef void (VOIP_API *PHONE_LINE_RECORD_CALLBACK_PROC)(PHONE_LINE_RECORD_DATA *pPhoneLineRecordData);
typedef void (VOIP_API *PHONE_LINE_VU_METER_CALLBACK_PROC)(PHONE_LINE_VU_METER_DATA *pPhoneLineVuMeterData);
typedef void (VOIP_API *RTP_CALLBACK_PROC)(RAW_RTP_DATA *pRawRtpData);
typedef void (VOIP_API *DTMF_DECODER_CALLBACK_PROC)(DTMF_DETECT_DATA *pDtmfDetectData);
typedef void (VOIP_API *DTMF_GENERATOR_CALLBACK_PROC)(DTMF_GEN_DATA *pGeneratorData);



// this structure is used to specify startup conditions for
// the VOIP Media Engine.
//
typedef struct START_SIP_TELEPHONY_PARAMS
{
	unsigned char *pPersonalityMicrocode;
	unsigned int NumPhoneLinesRequested;
	LINE_MODE LineMode;
	SIPCALLBACKPROC UserNotifyCallbackProc;
	void *pUserDefinedData;
	BYTE IpAddressOfThisHost[NUM_BYTES_IP_ADDRESS];
	int SipPort;
	int MaxSipMesageLength;
	DWORD SipUdpReceiveBufferSizeInBytes;
	DWORD SipUdpTransmitBufferSizeInBytes;
	int MaxSipMessageReceiveFifoLength;
	int MaxRtpPacketLength;
	char *pPhoneName;
	char *pPhoneDisplayName;
	BOOL CallConferenceEnabled;
	BOOL FarEndCallTransferEnabled;
	BOOL RandomlyAssignIncomingCallsToPhoneLines;
	int MinLocalRtpPort;
	int MaxLocalRtpPort;
	BOOL UseSequentialRtpPorts;
	int ZeroBasedAudioInDeviceId;
	int ZeroBasedAudioOutDeviceId;
	AUDIO_BANDWIDTH AudioRecordBandWidth;
	AUDIO_BANDWIDTH AudioPlaybackBandWidth;
	int PlaybackBufferingDefault;
	int PlaybackBufferingDuringSounds;
	int PhoneLineTransmitBuffering;
	BOOL LogSipMessages;
	char *pSipLogFileName;
	BOOL EnableEventLogServers;
	char *pEventLogServerList;
	char *pEventLogServerPortList;
	BOOL EnablePhoneLineRecording;
	int PhoneLineRecordBuffering;
	int MaxMixerLinebuffers;
	BOOL SendLineInitializedEvents;
	ULONG StartupFlags;							// Note: ignored when performing a restart operation.

}START_SIP_TELEPHONY_PARAMS;



#ifdef __cplusplus
extern "C"
{
#endif

// LanScape Media Engine Telephony API.
//
TELEPHONY_RETURN_VALUE VOIP_API InitializeMediaEngine(void *pData, DWORD ThreadStackSizeInBytes);
TELEPHONY_RETURN_VALUE VOIP_API UnInitializeMediaEngine(void);
TELEPHONY_RETURN_VALUE VOIP_API StartSipTelephony(START_SIP_TELEPHONY_PARAMS *pStartupParams, SIPHANDLE *pStateMachineHandle);
TELEPHONY_RETURN_VALUE VOIP_API StopSipTelephony(SIPHANDLE hStateMachine);
TELEPHONY_RETURN_VALUE VOIP_API ReStartSipTelephony(START_SIP_TELEPHONY_PARAMS *pStartupParams, SIPHANDLE *pStateMachineHandle);
TELEPHONY_RETURN_VALUE VOIP_API SipTelephonyEnable(SIPHANDLE hStateMachine);
TELEPHONY_RETURN_VALUE VOIP_API MakeCall(SIPHANDLE hStateMachine, char *pUserNameOrPhoneNumber,
									char *pDestinationAddress, int DestinationSipPort,
										 int PhoneLine, BOOL Synchronous, DWORD TimeOutMs);
TELEPHONY_RETURN_VALUE VOIP_API MakeCallUri(SIPHANDLE hStateMachine, char *pSipUri,BOOL UseProxy,
									int PhoneLine, BOOL Synchronous, DWORD TimeOutMs);
TELEPHONY_RETURN_VALUE VOIP_API TerminateCall(SIPHANDLE hStateMachine, int PhoneLine, BOOL Synchronous, DWORD TimeOutMs);
TELEPHONY_RETURN_VALUE VOIP_API GetNumActiveCalls(SIPHANDLE hStateMachine, int *pNumActiveCalls);
TELEPHONY_RETURN_VALUE VOIP_API SetCallAnswerTimeout(SIPHANDLE hStateMachine, DWORD CallAnswerTimeoutSeconds);
TELEPHONY_RETURN_VALUE VOIP_API GoOffHook(SIPHANDLE hStateMachine, int PhoneLine);
TELEPHONY_RETURN_VALUE VOIP_API AbortIncomingCall(SIPHANDLE hStateMachine, int PhoneLine, int AbortSipStatusCode, char *pAbortSipReasonPhrase);
TELEPHONY_RETURN_VALUE VOIP_API SetPhoneName(SIPHANDLE hStateMachine, int PhoneLine, char *pPhoneName);
TELEPHONY_RETURN_VALUE VOIP_API SetFromHeaderUserName(SIPHANDLE hStateMachine, int PhoneLine, char *pFromHeaderUserName);
TELEPHONY_RETURN_VALUE VOIP_API SetAudioMediaFormat(SIPHANDLE hStateMachine, int PhoneLine, MEDIA_FORMAT_AUDIO MediaFormat);
TELEPHONY_RETURN_VALUE VOIP_API SetAudioMediaFormats(SIPHANDLE hStateMachine, int PhoneLine, int NumMediaFormats, MEDIA_FORMAT_AUDIO *pMediaFormats);
TELEPHONY_RETURN_VALUE VOIP_API GetIncomingCallInfo(SIPHANDLE hStateMachine, int PhoneLine, SIP_INCOMING_CALL_INFO *pIncomingCallInfo);
TELEPHONY_RETURN_VALUE VOIP_API GetOutgoingCallInfo(SIPHANDLE hStateMachine, int PhoneLine,SIP_OUTGOING_CALL_INFO *pOutgoingCallInfo);
TELEPHONY_RETURN_VALUE VOIP_API GetOutgoingCallErrorInfo(SIPHANDLE hStateMachine, int PhoneLine,SIP_OUTGOING_CALL_ERROR_INFO *pOutgoingCallErrorInfo);
TELEPHONY_RETURN_VALUE VOIP_API GetActiveCallInfo(SIPHANDLE hStateMachine, int PhoneLine, SIP_ACTIVE_CALL_INFO *pActiveCallInfo);
TELEPHONY_RETURN_VALUE VOIP_API GetNumberOfActiveCalls(SIPHANDLE hStateMachine, int *pNumberOfActiveCalls);
TELEPHONY_RETURN_VALUE VOIP_API SetLocalAudioLoopback(SIPHANDLE hStateMachine, BOOL EnableState);
TELEPHONY_RETURN_VALUE VOIP_API GetCallInstanceDataLength(SIPHANDLE hStateMachine, int PhoneLine, int *pUserCallInstanceLength);
TELEPHONY_RETURN_VALUE VOIP_API GetCallInstanceData(SIPHANDLE hStateMachine, int PhoneLine, void *pUserCallInstanceData);
TELEPHONY_RETURN_VALUE VOIP_API SetCallInstanceData(SIPHANDLE hStateMachine, int PhoneLine, void *pUserCallInstanceData, int UserCallInstanceLength);
TELEPHONY_RETURN_VALUE VOIP_API EnableDialTone(SIPHANDLE hStateMachine, BOOL EnableState);
TELEPHONY_RETURN_VALUE VOIP_API EnableDtmfDigits(SIPHANDLE hStateMachine, BOOL EnableState);
TELEPHONY_RETURN_VALUE VOIP_API EnableOutgoingPhoneRing(SIPHANDLE hStateMachine, BOOL EnableState);
TELEPHONY_RETURN_VALUE VOIP_API EnableBusySignal(SIPHANDLE hStateMachine, BOOL EnableState);
TELEPHONY_RETURN_VALUE VOIP_API EnableErrorSignal(SIPHANDLE hStateMachine, BOOL EnableState);
TELEPHONY_RETURN_VALUE VOIP_API EnableIncomingPhoneRing(SIPHANDLE hStateMachine, BOOL EnableState);
TELEPHONY_RETURN_VALUE VOIP_API EnableSipProxyServer(SIPHANDLE hStateMachine, char *pSipProxyAddress, DWORD SipProxyPort);
TELEPHONY_RETURN_VALUE VOIP_API DisableSipProxyServer(SIPHANDLE hStateMachine);
TELEPHONY_RETURN_VALUE VOIP_API EnableOutboundSipProxyServer(SIPHANDLE hStateMachine, char *pOutboundSipProxyAddress, DWORD OutboundSipProxyPort);
TELEPHONY_RETURN_VALUE VOIP_API DisableOutboundSipProxyServer(SIPHANDLE hStateMachine);
TELEPHONY_RETURN_VALUE VOIP_API SetCallTerminateTimeout(SIPHANDLE hStateMachine, DWORD CallTerminateTimeoutMs);
TELEPHONY_RETURN_VALUE VOIP_API SetInboundInviteAckTimeout(SIPHANDLE hStateMachine, DWORD InboundInviteAckTimeoutMs);
TELEPHONY_RETURN_VALUE VOIP_API SetReinviteTimeout(SIPHANDLE hStateMachine, DWORD ReinviteTimeoutMs);
TELEPHONY_RETURN_VALUE VOIP_API ConnectIncomingCallWithoutInviteAck(SIPHANDLE hStateMachine, BOOL EnableState);
TELEPHONY_RETURN_VALUE VOIP_API SetOutgoingLineOnly(SIPHANDLE hStateMachine, int PhoneLine, BOOL EnableState);
TELEPHONY_RETURN_VALUE VOIP_API SetCallHoldType(SIPHANDLE hStateMachine, CALL_HOLD_TYPE CallHoldType);
TELEPHONY_RETURN_VALUE VOIP_API GetNumPhoneLines(SIPHANDLE hStateMachine, int *pNumPhoneLines);
TELEPHONY_RETURN_VALUE VOIP_API GetMaxNumPhoneLines(SIPHANDLE hStateMachine, int *pMaxNumPhoneLines);
TELEPHONY_RETURN_VALUE VOIP_API GetLineStatus(SIPHANDLE hStateMachine, int PhoneLine, LINE_STATE *pLineState);
TELEPHONY_RETURN_VALUE VOIP_API BusyOutLine(SIPHANDLE hStateMachine, int PhoneLine, BOOL BusyOutState);
TELEPHONY_RETURN_VALUE VOIP_API HoldLine(SIPHANDLE hStateMachine, int PhoneLine, BOOL HoldState);
TELEPHONY_RETURN_VALUE VOIP_API ConferenceLine(SIPHANDLE hStateMachine, int PhoneLine, BOOL ConferenceState);
TELEPHONY_RETURN_VALUE VOIP_API SetConferenceGroupIds(SIPHANDLE hStateMachine, int PhoneLine, int NumConferenceGroupIds, char **ppConferenceGroupIds);
TELEPHONY_RETURN_VALUE VOIP_API TransferLineUri(SIPHANDLE hStateMachine, char *pSipUri, BOOL UseProxy, int PhoneLine);
TELEPHONY_RETURN_VALUE VOIP_API TransferLine(SIPHANDLE hStateMachine, char *pUserNameOrPhoneNumber, char *pDestinationAddress,
										int DestinationSipPort, int PhoneLine);


// Registrar Support.
//
TELEPHONY_RETURN_VALUE VOIP_API EnableSipRegisterServer(
		SIPHANDLE hStateMachine,
		char *pNamesToRegister,
		BOOL RegisterPhoneLines,
		BOOL RegisterThroughProxy,
		char *pRegistrationServerAddress,
		DWORD RegistrationServerPort,
		DWORD RegistrationUpdateIntervalSeconds,
		DWORD RegistrationExpireTimeSeconds,
		DWORD TimeOutMs,
		BOOL RegisterNatAutoDetectEnabled
		);

TELEPHONY_RETURN_VALUE VOIP_API TriggerRegistration(SIPHANDLE hStateMachine);
TELEPHONY_RETURN_VALUE VOIP_API RegistationErrorRetryTime(SIPHANDLE hStateMachine, DWORD RegistationErrorRetryTimeMs);
TELEPHONY_RETURN_VALUE VOIP_API DisableSipRegisterServer(SIPHANDLE hStateMachine);


TELEPHONY_RETURN_VALUE VOIP_API EnableSipDomain(SIPHANDLE hStateMachine, char *pSipDomainName);
TELEPHONY_RETURN_VALUE VOIP_API DisableSipDomain(SIPHANDLE hStateMachine);
TELEPHONY_RETURN_VALUE VOIP_API SetInCallProcessPriority(SIPHANDLE hStateMachine, IN_CALL_PROCESS_PRIORITY InCallProcessPriority);
TELEPHONY_RETURN_VALUE VOIP_API SetWanIpAddress(SIPHANDLE hStateMachine, char *pIpAddressStr);


// Audio subsystem noise discrimination/control
//
TELEPHONY_RETURN_VALUE VOIP_API GetMinNoiseThreshold(SIPHANDLE hStateMachine, DWORD *pRet);
TELEPHONY_RETURN_VALUE VOIP_API GetMaxNoiseThreshold(SIPHANDLE hStateMachine, DWORD *pRet);
TELEPHONY_RETURN_VALUE VOIP_API GetNoiseThreshold(SIPHANDLE hStateMachine, DWORD *pRet);
TELEPHONY_RETURN_VALUE VOIP_API GetNoiseDiscriminationEnableState(SIPHANDLE hStateMachine, BOOL *pRet);
TELEPHONY_RETURN_VALUE VOIP_API GetSilenceDecay(SIPHANDLE hStateMachine, DWORD *pRet);
TELEPHONY_RETURN_VALUE VOIP_API SetNoiseThreshold(SIPHANDLE hStateMachine, DWORD NoiseThreshold);
TELEPHONY_RETURN_VALUE VOIP_API SetNoiseDiscriminationEnableState(SIPHANDLE hStateMachine, BOOL NoiseDiscriminationEnableState);
TELEPHONY_RETURN_VALUE VOIP_API SetSilenceDecay(SIPHANDLE hStateMachine, DWORD SilenceDecay);
TELEPHONY_RETURN_VALUE VOIP_API GetJitterTimeMs(SIPHANDLE hStateMachine, int PhoneLine, DWORD *pRet);
TELEPHONY_RETURN_VALUE VOIP_API GetEnableJitter(SIPHANDLE hStateMachine, int PhoneLine, BOOL *pRet);
TELEPHONY_RETURN_VALUE VOIP_API SetJitterTimeMs(SIPHANDLE hStateMachine, int PhoneLine, DWORD JitterTimeMs);
TELEPHONY_RETURN_VALUE VOIP_API SetEnableJitter(SIPHANDLE hStateMachine, int PhoneLine, BOOL EnableState);


// Digital audio In/Out device support.
//
int VOIP_API GetNumDigitalAudioOutputDevices(void);
BOOL VOIP_API GetDigitalAudioOutputDevice(int ZeroBasedDeviceIndex, WAVEOUTCAPS *pWAVEOUTCAPS);
int VOIP_API GetNumDigitalAudioInputDevices(void);
BOOL VOIP_API GetDigitalAudioInputDevice(int ZeroBasedDeviceIndex, WAVEINCAPS *pWAVEINCAPS);


// Speech engine support.
//
TELEPHONY_RETURN_VALUE VOIP_API SetSpeechRecognitionCallback(
					SIPHANDLE hStateMachine,
					AUDIO_BANDWIDTH SpeechEnginebandwidth,
					SPEECH_RECOGNITION_CALLBACK_PROC pSpeechBufferReadyCallback,
					void *pUserData);


// IVR Receiver support on a per phone line basis.
//
TELEPHONY_RETURN_VALUE VOIP_API OpenRxIvrChannel(
			SIPHANDLE hStateMachine,
			int PhoneLine,
			IVRCALLBACKPROC pRxIvrCallback,
			void *pRxIvrCallbackUserData,
			BOOL PerformConversion,
			AUDIO_BANDWIDTH ReceiveIvrDataType,
			IVRRXHANDLE *pIvrRxHandle,
			int *pSamplesPerIvrBuffer,
			int *pBytesPerIvrBuffer,
			BOOL *pSamplesInByteArray
			);

TELEPHONY_RETURN_VALUE VOIP_API CloseRxIvrChannel(IVRRXHANDLE IvrRxHandle);


// IVR Transmitter support on a per phone line basis.
//
TELEPHONY_RETURN_VALUE VOIP_API OpenTxIvrChannel(SIPHANDLE hStateMachine, int PhoneLine, int ChannelNumber, IVRTXHANDLE *pReturnedIvrTxHandle);
TELEPHONY_RETURN_VALUE VOIP_API CloseTxIvrChannel(IVRTXHANDLE IvrTxHandle);
TELEPHONY_RETURN_VALUE VOIP_API SetTxIvrBufferEvent(IVRTXHANDLE IvrTxHandle, HANDLE TransmitBufferAvailableEvent);
TELEPHONY_RETURN_VALUE VOIP_API SetTxIvrDataType(IVRTXHANDLE IvrTxHandle, AUDIO_BANDWIDTH TransmitIvrDataType);

TELEPHONY_RETURN_VALUE VOIP_API GetTxIvrSampleBlockSize(
			IVRTXHANDLE IvrTxHandle, 
			AUDIO_BANDWIDTH TransmitIvrDataType,
			int *pSamplesPerIvrBuffer, 
			int *pBytesPerIvrBuffer,
			BOOL *pSamplesInByteArray
			);

TELEPHONY_RETURN_VALUE VOIP_API GetNumIvrTxBuffers(IVRTXHANDLE IvrTxHandle, int *pNumIvrTxBuffers);
TELEPHONY_RETURN_VALUE VOIP_API TransmitInCallIvrData(IVRTXHANDLE IvrTxHandle, void *pSampleBuffer);
TELEPHONY_RETURN_VALUE VOIP_API TransmitOnHoldIvrData(IVRTXHANDLE IvrTxHandle, void *pSampleBuffer);
TELEPHONY_RETURN_VALUE VOIP_API WaitForIvrTransmitComplete(IVRTXHANDLE IvrTxHandle);
TELEPHONY_RETURN_VALUE VOIP_API StopIvrTransmit(IVRTXHANDLE IvrTxHandle);


// Audio Output support.
//
TELEPHONY_RETURN_VALUE VOIP_API OpenAudioOutChannel(SIPHANDLE hStateMachine, int AudioOutLine, AUDIOHANDLE *phAudioOut);
TELEPHONY_RETURN_VALUE VOIP_API CloseAudioOutChannel(AUDIOHANDLE hAudioOut);
TELEPHONY_RETURN_VALUE VOIP_API SetAudioOutBufferEvent(AUDIOHANDLE hAudioOut, HANDLE TransmitBufferAvailableEvent);
TELEPHONY_RETURN_VALUE VOIP_API SetAudioOutDataType(AUDIOHANDLE hAudioOut, AUDIO_BANDWIDTH AudioOutDataType);
TELEPHONY_RETURN_VALUE VOIP_API GetAudioOutSampleBlockSize(AUDIOHANDLE hAudioOut, int *pSamplesPerAudioOutBuffer,
															int *pBytesPerAudioOutBuffer, BOOL *pSamplesInByteArray);
TELEPHONY_RETURN_VALUE VOIP_API GetNumAudioOutBuffers(AUDIOHANDLE hAudioOut, int *pNumAudioOutBuffers);
TELEPHONY_RETURN_VALUE VOIP_API WaitForAudioOutComplete(AUDIOHANDLE hAudioOut);
TELEPHONY_RETURN_VALUE VOIP_API StopAudioOutput(AUDIOHANDLE hAudioOut);
TELEPHONY_RETURN_VALUE VOIP_API WriteAudioOutData(AUDIOHANDLE hAudioOut, void *pSampleBuffer);


// Volume support.
//
TELEPHONY_RETURN_VALUE VOIP_API SetLocalAudioLoopbackVolume(SIPHANDLE hStateMachine, int Volume);
TELEPHONY_RETURN_VALUE VOIP_API SetPhoneLineVolume(SIPHANDLE hStateMachine, int PhoneLine, int RxVolume, int TxVolume);


// Phone Line loopback.
//
TELEPHONY_RETURN_VALUE  VOIP_API SetPhoneLineLoopBack(SIPHANDLE hStateMachine, int PhoneLine, BOOL LoopBackEnabled);
TELEPHONY_RETURN_VALUE  VOIP_API GetPhoneLineLoopBack(SIPHANDLE hStateMachine, int PhoneLine, BOOL *pLoopBackEnabled);


// Telephony Engine version information.
//
BOOL VOIP_API GetMediaEngineVersionInfo(TELPHONY_ENGINE_VERSION_INFO *pVersionInfo);


// Telephony Engine subscribe event interface.
//
TELEPHONY_RETURN_VALUE VOIP_API StartEventSubscription(
	SIPHANDLE hStateMachine,
	char *pLocalPhoneName,
	char *pDestUserNameOrPhoneNumber,
	char *pDestinationAddress,
	int DestinationSipPort,
	BOOL SubscribeThroughProxy,
	char *pEventName,
	char *pEventParameter,
	DWORD SubscriptionIntervalSeconds,
	DWORD SubscriptionResponseTimeoutMs,
	DWORD SubscriptionRetryTimeoutSeconds,
	EVENT_SUBSCRIBE_HANDLE *pEventSubscribeHandle
	);

TELEPHONY_RETURN_VALUE VOIP_API StopEventSubscription(SIPHANDLE hStateMachine, EVENT_SUBSCRIBE_HANDLE EventSubscribeHandle);
TELEPHONY_RETURN_VALUE VOIP_API TriggerSubscription(SIPHANDLE hStateMachine, EVENT_SUBSCRIBE_HANDLE EventSubscribeHandle);



// Telephony Engine Notify event interface.
//
TELEPHONY_RETURN_VALUE VOIP_API SetReceivedUnsolicitedNotifyState(
		SIPHANDLE hStateMachine,
		BOOL EnableState
		);

TELEPHONY_RETURN_VALUE VOIP_API SendEventNotification(
		SIPHANDLE hStateMachine,
		NOTIFY_HANDLE NotifyHandle,
		char *pEventParameter,
		DWORD NotificationResponseTimeoutMs,
		DWORD *pNotifyResponseCode
		);

TELEPHONY_RETURN_VALUE VOIP_API CloseNotifyHandle(
		SIPHANDLE hStateMachine,
		NOTIFY_HANDLE NotifyHandle
		);

TELEPHONY_RETURN_VALUE VOIP_API GetPersistNotifyHandleBufferSize(
		SIPHANDLE hStateMachine,
		NOTIFY_HANDLE NotifyHandle,
		DWORD *pBufSizeInBytes
		);


TELEPHONY_RETURN_VALUE VOIP_API PersistNotifyHandle(
		SIPHANDLE hStateMachine,
		NOTIFY_HANDLE NotifyHandle,
		char *pPersistData
		);

TELEPHONY_RETURN_VALUE VOIP_API RecreateNotifyHandle(
		SIPHANDLE hStateMachine,
		char *pPersistData,
		NOTIFY_HANDLE *pNotifyHandle
		);

TELEPHONY_RETURN_VALUE VOIP_API RecreateSubscribeRequest(
		SIPHANDLE hStateMachine,
		NOTIFY_HANDLE NotifyHandle,
		SUBSCRIBE_REQUEST **ppDestSubscribeRequest
		);

TELEPHONY_RETURN_VALUE VOIP_API DeleteSubscribeRequest(
		SIPHANDLE hStateMachine,
		SUBSCRIBE_REQUEST *pDestSubscribeRequest
		);


// Interfaces for manipulating challenge authentication information.
//
TELEPHONY_RETURN_VALUE VOIP_API AddAuthorizationCredentials(
		SIPHANDLE hStateMachine,
		char *pUserName,
		char *pPassword,
		char *pRealm
		);

TELEPHONY_RETURN_VALUE VOIP_API DeleteAuthorizationCredentials(
		SIPHANDLE hStateMachine,
		char *pUserName,
		char *pPassword,
		char *pRealm
		);

TELEPHONY_RETURN_VALUE VOIP_API SetChallengeAuthenticationState(SIPHANDLE hStateMachine, BOOL EnableState);

TELEPHONY_RETURN_VALUE VOIP_API VerifyChallengeResponse(
		SIPHANDLE hStateMachine,
		CHALLENGE_HANDLE hChallenge,
		char *pPassword,
		char *pRealm
		);

TELEPHONY_RETURN_VALUE VOIP_API SetAuthCredentialExpireTime(
		SIPHANDLE hStateMachine,
		DWORD ExpireTimeTimeSeconds
		);


// retrieving challenge error information.
//
TELEPHONY_RETURN_VALUE VOIP_API GetChallengeErrorData(
		SIPHANDLE hStateMachine,
		int PhoneLine,
		CHALLENGE_ERROR_DATA *pChallengeErrorData
		);

TELEPHONY_RETURN_VALUE VOIP_API GetRegisterChallengeErrorData(
		SIPHANDLE hStateMachine,
		CHALLENGE_ERROR_DATA *pChallengeErrorData
		);

TELEPHONY_RETURN_VALUE VOIP_API GetSubscribeChallengeErrorData(
		SIPHANDLE hStateMachine,
		EVENT_SUBSCRIBE_HANDLE EventSubscribeHandle,
		CHALLENGE_ERROR_DATA *pChallengeErrorData
		);

TELEPHONY_RETURN_VALUE VOIP_API GetNotifyChallengeErrorData(
		SIPHANDLE hStateMachine,
		NOTIFY_HANDLE NotifyHandle,
		CHALLENGE_ERROR_DATA *pChallengeErrorData
		);




// Special functions.
//
TELEPHONY_RETURN_VALUE VOIP_API SendOptionsForInboundCall(
		SIPHANDLE hStateMachine,
		BOOL EnableState
		);

TELEPHONY_RETURN_VALUE VOIP_API UseRportInViaHeader(
		SIPHANDLE hStateMachine,
		BOOL EnableState
		);

TELEPHONY_RETURN_VALUE VOIP_API UseBranchInViaHeader(
		SIPHANDLE hStateMachine,
		BOOL EnableState
		);

TELEPHONY_RETURN_VALUE VOIP_API EnableKeepAliveTransmissions(
		SIPHANDLE hStateMachine,
		BOOL KeepAliveSip,
		BOOL KeepAliveRtp,
		DWORD NatSessionTimeMs
		);

TELEPHONY_RETURN_VALUE VOIP_API SetSipLogServer(
		SIPHANDLE hStateMachine,
		BOOL EnableSipLogServer,
		char *pSipLogServer,
		int SipLogServerPort
		);

TELEPHONY_RETURN_VALUE VOIP_API SetEventLogServer(
		SIPHANDLE hStateMachine, 
		BOOL EnableEventLogServer,
		char *pEventLogServer,
		int EventLogServerPort
		);

char * VOIP_API GetTelephonyStatusString(TELEPHONY_RETURN_VALUE SipStatus);
TELEPHONY_RETURN_VALUE VOIP_API RandomlyAssignIncomingCallsToPhoneLines(SIPHANDLE hStateMachine, BOOL EnableSate);
TELEPHONY_RETURN_VALUE VOIP_API SendSipKeepAlive(SIPHANDLE hStateMachine, char *pHost, int Port, BOOL EnableState, int KeepAliveSeconds);
TELEPHONY_RETURN_VALUE VOIP_API SetNumByeRetransmissions(SIPHANDLE hStateMachine, DWORD NumByeRetransmissions);
TELEPHONY_RETURN_VALUE VOIP_API ModifySipMessage(SIPHANDLE hStateMachine, char **ppOriginalSipMsgStr, char *pNewSipMsgStr);
TELEPHONY_RETURN_VALUE VOIP_API SendUdpDatagram(void *pDataToTransmit, int DataLengthInBytes, char *pDestHost, int DestPort);
TELEPHONY_RETURN_VALUE VOIP_API SendUdpDatagramUsingSipPort(SIPHANDLE hStateMachine, void *pDataToTransmit, int DataLengthInBytes, char *pDestHost, int DestPort);


// Call state recording.
//
TELEPHONY_RETURN_VALUE VOIP_API SetCallStateRecording(
		SIPHANDLE hStateMachine,
		int PhoneLine,
		BOOL EnableState
		);

TELEPHONY_RETURN_VALUE VOIP_API GetNumRecordedCallStates(
		SIPHANDLE hStateMachine,
		int PhoneLine,
		int *pNumRecordedCallStates
		);

TELEPHONY_RETURN_VALUE VOIP_API GetRecordedCallStates(
		SIPHANDLE hStateMachine,
		int PhoneLine,
		TELEPHONY_RETURN_VALUE *pRecordedCallStates
		);

TELEPHONY_RETURN_VALUE VOIP_API ClearRecordedCallStates(
		SIPHANDLE hStateMachine,
		int PhoneLine
		);


// Codec related.
//
TELEPHONY_RETURN_VALUE VOIP_API SetDefaultReceiveIlbcFrameMode(SIPHANDLE hStateMachine, int ModeInMs);



// Format and rate conversion.
//
FORMAT_RATE_STATUS VOIP_API CreateFormatRateConverter(SIPHANDLE hStateMachine, AUDIO_BANDWIDTH SrcDataType, AUDIO_BANDWIDTH DestDataType, FORMAT_RATE_CONVERT_HANDLE *pConverterHandle);
FORMAT_RATE_STATUS VOIP_API DestroyFormatRateConverter(FORMAT_RATE_CONVERT_HANDLE ConverterHandle);
FORMAT_RATE_STATUS VOIP_API FormatRateGetSampleBlockSize(FORMAT_RATE_CONVERT_HANDLE ConverterHandle, AUDIO_BANDWIDTH DataType, int *pSampleBlockSize);
FORMAT_RATE_STATUS VOIP_API FormatRateConvert(FORMAT_RATE_CONVERT_HANDLE ConverterHandle, BYTE *pSrc, BYTE *pDest, int *pBytesConverted);


// In-band DTMF encoder/decoder
//
TELEPHONY_RETURN_VALUE VOIP_API CreateDtmfDecoder(
		SIPHANDLE hStateMachine,
		BOOL UsePcm,
		int SampleRate,
		int UserSamplesPerInputBlock,
		int FilterBlockN,
		BOOL ImmediateEvaluation,
		DTMF_DECODER_CALLBACK_PROC CallbackProc,
		void *pInstanceData,
		HDTMFDECODER *pDtmfDecoderHandle
		);

BOOL VOIP_API SetDtmfTuningTables(
		HDTMFDECODER hDtmfDecoder,
		void *pDtmfDecoderRatioTuningTable,
		void *pDtmfDecoderMagnitudeTuningTable
		);

TELEPHONY_RETURN_VALUE VOIP_API DestroyDtmfDecoder(HDTMFDECODER hDtmfDecoder);
BOOL VOIP_API DtmfDecoderWrite(HDTMFDECODER hDtmfDecoder, void *pSampleBuffer);

TELEPHONY_RETURN_VALUE VOIP_API CreateDtmfGenerator(
		SIPHANDLE hStateMachine,
		DTMF_GENERATOR_CALLBACK_PROC CallbackProc,
		void *pInstanceData,
		HDTMFGENERATOR *pDtmfGeneratorHandle
		);

TELEPHONY_RETURN_VALUE VOIP_API DestroyDtmfGenerator(HDTMFGENERATOR hDtmfGenerator);
BOOL VOIP_API StartDtmfTone(HDTMFGENERATOR hDtmfGenerator, DTMF_TONE DtmfTone, DWORD DurationMs);
void VOIP_API WaitForToneCompletion(HDTMFGENERATOR hDtmfGenerator);
BOOL VOIP_API StopDtmfTone(HDTMFGENERATOR hDtmfGenerator);
BOOL VOIP_API SetDtmfAmplitude(HDTMFGENERATOR hDtmfGenerator, int Amplitude);



// Simple wave file reading.
//
TELEPHONY_RETURN_VALUE VOIP_API OpenWaveFile(SIPHANDLE hStateMachine, char *pWaveFileName, int BytesPerWaveBuffer, HWAVEFILE *pWaveFileHandleint, int *pBytesPerSample);
void VOIP_API CloseWaveFile(HWAVEFILE hWaveFile);
BOOL VOIP_API GetWaveBuffer(HWAVEFILE hWaveFile, void *pDest);



// Phone line recording.
//
TELEPHONY_RETURN_VALUE VOIP_API StartPhoneLineRecording(
			SIPHANDLE hStateMachine,
			int PhoneLine,
			BOOL RecordToFile,
			BOOL RecordFileRaw,
			char *pPhoneLineRecordDirectory,
			PHONE_LINE_RECORD_CALLBACK_PROC PhoneLineRecordCallback,
			void *pUserData
			);

TELEPHONY_RETURN_VALUE VOIP_API StopPhoneLineRecording(
			SIPHANDLE hStateMachine,
			int PhoneLine
			);


// RTP packet access.
//
TELEPHONY_RETURN_VALUE VOIP_API EnableRawRtpPacketAccess(
			SIPHANDLE hStateMachine,
			int PhoneLine,
			BOOL EnableState,
			RTP_CALLBACK_PROC RtpCallback,
			void *pUserData
			);



// Phone line VU meter support.
TELEPHONY_RETURN_VALUE VOIP_API SetPhoneLineVuMeterCallback(
		SIPHANDLE hStateMachine,
		int PhoneLine,
		PHONE_LINE_VU_METER_CALLBACK_PROC PhoneLineVuMeterCallback,
		void *pUserData
		);




#ifdef __cplusplus
} // extern "C"
#endif






#endif // #ifndef _SipTelephonyApi_h_






