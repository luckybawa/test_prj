using Microsoft.Azure.Documents.Client;
using System.Threading.Tasks;

namespace DocumentDB.Repository
{
    public interface IDbInitializer
    {
        DocumentClient GetClient(string endpointUrl, string authorizationKey, ConnectionPolicy connectionPolicy = null);
    }
}