#include "amrcodec.h"

int AMR_Decoder_FrameSize[16];

AMREncoder* AMR_Encoder_Init()
{
	return new AMREncoder();
}

void AMR_Encoder_Destroy(AMREncoder* enc)
{
	delete enc;
}

int AMR_Encoder_Encode(AMREncoder* enc, char* in, int inIndex, char* out, int outIndex)
{
	unsigned char outbuf[32];
	unsigned char buffer[320];

	for (int i = 0; i < 320; ++i)
		buffer[i] = in[inIndex + i];
	
	int counter = enc->EncodeFrame(buffer, outbuf);

	for (int i = 0; i < counter; ++i)
		out[outIndex + i] = outbuf[i];

	return counter;
}


AMRDecoder* AMR_Decoder_Init()
{
	AMR_Decoder_FrameSize[0] = 13;
	AMR_Decoder_FrameSize[1] = 14;
	AMR_Decoder_FrameSize[2] = 16;
	AMR_Decoder_FrameSize[3] = 18;
	AMR_Decoder_FrameSize[4] = 20;
	AMR_Decoder_FrameSize[5] = 21;
	AMR_Decoder_FrameSize[6] = 27;
	AMR_Decoder_FrameSize[7] = 32;
	AMR_Decoder_FrameSize[8] = 6;
	AMR_Decoder_FrameSize[15] = 1;

	return new AMRDecoder;
}

void AMR_Decoder_Destroy(AMRDecoder* dec)
{
	delete dec;
}

int AMR_Decoder_Decode(AMRDecoder* dec, unsigned char* in, int inIndex, unsigned char* out, int outIndex)
{
	unsigned char outbuf[320];
	unsigned char buffer[32];

	int frameType = (in[0] >> 3) & 0x0f;
	for (int i = 0; i < AMR_Decoder_FrameSize[frameType]; ++i)
		buffer[i] = in[i + inIndex];

	int counter = dec->DecodeFrame(buffer, outbuf);

	for (int i = 0; i < 320; ++i)
		out[outIndex + i] = outbuf[i];

	return counter;
}
