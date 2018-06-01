using Accounts.Service.Contract.Repository.Interfaces;
using CORE2;
using DocumentDB.Repository;
using Models.Accounts;

namespace Accounts.Service.Contract.Repository
{
    public  class GroupRepository : DocumentRepository<Group>, IGroupRepository
    {
        public GroupRepository(DocumentServiceSettings settings,string partitionKey) 
            : base(settings, partitionKey)
        {

        }
    }
}
