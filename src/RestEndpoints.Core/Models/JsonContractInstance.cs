using System;
using Newtonsoft.Json;

namespace RestEndpoints.Core.Models
{
    public class JsonContractInstance : ContractInstance
    {
        private readonly string json;

        public JsonContractInstance(string json)
        {
            this.json = json;
        }

        public override object CreateMessage(Type type)
        {
            return JsonConvert.DeserializeObject(json, type);
        }
    }
}