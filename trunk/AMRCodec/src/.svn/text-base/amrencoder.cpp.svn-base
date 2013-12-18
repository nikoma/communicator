#include "amrencoder.h"
#include <cstddef>
#include <cstring>
#include "interf_enc.h"
#include "sp_enc.h"

AMREncoder::AMREncoder() :
	codecState(Encoder_Interface_init(0)),
	inputBuffer(new short[160])
{
}

AMREncoder::~AMREncoder()
{
	delete [] inputBuffer;
	
	if (codecState != NULL)
		Encoder_Interface_exit(codecState);
}

void AMREncoder::Reset()
{
	Encoder_Interface_reset(codecState, 0);
}

unsigned AMREncoder::EncodeFrame(
	const void *ib, // input buffer of 160 16-bit PCM samples
	void *ob, // output buffer (max 32 bytes)
	FrameEncodingModes mode // encoding mode to use
	)
{
	if (ib == NULL || ob == NULL || codecState == NULL || mode < 0 || mode > 8)
		return 0;

	memcpy(inputBuffer, ib, 160 * 2);

	return Encoder_Interface_Encode(codecState, static_cast<Mode>(mode),
		inputBuffer, reinterpret_cast<unsigned char*>(ob), 0
		);
}
