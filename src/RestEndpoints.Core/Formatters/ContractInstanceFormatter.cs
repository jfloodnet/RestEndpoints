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
    public class ContractInstanceJsonFormatter : MediaTypeFormatter
    {
        public ContractInstanceJsonFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
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
            var body = await content.ReadAsStringAsync();
            return CreateContractInsanceFromJson(body);
        }

        public ContractInstance CreateContractInsanceFromJson(string body)
        {
            var values = JsonConvert.DeserializeObject<JObject>(body)
                .Properties()
                .ToDictionary(
                    k => k.Name,
                    v => v.Value.ToString());

            return new ContractInstance(values);
        }

    }
    public class ContractInstanceFormDataFormatter : MediaTypeFormatter
    {
        public ContractInstanceFormDataFormatter()
        {
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
            return CreateContractInsanceFrom(formData);
        }

        public ContractInstance CreateContractInsanceFrom(NameValueCollection formData)
        {
            return new ContractInstance(formData.AllKeys.ToDictionary(k => k, k => formData[k]));
        }
    }
}