using ISLWebService.Models;

namespace ISLWebService.Abstractions
{
    public interface ITaskService
    {
        Task<IEnumerable<Compiti>> ElencoTask();
        Task<IEnumerable<Compiti>> ElencoTaskArchiviate();
        Task<Compiti> CercaTaskId(uint id);
        Task<bool> InserisciTask(Compiti task);
        Task<bool> ModificaTask(Compiti task);
        Task<bool> RimuoviTask(Compiti task);
    }
}
