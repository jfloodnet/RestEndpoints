using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NServiceBus.ReSTEndpoint.Models;

namespace NServiceBus.ReSTEndpoint
{
    public class Endpoints
    {
        private readonly Dictionary<string, ContractDescriptor[]> map;

        private Endpoints()
        {
            this.map = new Dictionary<string,ContractDescriptor[]>();
        }  

        private Endpoints(Dictionary<string, ContractDescriptor[]> map)
        {
            this.map = map;
        }        

        public static Contracts Map(string endpointName)
        {
            return new Contracts(endpointName, new Endpoints());
        }

        public Contracts ThenMap(string endpointName)
        {
            return new Contracts(endpointName, this);
        }

        public class Contracts
        {
            private readonly Endpoints endpoints;

            private readonly string endpointName;

            private Assembly[] searchAssemblies = new Assembly[] { };

            private Dictionary<string, Type> contracts = new Dictionary<string, Type>();

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

                return new Endpoints(this.endpoints.map.Union(thisEndpoint).ToDictionary(x => x.Key, x => x.Value));
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