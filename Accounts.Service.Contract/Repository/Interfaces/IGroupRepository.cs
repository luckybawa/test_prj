using DocumentDB.Repository;
using Models.Accounts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Accounts.Service.Contract.Repository.Interfaces
{
    public interface IGroupRepository : IDocumentRepository<Group>
    {
    }
}
