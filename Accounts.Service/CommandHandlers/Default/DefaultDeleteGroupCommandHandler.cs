using Accounts.Service.Contract.Commands;
using Accounts.Service.Contract.Repository.Interfaces;
using Models.Accounts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Service.CommandHandlers.Default
{
    public class DefaultDeleteGroupCommandHandler : IDefaultCommandHandler<DeleteGroupCommand>
    {
        private readonly IGroupRepository _groupRepository;

        public DefaultDeleteGroupCommandHandler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task Handle(DeleteGroupCommand command)
        {
            await _groupRepository.RemoveDocumentAsync(command.Id);
        }
    }
}
