using Accounts.Service.Contract.Commands;
using Accounts.Service.Contract.Repository.Interfaces;
using Models.Accounts;
using System.Threading.Tasks;

namespace Accounts.Service.CommandHandlers.Default
{
    public class ReactivateGroupCommandHandler : IDefaultCommandHandler<ReactivateGroupCommand>
    {
        private readonly IGroupRepository _groupRepository;

        public ReactivateGroupCommandHandler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task Handle(ReactivateGroupCommand command)
        {
            Group groupToReactivate = await _groupRepository.GetByIdAsync(command.CompanyId);

            if(!groupToReactivate.Status.Equals(GroupStatus.ACTIVE))
            {
                groupToReactivate.Status = GroupStatus.ACTIVE;
                groupToReactivate.LastChangedCommandInformation = command.GetCommandInformation();
                await _groupRepository.AddOrUpdateAsync(groupToReactivate);   
            }
        }
    }
}
