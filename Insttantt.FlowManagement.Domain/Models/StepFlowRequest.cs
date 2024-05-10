using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insttantt.FlowManagement.Domain.Models
{
    public class StepFlowRequest
    {
        public int StepFlowId { get; set; }

        public int FlowId { get; set; }
        public int StepId { get; set; }

        public bool? ReceiveDataPrevious { get; set; }

        public int? PreviousStep { get; set; }

        public int ExecOrder { get; set; }
    }
}
