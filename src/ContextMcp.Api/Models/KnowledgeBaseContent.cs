using System.ComponentModel;

namespace ContextMcp.Api.Models
{
    [Description("Base type for all structured knowledge content variants.")]
    public abstract class KnowledgeBaseContent
    {
        [Description("Discriminator indicating the concrete content type.")]
        public abstract string Type { get; }
    }
}
