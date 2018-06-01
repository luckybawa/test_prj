using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CORE2;
using Accounts.Service.Contract.Commands;
using Accounts.Service.Contract.Requests.Company;
using Microsoft.Extensions.Logging;
using Accounts.Service.Contract.Queries;
using Accounts.Service.Contract.DTOs;
using Accounts.Company.API.Utilities;

namespace Accounts.Company.API.Controllers
{
    [Route("api/[controller]")]
    public class CompaniesController : Controller
    {
        #region Variables
        private readonly ILogger<CompaniesController> _logger; 
        #endregion

        #region Constructor
        public CompaniesController(ILogger<CompaniesController> logger)
        {
            _logger = logger;
            _logger.LogInformation("Companies controller instantiated");
        } 
        #endregion

        #region // GET api/companies
        public async Task<IActionResult> GetAllGroups(
            [FromServices] IQueryHandler<GetAllGroupSmallDetailDTOQuery, ListGroupSmallDetailDTO> _getAllGroupSmallDetailDTOQueryHandler
            )
        {
            _logger.LogInformation("Running GET method to FETCH all group in database");
            //Gets Bizfly identity from header.
            BizflyIdentity bizflyIdentity = Request.Headers.GetIdentityFromHeaders();

            GetAllGroupSmallDetailDTOQuery query = new GetAllGroupSmallDetailDTOQuery(bizflyIdentity);

            ListGroupSmallDetailDTO result = await _getAllGroupSmallDetailDTOQueryHandler.HandleAsync(query);

            if (ModelState.IsValid)
            {
                _logger.LogInformation("");
                return Ok(result);
            }
            return BadRequest(ModelState);
        }
        #endregion

        #region // GET api/companies/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(
            string id,
            [FromServices] IQueryHandler<GetGroupSmallDetailDTOQuery, GroupSmallDetailDTO> _getGroupSmallDetailDTOQueryHandler
            )
        {
            _logger.LogInformation("Running GET method to FETCH group by ID");
            //Gets Bizfly identity from header.
            BizflyIdentity bizflyIdentity = Request.Headers.GetIdentityFromHeaders();

            GetGroupSmallDetailDTOQuery query = new GetGroupSmallDetailDTOQuery(bizflyIdentity, id);
            GroupSmallDetailDTO result = await _getGroupSmallDetailDTOQueryHandler.HandleAsync(query);

            if (ModelState.IsValid)
            {
                return Ok(result);
            }

            return BadRequest(ModelState);
        }
        #endregion

        #region // POST api/companies/{id}/suspend
        [HttpPost("{id}/suspend")]
        public async Task<IActionResult> PostToSuspendGroup(
            string id,
            [FromServices] ICommandHandler<SuspendGroupCommand> _suspendGroupCommandHandler)
        {
            _logger.LogInformation("Running POST method to SUSPEND group by ID");
            //Gets Bizfly identity from header.
            BizflyIdentity bizflyIdentity = Request.Headers.GetIdentityFromHeaders();

            SuspendGroupCommand command = new SuspendGroupCommand(bizflyIdentity, id, bizflyIdentity.UserId);

            await _suspendGroupCommandHandler.Handle(command);


            if (ModelState.IsValid)
                return Ok();
            return BadRequest(ModelState);

        }
        #endregion

        #region // POST api/companies/{id}/reactivate
        [HttpPost("{id}/reactivate")]
        public async Task<IActionResult> PostToReactiveGroup(
           string id,
           [FromServices] ICommandHandler<ReactivateGroupCommand> _reactivateGroupCommandHandler
           )
        {
            _logger.LogInformation("Running POST method to REACTIVATE group by ID");
            //Gets Bizfly identity from header.
            BizflyIdentity bizflyIdentity = Request.Headers.GetIdentityFromHeaders();

            ReactivateGroupCommand command = new ReactivateGroupCommand(bizflyIdentity, id, bizflyIdentity.UserId);
            await _reactivateGroupCommandHandler.Handle(command);


            if (ModelState.IsValid)
                return Ok();
            return BadRequest(ModelState);

        }
        #endregion

        #region // POST api/companies
        [HttpPost()]
        public async Task<IActionResult> PostToAddNewGroup(
            [FromBody] AddCompanyRequest request,
            [FromServices] ICommandHandler<AddGroupCommand> _addGroupCommandHandler
            )
        {
            _logger.LogInformation("Running POST method to ADD new group");
            if (request == null) { request = new AddCompanyRequest(); }


            //Gets Bizfly identity from header.
            BizflyIdentity bizflyIdentity = Request.Headers.GetIdentityFromHeaders();

            AddGroupCommand command = new AddGroupCommand(bizflyIdentity, request.NewGroupId, bizflyIdentity.UserId, request.GroupName, "COMPANY");
            await _addGroupCommandHandler.Handle(command);

            if (ModelState.IsValid)
            {
                _logger.LogInformation("New Group is created with id = {0}", request.NewGroupId);
                return CreatedAtAction(nameof(Get), new { id = request.NewGroupId }, null);
            }
            return BadRequest(ModelState);

        }
        #endregion

        #region // DELETE api/companies/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            string id,
            [FromServices]ICommandHandler<DeleteGroupCommand> _deleteGroupCommandHandler)
        {
            _logger.LogInformation("Running DELETE method for GroupId = {0} ", id);

            //Gets Bizfly identity from header.
            BizflyIdentity bizflyIdentity = Request.Headers.GetIdentityFromHeaders();


            DeleteGroupCommand command = new DeleteGroupCommand(bizflyIdentity, id, bizflyIdentity.UserId);
            await _deleteGroupCommandHandler.Handle(command);

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Group is deleted by ID = {0}", id);
                return NoContent();
            }
            return BadRequest(ModelState);

        } 
        #endregion
    }
}
