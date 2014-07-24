using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RestEndpoints.Core.Models;

namespace RestEndpoints.Core
{
    public class Endpoints
    {
        private readonly Dictionary<string, ContractDescriptor[]> map;
        private Action<string, object> dispatch = (e,c) => {};

        public Endpoints()
        {
            this.map = new Dictionary<string, ContractDescriptor[]>();
        }  

        private Endpoints(IEnumerable<KeyValuePair<string, ContractDescriptor[]>> map)
        {
            this.map = map.ToDictionary(x => x.Key,x => x.Value);
        }

        public Contracts ForEndpoint(string endpointName)
        {
            return new Contracts(endpointName, this);
        }

        public Endpoints DispatchWith(Action<string, object> dispatch)
        {
            this.dispatch = dispatch;
            return this;
        }

        public class Contracts
        {
            private readonly Endpoints endpoints;

            private readonly string endpointName;

            private readonly IEnumerable<Assembly> searchAssemblies = new Assembly[] { };

            internal Contracts(string endpointName, Endpoints endpoints, Assembly[] searchAssemblies = null)
            {
                this.endpointName = endpointName;
                this.endpoints = endpoints;
                this.searchAssemblies = searchAssemblies;
            }

            public Contracts LookIn(params Assembly[] assembly)
            {
                return new Contracts(this.endpointName, this.endpoints, assembly);
            }

            public Endpoints For(Func<Type, bool> isContract)
            {
                var contracts =
                    searchAssemblies.SelectMany(
                        assembly => assembly.GetTypes().Where(isContract));

                var thisEndpoint = new Dictionary<string, ContractDescriptor[]>
                {
                    { endpointName, contracts.Select(c => ToDescriptor(c, endpointName)).ToArray() }
                };

                return new Endpoints(this.endpoints.map.Union(thisEndpoint));
            }
        }

        public string[] AllEndpoints()
        {
            return this.map.Select(x => x.Key).ToArray();
        }

        public ContractDescriptor FindContract(string endpointName, string contractName)
        {
            return ContractsIn(endpointName).First(x => x.Name.Equals(contractName));
        }

        public ContractDescriptor[] ContractsIn(string endpointName)
        {
            ContractDescriptor[] contracts = null;
            if (!map.TryGetValue(endpointName, out contracts))
                return null;

            return contracts;
        }

        public void Dispatch(string endpoint, object message)
        {
            this.dispatch(endpoint, message);
        }

        private static ContractDescriptor ToDescriptor(Type type, string endpointName)
        {
            return new ContractDescriptor
            {
                Endpoint = endpointName,
                Name = type.Name,
                Type = type,
            };
        }
    }
}