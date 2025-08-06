using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DiffDecompile.Core.Utilities.Exceptions;

#pragma warning disable 1591

namespace DiffDecompile.Core
{
    public class DiffDecompileManager : IDiffDecompile
    {
        private int _filesize = 0;
        private bool _valid = false;
        private DiffDecompileFileFormat _curDecompileFile = new DiffDecompileFileFormat();

        public DecompileSourceType SourceType => _curDecompileFile.SourceType;

        public string Magic => _curDecompileFile.Magic;

        public short Version => _curDecompileFile.Version;

        public ushort Count => _curDecompileFile.Count;

        public uint DataDirectoryBase => _curDecompileFile.DataDirectoryBase;

        private bool _check(MemoryStream ms, out int errorcode)
        {
            byte sourcetype = (byte)ms.ReadByte();
            if (sourcetype >= 0x80)
            {
                errorcode = 0x01;
                return false;
            }
            _curDecompileFile.SourceType = (DecompileSourceType)sourcetype;

            if (0x00 != ms.ReadByte())
            {
                errorcode = 0x02;
                return false;
            }
            _curDecompileFile.Reserved = 0x00;


            byte[] magic = new byte[14];
            ms.Read(magic, 0, magic.Length);
            if (0x00 != magic[13] || "diffdecompile\0" != Encoding.ASCII.GetString(magic))
            {
                errorcode = 0x03;
                return false;
            }
            _curDecompileFile.Magic = "diffdecompile\0";

            byte[] version = new byte[2];
            ms.Read(version, 0, version.Length);
            _curDecompileFile.Version = BitConverter.ToInt16(version, 0);

            byte[] count = new byte[2];
            ms.Read(count, 0, count.Length);
            _curDecompileFile.Count = BitConverter.ToUInt16(count, 0);

            byte[] datadirectory = new byte[4];
            ms.Read(datadirectory, 0, datadirectory.Length);
            _curDecompileFile.DataDirectoryBase = BitConverter.ToUInt32(datadirectory, 0);
            if (_curDecompileFile.DataDirectoryBase < 0x18)
            {
                throw new DiffDecompileFileFormatException($"file check error DataDirectoryBase is invalid");
            }
            errorcode = 0;
            return true;
        }

        private List<DiffDecompileEntryFormat> parseDiffDecompileEntries(MemoryStream ms)
        {
            List<DiffDecompileEntryFormat> diffDecompileEntryFormats = new List<DiffDecompileEntryFormat>();

            for (ushort i = 0; i < _curDecompileFile.Count; i++)
            {
                DiffDecompileEntryFormat diffDecompileEntry = new DiffDecompileEntryFormat();
                if(_curDecompileFile.Version == 2)
                {
                    byte[] flag = new byte[2];
                    ms.Read(flag, 0, 2);
                    diffDecompileEntry.Flag = BitConverter.ToUInt16(flag, 0);
                }
                ms.Position += 2;
                diffDecompileEntry.Id += i;

                byte[] similarity = new byte[4];
                ms.Read(similarity, 0, 4);
                diffDecompileEntry.Similarity = BitConverter.ToSingle(similarity, 0);

                byte[] condidence = new byte[4];
                ms.Read(condidence, 0, 4);
                diffDecompileEntry.Confidence = BitConverter.ToSingle(condidence, 0);

                byte[] primarylen = new byte[2];
                ms.Read(primarylen, 0, 2);
                diffDecompileEntry.PrimaryLen = BitConverter.ToUInt16(primarylen, 0);

                byte[] primaryname = new byte[diffDecompileEntry.PrimaryLen];
                ms.Read(primaryname, 0, diffDecompileEntry.PrimaryLen);
                diffDecompileEntry.PrimaryName = Encoding.UTF8.GetString(primaryname);

                byte[] primaryaddr = new byte[8];
                ms.Read(primaryaddr, 0, 8);
                diffDecompileEntry.PrimaryAddress = BitConverter.ToInt64(primaryaddr, 0);

                diffDecompileEntry.PrimaryDataDirectory = parseDiffDataDirectory(ms);

                byte[] secondarylen = new byte[2];
                ms.Read(secondarylen, 0, 2);
                diffDecompileEntry.SecondaryLen = BitConverter.ToUInt16(secondarylen, 0);

                byte[] secondaryname = new byte[diffDecompileEntry.SecondaryLen];
                ms.Read(secondaryname, 0, diffDecompileEntry.SecondaryLen);
                diffDecompileEntry.SecondaryName = Encoding.UTF8.GetString(secondaryname);

                byte[] secondaryaddr = new byte[8];
                ms.Read(secondaryaddr, 0, 8);
                diffDecompileEntry.SecondaryAddress = BitConverter.ToInt64(secondaryaddr, 0);

                diffDecompileEntry.SecondaryDataDirectory = parseDiffDataDirectory(ms);

                diffDecompileEntryFormats.Add(diffDecompileEntry);
            }

            return diffDecompileEntryFormats;
        }

