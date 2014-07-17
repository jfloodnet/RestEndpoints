using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace NServiceBus.ReSTEndpoint
{
    public class EndpointController : ApiController
    {
        private readonly Contracts contracts;

        public EndpointController(Contracts  contracts)
        {
            this.contracts = contracts;
        }

        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(contracts.AllCommands());
        }

        public HttpResponseMessage Get(string commandName)
        {
            return Request.CreateResponse(contracts.FindCommand(commandName));
        }

        public HttpResponseMessage Post(object command)
        {
            return Request.CreateResponse("Post(object command)");
        }
    }

    public class Contracts
    {
        private Assembly[] searchAssemblies = new Assembly[] { };

        private Dictionary<string, Type> commands = new Dictionary<string, Type>();

        private Contracts(Assembly[] searchAssemblies)
        {
            this.searchAssemblies = searchAssemblies;
        }

        public static Contracts LookIn(params Assembly[] assembly)
        {
            return new Contracts(assembly);
        }

        public Contracts For(Func<Type, bool> isCommand)
        {
            foreach (var commandType in 
                searchAssemblies.SelectMany(
                assembly => assembly.GetTypes().Where(t => isCommand(t))))
            {
                commands.Add(commandType.Name, commandType);
            }

            return this;
        }

        public CommandDescriptor FindCommand(string command)
        {
            Type type= null;
            if (!commands.TryGetValue(command, out type))
                return null;

            return ToDescriptor(type);        
        }

        public CommandDescriptor[] AllCommands()
        {
            return commands.Select(kv => ToDescriptor(kv.Value)).ToArray();
        }

        private CommandDescriptor ToDescriptor(Type type)
        {
            return new CommandDescriptor
            {
                CommandName = type.Name,
                Properties = type.GetProperties().Select(x => x.Name).ToArray()
            };    
        }
    }
}
