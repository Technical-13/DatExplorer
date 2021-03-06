﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogwai.DDO.Explorer
{
    public class DatFile
    {
        public uint FileOffset { get; set; }

        public uint FileId { get; set; }

        public uint Size1 { get; set; }

        public uint Size2 { get; set; }

        public uint Timestamp { get; set; }

        public DateTime FileDate
        {
            get
            {
                return new DateTime(1970, 1, 1).ToLocalTime().AddSeconds(Timestamp);
            }
        }

        public uint Version { get; set; }

        public uint Unknown1 { get; set; }

        public uint Unknown2 { get; set; }

        public uint FileType { get; set; }

        public string UserDefinedName { get; set; }

        public static DatFile FromDirectoryBuffer(byte[] buffer, int index)
        {
            DatFile df = new DatFile();
            df.Unknown1 = BitConverter.ToUInt32(buffer, index);
            df.FileType = BitConverter.ToUInt32(buffer, index + 4);
            df.FileId = BitConverter.ToUInt32(buffer, index + 8);
            df.FileOffset = BitConverter.ToUInt32(buffer, index + 12);
            df.Size1 = BitConverter.ToUInt32(buffer, index + 16);
            df.Timestamp = BitConverter.ToUInt32(buffer, index + 20);
            df.Unknown2 = BitConverter.ToUInt32(buffer, index + 24);
            df.Size2 = BitConverter.ToUInt32(buffer, index + 28);

            if (df.FileId > 0 && df.FileOffset > 0)
                return df;
            else
                return null;
        }

        public static KnownFileType GetActualFileType(byte[] fileBuffer)
        {
            uint dword1 = BitConverter.ToUInt32(fileBuffer, 0);
            uint dword2 = BitConverter.ToUInt32(fileBuffer, 4);
            uint dword3 = BitConverter.ToUInt32(fileBuffer, 8);
            uint dword4 = BitConverter.ToUInt32(fileBuffer, 12);

            switch (dword1)
            {
                case 1179011410:
                    // "RIFF"
                    if (dword3 == 1163280727)
                        return KnownFileType.Wave;
                    break;
                case 1399285583:
                    // "OggS"
                    if (dword2 == 512 && dword3 == 0)
                        return KnownFileType.Ogg;
                    break;
            }

            return KnownFileType.Unknown;
        }
    }
}
