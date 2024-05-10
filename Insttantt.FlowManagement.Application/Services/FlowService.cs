using Insttantt.FlowManagement.Application.Common.Interfaces.Repository;
using Insttantt.FlowManagement.Application.Common.Interfaces.Services;
using Insttantt.FlowManagement.Application.Middleware;
using Insttantt.FlowManagement.Domain.Entities;
using Insttantt.FlowManagement.Domain.Models;
using Insttantt.FlowManagement.Domain.Pattern;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insttantt.FlowManagement.Application.Services
{
    public class FlowService : IFlowService
    {
        #region Global Variables
        private readonly IFlowRepository _flowRepository;
        private readonly ILogger<ExceptionHandler> _logger;
        #endregion

        #region Constructor
        public FlowService(IFlowRepository flowRepository, ILogger<ExceptionHandler> logger)
        {
            _flowRepository = flowRepository;
            _logger = logger;
        }
        #endregion

        #region Public Methods
        public async Task<IEnumerable<Flow>> GetAllFlowAsync()
        {
            try
            {
                return await _flowRepository.GetAllFlowsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Flow Service (GetAllFlowsAsync) error: {ex.Message}");
                throw;
            }
        }

        public async Task<Flow> GetFlowByIdAsync(int id)
        {
            try
            {
                return await _flowRepository.GetFlowByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Flow Service (GetFlowByIdAsync) error: {ex.Message}");
                throw;
            }
        }

        public async Task<Flow> AddFlowAsync(FlowRequest flow)
        {
            try
            {
                var entity= await ToFlowBuild(flow); 
                return await _flowRepository.AddFlowAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Flow Service (AddFlowAsync) error: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateFlowAsync(int id, FlowRequest flow)
        {
            try
            {
                var entity = await ToFlowBuild(id, flow);
                await _flowRepository.UpdateFlowAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Flow Service (UpdateFlowAsync) error: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteFlowAsync(int id)
        {
            try
            {
                await _flowRepository.DeleteFlowAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Flow Service (DeleteFlowAsync) error: {ex.Message}");
                throw;
            }
        }
        #endregion

        #region PrivateMethods
        private async Task<Flow> ToFlowBuild(int id, FlowRequest request)
        {
            var flow = new FlowBuilder();

            flow.SetDetails(request.FlowName, request.FlowDescription!);
            flow.WithId(id);
            if(request.StepFlowList != null)
            {
                foreach (var stepFlow in request.StepFlowList)
                {
                    flow.AddStep(await GetStepsFlows(id, stepFlow!));
                }
            }
           return  flow.Build();
        }

        private async Task<Flow> ToFlowBuild( FlowRequest request)
        {
            var flow = new FlowBuilder();

            flow.SetDetails(request.FlowName, request.FlowDescription!);
            if (request.StepFlowList != null)
            {
                foreach (var stepFlow in request.StepFlowList)
                {
                    flow.AddStep(await GetStepsFlows(stepFlow!));
                }
            }
            return flow.Build();
        }

        private async Task<StepFlow> GetStepsFlows(int id, StepFlowRequest request)
        {
            var stepsFlows = new StepFlow();

                stepsFlows = new StepFlowBuilder(request.FlowId,request.StepId)
                    .WithStepFlowId(id)
                    .WithPreviousStep(request.PreviousStep)
                    .WithReceiveDataPrevious(request.ReceiveDataPrevious)
                    .WithExecOrder(request.ExecOrder)
                    .Build();
            return await Task.FromResult(stepsFlows);

        }

        private async Task<StepFlow> GetStepsFlows( StepFlowRequest request)
        {
            var stepsFlows = new StepFlow();

            stepsFlows = new StepFlowBuilder(request.FlowId, request.StepId)
                .WithPreviousStep(request.PreviousStep)
                .WithReceiveDataPrevious(request.ReceiveDataPrevious)
                .WithExecOrder(request.ExecOrder)
                .Build();
            return await Task.FromResult(stepsFlows);

        }

        private async Task<IEnumerable<FlowResponse>> MapToFlowResponse(IEnumerable<Flow> flow)
        {
            return await Task.FromResult(flow.Select(f => new FlowResponse
            {
                FlowId = f.FlowId,
                FlowName = f.FlowName,
                FlowDescription = f.FlowDescription,
                StepFlowList = f.StepFlow!.Select(sf => new StepFlowResponse
                {
                    StepFlowId = sf.StepFlowId,
                    FlowId = sf.FlowId,
                    StepId = sf.StepId,
                    ReceiveDataPrevious = sf.ReceiveDataPrevious,
                    PreviousStep = sf.PreviousStep,
                    ExecOrder = sf.ExecOrder
                }).ToList()
            }));
        }
        #endregion
    }
}
