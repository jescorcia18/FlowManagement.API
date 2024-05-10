using Insttantt.FlowManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insttantt.FlowManagement.Domain.Models
{
    public class FlowRequest
    {
        public string FlowName { get; set; } = string.Empty;
        public string? FlowDescription { get; set; }

        public List<StepFlowRequest>? StepFlowList { get; set; }
    }
}
