using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

#pragma warning disable 8618

namespace DiffClient.DataModel
{
    internal class Element
    {
        public string File { get; set; }
    }

    internal class MonthElement
    {
        public string Date { get; set; }

        public int Count { get; set; }

        public Element[] Items { get; set; }
    }

    internal class OSElement
    {
        public string OS { get; set; }

        public int Count { get; set; }

        public MonthElement[] Items { get; set; }
    }

    internal class CloudDiffDecomile
    {
        public string Date { get; set; }

        public int Count { get; set; }

        public OSElement[] Items { get; set; }

        public static CloudDiffDecomile Parse(byte[] data)
        {
            var tmp = JsonSerializer.Deserialize<CloudDiffDecomile>(data);
            if (tmp != null)
            {
                return tmp;
            }
            throw new Exception("cloud deserialize failed");
        }

        public static CloudDiffDecomile ParseFromFile(string filename)  => Parse(File.ReadAllBytes(filename));
        public static CloudDiffDecomile ParseFromUrl(string url)
        {
            HttpClient wc = new();
            return Parse(wc.GetByteArrayAsync(url).Result);
        }
    }
}
