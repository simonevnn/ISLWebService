using AutoMapper;
using ISLWebService.Abstractions;
using ISLWebService.Dto;
using ISLWebService.Models;
using ISLWebService.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ISLWebService.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/utenti")]
    [Authorize(Roles = "RESPONSABILE, LAVORATORE")] //autorizzati entrambi, però va specificato nei metodi di amministrazione l'autorizzazione soltanto per il responsabile e nell'autenticazione l'AllowAnonymous (altrimenti non ci si può autenticare)
    public class UtentiController: Controller
    {
        private readonly IUtentiService repository;
        private readonly IMapper mapper;

        public UtentiController(IUtentiService repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        /**
         * AUTENTICAZIONE / GENERAZIONE JWT
         */
        [HttpPost("auth")]
        [ProducesResponseType(200, Type = typeof(ActionResult<JwtTokenDto>))]
        [ProducesResponseType(400, Type = typeof(ActionResult<InfoMsg>))]
        [AllowAnonymous]
        public async Task<ActionResult<JwtTokenDto>> Authenticate([FromBody] AuthDto userParam)
        {
            string tokenJwt = "";

            if (!await repository.Autentica(userParam.Username, userParam.Password))
                return BadRequest(new InfoMsg("Username o password errati.",DateTime.Today));
            else
                tokenJwt = await repository.GeneraToken(userParam.Username);

            return Ok(new JwtTokenDto(tokenJwt));
        }

        /**
         * RICERCA PER USERNAME
         */
        [HttpGet("cerca/username/{username}")]
        [ProducesResponseType(200, Type = typeof(ActionResult<UtentiDto>))]
        [ProducesResponseType(404, Type = typeof(ActionResult<InfoMsg>))]
        public async Task<ActionResult<UtentiDto>> GetUser(string username)
        {
            var utente = await repository.CercaUtente(username);

            if(utente == null)
                return NotFound(new InfoMsg($"Impossibile trovare l'utente con username {username}", DateTime.Today));

            return Ok(GetUtenteDto(utente));
        }

        /**
         * RICERCA PER FILTRO
         */
        [HttpGet("cerca/filtro/{filter}")]
        [ProducesResponseType(200, Type = typeof(ActionResult<IEnumerable<UtentiDto>>))]
        [ProducesResponseType(404, Type = typeof(ActionResult<InfoMsg>))]
        [Authorize(Roles = "RESPONSABILE")]
        public async Task<ActionResult<IEnumerable<UtentiDto>>> GetUsersByFilter(string filter)
        {

            var listaUtenti = await repository.CercaUtentiFiltro(filter);

            if (!listaUtenti.Any())
                return NotFound(new InfoMsg($"Impossibile trovare utenti", DateTime.Today));

            var listaUtentiDto = new List<UtentiDto>();

            foreach (var utente in listaUtenti)
                listaUtentiDto.Add(GetUtenteDto(utente));

            return Ok(listaUtentiDto);
        }

        /**
         * RICERCA PER RUOLO
         */
        [HttpGet("cerca/ruolo/{ruolo}")]
        [ProducesResponseType(200, Type = typeof(ActionResult<IEnumerable<UtentiDto>>))]
        [ProducesResponseType(404, Type = typeof(ActionResult<InfoMsg>))]
        [Authorize(Roles = "RESPONSABILE")]
        public async Task<ActionResult<IEnumerable<UtentiDto>>> GetUsersByRole(string ruolo)
        {
            var listaUtenti = await repository.CercaUtentiRuolo(ruolo);

            if (!listaUtenti.Any())
                return NotFound(new InfoMsg($"Impossibile trovare utenti con il ruolo indicato", DateTime.Today));

            var listaUtentiDto = new List<UtentiDto>();

            foreach (var utente in listaUtenti)
                listaUtentiDto.Add(GetUtenteDto(utente));

            return Ok(listaUtentiDto);
        }

        /**
         * ELENCO UTENTI
         */
        [HttpGet("elenco")]
        [ProducesResponseType(200, Type = typeof(ActionResult<IEnumerable<UtentiDto>>))]
        [ProducesResponseType(404, Type = typeof(ActionResult<InfoMsg>))]
        [Authorize(Roles = "RESPONSABILE")]
        public async Task<ActionResult<IEnumerable<UtentiDto>>> GetAllUsers()
        {
            var listaUtenti = await repository.ElencoUtenti();

            if (!listaUtenti.Any())
                return NotFound(new InfoMsg("Nessun utente nel Database.", DateTime.Today));

            var listaUtentiDto = new List<UtentiDto>();

            foreach (var utente in listaUtenti)
                listaUtentiDto.Add(GetUtenteDto(utente));

            return Ok(listaUtentiDto);
        }

        /**
         * ELENCO RUOLI
         */
        [HttpGet("ruoli")]
        [ProducesResponseType(200, Type = typeof(ActionResult<IEnumerable<RuoliDto>>))]
        [ProducesResponseType(404, Type = typeof(ActionResult<InfoMsg>))]
        [Authorize(Roles = "RESPONSABILE")]
        public async Task<ActionResult<IEnumerable<RuoliDto>>> GetAllRoles()
        {
            var listaRuoli = await repository.ElencoRuoli();

            if (!listaRuoli.Any())
                return NotFound(new InfoMsg("Nessun ruolo nel Database.", DateTime.Today));

            var listaRuoliDto = new List<RuoliDto>();

            foreach (var ruolo in listaRuoli)
                listaRuoliDto.Add(GetRuoloDto(ruolo));

            return Ok(listaRuoliDto);
        }

        /**
         * INSERIMENTO
         */
        [HttpPost("inserimento")]
        [ProducesResponseType(200, Type = typeof(ActionResult<InfoMsg>))]
        [ProducesResponseType(400, Type = typeof(ActionResult<InfoMsg>) )]
        [ProducesResponseType(422, Type = typeof(ActionResult<InfoMsg>))]
        [ProducesResponseType(500, Type = typeof(ActionResult<InfoMsg>))]
        [Authorize(Roles = "RESPONSABILE")]
        public async Task<ActionResult<InfoMsg>> AddUser([FromBody] Utenti utente)
        {
            if (utente == null)
                return BadRequest(new InfoMsg("Specificare i dati dell'utente.", DateTime.Today));

            if (!ModelState.IsValid)
            {
                string errStr = "";

                foreach(var modelState in ModelState.Values)
                    foreach(var modelError in modelState.Errors)
                        errStr += modelError.ErrorMessage + " - ";

                return BadRequest(new InfoMsg(errStr,DateTime.Today));
            }

            if (await repository.CercaUtente(utente.Username) != null)
                return StatusCode(422, new InfoMsg($"Lo username {utente.Username} è già in uso!", DateTime.Today));

            PasswordHasher hasher = new PasswordHasher();
            utente.Password = hasher.Hash(utente.Password);

            if (!await repository.InserisciUtente(utente))
                return StatusCode(500, new InfoMsg("Errore durante l'inserimento dell'utente.", DateTime.Today));

            return Ok(new InfoMsg($"Inserimento dell'utente {utente.Username} avvenuto con successo.", DateTime.Today));
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
        public async Task<ActionResult<InfoMsg>> UpdateUser([FromBody] Utenti utente)
        {
            if (utente == null)
                return BadRequest(new InfoMsg("Specificare i dati dell'utente.", DateTime.Today));

            if (await repository.CercaUtente(utente.Username) == null)
                return NotFound(new InfoMsg($"L'utente {utente.Username} non esiste.", DateTime.Today));

            if (!ModelState.IsValid)
            {
                string errStr = "";

                foreach (var modelState in ModelState.Values)
                    foreach (var modelError in modelState.Errors)
                        errStr += modelError.ErrorMessage + " - ";

                return BadRequest(new InfoMsg(errStr, DateTime.Today));
            }

            PasswordHasher hasher = new PasswordHasher();
            utente.Password = hasher.Hash(utente.Password);

            if (!await repository.ModificaUtente(utente))
                return StatusCode(500, new InfoMsg("Errore durante la modifica dell'utente.", DateTime.Today));

            return Ok(new InfoMsg($"Modifica dell'utente {utente.Username} avvenuta con successo.", DateTime.Today));
        }

        /**
         * RIMOZIONE
         */
        [HttpDelete("rimozione/{username}")]
        [ProducesResponseType(200, Type = typeof(ActionResult<InfoMsg>))]
        [ProducesResponseType(400, Type = typeof(ActionResult<InfoMsg>))]
        [ProducesResponseType(404, Type = typeof(ActionResult<InfoMsg>))]
        [ProducesResponseType(500, Type = typeof(ActionResult<InfoMsg>))]
        [Authorize(Roles = "RESPONSABILE")]
        public async Task<ActionResult<InfoMsg>> DeleteUser(string username)
        {
            if (username.IsNullOrEmpty())
                return BadRequest(new InfoMsg("Specificare lo username dell'utente da rimuovere.", DateTime.Today));

            var utente = await repository.CercaUtenteNoRuoli(username);

            if (utente == null)
                return NotFound(new InfoMsg($"L'utente {username} non esiste.", DateTime.Today));

            if (!await repository.RimuoviUtente(utente))
                return StatusCode(500, new InfoMsg("Errore durante la rimozione dell'utente.", DateTime.Today));

            return Ok(new InfoMsg($"Rimozione dell'utente {username} avvenuta con successo.", DateTime.Today));
        }

        /**
         * TRASFORMAZIONE DTO
         */
        private UtentiDto GetUtenteDto(Utenti utente) => mapper.Map<UtentiDto>(utente);
        private RuoliDto GetRuoloDto(Ruoli ruolo) => mapper.Map<RuoliDto>(ruolo);
    }
}
