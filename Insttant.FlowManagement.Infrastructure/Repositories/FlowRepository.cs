
using Insttantt.FlowManagement.Application.Common.Interfaces.Repository;
using Insttantt.FlowManagement.Application.Middleware;
using Insttantt.FlowManagement.Domain.Entities;
using Insttantt.FlowManagement.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Insttantt.FlowManagement.Infrastructure.Repositories
{
    public class FlowRepository : IFlowRepository
    {
        #region Global Variables
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ExceptionHandler> _logger;
        #endregion

        #region Constructor
        public FlowRepository(ApplicationDbContext context, ILogger<ExceptionHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        #endregion

        #region Public Methods
        public async Task<IEnumerable<Flow>> GetAllFlowsAsync()
        {
            try
            {
                return await _context.flow.Include(x => x.StepFlow).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Flow Repository (GetAll) error: {ex.Message}");
                throw;
            }
        }
        public async Task<Flow> GetFlowByIdAsync(int id)
        {
            try
            {
                var result = await _context.flow
                    .Include(s => s.StepFlow)
                    .FirstOrDefaultAsync(s => s.FlowId == id);

                return result!;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Flow Repository (GetById) error: {ex.Message}");
                throw;
            }

        }
        public async Task<Flow> AddFlowAsync(Flow flow)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.flow.Add(flow);
                    await _context.SaveChangesAsync();

                    transaction.Commit();

                    return flow;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogError($"Flow Repository (Add) error: {ex.Message}");
                    throw; 
                }
            }

        }
        public async Task UpdateFlowAsync(Flow flow)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Obtain the existing database flow
                    var flowExist = await _context.flow.Include(f => f.StepFlow).FirstOrDefaultAsync(f => f.FlowId == flow.FlowId);

                    if (flowExist != null)
                    {
                        // Update existing flow properties
                        flowExist.FlowName = flow.FlowName;
                        flowExist.FlowDescription = flow.FlowDescription;

                        // Update the flowsteps associated with the flow
                        foreach (var stepUpdate in flow.StepFlow)
                        {
                            // Search for the existing stepFlow in the existing flow
                            var stepFlowExist = flowExist.StepFlow.FirstOrDefault(sf => sf.StepFlowId == stepUpdate.StepFlowId);

                            if (stepFlowExist != null)
                            {
                                // Update existing stepFlow properties
                                stepFlowExist.ReceiveDataPrevious = stepUpdate.ReceiveDataPrevious;
                                stepFlowExist.PreviousStep = stepUpdate.PreviousStep;
                                stepFlowExist.ExecOrder = stepUpdate.ExecOrder;
                            }
                        }

                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();
                    }
                    else
                    {
                        throw new ArgumentException("The flow to be updated was not found.");
                    }
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }

        }
        public async Task DeleteFlowAsync(int id)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Obtener el flujo existente de la base de datos con sus pasos asociados
                    var deleteFlow = await _context.flow.Include(f => f.StepFlow).FirstOrDefaultAsync(f => f.FlowId == id);

                    if (deleteFlow != null)
                    {
                        // Eliminate flow steps associated with the flow
                        _context.stepFlow.RemoveRange(deleteFlow.StepFlow);

                        // Eliminate the flow
                        _context.flow.Remove(deleteFlow);

                        _context.SaveChanges();

                        transaction.Commit();
                    }
                    else
                    {
                        throw new ArgumentException("The flow to be eliminated was not found.");
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        #endregion
    }
}
