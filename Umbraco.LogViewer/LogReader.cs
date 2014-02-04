using System;
using System.IO;
using System.Text;
using System.Web;

namespace Umbraco.LogViewer
{
    public static class LogReader
    {
        public static object GetLast100()
        {
            var path = HttpContext.Current.Server.MapPath("~/App_Data/Logs/UmbracoTraceLog.txt");
            object result;
            if (File.Exists(path))
            {
                var lastNLines = ReadEndTokens(path, 100, Encoding.Default, "\n");
                result = new
                {
                    data = lastNLines
                };
            }
            else
            {
                result = new {data = "File not found."};
            }
            return result;
        }

        public static string ReadEndTokens(string path, Int64 numberOfTokens, Encoding encoding, string tokenSeparator)
        {
            int sizeOfChar = encoding.GetByteCount("\n");
            byte[] buffer = encoding.GetBytes(tokenSeparator);

            using (var fs = new FileStream(path, FileMode.Open))
            {
                Int64 tokenCount = 0;
                Int64 endPosition = fs.Length / sizeOfChar;

                for (Int64 position = sizeOfChar; position < endPosition; position += sizeOfChar)
                {
                    fs.Seek(-position, SeekOrigin.End);
                    fs.Read(buffer, 0, buffer.Length);

                    if (encoding.GetString(buffer) == tokenSeparator)
                    {
                        tokenCount++;
                        if (tokenCount == numberOfTokens)
                        {
                            byte[] returnBuffer = new byte[fs.Length - fs.Position];
                            fs.Read(returnBuffer, 0, returnBuffer.Length);
                            return encoding.GetString(returnBuffer);
                        }
                    }
                }

                // handle case where number of tokens in file is less than numberOfTokens
                fs.Seek(0, SeekOrigin.Begin);
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                return encoding.GetString(buffer);
            }
        }
    }
}