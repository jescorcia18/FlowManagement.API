using Insttantt.FlowManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insttantt.FlowManagement.Application.Common.Interfaces.Repository
{
    public interface IFlowRepository
    {
        Task<IEnumerable<Flow>> GetAllFlowsAsync();
        Task<Flow> GetFlowByIdAsync(int id);
        Task<Flow> AddFlowAsync(Flow flow);
        Task UpdateFlowAsync(Flow flow);
        Task DeleteFlowAsync(int id);
    }
}
