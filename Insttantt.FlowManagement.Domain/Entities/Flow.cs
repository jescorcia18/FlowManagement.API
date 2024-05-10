using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Insttantt.FlowManagement.Domain.Entities
{
    [Table("Flow")]
    public class Flow
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FlowId { get; set; }

        [Required]
        public string FlowName { get; set; } = string.Empty;
        public string? FlowDescription { get; set; }

        public virtual ICollection<StepFlow> StepFlow { get; set; }

        public Flow()
        {
            StepFlow = new List<StepFlow>();
        }
    }
}
