using Insttantt.FlowManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insttantt.FlowManagement.Domain.Pattern
{
    public interface IFlowBuilder
    {
        FlowBuilder WithId(int id);
        void SetDetails(string name, string description);
        void AddStep(StepFlow paso);
        Flow Build();
    }
}
