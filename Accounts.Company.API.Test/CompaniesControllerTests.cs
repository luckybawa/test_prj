using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Accounts.Company.API.Controllers;
using CORE2;
using Accounts.Service.Contract.Requests.Company;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Moq;
using Accounts.Service.CommandHandlers;
using Accounts.Service.Contract.Commands;
using Microsoft.Extensions.Logging;
using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;
using Accounts.Service.Contract.Queries;
using Accounts.Service.Contract.DTOs;
using Models.Accounts;
using System.Linq;
using CORE2.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Accounts.Company.API.Test
{
    [TestClass]
    public class CompaniesControllerTests
    {
        CompaniesController controller;
        Mock<ILogger<CompaniesController>> mocklogger;

        // these value don't affect anything in these Test because all the processing (validation, etc)
        //of these values are done in command handlers and this test are for controller. So the testing of original values are done in CommandHandlerTests  
        string headerUserId = "test-user-id";
        string headerGroupId = "test-group-id";
        string headerGroupType = "test-group-type";
        string permission ="admin,can do anthing,doesn't,matter";


        [TestInitialize]
        public void Init()
        {

            mocklogger = new Mock<ILogger<CompaniesController>>();
            controller = new CompaniesController(mocklogger.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            controller.ControllerContext.HttpContext.Request.Headers["userId"] = headerUserId;
            controller.ControllerContext.HttpContext.Request.Headers["groupId"] = headerGroupId;
            controller.ControllerContext.HttpContext.Request.Headers["groupType"] = headerGroupType;
            controller.ControllerContext.HttpContext.Request.Headers["permissions"] = permission;


        }

        [TestMethod]
        public async Task Test_PostActionWhenEverythingIsOk()
        {

            AddCompanyRequest payload = new AddCompanyRequest
            {
                GroupName = "test-group-name",
                NewGroupId = "5c261254-f832-49ad-9ad2-8c76c8e15359"
            };
            headerUserId = "5c261254-f832-49ad-9ad2-8c76c8e15359";
           var  mockAddGroupHandler = new Mock<IDefaultCommandHandler<AddGroupCommand>>();
            var actionResult = await controller.PostToAddNewGroup(payload, mockAddGroupHandler.Object);

            var createdResult = actionResult as CreatedAtActionResult;

            Assert.IsInstanceOfType(actionResult, typeof(CreatedAtActionResult));

            Assert.AreEqual("Get", createdResult.ActionName);
            Assert.AreEqual(payload.NewGroupId, createdResult.RouteValues["id"]);

            mockAddGroupHandler.Verify();


        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public async Task Test_PostActionWhen_HandlerThrowsError()
        {

            // Arrange
            // Fake payload
            AddCompanyRequest payload = new AddCompanyRequest
            {
                GroupName = string.Empty,
                NewGroupId = string.Empty
            };
            var mockAddGroupHandler = new Mock<IDefaultCommandHandler<AddGroupCommand>>();

            List<ValidationFailure> failure = new List<ValidationFailure>
            {
                new ValidationFailure("id","")
            };

            mockAddGroupHandler
                .Setup(hdl => hdl.Handle(It.IsAny<AddGroupCommand>()))
                .Throws(new ValidationException(failure))
                .Verifiable();
            var result = await controller.PostToAddNewGroup(payload, mockAddGroupHandler.Object);
        }

        [TestMethod]
        public async Task Test_DeleteActionWhenEverythingIsOk()
        {

          

            Group group = new Group("COMAPNY", "test-group", "5c261254-f832-49ad-9ad2-8c76c8e15359", GroupStatus.ACTIVE, null, null);

            GroupSmallDetailDTO groupDto = new GroupSmallDetailDTO(group);



            headerUserId = "5c261254-f832-49ad-9ad2-8c76c8e15359";
           var mockDeleteGroupHandler = new Mock<IDefaultCommandHandler<DeleteGroupCommand>>();
            mockDeleteGroupHandler.Setup(hdl => hdl.Handle(It.IsAny<DeleteGroupCommand>()))
         .Returns(Task.CompletedTask)
         .Verifiable();
            var result = await controller.Delete("5c261254-f832-49ad-9ad2-8c76c8e15359", mockDeleteGroupHandler.Object);

            Assert.IsInstanceOfType(result, typeof(NoContentResult));

            mockDeleteGroupHandler.Verify();
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public async Task Test_DeleteActionWhen_HandlerThrowsError()
        {

            // Arrange
            string id = string.Empty;

            Group group = new Group("COMAPNY", "test-group", "5c261254-f832-49ad-9ad2-8c76c8e15359", GroupStatus.ACTIVE, null, null);

            GroupSmallDetailDTO groupDto = new GroupSmallDetailDTO(group);
           var mockDeleteGroupHandler = new Mock<IDefaultCommandHandler<DeleteGroupCommand>>();

            List<ValidationFailure> failure = new List<ValidationFailure>
            {
                new ValidationFailure("id","")
            };

            mockDeleteGroupHandler
                .Setup(hdl => hdl.Handle(It.IsAny<DeleteGroupCommand>()))
                .Throws(new ValidationException(failure))
                .Verifiable();

            // Act
            var result = await controller.Delete(id, mockDeleteGroupHandler.Object);

        }


        [TestMethod]

        public async Task Test_GetActionWhen_EverythingIsOk()
        { 

            IEnumerable<Group> listOfGroup = new List<Group>()
            {
                new Group(),
                new Group(),
                new Group()
            };
            var expected = new ListGroupSmallDetailDTO(listOfGroup);
            var mockGetAllSmallDetailQueryHandler = new Mock<IQueryHandler<GetAllGroupSmallDetailDTOQuery, ListGroupSmallDetailDTO>>();
            mockGetAllSmallDetailQueryHandler
                .Setup(x => x.HandleAsync(It.IsAny<GetAllGroupSmallDetailDTOQuery>()))
                .ReturnsAsync(expected);

            var actionResult = await controller.GetAllGroups(mockGetAllSmallDetailQueryHandler.Object);


            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            var okResult = actionResult as OkObjectResult;

            var actual = okResult.Value;

            Assert.AreSame(expected, actual);
            Assert.AreEqual(expected.Count, listOfGroup.Count());

        }

        [TestMethod]
        public async Task TestGetAction_And_ReturnsGroup_WhenEverythingIsOK()
        {
            string id = "test-id";
            GroupSmallDetailDTO expected = new GroupSmallDetailDTO(new Group())
            {
                GroupId = id
            };

            var mockGetGroupSmallDetailQueryHandler = new Mock<IQueryHandler<GetGroupSmallDetailDTOQuery, GroupSmallDetailDTO>>();
            mockGetGroupSmallDetailQueryHandler
                .Setup(x => x.HandleAsync(It.IsAny<GetGroupSmallDetailDTOQuery>()))
                .ReturnsAsync(expected);

            // Act
            var actionResult = await controller.Get(id, mockGetGroupSmallDetailQueryHandler.Object);


            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));

            var okResult = actionResult as OkObjectResult;
            GroupSmallDetailDTO actual = okResult.Value as GroupSmallDetailDTO;

            Assert.AreSame(expected, actual);
            Assert.AreEqual(id, actual.GroupId);

        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public async Task TestGetAction_And_ThrowException_WhenGroupNotFound()
        {
            // Arrange
            string id = "test-id";
            var mockGetGroupSmallDetailQueryHandler = new Mock<IQueryHandler<GetGroupSmallDetailDTOQuery, GroupSmallDetailDTO>>();
            mockGetGroupSmallDetailQueryHandler
                .Setup(x => x.HandleAsync(It.IsAny<GetGroupSmallDetailDTOQuery>()))
                .Throws<ResourceNotFoundException>();

            // Act
            var actionResult = await controller.Get(id, mockGetGroupSmallDetailQueryHandler.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task TestGetAction_And_ThrowExce_WhenSomethingWentWrong()
        {
            // Arrange
            string id = "test-id";
            var mockGetGroupSmallDetailQueryHandler = new Mock<IQueryHandler<GetGroupSmallDetailDTOQuery, GroupSmallDetailDTO>>();
            mockGetGroupSmallDetailQueryHandler
                .Setup(x => x.HandleAsync(It.IsAny<GetGroupSmallDetailDTOQuery>()))
                .Throws<Exception>();

            // Act
            var actionResult = await controller.Get(id, mockGetGroupSmallDetailQueryHandler.Object); 
        }

        [TestMethod]
        public async Task Test_PostAction_toSuspend_WhenEverythingIsOk()
        {

            string id = "test-id";
            var mockSuspendGroupCommandHandler = new Mock<IDefaultCommandHandler<SuspendGroupCommand>>();
            var actionResult = await controller.PostToSuspendGroup(id, mockSuspendGroupCommandHandler.Object);

            Assert.IsInstanceOfType(actionResult, typeof(OkResult));

            mockSuspendGroupCommandHandler.Verify();
        }


        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public async Task TestPostAction_toSuspendGroup_WhenParameterAreIncorrect()
        {
            string id = "test-id";

            List<ValidationFailure> failure = new List<ValidationFailure>
            {
                new ValidationFailure("GroupName",""),
                new ValidationFailure("NewGroupId","")
            };

            var mockSuspendGroupCommandHandler = new Mock<IDefaultCommandHandler<SuspendGroupCommand>>();
            mockSuspendGroupCommandHandler
                .Setup(hdl => hdl.Handle(It.IsAny<SuspendGroupCommand>()))
                .Throws(new ValidationException(failure))
                .Verifiable();

            var result = await controller.PostToSuspendGroup(id, mockSuspendGroupCommandHandler.Object);
            mockSuspendGroupCommandHandler.Verify();

        }
    }
}
