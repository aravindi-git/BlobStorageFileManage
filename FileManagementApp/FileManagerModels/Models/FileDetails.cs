using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileManagerModels.Models
{
    public class FileDetails
    {
        public string Name { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public Stream FileStream { get; set; }
    } 
}
