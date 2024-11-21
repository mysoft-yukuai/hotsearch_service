using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Segmenter.Common
{
    public static class FileExtension
    {
        public static string ReadEmbeddedAllLine(Stream file)
        {
            return ReadEmbeddedAllLine(file, Encoding.UTF8);
        }

        public static string ReadEmbeddedAllLine(Stream file,Encoding encoding)
        {
            using var sr = new StreamReader(file, encoding);
            return sr.ReadToEnd();
        }

        public static List<string> ReadEmbeddedAllLines(Stream file, Encoding encoding)
        {
            List<string> list = [];
            using (StreamReader streamReader = new(file, encoding))
            {
                string item;
                while ((item = streamReader.ReadLine()) != null)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        public static List<string> ReadEmbeddedAllLines(Stream file)
        {
            return ReadEmbeddedAllLines(file, Encoding.UTF8);
        }
    }
}
