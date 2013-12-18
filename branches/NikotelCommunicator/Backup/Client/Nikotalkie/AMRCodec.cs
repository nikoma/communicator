using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.DirectX.DirectSound;

namespace Remwave.Client
{
    class AMRCodec
    {

        public AMRCodec()
        {

        }

        public MemoryStream Decode(MemoryStream memoryStream)
        {
            byte[] buffer320 = new byte[320];
            byte[] buffer32 = new byte[32];
            MemoryStream amr2rawData = new MemoryStream();

            AMR_Decoder dec = new AMR_Decoder();
            int start = 6; //skip header of amr file
            int end = Convert.ToInt32(memoryStream.Length);
            memoryStream.Position = start;
            while (true)
            {
                memoryStream.Read(buffer32, 0, 32);
                dec.Decode(buffer32, 0, buffer320, 0);
                amr2rawData.Write(buffer320, 0, buffer320.Length);
                start += 32;
                if (start >= end) break;
            }
            return amr2rawData;
        }

        public MemoryStream Encode(MemoryStream memoryStream)
        {
            memoryStream.Position = 0;
            byte[] buffer320 = new byte[320];
            byte[] buffer32 = new byte[32];
            MemoryStream raw2armData = new MemoryStream();
            raw2armData.Write(Encoding.UTF8.GetBytes("#!AMR\n"), 0, 6);
            AMR_Encoder enc = new AMR_Encoder();
            int start = 0;
            int end = Convert.ToInt32(memoryStream.Length);
            memoryStream.Position = start;
            while (true)
            {
                if (end - start < 320)
                {
                    memoryStream.Read(buffer320, 0, end - start);
                    for (int i = end - start; i < 320; i++)
                    {
                        buffer320[i] = 0;
                    }
                }
                else
                {
                    memoryStream.Read(buffer320, 0, 320);
                }
                enc.Encode(buffer320, 0, buffer32, 0);
                raw2armData.Write(buffer32, 0, buffer32.Length);
                start += 320;
                if (start >= end) break;
            }

            return raw2armData;
        }
    }
}
