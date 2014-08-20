using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RestEndpoints.Core.Utils;

namespace RestEndpoints.Core.Models
{
    public class FormDataContractInstance : ContractInstance
    {
        private readonly Dictionary<string, string> message;
        private Dictionary<string, Exception>  exceptions = new Dictionary<string, Exception>();

        public FormDataContractInstance(Dictionary<string, string> value)
        {
            this.message = value;
        }

        public override object CreateMessage(Type type)
        {
            var instance = CreateMessage(type, Property.From(message));

            if (exceptions.Any())
            {
                throw MessageInstantiationException();
            }

            return instance;
        }


        private object CreateMessage(Type type, Property[] properties)
        {
            if (!properties.Any())
                return null;

            var root = Activator.CreateInstance(type);

            foreach (var info in type.GetProperties())
            {
                var property = properties.First(x => x.Name == info.Name);

                try
                {
                    SetValue(root, property, info);
                }
                catch (Exception ex)
                {
                    exceptions.Add(property.FullName, ex);
                }
            }

            return root;
        }

        private void SetValue(object root, Property property, PropertyInfo info)
        {
            object value = null;
            if (property.IsLeaf() && property.HasValue())
            {
                value = property.Value.Convert(info.PropertyType);
            }
            else
            {
                value = CreateMessage(info.PropertyType, property.Properties);
            }
            info.SetValue(root, value);
        }

        private Exception MessageInstantiationException()
        {
            return new MessageInstantiationException(exceptions.Select(kvp => string.Format("{0} : {1}", kvp.Key, kvp.Value.Message)).ToArray());
        }
    }


    public class MessageInstantiationException : Exception
    {
        public readonly string[] ErrorMessages;

        public MessageInstantiationException(string[] errorMessage)
        {
            this.ErrorMessages = errorMessage;
        }
    }
}