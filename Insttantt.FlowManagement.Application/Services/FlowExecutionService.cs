using Insttantt.FlowManagement.Application.Common.Interfaces.Repository;
using Insttantt.FlowManagement.Application.Common.Interfaces.Services;
using Insttantt.FlowManagement.Domain.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Net;
using RestSharp;

using Insttantt.FlowManagement.Application.Middleware;
using Microsoft.Extensions.Logging;

namespace Insttantt.FlowManagement.Application.Services
{
    public class FlowExecutionService : IFlowExecutionService
    {
        #region Global Variables
        private readonly HttpClient _httpClient;
        private readonly IFlowRepository _flowRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ExceptionHandler> _logger;
        #endregion

        #region Constructor
        public FlowExecutionService(HttpClient httpClient, IFlowRepository flowRepository, IConfiguration configuration, ILogger<ExceptionHandler> logger)
        {
            _httpClient = httpClient;
            _flowRepository = flowRepository;
            _configuration = configuration;
            _logger = logger;
        }
        #endregion

        #region Public Methods

        public async Task ExecuteFlow(int flowId)
        {
            var flow = await _flowRepository.GetFlowByIdAsync(flowId);
            if (flow != null)
            {
                if (flow.StepFlow != null && flow.StepFlow.Count > 0)
                {
                    //Sort the steps in order of execution
                    var orderedSteps = flow.StepFlow.OrderBy(s => s.ExecOrder).ToList();

                    // List to store step execution tasks
                    var executionTasks = new List<Task>();
                    Parallel.ForEach(orderedSteps, async step =>
                    {
                        if (step.ReceiveDataPrevious == true && step.PreviousStep != null)
                        {
                            //Wait for the previous step to complete before running this step
                            WaitForPreviousStepCompletion(step.PreviousStep.Value).Wait();
                        }
                        var stepResult = await GetStepById(step.StepId);
                        executionTasks.Add(ExecuteStep(stepResult));
                    });

                    // Wait for all step execution tasks to complete
                    await Task.WhenAll(executionTasks);
                }
            }
        }
        #endregion

        #region Private Methods

        private async Task WaitForPreviousStepCompletion(int previousStepId)
        {
            bool isPreviousStepCompleted = false;

            var previousStep = await GetStepById(previousStepId);

            while (!isPreviousStepCompleted)
            {
                try
                {
                    var response = await ExecuteStep(previousStep);

                    if (string.IsNullOrEmpty(response)!)
                    {
                        isPreviousStepCompleted = true;
                        _logger.LogError($"FlowExecution: request made successfully!");
                    }
                    else
                    {
                        _logger.LogError($"FlowExecution: request made unsuccessfully!");
                        isPreviousStepCompleted = true;
                    }
                }
                catch (Exception)
                {
                    // If an error occurs while making the request, wait a while before trying again
                    _logger.LogError($"FlowExecution: An error occurred while making the request");
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
            }
        }
        private async Task<string?> ExecuteStep(Steps step)
        {
            var fieldInputList = new List<Fields>();
            var fieldOutPutList = new List<Fields>();

            var client = new RestClient(step.UrlEndPoint);
            var method = await GetMethodType(step.RequestType);
            var parameterType = await GetParameterType(step.ParameterType);
            var restRequest = new RestRequest(string.Empty, method);
            restRequest.AddHeader("Content-Type", "application/json");

            if (step.StepFieldsList != null && step.StepFieldsList.Count > 0)
            {
                foreach (var stepfield in step.StepFieldsList)
                {
                    var fields = await GetFieldsById(stepfield.FieldId);
                    if (fields != null)
                    {
                        if (stepfield.InputOuput == "I")
                            fieldInputList.Add(fields);
                        else
                            fieldOutPutList.Add(fields);
                    }
                }
                var parameter = await SerializeFieldsAsync(fieldInputList);
                restRequest.AddParameter("application/json", parameter, parameterType);
            }

            var response = await client.ExecuteAsync(restRequest);
            var resp = response.Content;
            var result = new object();
            if (response.StatusCode == HttpStatusCode.OK && response.Content != null)
            {
                if (fieldOutPutList != null && fieldOutPutList.Count > 0)
                {
                    var objOutput = await SerializeFieldsAsync(fieldOutPutList);
                    result = JsonConvert.DeserializeObject<object>(resp!);
                }
                return JsonConvert.SerializeObject(result);
            }
            else
                return null;
        }

        private async Task<Method> GetMethodType(string method)
        {
            switch (method)
            {
                case "GET":
                    return Method.Get;
                case "POST":
                    return Method.Post;
                case "PUT":
                    return Method.Put;
                case "DELETE":
                    return Method.Delete;
                default:
                    return Method.Get;
            }
        }

        private async Task<ParameterType> GetParameterType(string parameter)
        {
            switch (parameter)
            {
                case "BODY":
                    return ParameterType.RequestBody;
                case "HEADER":
                    return ParameterType.HttpHeader;
                case "QUERY":
                    return ParameterType.QueryString;
                case "PATH":
                    return ParameterType.UrlSegment;
                case "GETPOST":
                    return ParameterType.GetOrPost;
                default:
                    return ParameterType.RequestBody;
            }
        }

        private async Task<Steps> GetStepById(int stepId)
        {
            using (var httpClient = new HttpClient())
            {
                var url = _configuration["Api:Step"];
                var response = await httpClient.GetAsync($"{url}{stepId}");
                response.EnsureSuccessStatusCode();
                var resp = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Steps>(resp);
                return result!;
            }
        }

        private async Task<Fields> GetFieldsById(int fieldId)
        {
            using (var httpClient = new HttpClient())
            {
                var url = _configuration["Api:Field"];
                var response = await httpClient.GetAsync($"{url}{fieldId}");
                response.EnsureSuccessStatusCode();
                var resp = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Fields>(resp);
                return result!;
            }
        }

        private async Task<string> SerializeFieldsAsync(List<Fields> fields)
        {
            return await Task.Run(() => JsonConvert.SerializeObject(fields));
        }
        #endregion
    }
}
