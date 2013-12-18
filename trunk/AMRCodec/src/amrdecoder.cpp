#include "amrdecoder.h"
#include <cstddef>
#include <cstring>
#include "interf_dec.h"
#include "sp_dec.h"

#ifdef IF2
const unsigned AMRDecoder::frameSize[16] = { 13, 14, 16, 18, 19, 21, 26, 31, 6, 0, 0, 0, 0, 0, 0, 1 };
#else
const unsigned AMRDecoder::frameSize[16] = { 13, 14, 16, 18, 20, 21, 27, 32, 6, 0, 0, 0, 0, 0, 0, 1 };
#endif

AMRDecoder::AMRDecoder() :
	codecState(Decoder_Interface_init())
{
}

AMRDecoder::~AMRDecoder()
{
	if (codecState != NULL)
		Decoder_Interface_exit(codecState);
}

void AMRDecoder::Reset()
{
	Decoder_Interface_reset(codecState);
}

unsigned AMRDecoder::DecodeFrame(
	const void *inputBuffer, // input buffer with AMR frame data (max 32 bytes)
	void *outputBuffer // output buffer (160 16-bit words - 320 bytes)
	)
{
	if (inputBuffer == NULL || outputBuffer == NULL)
		return 0;

	memset(outputBuffer, 0, 160 * 2);

#ifdef IF2
	unsigned int frameType = *reinterpret_cast<const unsigned char*>(inputBuffer) & 0x000F;
#else
	unsigned int frameType = (*reinterpret_cast<const unsigned char*>(inputBuffer) >> 3) & 0x000F;
#endif

	Decoder_Interface_Decode(codecState,
		reinterpret_cast<unsigned char*>(const_cast<void*>(inputBuffer)),
		reinterpret_cast<short*>(outputBuffer), 
		0);

	return frameSize[frameType];
}
