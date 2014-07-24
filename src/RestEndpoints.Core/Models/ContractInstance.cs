using System;
using System.Collections.Generic;
using RestEndpoints.Core.Utils;

namespace RestEndpoints.Core.Models
{
    public class ContractInstance
    {
        private readonly Dictionary<string, string> message;

        public ContractInstance(Dictionary<string, string> value)
        {
            this.message = value;
        }

        public object ToInstanceOf(Type type)
        {
            var contract = Activator.CreateInstance(type);

            foreach (var property in type.GetProperties())
            {
                var propertyValue = message[property.Name];

                if (string.IsNullOrEmpty(propertyValue))
                    continue;

                object value = null;
                if (propertyValue.TryConvert(property.PropertyType, out value))
                {
                    property.SetValue(contract, value);
                }
            }

            return contract;
        }
    }
}