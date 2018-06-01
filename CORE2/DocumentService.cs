using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE2
{
    public class DocumentService
    {
        public DocumentClient DocumentClient { get; }
        private string DatabaseId { get; }
        private Uri DatabaseUri => UriFactory.CreateDatabaseUri(this.DatabaseId);
        private string CollectionId { get; }
        private Uri CollectionUri => UriFactory.CreateDocumentCollectionUri(this.DatabaseId, CollectionId);
        
        public DocumentService(DocumentServiceSettings settings, bool createDatabaseIfNotExists = false, bool createCollectionIfNotExists = false, bool usePartitionKey = true)
        {
            string endpoint = settings.Endpoint;
            string authKey = settings.AuthKey;
            string databaseId = settings.DatabaseId;
            string collectionId = settings.CollectionId;
            string partitionKey = settings.PartitionKey;

            //set
            this.DocumentClient = new DocumentClient(new Uri(endpoint), authKey);
            //    , new ConnectionPolicy()
            //{
            //    ConnectionMode = ConnectionMode.Direct,
            //    ConnectionProtocol = Protocol.Tcp
            //});
            this.DatabaseId = databaseId;
            this.CollectionId = collectionId;

            if (createDatabaseIfNotExists)
            {
                this.DocumentClient.CreateDatabaseIfNotExistsAsync(new Database() { Id = databaseId }).Wait();
            }
            else
            {
                this.DocumentClient.ReadDatabaseAsync(DatabaseUri).Wait();
            }

            if (createCollectionIfNotExists)
            {
                if (usePartitionKey)
                {
                    DocumentClient.CreateDocumentCollectionIfNotExistsAsync(DatabaseUri,
                    new DocumentCollection()
                    {
                        Id = collectionId,
                        PartitionKey = new PartitionKeyDefinition() { Paths = new System.Collections.ObjectModel.Collection<string> { partitionKey } }
                    },
                    new RequestOptions() { OfferThroughput = 400 }).Wait();
                }
                else
                {
                    DocumentClient.CreateDocumentCollectionIfNotExistsAsync(DatabaseUri,
                  new DocumentCollection()
                  {
                      Id = collectionId
                  //PartitionKey = new PartitionKeyDefinition() { Paths = new Collection<string> { partitionKey } }
              },
                  new RequestOptions() { OfferThroughput = 400 }).Wait();
                }
            }
            else
            {
                this.DocumentClient.ReadDatabaseAsync(DatabaseUri).Wait();
            }
        }
        private RequestOptions getPartitionRequestOptions(string partitionKey)
        {
            return new RequestOptions()
            {
                PartitionKey = new PartitionKey(partitionKey)
            };
        }
        private FeedOptions getCrossPartitionFeedOptions()
        {
            return new FeedOptions()
            {
                EnableCrossPartitionQuery = true
            };
        }

        private Uri getDocumentUri(string id)
        {
            return UriFactory.CreateDocumentUri(this.DatabaseId, this.CollectionId, id);
        }

        public async Task CreateNode<T>(T document) where T : Node
        {
            var result = await DocumentClient.CreateDocumentAsync(this.CollectionUri, document, getPartitionRequestOptions(document.GroupId));
        }

        public async Task UpdateNode<T>(T document) where T : Node
        {
            var result = await DocumentClient.ReplaceDocumentAsync(document, getPartitionRequestOptions(document.GroupId));
        }

        public async Task RemoveNode<T>(T document) where T : Node
        {
            var result = await DocumentClient.DeleteDocumentAsync(getDocumentUri(document.Id), getPartitionRequestOptions(document.GroupId));
        }

        public async Task WipeAll()
    {
        //var sql = "SELECT c* FROM c ORDER BY c.partitionKey";
        var query = DocumentClient.CreateDocumentQuery(CollectionUri, new FeedOptions() { EnableCrossPartitionQuery = true }).ToList();

        var deleteTasks = new List<Task>();
        foreach (dynamic document in query)
        {
            deleteTasks.Add(this.DocumentClient.DeleteDocumentAsync(
                UriFactory.CreateDocumentUri(this.DatabaseId, this.CollectionId, document.id),
                new RequestOptions() { PartitionKey = new PartitionKey(document.groupId) }));
        }

        Task.WaitAll(deleteTasks.ToArray());

    }
    public async Task WipeAllInPartition(string partitionKey)
    {
        var docs = DocumentClient.CreateDocumentQuery(this.CollectionUri, new FeedOptions() { PartitionKey = new PartitionKey(partitionKey) }).ToList();

        foreach (dynamic document in docs)
        {
            await this.DocumentClient.DeleteDocumentAsync(
                UriFactory.CreateDocumentUri(this.DatabaseId, this.CollectionId, document.id),
                new RequestOptions() { PartitionKey = new PartitionKey(partitionKey) });
        }
    }
}
}