using System;

namespace BS.Unpacker
{
    class PakCipher
    {
        public static Byte[] iDecrypt(Byte[] lpBuffer, Int32 dwSize)
        {
            Byte bTemp1 = lpBuffer[dwSize - 1];
            Byte bTemp2 = 0;

            for (Int32 i = 0; i < dwSize; i++)
            {
                bTemp2 = bTemp1;
                bTemp1 = lpBuffer[i];
                lpBuffer[i] = (Byte)((bTemp2 & 0x55) | (Byte)(bTemp1 & 0xAA));
            }

            return lpBuffer;
        }
    }
}
