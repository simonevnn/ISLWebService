using ISLWebService.Abstractions;
using ISLWebService.Dto;
using ISLWebService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISLWebService.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/task")]
    [Authorize(Roles = "RESPONSABILE, LAVORATORE")]
    public class TaskController: Controller
    {
        private readonly ITaskService repository;

        public TaskController(ITaskService repository)
        {
            this.repository = repository;
        }

        /**
         * ELENCO TASK
         */
        [HttpGet("elenco")]
        [ProducesResponseType(200, Type = typeof(ActionResult<IEnumerable<Compiti>>))]
        [ProducesResponseType(404, Type = typeof(ActionResult<InfoMsg>))]
        public async Task<ActionResult<IEnumerable<Compiti>>> GetAllTasks()
        {
            var listaTask = await repository.ElencoTask();

            if(!listaTask.Any())
                return NotFound(new InfoMsg("Nessuna task nel database.", DateTime.Today));

            return Ok(listaTask);
        }

        /**
         * ELENCO TASK ARCHIVIATE
         */
        [HttpGet("archiviate")]
        [ProducesResponseType(200, Type = typeof(ActionResult<IEnumerable<Compiti>>))]
        [ProducesResponseType(404, Type = typeof(ActionResult<InfoMsg>))]
        public async Task<ActionResult<IEnumerable<Compiti>>> GetAllCompleted()
        {
            var listaTask = await repository.ElencoTaskArchiviate();

            if (!listaTask.Any())
                return NotFound(new InfoMsg("Nessuna task archiviata nel database.", DateTime.Today));

            return Ok(listaTask);
        }

        /**
         * RICERCA PER ID
         */
        [HttpGet("cerca/{id}")]
        [ProducesResponseType(200, Type = typeof(ActionResult<Compiti>))]
        [ProducesResponseType(404, Type = typeof(ActionResult<InfoMsg>))]
        public async Task<ActionResult<Compiti>> GetTaskById(uint id)
        {
            var task = await repository.CercaTaskId(id);

            if (task == null) return NotFound(new InfoMsg($"Impossibile trovare la task con id {id}", DateTime.Today));

            return Ok(task);
        }

        /**
         * INSERIMENTO
         */
        [HttpPost("inserimento")]
        [ProducesResponseType(200, Type = typeof(ActionResult<InfoMsg>))]
        [ProducesResponseType(400, Type = typeof(ActionResult<InfoMsg>))]
        [ProducesResponseType(500, Type = typeof(ActionResult<InfoMsg>))]
        [Authorize(Roles = "RESPONSABILE")]
        public async Task<ActionResult<InfoMsg>> AddTask([FromBody] Compiti task)
        {
            if (task == null)
                return BadRequest(new InfoMsg("Specificare i dati della task.", DateTime.Today));

            if (!ModelState.IsValid)
            {
                string errStr = "";

                foreach (var modelSate in ModelState.Values)
                    foreach (var modelError in modelSate.Errors)
                        errStr += modelError.ErrorMessage + " - ";

                return BadRequest(new InfoMsg(errStr, DateTime.Today));
            }

            if(!await repository.InserisciTask(task))
                return StatusCode(500, new InfoMsg("Errore durante l'inserimento della task.", DateTime.Today));

            return Ok(new InfoMsg($"Inserimento della task avvenuto con successo.", DateTime.Today));
        }

        /**
         * MODIFICA
         */
        [HttpPut("modifica")]
        [ProducesResponseType(200, Type = typeof(ActionResult<InfoMsg>))]
        [ProducesResponseType(400, Type = typeof(ActionResult<InfoMsg>))]
        [ProducesResponseType(404, Type = typeof(ActionResult<InfoMsg>))]
        [ProducesResponseType(500, Type = typeof(ActionResult<InfoMsg>))]
        [Authorize(Roles = "RESPONSABILE")]
        public async Task<ActionResult<InfoMsg>> UpdateTask([FromBody] Compiti task)
        {
            if (task == null)
                return BadRequest(new InfoMsg("Specificare i dati della task.", DateTime.Today));

            if (!ModelState.IsValid)
            {
                string errStr = "";

                foreach (var modelSate in ModelState.Values)
                    foreach (var modelError in modelSate.Errors)
                        errStr += modelError.ErrorMessage + " - ";

                return BadRequest(new InfoMsg(errStr, DateTime.Today));
            }

            if(await repository.CercaTaskId(task.Id) == null)
                return NotFound(new InfoMsg($"La task con id {task.Id} non esiste.", DateTime.Today));

            if (!await repository.ModificaTask(task))
                return StatusCode(500, new InfoMsg("Errore durante la modifica della task.", DateTime.Today));

            return Ok(new InfoMsg($"Modifica della task avvenuta con successo.", DateTime.Today));
        }

        /**
         * ARCHIVIAZIONE
         */
        [HttpDelete("archivia/{id}")]
        [ProducesResponseType(200, Type = typeof(ActionResult<InfoMsg>))]
        [ProducesResponseType(404, Type = typeof(ActionResult<InfoMsg>))]
        [ProducesResponseType(422, Type = typeof(ActionResult<InfoMsg>))]
        [ProducesResponseType(500, Type = typeof(ActionResult<InfoMsg>))]
        public async Task<ActionResult<InfoMsg>> CompleteTask(uint id)
        {
            var task = repository.CercaTaskId(id);

            if (task == null)
                return NotFound(new InfoMsg($"La task con id {id} non esiste.", DateTime.Today));

            Compiti taskObj = await task;

            if (taskObj.Status == 5)
                return StatusCode(422, new InfoMsg($"La task con id {id} risulta già archiviata.", DateTime.Today));

            taskObj.Status = 5;

            if (!await repository.ModificaTask(taskObj))
                return StatusCode(500, new InfoMsg("Errore durante l'archiviazione della task.", DateTime.Today));

            return Ok(new InfoMsg($"Task archiviata con successo.", DateTime.Today));
        }

        /**
         * RIMOZIONE
         */
        [HttpDelete("rimozione/{id}")]
        [ProducesResponseType(200, Type = typeof(ActionResult<InfoMsg>))]
        [ProducesResponseType(404, Type = typeof(ActionResult<InfoMsg>))]
        [ProducesResponseType(500, Type = typeof(ActionResult<InfoMsg>))]
        [Authorize(Roles = "RESPONSABILE")]
        public async Task<ActionResult<InfoMsg>> DeleteTask(uint id)
        {
            var task = await repository.CercaTaskId(id);

            if(task == null)
                return StatusCode(404, new InfoMsg($"La task con id {id} non esiste.", DateTime.Today));

            if (!await repository.RimuoviTask(task))
                return StatusCode(500, new InfoMsg("Errore durante la rimozione della task.", DateTime.Today));

            return Ok(new InfoMsg($"Rimozione della task avvenuta con successo.", DateTime.Today));
        }
    }
}
