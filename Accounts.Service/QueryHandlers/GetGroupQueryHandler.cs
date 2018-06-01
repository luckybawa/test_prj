using Accounts.Service.Contract.DTOs;
using Accounts.Service.Contract.Queries;
using Accounts.Service.Contract.Repository.Interfaces;
using CORE2;
using Models.Accounts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Service.QueryHandlers
{
    public class GetGroupQueryHandler : IQueryHandler<GetGroupSmallDetailDTOQuery, GroupSmallDetailDTO>
    {
        IGroupRepository _groupRepository;
        public GetGroupQueryHandler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;

        }
        public async Task<GroupSmallDetailDTO> HandleAsync(GetGroupSmallDetailDTOQuery query)
        {
            Group group = await _groupRepository.GetByIdAsync(query.Id);
            GroupSmallDetailDTO groupDto = new GroupSmallDetailDTO(group);
            return groupDto;
        }
    }
}
