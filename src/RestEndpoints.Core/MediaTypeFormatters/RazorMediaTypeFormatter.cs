using System;
using System.IO;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RestEndpoints.Core.MediaTypeFormatters
{
    public class RazorMediaTypeFormatter<T> : MediaTypeFormatter
    {
        private string _template;

        public RazorMediaTypeFormatter(string template, params MediaTypeHeaderValue[] mediaTypes)
        {
            foreach (var mediaType in mediaTypes)
                SupportedMediaTypes.Add(mediaType);

            _template = template;
        }

        public override bool CanWriteType(Type type)
        {
            return (type == typeof(T) || type.IsSubclassOf(typeof(T)));
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, System.Net.Http.HttpContent content, TransportContext transportContext)
        {
            return Task.Factory.StartNew(() => WriteStream(value, writeStream));
        }

        void WriteStream(object value, Stream stream)
        {
            var response = RazorEngine.Razor.Parse(_template, value);
            var streamWriter = new StreamWriter(stream);
            streamWriter.Write(response);
            streamWriter.Flush();
        }

        public override bool CanReadType(Type type)
        {
            throw new NotImplementedException();
        }
    }
}
