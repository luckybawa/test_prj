using Accounts.Service.Contract.Commands;
using Accounts.Service.Contract.Repository.Interfaces;
using Models.Accounts;
using System.Threading.Tasks;

namespace Accounts.Service.CommandHandlers.Default
{
    public class DefaultAddGroupCommandHandler : IDefaultCommandHandler<AddGroupCommand>
    {
        private readonly IGroupRepository _groupRepository;

        public DefaultAddGroupCommandHandler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task Handle(AddGroupCommand command)
        {
            var commandInformation = command.GetCommandInformation();

            var group = new Group(command.NewGroupId, "Company", command.GroupName, command.OwnerUserId, GroupStatus.ACTIVE, null, null, commandInformation);
            
            await _groupRepository.AddAsync(group);
        }

    }
}
