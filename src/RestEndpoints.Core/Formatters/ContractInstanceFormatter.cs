using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using RestEndpoints.Core.Models;

namespace RestEndpoints.Core.Formatters
{
    public class ContractInstanceFormDataFormatter : MediaTypeFormatter
    {
        public ContractInstanceFormDataFormatter()
        {
            SupportedMediaTypes.Clear();
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/x-www-form-urlencoded"));
        }

        public override bool CanReadType(Type type)
        {
            return type == typeof(ContractInstance);
        }

        public override bool CanWriteType(Type type)
        {
            return false;
        }

        public override async Task<object> ReadFromStreamAsync(
            Type type,
            Stream readStream,
            HttpContent content,
            IFormatterLogger formatterLogger,
            CancellationToken cancellationToken)
        {
            var formData = await content.ReadAsFormDataAsync(cancellationToken);
            return new ContractInstance(formData.AllKeys.ToDictionary(k => k, k => formData[k]));
        }
    }
}