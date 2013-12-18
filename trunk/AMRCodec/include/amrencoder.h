#ifndef __amrencoder__h__included__
#define __amrencoder__h__included__

class AMREncoder {
public:
	enum FrameEncodingModes {
		Mode_4_75 = 0,
		Mode_5_15 = 1,
		Mode_5_90 = 2,
		Mode_6_70 = 3,
		Mode_7_40 = 4,
		Mode_7_95 = 5,
		Mode_10_20 = 6,
		Mode_12_20 = 7
	};

	AMREncoder();
	~AMREncoder();

	// Resets codec state information, should be called before
	// each new stream is encoded
	void Reset();

	// Encodes a single 20ms frame of PCM data
	// @return	number of bytes in an encoded frame, 0 in case of failure
	unsigned EncodeFrame(
		const void *inputBuffer, // input buffer of 160 16-bit PCM samples
		void *outputBuffer, // output buffer (max 32 bytes)
		FrameEncodingModes mode = Mode_12_20 // encoding mode to use
		);

private:
	AMREncoder(const AMREncoder&);
	AMREncoder& operator=(const AMREncoder&);

private:
	void *codecState;
	short *inputBuffer;
};

#endif // __amrencoder__h__included__
