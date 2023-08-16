using System;

namespace BS.Unpacker
{
    class PakEntry
    {
        public Int32 dwNameLength { get; set; }
        public String m_FileName { get; set; }
        public UInt32 dwOffset { get; set; }
        public Int32 dwSize { get; set; }
        public UInt32 dwEncFlags { get; set; } //0x20000001 encrypted (XML, TXT)
        public UInt32 dwReserved { get; set; } //always 0
    }
}
