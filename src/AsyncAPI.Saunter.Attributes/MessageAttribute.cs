using System;

namespace Saunter.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class MessageAttribute : Attribute
    {
        public MessageAttribute(Type payloadType)
        {
            PayloadType = payloadType;
        }

        public MessageAttribute(Type payloadType, params string[] tags)
        {
            PayloadType = payloadType;
            Tags = tags;
        }

        /// <summary>
        /// The type to use to generate the message payload schema.
        /// </summary>
        public Type PayloadType { get; }

        /// <summary>
        /// The type to use to generate the message headers schema.
        /// </summary>
        public Type HeadersType { get; set; }

        /// <summary>
        /// A machine-friendly name for the message.
        /// Defaults to the generated schemaId.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A human-friendly title for the message.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// A short summary of what the message is about.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// A verbose explanation of the message.
        /// CommonMark syntax can be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The name of a message bindings item to reference.
        /// The bindings must be added to components/messageBindings with the same name.
        /// </summary>
        public string BindingsRef { get; set; }

        /// <summary>
        /// Unique string used to identify the message. The id MUST be unique among all messages
        /// described in the API. The messageId value is case-sensitive. Tools and libraries MAY
        /// use the messageId to uniquely identify a message, therefore, it is RECOMMENDED to
        /// follow common programming naming conventions.
        /// </summary>
        public string MessageId { get; set; }

        /// <summary>
        /// A list of tags for API documentation control. Tags can be used for logical grouping of messages.
        /// </summary>
        public string[] Tags { get; }
    }
}
