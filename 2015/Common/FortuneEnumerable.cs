using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class FortuneEnumerableFactory
    {
        private const string _fortunesUrl = "https://raw.githubusercontent.com/bmc/fortunes/master/fortunes";

        private static string FilePath => Path.Combine(Path.GetTempPath(), "fortunes");

        public static async Task<FortuneEnumerable> Create()
        {
            if (!File.Exists(FilePath))
            {
                var handler = new HttpClientHandler();
                handler.Proxy = System.Net.WebRequest.DefaultWebProxy;
                handler.UseProxy = true;

                var client = new HttpClient(handler);
                var response = await client.GetAsync(_fortunesUrl);

                File.WriteAllText(FilePath, await response.Content.ReadAsStringAsync());
            }

            return new FortuneEnumerable(FilePath);
        }
    }

    public class FortuneEnumerable : IEnumerable<string>
    {
        private string _fortunePath;

        internal FortuneEnumerable(string fortunePath)
        {
            _fortunePath = fortunePath;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return new FortuneEnumerator(_fortunePath);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new FortuneEnumerator(_fortunePath);
        }
    }

    public class FortuneEnumerator : IEnumerator<string>
    {
        StreamReader _reader;

        public string Current { get; private set; }

        object IEnumerator.Current => Current;

        public FortuneEnumerator(string fortunePath)
        {
            _reader = new StreamReader(fortunePath);
        }

        public void Dispose()
        {
            if (_reader != null)
                _reader.Dispose();
        }

        public bool MoveNext()
        {
            if (_reader.EndOfStream)
                return false;

            string line;
            var sb = new StringBuilder();
            while(!_reader.EndOfStream)
            {
                line = _reader.ReadLine();
                if (line.Trim() == "%")
                    break;

                sb.AppendLine(line);
            }

            Current = sb.ToString();

            return !string.IsNullOrEmpty(Current);
        }

        public void Reset()
        {
            _reader.BaseStream.Seek(0, SeekOrigin.Begin);
        }
    }
}
