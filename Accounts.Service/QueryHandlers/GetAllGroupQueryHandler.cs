using Accounts.Service.Contract.DTOs;
using Accounts.Service.Contract.Queries;
using Accounts.Service.Contract.Repository.Interfaces;
using CORE2;
using Models.Accounts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Accounts.Service.QueryHandlers
{
    public class GetAllGroupQueryHandler : IQueryHandler<GetAllGroupSmallDetailDTOQuery, ListGroupSmallDetailDTO>
    {
        IGroupRepository _groupRepository;
        public GetAllGroupQueryHandler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }
        public async Task<ListGroupSmallDetailDTO> HandleAsync(GetAllGroupSmallDetailDTOQuery query)
        {
            IEnumerable<Group>  domainGroupResult = await _groupRepository.GetAllAsync();  
            ListGroupSmallDetailDTO grouplist = new ListGroupSmallDetailDTO(domainGroupResult);
            return grouplist;
        }
    }
}
