using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NServiceBus.ReSTEndpoint.Models;

namespace NServiceBus.ReSTEndpoint
{
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
                    assembly => assembly.GetTypes().Where(isCommand)))
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
                CommandType = type,
            };    
        }
    }
}