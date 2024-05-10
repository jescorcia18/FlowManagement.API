using Insttantt.FlowManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insttantt.FlowManagement.Domain.Models
{
    public class FlowResponse
    {
        public int FlowId { get; set; }

        public string FlowName { get; set; } = string.Empty;
        public string? FlowDescription { get; set; }

        public List<StepFlowResponse>? StepFlowList { get; set; }
    }
}
