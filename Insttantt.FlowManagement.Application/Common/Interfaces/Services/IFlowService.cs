using Insttantt.FlowManagement.Domain.Entities;
using Insttantt.FlowManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insttantt.FlowManagement.Application.Common.Interfaces.Services
{
    public interface IFlowService
    {
        Task<IEnumerable<Flow>> GetAllFlowAsync();
        Task<Flow> GetFlowByIdAsync(int id);
        Task<Flow> AddFlowAsync(FlowRequest flow);
        Task UpdateFlowAsync(int id, FlowRequest flow);
        Task DeleteFlowAsync(int id);
    }
}
