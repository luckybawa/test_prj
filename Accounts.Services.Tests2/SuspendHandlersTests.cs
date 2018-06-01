using Accounts.Service.Contract.Commands;
using Core;
using CORE2.Exceptions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Accounts.Services.Tests2
{
    public class SuspendHandlersTests

    {
        [Fact]
        public async Task SuspendHandlers_ThrowsResourceNotFoundExc()
        {
            var identity = new BizflyIdentity();

            string groupId = "test-group-id";
            string ownerId = "test-user-id";
            _mockGroupRepository
               .Setup(repo => repo.GetByIdAsync(groupId))
               .Throws(new ResourceNotFoundException())
               .Verifiable();


            //  _mockGroupRepository
            //.Setup(repo => repo.AddOrUpdateAsync(It.IsAny<Group>(), null))
            //.Throws(new ResourceNotFoundException());



            SuspendGroupCommand command = new SuspendGroupCommand(identity, ownerId, groupId);


            await _suspendGroupHandler.Handle(command);

            _mockGroupRepository.Verify();
        }
    }
}
