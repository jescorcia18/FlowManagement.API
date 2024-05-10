using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insttantt.FlowManagement.Application.Common.Interfaces.Services
{
    public interface IFlowExecutionService
    {
       Task ExecuteFlow(int flowId);
    }
}
