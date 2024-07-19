using ISLWebService.Abstractions;
using ISLWebService.Models;
using Microsoft.EntityFrameworkCore;

namespace ISLWebService.Services
{
    public class BancaliService : IBancaliService
    {
        private readonly ISLDbContext dbContext;

        public BancaliService(ISLDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Bancali>> ElencoBancali()
        {
            return await dbContext.Bancali
                .Include(b => b.Posto)
                    .ThenInclude(p => p.Scaffale)
                    .ThenInclude(s => s.Area)
                .ToListAsync();
        }

        public async Task<IEnumerable<Magazzini>> ElencoMagazzini()
        {
            return await dbContext.Magazzini
                .ToListAsync();
        }

        public async Task<IEnumerable<Aree>> ElencoAree(string codiceMagazzino)
        {
            return await dbContext.Aree
                .Where(a => a.CodiceMagazzino == codiceMagazzino)
                .ToListAsync();
        }

        public async Task<IEnumerable<Scaffali>> ElencoScaffali(string codiceMagazzino, string codiceArea)
        {
            return await dbContext.Scaffali
                .Where(s => s.CodiceMagazzino == codiceMagazzino && s.CodiceArea == codiceArea)
                .ToListAsync();
        }

        public async Task<IEnumerable<Posti>> ElencoPosti(string codiceMagazzino, string codiceArea, string codiceScaffale)
        {
            return await dbContext.Posti
                .Where(p => p.CodiceMagazzino == codiceMagazzino && p.CodiceArea == codiceArea && p.CodiceScaffale == codiceScaffale)
                .ToListAsync();
        }

        public async Task<Magazzini> CercaMagazzino(string codiceMagazzino)
        {
            return await dbContext.Magazzini
                .Where(m => m.CodiceMagazzino == codiceMagazzino)
                .FirstOrDefaultAsync();
        }

        public async Task<Aree> CercaArea(string codiceMagazzino, string codiceArea)
        {
            return await dbContext.Aree
                .Where(a => a.CodiceMagazzino == codiceMagazzino && a.CodiceArea == codiceArea)
                .FirstOrDefaultAsync();
        }

        public async Task<Scaffali> CercaScaffale(string codiceMagazzino, string codiceArea, string codiceScaffale)
        {
            return await dbContext.Scaffali
                .Where(s => s.CodiceMagazzino == codiceMagazzino && s.CodiceArea == codiceArea && s.CodiceScaffale == codiceScaffale)
                .FirstOrDefaultAsync();
        }

        public async Task<Posti> CercaPosto(string codiceMagazzino, string codiceArea, string codiceScaffale, string codicePosto)
        {
            return await dbContext.Posti
                .Where(p => p.CodiceMagazzino == codiceMagazzino && p.CodiceArea == codiceArea && p.CodiceScaffale == codiceScaffale && p.CodicePosto == codicePosto)
                .Include(p => p.Bancale)
                .FirstOrDefaultAsync();
        }

        public async Task<Bancali> CercaBancaleCodice(string numeroSeriale)
        {
            return await dbContext.Bancali
                .Where(b => b.NumeroSeriale == numeroSeriale)
                /*.Include(b => b.Posto)
                    .ThenInclude(p => p.Scaffale)
                    .ThenInclude(s => s.Area)*/
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Bancali>> CercaBancaliFiltro(string filtro)
        {
            return await dbContext.Bancali
                .Where(b => b.TipologiaCarico.Contains(filtro) || b.Mittente.Contains(filtro))
                .ToListAsync();
        }

        public async Task<Bancali> CercaBancalePosto(string codiceMagazzino, string codiceArea, string codiceScaffale, string codicePosto)
        {
            return await dbContext.Bancali
                .Where(b => b.CodiceMagazzino == codiceMagazzino && b.CodiceArea == codiceArea && b.CodiceScaffale == codiceScaffale && b.CodicePosto == codicePosto)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Bancali>> CercaBancaliScaffale(string codiceMagazzino, string codiceArea, string codiceScaffale)
        {
            return await dbContext.Bancali
                .Where(b => b.CodiceMagazzino == codiceMagazzino && b.CodiceArea == codiceArea && b.CodiceScaffale == codiceScaffale)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Bancali>> CercaBancaliArea(string codiceMagazzino, string codiceArea)
        {
            return await dbContext.Bancali
                .Where(b => b.CodiceMagazzino == codiceMagazzino && b.CodiceArea == codiceArea)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Bancali>> CercaCarichiSpeciali()
        {
            return await dbContext.Bancali
                .Where(b => b.CaricoSpeciale)
                .ToListAsync();
        }

        public async Task<bool> InserisciBancale(Bancali bancale)
        {
            dbContext.Add(bancale);
            return await Salva();
        }
        
        public async Task<bool> ModificaBancale(Bancali bancale)
        {
            dbContext.Update(bancale);
            return await Salva();
        }
        
        public async Task<bool> RimuoviBancale(Bancali bancale)
        {
            dbContext.Remove(bancale);
            return await Salva();
        }

        public async Task<string> GeneraNumeroSeriale()
        {
            Random generatore = new Random();
            return generatore.Next(0, 9999999).ToString()+generatore.Next(0, 999999).ToString();    //viene generato metà e metà
        }

        private async Task<bool> Salva()
        {
            var saved = await dbContext.SaveChangesAsync();
            return saved >= 0 ? true : false;
        }

    }
}
