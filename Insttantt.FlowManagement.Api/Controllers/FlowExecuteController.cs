using Insttantt.FlowManagement.Application.Common.Interfaces.Services;
using Insttantt.FlowManagement.Application.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace Insttantt.FlowManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlowExecuteController : ControllerBase
    {
        #region Global Variables
        private readonly IFlowExecutionService _flowExecutionService;
        private readonly ILogger<ExceptionHandler> _logger;
        #endregion

        #region Constructor
        public FlowExecuteController(IFlowExecutionService flowExecutionService, ILogger<ExceptionHandler> logger)
        {
            _flowExecutionService = flowExecutionService;
            _logger = logger;
        }
        #endregion

        [HttpPost]
        public async Task<IActionResult> RunFlow([FromBody] int flowId)
        {
            try
            {
               await _flowExecutionService.ExecuteFlow(flowId);
                return Ok("The flow execution was successful.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error RunFlow Controller: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}
