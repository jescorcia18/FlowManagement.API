
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Insttantt.FlowManagement.Domain.Entities
{

    [Table("StepFlow")]
    public class StepFlow
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StepFlowId { get; set; }

        public int FlowId { get; set; }
        public int StepId { get; set; }

        public bool? ReceiveDataPrevious { get; set; }

        public int? PreviousStep { get; set; }

        public int ExecOrder { get; set; }

    }
}
