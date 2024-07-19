using AutoMapper;
using ISLWebService.Abstractions;
using ISLWebService.Dto;
using ISLWebService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ISLWebService.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/bancali")]
    [Authorize(Roles = "RESPONSABILE, LAVORATORE")]
    public class BancaliController: Controller
    {
        private readonly IBancaliService repository;
        private readonly IMapper mapper;

        public BancaliController(IBancaliService repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        /**
         * ELENCO BANCALI
         */
        [HttpGet("elenco/bancali")]
        [ProducesResponseType(200, Type = typeof(ActionResult<IEnumerable<BancaliDto>>))]
        [ProducesResponseType(404, Type = typeof(ActionResult<InfoMsg>))]
        public async Task<ActionResult<IEnumerable<BancaliDto>>> GetAllPallets()
        {
            var listaBancali = await repository.ElencoBancali();

            if (!listaBancali.Any())
                return NotFound(new InfoMsg("Nessun bancale nel magazzino.", DateTime.Today));

            var listaBancaliDto = new List<BancaliDto>();

            foreach (var bancale in listaBancali)
                listaBancaliDto.Add(GetBancaleDto(bancale));

            return Ok(listaBancaliDto);
        }

        /**
         * ELENCO MAGAZZINI
         */ 
        [HttpGet("elenco/magazzini")]
        [ProducesResponseType(200, Type = typeof(ActionResult<IEnumerable<Magazzini>>))]
        [ProducesResponseType(404, Type = typeof(ActionResult<InfoMsg>))]
        public async Task<ActionResult<IEnumerable<Magazzini>>> GetWarehouses()
        {
            var listaMagazzini = await repository.ElencoMagazzini();

            if (!listaMagazzini.Any())
                return NotFound(new InfoMsg("Nessun magazzino nel database.", DateTime.Today));

            return Ok(listaMagazzini);
        }

        /**
         * ELENCO AREE
         */
        [HttpGet("elenco/aree/{codiceMagazzino}")]
        [ProducesResponseType(200, Type = typeof(ActionResult<IEnumerable<Aree>>))]
        [ProducesResponseType(422, Type = typeof(ActionResult<InfoMsg>))]
        [ProducesResponseType(404, Type = typeof(ActionResult<InfoMsg>))]
        public async Task<ActionResult<IEnumerable<Aree>>> GetAreasByWarehouse(string codiceMagazzino)
        {
            var magazzino = await repository.CercaMagazzino(codiceMagazzino);

            if (magazzino == null) return StatusCode(422,new InfoMsg($"Il magazzino indicato non esiste.", DateTime.Today));

            var listaAree = await repository.ElencoAree(codiceMagazzino);

            if (!listaAree.Any())
                return NotFound(new InfoMsg("Nessuna area nel database.", DateTime.Today));

            return Ok(listaAree);
        }

        /**
         * ELENCO SCAFFALI
         */
        [HttpGet("elenco/scaffali/{codiceMagazzino}/{codiceArea}")]
        [ProducesResponseType(200, Type = typeof(ActionResult<IEnumerable<Scaffali>>))]
        [ProducesResponseType(400, Type = typeof(ActionResult<InfoMsg>))]
        [ProducesResponseType(404, Type = typeof(ActionResult<InfoMsg>))]
        public async Task<ActionResult<IEnumerable<Scaffali>>> GetRacksByArea(string codiceMagazzino, string codiceArea)
        {
            var area = await repository.CercaArea(codiceMagazzino, codiceArea);

            if (area == null) return StatusCode(422, new InfoMsg($"L'area indicata non esiste.", DateTime.Today));

            var listaScaffali = await repository.ElencoScaffali(codiceMagazzino,codiceArea);

            if (!listaScaffali.Any())
                return NotFound(new InfoMsg("Nessuno scaffale nell'area indicata.", DateTime.Today));

            return Ok(listaScaffali);
        }

        /**
         * ELENCO POSTI
         */
        [HttpGet("elenco/posti/{codiceMagazzino}/{codiceArea}/{codiceScaffale}")]
        [ProducesResponseType(200, Type = typeof(ActionResult<IEnumerable<Posti>>))]
        [ProducesResponseType(400, Type = typeof(ActionResult<InfoMsg>))]
        [ProducesResponseType(404, Type = typeof(ActionResult<InfoMsg>))]
        public async Task<ActionResult<IEnumerable<Posti>>> GetShelfsByRack(string codiceMagazzino, string codiceArea, string codiceScaffale)
        {
            var scaffale = await repository.CercaScaffale(codiceMagazzino, codiceArea, codiceScaffale);

            if (scaffale == null) return StatusCode(422, new InfoMsg($"Lo scaffale indicato non esiste.", DateTime.Today));

            var listaPosti = await repository.ElencoPosti(codiceMagazzino, codiceArea, codiceScaffale);

            if (!listaPosti.Any())
                return NotFound(new InfoMsg("Nessun posto nello scaffale indicato.", DateTime.Today));

            return Ok(listaPosti);
        }

        /**
         * RICERCA BANCALE PER CODICE
         */
        [HttpGet("cerca/codice/{codice}")]
        [ProducesResponseType(200, Type = typeof(ActionResult<BancaliDto>))]
        [ProducesResponseType(404, Type = typeof(ActionResult<InfoMsg>))]
        public async Task<ActionResult<BancaliDto>> GetPalletByCode(string codice)
        {
            var bancale = await repository.CercaBancaleCodice(codice);

            if (bancale == null) return NotFound(new InfoMsg($"Impossibile trovare il bancale con numero seriale {codice}", DateTime.Today));

            return Ok(GetBancaleDto(bancale));
        }

        /**
         * RICERCA BANCALE PER FILTRO
         */
        [HttpGet("cerca/filtro/{filtro}")]
        [ProducesResponseType(200, Type = typeof(ActionResult<IEnumerable<BancaliDto>>))]
        [ProducesResponseType(404, Type = typeof(ActionResult<InfoMsg>))]
        public async Task<ActionResult<IEnumerable<BancaliDto>>> GetPalletsByFilter(string filtro)
        {
            var listaBancali = await repository.CercaBancaliFiltro(filtro);

            if (!listaBancali.Any())
                return NotFound(new InfoMsg("Nessun bancale corrispondente al filtro fornito.", DateTime.Today));

            var listaBancaliDto = new List<BancaliDto>();

            foreach (var bancale in listaBancali)
                listaBancaliDto.Add(GetBancaleDto(bancale));

            return Ok(listaBancaliDto);
        }

        /**
         * RICERCA BANCALE PER POSTO
         */
        [HttpGet("cerca/posto/{codiceMagazzino}/{codiceArea}/{codiceScaffale}/{codicePosto}")]
        [ProducesResponseType(200, Type = typeof(ActionResult<BancaliDto>))]
        [ProducesResponseType(400, Type = typeof(ActionResult<InfoMsg>))]
        [ProducesResponseType(404, Type = typeof(ActionResult<InfoMsg>))]
        public async Task<ActionResult<BancaliDto>> GetPalletByShelf(string codiceMagazzino, string codiceArea, string codiceScaffale, string codicePosto)
        {
            var posto = await repository.CercaPosto(codiceMagazzino, codiceArea, codiceScaffale, codicePosto);

            if(posto == null) return StatusCode(422, new InfoMsg($"Il posto indicato non esiste.", DateTime.Today));

            var bancale = await repository.CercaBancalePosto(codiceMagazzino, codiceArea, codiceScaffale, codicePosto);

            if (bancale == null) return NotFound(new InfoMsg($"Nessun bancale nel posto indicato.", DateTime.Today));

            return Ok(GetBancaleDto(bancale));
        }

        /**
         * RICERCA BANCALI PER SCAFFALE
         */
        [HttpGet("cerca/scaffale/{codiceMagazzino}/{codiceArea}/{codiceScaffale}")]
        [ProducesResponseType(200, Type = typeof(ActionResult<IEnumerable<BancaliDto>>))]
        [ProducesResponseType(400, Type = typeof(ActionResult<InfoMsg>))]
        [ProducesResponseType(404, Type = typeof(ActionResult<InfoMsg>))]
        public async Task<ActionResult<IEnumerable<BancaliDto>>> GetPalletsByRack(string codiceMagazzino, string codiceArea, string codiceScaffale)
        {
            var scaffale = await repository.CercaScaffale(codiceMagazzino, codiceArea, codiceScaffale);

            if (scaffale == null) return StatusCode(422, new InfoMsg($"Lo scaffale indicato non esiste.", DateTime.Today));

            var listaBancali = await repository.CercaBancaliScaffale(codiceMagazzino, codiceArea, codiceScaffale);

            if (!listaBancali.Any())
                return NotFound(new InfoMsg($"Nessun bancale nello scaffale indicato.", DateTime.Today));

            var listaBancaliDto = new List<BancaliDto>();

            foreach (var bancale in listaBancali)
                listaBancaliDto.Add(GetBancaleDto(bancale));

            return Ok(listaBancaliDto);
        }

        /**
         * RICERCA BANCALI PER AREA
         */
        [HttpGet("cerca/area/{codiceMagazzino}/{codiceArea}")]
        [ProducesResponseType(200, Type = typeof(ActionResult<IEnumerable<BancaliDto>>))]
        [ProducesResponseType(400, Type = typeof(ActionResult<InfoMsg>))]
        [ProducesResponseType(404, Type = typeof(ActionResult<InfoMsg>))]
        public async Task<ActionResult<IEnumerable<BancaliDto>>> GetPalletsByZone(string codiceMagazzino, string codiceArea)
        {
            var area = await repository.CercaArea(codiceMagazzino, codiceArea);

            if (area == null) return StatusCode(422, new InfoMsg($"L'area indicata non esiste.", DateTime.Today));

            var listaBancali = await repository.CercaBancaliArea(codiceMagazzino, codiceArea);

            if (!listaBancali.Any())
                return NotFound(new InfoMsg($"Nessun bancale nell'area {codiceArea}.", DateTime.Today));

            var listaBancaliDto = new List<BancaliDto>();

            foreach (var bancale in listaBancali)
                listaBancaliDto.Add(GetBancaleDto(bancale));

            return Ok(listaBancaliDto);
        }

        /**
         * RICERCA BANCALI SPECIALI
         */
        [HttpGet("cerca/speciali")]
        [ProducesResponseType(200, Type = typeof(ActionResult<IEnumerable<BancaliDto>>))]
        [ProducesResponseType(404, Type = typeof(ActionResult<InfoMsg>))]
        public async Task<ActionResult<IEnumerable<BancaliDto>>> GetSpecialPallets()
        {
            var listaBancali = await repository.CercaCarichiSpeciali();

            if (!listaBancali.Any())
                return NotFound(new InfoMsg("Nessun carico speciale nel magazzino.", DateTime.Today));

            var listaBancaliDto = new List<BancaliDto>();

            foreach (var bancale in listaBancali)
                listaBancaliDto.Add(GetBancaleDto(bancale));

            return Ok(listaBancaliDto);
        }

        /**
         * INSERIMENTO
         */
        [HttpPost("inserimento")]
        [ProducesResponseType(200, Type = typeof(ActionResult<InfoMsg>))]
        [ProducesResponseType(400, Type = typeof(ActionResult<InfoMsg>))]
        [ProducesResponseType(422, Type = typeof(ActionResult<InfoMsg>))]
        [ProducesResponseType(500, Type = typeof(ActionResult<InfoMsg>))]
        public async Task<ActionResult<InfoMsg>> AddPallet([FromBody] Bancali bancale)
        {
            if(bancale == null)
                return BadRequest(new InfoMsg("Specificare i dati del bancale.", DateTime.Today));

            if (!ModelState.IsValid)
            {
                string errStr = "";

                foreach(var modelSate in ModelState.Values)
                    foreach(var modelError in modelSate.Errors)
                        errStr += modelError.ErrorMessage + " - ";

                return BadRequest(new InfoMsg(errStr,DateTime.Today));
            }

            Posti postoDesiderato = await repository.CercaPosto(bancale.CodiceMagazzino,bancale.CodiceArea,bancale.CodiceScaffale,bancale.CodicePosto);

            //controllo esistenza del posto
            if (postoDesiderato == null)
                return StatusCode(422, new InfoMsg($"Il posto indicato non esiste!", DateTime.Today));

            //controllo posto occupato
            if (postoDesiderato.Bancale != null)
                return StatusCode(422, new InfoMsg($"Il posto indicato è già occupato!", DateTime.Today));
            
            //controlli misure/peso posto
            if (bancale.Peso>postoDesiderato.MaxPeso)
                return StatusCode(422, new InfoMsg($"Il bancale {bancale.NumeroSeriale} supera il peso massimo per il posto {postoDesiderato.CodicePosto}!", DateTime.Today));

            if (bancale.Altezza>postoDesiderato.MaxAltezza)
                return StatusCode(422, new InfoMsg($"Il bancale {bancale.NumeroSeriale} supera l'altezza massima per il posto {postoDesiderato.CodicePosto}!", DateTime.Today));

            if (bancale.Lunghezza>postoDesiderato.MaxLunghezza)
                return StatusCode(422, new InfoMsg($"Il bancale {bancale.NumeroSeriale} supera la lunghezza massima per il posto {postoDesiderato.CodicePosto}!", DateTime.Today));

            if (bancale.Larghezza>postoDesiderato.MaxLarghezza)
                return StatusCode(422, new InfoMsg($"Il bancale {bancale.NumeroSeriale} supera la larghezza massima per il posto {postoDesiderato.CodicePosto}!", DateTime.Today));

            //generazione del numero seriale
            var nuovoCodice = await repository.GeneraNumeroSeriale();
            while (await repository.CercaBancaleCodice(nuovoCodice) != null)    //controlla che non sia già presente un bancale con il seguente codice
                nuovoCodice = await repository.GeneraNumeroSeriale();

            bancale.NumeroSeriale = nuovoCodice;

            if (!await repository.InserisciBancale(bancale))
                return StatusCode(500, new InfoMsg("Errore durante l'inserimento del bancale.", DateTime.Today));

            return Ok(new InfoMsg($"Inserimento del bancale {bancale.NumeroSeriale} avvenuto con successo.", DateTime.Today));
        }

        /**
         * MODIFICA
         */
        [HttpPut("modifica")]
        [ProducesResponseType(200, Type = typeof(ActionResult<InfoMsg>))]
        [ProducesResponseType(400, Type = typeof(ActionResult<InfoMsg>))]
        [ProducesResponseType(422, Type = typeof(ActionResult<InfoMsg>))]
        [ProducesResponseType(500, Type = typeof(ActionResult<InfoMsg>))]
        public async Task<ActionResult<InfoMsg>> UpdatePallet([FromBody] Bancali bancale)
        {
            if (bancale == null)
                return BadRequest(new InfoMsg("Specificare i dati del bancale.", DateTime.Today));

            if (!ModelState.IsValid)
            {
                string errStr = "";

                foreach (var modelSate in ModelState.Values)
                    foreach (var modelError in modelSate.Errors)
                        errStr += modelError.ErrorMessage + " - ";

                return BadRequest(new InfoMsg(errStr, DateTime.Today));
            }

            if (await repository.CercaBancaleCodice(bancale.NumeroSeriale) == null) //cosa fare se serve modificare il numero seriale?
                return StatusCode(422, new InfoMsg($"Il bancale con codice {bancale.NumeroSeriale} non esiste.", DateTime.Today));

            //se viene modificato il posto, quindi il codice di quello in ingresso è diverso da quello già presente
            if(bancale.CodicePosto != (await repository.CercaBancaleCodice(bancale.NumeroSeriale)).CodicePosto)
            {
                Posti postoDesiderato = await repository.CercaPosto(bancale.CodiceMagazzino, bancale.CodiceArea, bancale.CodiceScaffale, bancale.CodicePosto);

                if(postoDesiderato == null)
                    return StatusCode(422, new InfoMsg($"Il posto {postoDesiderato.CodicePosto} non esiste!", DateTime.Today));

                if (postoDesiderato.Bancale != null)
                    return StatusCode(422, new InfoMsg($"Il posto {postoDesiderato.CodicePosto} è già occupato!", DateTime.Today));

                if (bancale.Peso > postoDesiderato.MaxPeso)
                    return StatusCode(422, new InfoMsg($"Il bancale {bancale.NumeroSeriale} supera il peso massimo per il posto {postoDesiderato.CodicePosto}!", DateTime.Today));

                if (bancale.Altezza > postoDesiderato.MaxAltezza)
                    return StatusCode(422, new InfoMsg($"Il bancale {bancale.NumeroSeriale} supera l'altezza massima per il posto {postoDesiderato.CodicePosto}!", DateTime.Today));

                if (bancale.Lunghezza > postoDesiderato.MaxLunghezza)
                    return StatusCode(422, new InfoMsg($"Il bancale {bancale.NumeroSeriale} supera la lunghezza massima per il posto {postoDesiderato.CodicePosto}!", DateTime.Today));

                if (bancale.Larghezza > postoDesiderato.MaxLarghezza)
                    return StatusCode(422, new InfoMsg($"Il bancale {bancale.NumeroSeriale} supera la larghezza massima per il posto {postoDesiderato.CodicePosto}!", DateTime.Today));
            }

            if (!await repository.ModificaBancale(bancale))
                return StatusCode(500, new InfoMsg("Errore durante la modifica del bancale.", DateTime.Today));

            return Ok(new InfoMsg($"Modifica del bancale {bancale.NumeroSeriale} avvenuto con successo.", DateTime.Today));
        }

        /**
         * RIMOZIONE
         */
        [HttpDelete("rimozione/{codice}")]
        [ProducesResponseType(200, Type = typeof(ActionResult<InfoMsg>))]
        [ProducesResponseType(400, Type = typeof(ActionResult<InfoMsg>))]
        [ProducesResponseType(422, Type = typeof(ActionResult<InfoMsg>))]
        [ProducesResponseType(500, Type = typeof(ActionResult<InfoMsg>))]
        public async Task<ActionResult<InfoMsg>> DeletePallet(string codice)
        {
            if (codice.IsNullOrEmpty())
                return BadRequest(new InfoMsg("Specificare il codice del pallet da rimuovere.", DateTime.Today));

            var bancale = await repository.CercaBancaleCodice(codice);

            if (bancale == null)
                return NotFound(new InfoMsg($"Il bancale con codice {codice} non esiste.", DateTime.Today));

            if(!await repository.RimuoviBancale(bancale))
                return StatusCode(500, new InfoMsg("Errore durante la rimozione del bancale.", DateTime.Today));

            return Ok(new InfoMsg($"Rimozione del bancale {codice} avvenuta con successo.", DateTime.Today));
        }

        /**
         * TRASFORMAZIONE DTO
         */
        private BancaliDto GetBancaleDto(Bancali bancale) => mapper.Map<BancaliDto>(bancale);
    }
}
