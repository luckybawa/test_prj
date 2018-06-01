using System;
using Microsoft.Azure.Documents.Client;
namespace DocumentDB.Repository
{
    public class DbInitializer : IDbInitializer
    {
        public DocumentClient GetClient(string endpointUrl, string authorizationKey, ConnectionPolicy connectionPolicy = null)
        {
            if (string.IsNullOrWhiteSpace(endpointUrl))
                throw new ArgumentNullException("endpointUrl");

            if (string.IsNullOrWhiteSpace(authorizationKey))
                throw new ArgumentNullException("authorizationKey");

            var _dbClient = new DocumentClient(new Uri(endpointUrl), authorizationKey, connectionPolicy ?? new ConnectionPolicy()
            {
                MaxConnectionLimit = 100,
                ConnectionMode = ConnectionMode.Direct,
                ConnectionProtocol = Protocol.Tcp,
                RetryOptions = new RetryOptions() { MaxRetryAttemptsOnThrottledRequests = 3, MaxRetryWaitTimeInSeconds = 60 }
            });

            return _dbClient;
        }
    }
}