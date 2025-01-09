using System;

namespace Saunter.Attributes
{
    /// <summary>
    /// Marks a class or interface as containing asyncapi channels.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public sealed class AsyncApiAttribute : Attribute
    {
        public string DocumentName { get; }

        public AsyncApiAttribute(string documentName = null)
        {
            DocumentName = documentName;
        }
    }
}
