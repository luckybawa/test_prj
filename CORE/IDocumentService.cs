using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;

namespace Core
{
    public interface IDocumentService
    {
        DocumentClient DocumentClient { get; }

        Task CreateNode<T>(T document) where T : Node;
        Task RemoveNode<T>(T document) where T : Node;
        Task UpdateNode<T>(T document) where T : Node;
        Task WipeAll();
        Task WipeAllInPartition(string partitionKey);
    }
}