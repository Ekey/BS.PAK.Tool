using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace BS.Unpacker
{
    class PakUnpack
    {
        private static List<PakEntry> m_EntryTable = new List<PakEntry>();

        public static void iDoIt(String m_Archive, String m_DstFolder)
        {
            using (FileStream TPakStream = File.OpenRead(m_Archive))
            {
                var lpHeader = TPakStream.ReadBytes(16);
                lpHeader = PakCipher.iDecrypt(lpHeader, lpHeader.Length);

                var m_Header = new PakHeader();
                using (var THeaderReader = new MemoryStream(lpHeader))
                {
                    m_Header.dwMagic = THeaderReader.ReadUInt32();
                    m_Header.dwTableOffset = THeaderReader.ReadUInt32();
                    m_Header.dwTableSize = THeaderReader.ReadInt32();
                    m_Header.dwTotalFiles = THeaderReader.ReadInt32();

                    if (m_Header.dwMagic != 0x4B434150)
                    {
                        throw new Exception("[ERROR]: Invalid magic of PAK archive file!");
                    }

                    THeaderReader.Dispose();
                }

                TPakStream.Seek(m_Header.dwTableOffset, SeekOrigin.Begin);

                var lpEntryTable = TPakStream.ReadBytes(m_Header.dwTableSize);
                lpEntryTable = PakCipher.iDecrypt(lpEntryTable, lpEntryTable.Length);

                m_EntryTable.Clear();
                using (var TEntryReader = new MemoryStream(lpEntryTable))
                {
                    for (Int32 i = 0; i < m_Header.dwTotalFiles; i++)
                    {
                        var m_Entry = new PakEntry();

                        m_Entry.dwNameLength = TEntryReader.ReadInt32();
                        m_Entry.m_FileName = Encoding.ASCII.GetString(TEntryReader.ReadBytes(m_Entry.dwNameLength)).Replace("/", "\\");
                        m_Entry.dwOffset = TEntryReader.ReadUInt32();
                        m_Entry.dwSize = TEntryReader.ReadInt32();
                        m_Entry.dwEncFlags = TEntryReader.ReadUInt32();
                        m_Entry.dwReserved = TEntryReader.ReadUInt32();

                        m_EntryTable.Add(m_Entry);
                    }

                    TEntryReader.Dispose();
                }

                foreach (var m_Entry in m_EntryTable)
                {
                    String m_FullPath = m_DstFolder + m_Entry.m_FileName;

                    Utils.iSetInfo("[UNPACKING]: " + m_Entry.m_FileName);
                    Utils.iCreateDirectory(m_FullPath);

                    TPakStream.Seek(m_Entry.dwOffset, SeekOrigin.Begin);

                    var lpBuffer = TPakStream.ReadBytes(m_Entry.dwSize);

                    if (m_Entry.dwEncFlags == 0x20000001)
                    {
                        lpBuffer = PakCipher.iDecrypt(lpBuffer, m_Entry.dwSize);
                    }

                    File.WriteAllBytes(m_FullPath, lpBuffer);
                }

                TPakStream.Dispose();
            }
        }
    }
}
