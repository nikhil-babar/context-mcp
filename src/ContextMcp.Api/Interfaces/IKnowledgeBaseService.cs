using ContextMcp.Api.Models.ToolRequests;
using ContextMcp.Api.Models.ToolResponses;

namespace ContextMcp.Api.Interfaces
{
    public interface IKnowledgeBaseService
    {
            /// <summary>
            /// Stores a knowledge item in the knowledge base, creating a new entry or updating an existing one.
            /// </summary>
            /// <param name="request">The knowledge item to store, including its content and metadata.</param>
            /// <param name="cancellationToken">Cancellation token for the operation.</param>
            /// <returns>The result of the storage operation, including the ID of the stored item.</returns>
            Task<PostKnowledgeBaseToolResponse> PostKnowledgeAsync(PostKnowledgeBaseToolRequest request, CancellationToken cancellationToken);
    }
}
