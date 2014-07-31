using System;

namespace RestEndpoints.Core.Models
{
    public abstract class ContractInstance
    {
        public abstract object CreateMessage(Type type);
    }
}