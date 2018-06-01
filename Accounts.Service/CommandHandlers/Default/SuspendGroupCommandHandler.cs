using Accounts.Service.Contract.Commands;
using Accounts.Service.Contract.Repository.Interfaces;
using Models.Accounts;
using System.Threading.Tasks;

namespace Accounts.Service.CommandHandlers.Default
{
    public class SuspendGroupCommandHandler : IDefaultCommandHandler<SuspendGroupCommand>
    {
        private readonly IGroupRepository _groupRepository;

        public SuspendGroupCommandHandler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task Handle(SuspendGroupCommand command)
        {
            Group groupToSuspend = await _groupRepository.GetByIdAsync(command.CompanyId);

            if (!groupToSuspend.Status.Equals(GroupStatus.SUSPENDED))
            {
                groupToSuspend.Status = GroupStatus.SUSPENDED;
                groupToSuspend.LastChangedCommandInformation = command.GetCommandInformation();
                await _groupRepository.AddOrUpdateAsync(groupToSuspend);
            }
        }
    }
}
