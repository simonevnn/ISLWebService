using ISLWebService.Models;

namespace ISLWebService.Abstractions
{
    public interface IBancaliService
    {
        Task<IEnumerable<Bancali>> ElencoBancali();
        Task<IEnumerable<Magazzini>> ElencoMagazzini();
        Task<IEnumerable<Aree>> ElencoAree(string codiceMagazzino);
        Task<IEnumerable<Scaffali>> ElencoScaffali(string codiceArea, string codiceMagazzino);
        Task<IEnumerable<Posti>> ElencoPosti(string codiceScaffale, string codiceArea, string codiceMagazzino);
        Task<Magazzini> CercaMagazzino(string codiceMagazzino);
        Task<Aree> CercaArea(string codiceMagazzino, string codiceArea);
        Task<Scaffali> CercaScaffale(string codiceMagazzino, string codiceArea, string codiceScaffale);
        Task<Posti> CercaPosto(string codiceMagazzino, string codiceArea, string codiceScaffale, string codicePosto);
        Task<Bancali> CercaBancaleCodice(string numeroSeriale);
        Task<IEnumerable<Bancali>> CercaBancaliFiltro(string filtro);
        Task<Bancali> CercaBancalePosto(string codiceMagazzino, string codiceArea, string codiceScaffale, string codicePosto);
        Task<IEnumerable<Bancali>> CercaBancaliScaffale(string codiceMagazzino, string codiceArea, string codiceScaffale);
        Task<IEnumerable<Bancali>> CercaBancaliArea(string codiceMagazzino, string codiceArea);
        Task<IEnumerable<Bancali>> CercaCarichiSpeciali();
        Task<bool> InserisciBancale(Bancali bancale);
        Task<bool> ModificaBancale(Bancali bancale);
        Task<bool> RimuoviBancale(Bancali bancale);
        Task<string> GeneraNumeroSeriale();
    }
}
