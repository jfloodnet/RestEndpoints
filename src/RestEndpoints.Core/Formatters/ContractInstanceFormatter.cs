using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using RestEndpoints.Core.Models;

namespace RestEndpoints.Core.Formatters
{
    public class ContractInstanceFormatter : MediaTypeFormatter
    {
        public ContractInstanceFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
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
            if (content.IsFormData())
            {
                var formData = await content.ReadAsFormDataAsync(cancellationToken);
                return CreateContractInsanceFrom(formData);
            }

            var json = await content.ReadAsStringAsync();
            return new JsonContractInstance(json);
        }

        public ContractInstance CreateContractInsanceFrom(NameValueCollection formData)
        {
            return new FormDataContractInstance(formData.AllKeys.ToDictionary(k => k, k => formData[k]));
        }
    }
}