
using Insttantt.FlowManagement.Domain.Entities;

namespace Insttantt.FlowManagement.Domain.Pattern
{
    public class FlowBuilder : IFlowBuilder
    {
        private Flow _flow;

        public FlowBuilder()
        {
            _flow = new Flow();
        }

        public FlowBuilder WithId(int id)
        {
            _flow.FlowId = id;
            return this;
        }

        public void SetDetails(string name, string description)
        {
            _flow.FlowName = name;
            _flow.FlowDescription = description;
        }

        public void AddStep(StepFlow paso)
        {
            _flow.StepFlow.Add(paso);
        }

        public Flow Build() { 
            return _flow;
        }
    }
}
