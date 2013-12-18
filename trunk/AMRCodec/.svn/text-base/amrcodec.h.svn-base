// libamrdotnet.h

#pragma once

#include <amrencoder.h>
#include <amrdecoder.h>

AMREncoder* AMR_Encoder_Init();

void AMR_Encoder_Destroy(AMREncoder* enc);

int AMR_Encoder_Encode(AMREncoder* enc, char* in, int inIndex, char* out, int outIndex);

extern int AMR_Decoder_FrameSize[16];

AMRDecoder* AMR_Decoder_Init();

void AMR_Decoder_Destroy(AMRDecoder* dec);

int AMR_Decoder_Decode(AMRDecoder* dec, unsigned char* in, int inIndex, unsigned char* out, int outIndex);
