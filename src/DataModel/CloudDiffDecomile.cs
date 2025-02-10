﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DiffClient.DataModel
{
    internal class CloudDiffDecomile
    {
        public string Date { get; set; }

        public int Count { get; set; }

        public string[] Items { get; set; }

        public static CloudDiffDecomile Parse(byte[] data)
        {
            var tmp = JsonSerializer.Deserialize<CloudDiffDecomile>(data);
            return tmp;
        }

        public static CloudDiffDecomile ParseFromFile(string filename)  => Parse(File.ReadAllBytes(filename));
        public static CloudDiffDecomile ParseFromUrl(string url)
        {
            using(WebClient wc = new WebClient())
            {
                return Parse(wc.DownloadData(url));
            }
        }

    }
}
