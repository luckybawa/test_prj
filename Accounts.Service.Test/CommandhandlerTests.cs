using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Accounts.Service.Contract.Commands;
using Accounts.Service.Validators;
using Accounts.Service.CommandHandlers.Default;
using Moq;
using Accounts.Service.Contract.Repository.Interfaces;
using Models.Accounts;
using Accounts.Service.Validators.Default;
using Accounts.Service.Authorisers.CompanyAuthorisers;
using CORE2;
using System.Threading.Tasks;
using FluentValidation;
using CORE2.Exceptions;

namespace Accounts.Service.Test
{
    [TestClass]
    public class CommandHandlerTests
    {
        Mock<IGroupRepository> _mockGroupRepository;
        AuthoriserDecorator<AddGroupCommand> _authoriserDecorator;
        ICommandHandler<SuspendGroupCommand> _suspendGroupHandler;

       [TestInitialize]
        public void Init()
        {
            _mockGroupRepository = new Mock<IGroupRepository>();
            _mockGroupRepository
                .Setup(repo => repo.AddAsync(It.IsAny<Group>(), null))
                .ReturnsAsync(It.IsAny<Group>());

           

            DefaultAddGroupCommandHandler addGroupCommandHandler = new DefaultAddGroupCommandHandler(_mockGroupRepository.Object);
            ValidationDecorator<AddGroupCommand> validationDecorator = new ValidationDecorator<AddGroupCommand>(addGroupCommandHandler, new DefaultAddGroupCommandValidator());

          _authoriserDecorator = new AuthoriserDecorator<AddGroupCommand>(validationDecorator, new CompanyAddGroupAuthoriser());

            _suspendGroupHandler = new SuspendGroupCommandHandler(_mockGroupRepository.Object);

        }

            

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public async Task HandlerThrowsException_WhenCommandIsInvalid()
        {
            var identity = new BizflyIdentity();

            

            //group id must be in Guid format
            string groupId = "878a04bd-ee90-40df-8745-354165893b7e";
            string groupName = "test-group";

           

           AddGroupCommand command = new AddGroupCommand(identity, groupId, identity.UserId, groupName, "COMPANY");

          
           await _authoriserDecorator.Handle(command);
        }

        [TestMethod]
        public async Task HandlerThrowsException_WhenIdentityIsInvalid()
        {
            var identity = new BizflyIdentity()
            {
                UserId = "878a04bd-ee90-40df-8745-354165893b7e"
            };



            //group id must be in Guid format
            string groupId = "878a04bd-ee90-40df-8745-354165893b7e";
            string groupName = "test-group";



            AddGroupCommand command = new AddGroupCommand(identity, groupId, identity.UserId, groupName, "COMPANY");


            await _authoriserDecorator.Handle(command);
            
        }


        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public async Task TestSuspendHandler_ToThrowNotFound_WhenGroupNotFound()
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

         

            SuspendGroupCommand command = new SuspendGroupCommand(identity, groupId, ownerId);


            await _suspendGroupHandler.Handle(command);

            _mockGroupRepository.Verify();
        }
    }
}
