#ifndef __amrdecoder__h__included__
#define __amrdecoder__h__included__

class AMRDecoder {
public:
	AMRDecoder();
	~AMRDecoder();

	// Resets codec state information, should be called before
	// each new stream is decoded
	void Reset();

	// Decodes a single 20ms AMR frame into PCM data
	// @return	number of bytes consumed from the input buffer, 0 in case of failure
	unsigned DecodeFrame(
		const void *inputBuffer, // input buffer with AMR frame data (max 32 bytes)
		void *outputBuffer // output buffer (160 16-bit words - 320 bytes)
		);

private:
	AMRDecoder(const AMRDecoder&);
	AMRDecoder& operator=(const AMRDecoder&);

private:
	void *codecState;
	static const unsigned frameSize[16];
};

#endif // __amrdecoder__h__included__
