using ISLWebService.Abstractions;
using ISLWebService.Models;
using Microsoft.EntityFrameworkCore;

namespace ISLWebService.Services
{
    public class TaskService: ITaskService
    {
        private readonly ISLDbContext dbContext;

        public TaskService(ISLDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Compiti>> ElencoTask()
        {
            return await dbContext.Task
                .Where(t => t.Status != 5)
                .Include(t => t.BancaliTask)
                    .ThenInclude(bt => bt.Bancale)
                .OrderBy(t => t.Urgenza)
                .ToListAsync();
        }

        public async Task<IEnumerable<Compiti>> ElencoTaskArchiviate()
        {
            return await dbContext.Task
                .Where(t => t.Status == 5)
                .Include(t => t.BancaliTask)
                    .ThenInclude(bt => bt.Bancale)
                .OrderBy(t => t.Urgenza)
                .ToListAsync();
        }

        public async Task<Compiti> CercaTaskId(uint id)
        {
            return await dbContext.Task
                .Where(t => t.Id == id)
                .Include(t => t.BancaliTask)
                    .ThenInclude(bt => bt.Bancale)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> InserisciTask(Compiti task)
        {
            dbContext.Add(task);
            return await Salva();
        }

        public async Task<bool> ModificaTask(Compiti task)
        {
            dbContext.Update(task);
            return await Salva();
        }

        public async Task<bool> RimuoviTask(Compiti task)
        {
            dbContext.Remove(task);
            return await Salva();
        }

        private async Task<bool> Salva()
        {
            var saved = await dbContext.SaveChangesAsync();
            return saved >= 0 ? true : false;
        }

    }
}
