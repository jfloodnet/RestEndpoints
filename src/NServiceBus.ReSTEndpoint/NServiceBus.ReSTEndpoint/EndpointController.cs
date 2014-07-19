using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace NServiceBus.ReSTEndpoint
{
    public class EndpointController : ApiController
    {
        private readonly Endpoints endpoints;

        public EndpointController(Endpoints endpoints)
        {
            this.endpoints = endpoints;
        }

        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(endpoints.AllEndpoints());
        }

        public HttpResponseMessage Get(string endpointName)
        {
            return Request.CreateResponse(endpoints.ContractsIn(endpointName));
        }

        public HttpResponseMessage Get(string endpointName, string contractName)
        {
            return Request.CreateResponse(endpoints.FindContract(endpointName, contractName));
        }

        public HttpResponseMessage Post(string endpointName, string contractName, FormDataCollection formData)
        {
            var descriptor = endpoints.FindContract(endpointName, contractName);
            var contract = Activator.CreateInstance(descriptor.Type);
            

            foreach (var property in descriptor.Type.GetProperties())
            {
                var propertyValue = formData[property.Name];     

                object value = null;
                if (propertyValue.TryConvert(property.PropertyType, out value))
                {
                    property.SetValue(contract, value);
                }
                else
                {
                    descriptor.ResponseMessage = string.Format("Value for '{0}' provided was in an incorrect format.", property.Name);
                    return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, descriptor);
                }
            }

            //bus.Send(command); !woo

            return Request.CreateResponse(descriptor);
        }
    }

    internal static class TypeConverter
    {
        public static bool TryConvert(this object obj, Type conversionType, out object value)
        {
            value = null;
            try
            {
                value = Convert.ChangeType(obj, conversionType);
                return true;
            }
            catch (Exception)
            {
                return false;                
            }
        }
    }
}
