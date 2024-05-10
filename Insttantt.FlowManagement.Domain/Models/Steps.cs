using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insttantt.FlowManagement.Domain.Models
{
    public class Steps
    {
        public int StepId { get; set; }
        public string StepName { get; set; } = string.Empty;
        public string StepDescription { get; set; } = string.Empty;
        public string UrlEndPoint { get; set; } = string.Empty;
        public string RequestType { get; set; } = string.Empty;
        public string ParameterType { get; set; } = string.Empty;
        public List<StepFieldsResponse>? StepFieldsList { get; set; }
    }
}
