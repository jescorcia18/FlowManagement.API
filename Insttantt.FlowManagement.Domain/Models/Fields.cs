using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insttantt.FlowManagement.Domain.Models
{
    public class Fields
    {
        public int FieldId { get; set; }
        public string FieldName { get; set; } = string.Empty;
        public string FieldType { get; set; } = string.Empty;
        public bool FieldRequired { get; set; }
        public string FieldValidation { get; set; } = string.Empty;
    }
}