        private DiffDataDirectory parseDiffDataDirectory(MemoryStream ms)
        {
            DiffDataDirectory diffDataDirectory = new DiffDataDirectory();

            byte[] virtualaddress = new byte[4];
            ms.Read(virtualaddress, 0, 4);
            diffDataDirectory.VirtualAddress = BitConverter.ToInt32(virtualaddress, 0);

            byte[] size = new byte[4];
            ms.Read(size, 0, 4);
            diffDataDirectory.Size = BitConverter.ToInt32(size, 0);
            return diffDataDirectory;
        }

        public IEnumerable<DiffDecompileEntry> Parse(byte[] data)
        {
            _filesize = data.Length;
            if (data.Length < 0x18)
            {
                throw new DiffDecompileFileFormatException($"length is not enough");
            }

            List<DiffDecompileEntry> diffDecompileEntries = new List<DiffDecompileEntry>();
            using (MemoryStream ms = new MemoryStream(data))
            {
                if (!_check(ms, out int errorcode))
                {
                    throw new DiffDecompileFileFormatException($"file check error ({errorcode})");
                }
                _valid = true;

                _curDecompileFile.Entries = parseDiffDecompileEntries(ms);

                try
                {
                    ms.Position = _curDecompileFile.DataDirectoryBase;
                }
                catch (Exception)
                {
                    throw new OffsetOverflowException("diffdecompile target parse failed");
                }

                foreach (var item in _curDecompileFile.Entries)
                {
                    DiffDecompileEntry decompileentry = item;
                    byte[] primarydata = new byte[item.PrimaryDataDirectory.Size];
                    ms.Read(primarydata, 0, primarydata.Length);
                    decompileentry.PrimaryData = primarydata;

                    byte[] secondarydata = new byte[item.SecondaryDataDirectory.Size];
                    ms.Read(secondarydata, 0, secondarydata.Length);
                    decompileentry.SecondaryData = secondarydata;

                    decompileentry.SourceType = _curDecompileFile.SourceType;
                    diffDecompileEntries.Add(decompileentry);
                }
            }
            return diffDecompileEntries;
        }

        public IEnumerable<DiffDecompileEntry> ParseFromFile(string filename)
        {
            return Parse(File.ReadAllBytes(filename));
        }
        
        public IEnumerable<DiffDecompileEntry> ParseFromUrl(string url)
        {
            if (!url.StartsWith("http"))
            {
                throw new DiffDecompileFileFormatException($"url format is not right");
            }

            Func<WebClient, int, byte[]> download_retry = (wc, retry) =>
            {
                var data = wc.DownloadData(url);
                while (data.Length == 0 && retry > 0)
                {
                    data = wc.DownloadData(url);
                    Thread.Sleep(100);
                    retry--;
                }
                return data;
            };

            using (WebClient wc = new WebClient())
            {
                return Parse(download_retry(wc, 10));
            }
        }

        public bool IsValid() => _valid;
    }
}

