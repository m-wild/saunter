using System;

namespace Saunter.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
    public class ChannelAttribute : Attribute
    {
        /// <summary>
        /// The name of the channel. 
        /// Format depends on the underlying messaging protocol's conventions.
        /// For example, amqp uses dot-separated paths 'light.measured'.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// An optional description of this channel item.
        /// CommonMark syntax can be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The name of a channel bindings item to reference.
        /// The bindings must be added to components/channelBindings with the same name.
        /// </summary>
        public string BindingsRef { get; set; }

        /// <summary>
        /// The servers on which this channel is available, specified as an optional unordered
        /// list of names (string keys) of Server Objects defined in the Servers Object (a map).
        /// If servers is absent or empty then this channel must be available on all servers
        /// defined in the Servers Object.
        /// </summary>
        public string[] Servers { get; set; }

        public ChannelAttribute(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}
