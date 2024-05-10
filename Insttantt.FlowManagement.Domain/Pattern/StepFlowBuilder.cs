using Insttantt.FlowManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insttantt.FlowManagement.Domain.Pattern
{
    public class StepFlowBuilder
    {
        private StepFlow stepFlow;

        public StepFlowBuilder(int flowId, int stepId)
        {
            stepFlow = new StepFlow
            {
                FlowId = flowId,
                StepId = stepId
            };
        }

        public StepFlowBuilder WithStepFlowId(int stepFlowId)
        {
            stepFlow.StepFlowId = stepFlowId;
            return this;
        }

        public StepFlowBuilder WithReceiveDataPrevious(bool? receiveDataPrevious)
        {
            stepFlow.ReceiveDataPrevious = receiveDataPrevious;
            return this;
        }

        public StepFlowBuilder WithPreviousStep(int? previousStep)
        {
            stepFlow.PreviousStep = previousStep;
            return this;
        }

        public StepFlowBuilder WithExecOrder(int execOrder)
        {
            stepFlow.ExecOrder = execOrder;
            return this;
        }

        public StepFlow Build()
        {
            return stepFlow;
        }
        public static StepFlowBuilder Build(int flowId, int stepId)
        {
            return new StepFlowBuilder(flowId, stepId);
        }
    }
}
