using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericTransactionLog.Currying
{
    public static class Funcs
    {
        public static readonly Func<StreamReader, IEnumerable<string>> ReadStringsFromStreamReader = sr => ReadStringsFromStreamReaderInternal(sr);
        private static IEnumerable<string> ReadStringsFromStreamReaderInternal(StreamReader streamReader) //you get better intellisense with public Funcs....
        {
            var line = streamReader.ReadLine();
            while (line != null)
            {
                yield return line;
                line = streamReader.ReadLine();
            }
        }

        public static readonly Func<Stream, StreamReader> StreamToStreamReader =  s => new StreamReader(s);

        public static readonly Action<string, Stream> AppendStringToStream = (value, appendStream) =>
        {
            var sw = new StreamWriter(appendStream);
            sw.Write(value);
            sw.Write(Environment.NewLine);
            sw.Flush();
        };
    }
}
