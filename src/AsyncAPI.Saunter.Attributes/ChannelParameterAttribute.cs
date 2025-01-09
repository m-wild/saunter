using System;

namespace Saunter.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
    public class ChannelParameterAttribute : Attribute
    {
        public ChannelParameterAttribute(string name, Type type)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }

        public string Name { get; }

        public Type Type { get; }

        public string Description { get; set; }

        public string Location { get; set; }
    }
}
