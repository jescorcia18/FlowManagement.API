using Insttantt.FlowManagement.Application.Common.Interfaces.Services;
using Insttantt.FlowManagement.Application.Middleware;
using Insttantt.FlowManagement.Domain.Entities;
using Insttantt.FlowManagement.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Insttantt.FlowManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlowsController : ControllerBase
    {
        #region Global Variables
        private readonly IFlowService _flowService;
        private readonly ILogger<ExceptionHandler> _logger;
        #endregion

        #region Constructor
        public FlowsController(IFlowService flowService, ILogger<ExceptionHandler> logger)
        {
            _flowService = flowService;
            _logger = logger;
        }
        #endregion


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Flow>>> GetAllFlows()
        {
            try
            {
                _logger.LogInformation($"Start Endpoint : FlowController.GetAllFlows");
                var step = await _flowService.GetAllFlowAsync();
                _logger.LogInformation($"Finish Endpoint : FlowController.GetAllFlows");
                return Ok(step);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error FlowController.GetAllFlows: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFlowById(int id)
        {
            try
            {
                _logger.LogInformation($"Start Endpoint : FlowController.GetFlowById");
                var step = await _flowService.GetFlowByIdAsync(id);
                _logger.LogInformation($"Finish Endpoint : FlowController.GetFlowById");
                if (step == null)
                {
                    return NotFound("Flow not found");
                }
                return Ok(step);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error FlowController.GetFlowById: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddFlow([FromBody] FlowRequest request)
        {
            try
            {
                _logger.LogInformation($"Start Endpoint : StepController.AddFlow");
                var flow = await _flowService.AddFlowAsync(request);
                return Ok(flow);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating flow: {ex.Message}");
                return BadRequest( "An error occurred while creating the flow.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStepFlow(int id, FlowRequest step)
        {
            try
            {
                _logger.LogInformation($"Start Endpoint : StepController.UpdateStepFlow");
                var stepExist = await _flowService.GetFlowByIdAsync(id);
                if (id != stepExist.FlowId || stepExist == null)
                {
                    return BadRequest($"Flow with Id: {id} does not exist");
                }
                await _flowService.UpdateFlowAsync(id, step);
                _logger.LogInformation($"Finish Endpoint : StepController.UpdateStep");
                return Ok("Update step is successull");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error StepController.UpdateStep: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlow(int id)
        {
            try
            {
                _logger.LogInformation($"Start Endpoint : FlowController.DeleteFlow");
                await _flowService.DeleteFlowAsync(id);
                _logger.LogInformation($"Finish Endpoint : FlowController.DeleteFlow");
                return Ok("Delete step is successull");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error FlowController.DeleteFlow: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}
