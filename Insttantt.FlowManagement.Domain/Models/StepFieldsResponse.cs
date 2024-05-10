using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insttantt.FlowManagement.Domain.Models
{
    public class StepFieldsResponse
    {
        public int StepFieldId { get; set; }
        public int StepId { get; set; }
        public int FieldId { get; set; }
        public string InputOuput { get; set; } = string.Empty;
    }
}
