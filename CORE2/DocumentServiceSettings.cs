using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace CORE2
{
    public class DocumentServiceSettings
    {
        public DocumentServiceSettings()
        {

        }

        public DocumentServiceSettings(string endpoint, string authKey, string databaseId, string collectionId, string partitionKey)
        {
            Endpoint = endpoint;
            AuthKey = authKey;
            DatabaseId = databaseId;
            CollectionId = collectionId;
            PartitionKey = partitionKey;
        }

         public string Endpoint { get; set; }
        public string AuthKey { get; set; }
        //public string DatabaseId { get; set; }
        public string CollectionId { get; set; }
        public string PartitionKey { get; set; }

        public DocumentClient Client { get; set; }
        public string DatabaseId { get; set; }
    }
}
