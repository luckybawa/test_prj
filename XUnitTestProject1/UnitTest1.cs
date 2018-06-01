using Accounts.Company.API.Controllers;
using Accounts.Service.Contract.Repository.Interfaces;
using Accounts.Service.Contract.Requests.Company;
using System;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            //AddCompanyRequest payload = new AddCompanyRequest
            //{
            //    GroupName = string.Empty,
            //    NewGroupId = string.Empty
            //};

            //var mockRepo = new Mock<IGroupRepository>();
            //mockRepo.Setup(repo => repo.AddOrUpdateAsync())
            //   .Returns(Task.CompletedTask)
            //   .Verifiable();
            //CompanyController controller = new CompanyController(db);



            //var result = await controller.Post(payload);

            //var badrequest = result as BadRequestObjectResult;


            //Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }
    }
}
