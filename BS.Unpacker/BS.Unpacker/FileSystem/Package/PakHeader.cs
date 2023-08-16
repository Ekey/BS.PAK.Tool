using System;

namespace BS.Unpacker
{
    class PakHeader
    {
        public UInt32 dwMagic { get; set; } // 0x4B434150 (PACK)
        public UInt32 dwTableOffset { get; set; }
        public Int32 dwTableSize { get; set; }
        public Int32 dwTotalFiles { get; set; }
    }
}
