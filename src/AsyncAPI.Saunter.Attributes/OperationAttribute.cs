using System;

namespace Saunter.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
    public abstract class OperationAttribute : Attribute
    {
        public OperationType OperationType { get; protected set; }

        public Type MessagePayloadType { get; protected set; }

        /// <summary>
        /// A short summary of what the operation is about.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Unique string used to identify the operation.
        /// The id MUST be unique among all operations described in the API.
        /// The operationId value is case-sensitive.
        /// Tools and libraries MAY use the operationId to uniquely identify an operation,
        /// therefore, it is RECOMMENDED to follow common programming naming conventions.
        /// </summary>
        public string OperationId { get; set; }

        /// <summary>
        /// A verbose explanation of the operation.
        /// CommonMark syntax can be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The name of an operation bindings item to reference.
        /// The bindings must be added to components/operationBindings with the same name.
        /// </summary>
        public string BindingsRef { get; set; }

        /// <summary>
        /// A list of tags for API documentation control. Tags can be used for logical grouping of operations.
        /// </summary>
        public string[] Tags { get; protected set; }
    }

    public class PublishOperationAttribute : OperationAttribute
    {
        public PublishOperationAttribute(Type messagePayloadType, params string[] tags)
        {
            OperationType = OperationType.Publish;
            MessagePayloadType = messagePayloadType;
            Tags = tags;
        }
        public PublishOperationAttribute(Type messagePayloadType)
        {
            OperationType = OperationType.Publish;
            MessagePayloadType = messagePayloadType;
        }

        public PublishOperationAttribute()
        {
            OperationType = OperationType.Publish;
        }
    }

    public class SubscribeOperationAttribute : OperationAttribute
    {
        public SubscribeOperationAttribute(Type messagePayloadType, params string[] tags)
        {
            OperationType = OperationType.Publish;
            MessagePayloadType = messagePayloadType;
            Tags = tags;
        }

        public SubscribeOperationAttribute(Type messagePayloadType)
        {
            OperationType = OperationType.Subscribe;
            MessagePayloadType = messagePayloadType;
        }

        public SubscribeOperationAttribute()
        {
            OperationType = OperationType.Subscribe;
        }
    }

    public enum OperationType
    {
        Publish,
        Subscribe
    }


}
