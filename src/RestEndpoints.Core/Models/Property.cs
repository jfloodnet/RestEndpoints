using System;
using System.Collections.Generic;
using System.Linq;

namespace RestEndpoints.Core.Models
{
    public class Property
    {
        public string FullName;
        public readonly string Name;
        public readonly string Value;
        public readonly  Property[] Properties = {};

        public Property(string name, params Property[] properties)
        {
            this.Name = name;
            this.FullName = name;
            this.Properties = properties;

            MakeFullName(this.FullName, properties);
        }

        public static void MakeFullName(string hierarchy, Property[] properties)
        {
            foreach (var property in properties)
            {
                property.FullName = string.Join(".", hierarchy, property.Name);
                if (!property.IsLeaf())
                {
                    MakeFullName(property.FullName, property.Properties);
                }
            }
        }

        public Property(string name, string nest)
        {
            this.Name = name;
            this.FullName = name;
            this.Value = nest;
        }


        public bool HasValue()
        {
            return !string.IsNullOrEmpty(Value);
        }

        public bool IsLeaf()
        {
            return !Properties.Any();
        }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(Value);
        }


        public static Property[] From(Dictionary<string, string> message)
        {
            var groupedByNodeName =
                from m in message
                let name = GetRootPropertyName(m.Key)
                group m by name into g
                select new
                {
                    Name = g.Key, 
                    Values = g.ToDictionary(kvp => RemoveRoot(kvp.Key))
                };

            var result = groupedByNodeName.Select(g =>
            {
                if (g.Values.Count() > 1)
                {
                    return new Property(
                        g.Name,
                        Property.From(
                            ScopeDownToCurrentProperty(g.Values)));
                }

                //We must be at the leaf of the node
                return new Property(
                    g.Name, 
                    g.Values.FirstOrDefault().Value.Value);
            });

            return result.ToArray();
        }

        private static string GetRootPropertyName(string hierarchy)
        {
            return hierarchy.Split('.').First();
        }

        private static string RemoveRoot(string hierarchy)
        {
            return string.Join(".", hierarchy.Split('.').Skip(1));
        }

        private static Dictionary<string, string> ScopeDownToCurrentProperty(Dictionary<string, KeyValuePair<string, string>> values)
        {
            return values.ToDictionary(x => x.Key, x => x.Value.Value);
        }
    }
}