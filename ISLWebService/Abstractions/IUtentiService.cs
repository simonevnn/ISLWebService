using ISLWebService.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace ISLWebService.Abstractions
{
    public interface IUtentiService
    {
        Task<IEnumerable<Utenti>> ElencoUtenti();
        Task<IEnumerable<Ruoli>> ElencoRuoli();
        Task<Utenti> CercaUtente(string username);
        Task<Utenti> CercaUtenteNoRuoli(string username);
        Task<IEnumerable<Utenti>> CercaUtentiFiltro(string filtro);
        Task<IEnumerable<Utenti>> CercaUtentiRuolo(string ruolo);
        Task<bool> InserisciUtente(Utenti utente);
        Task<bool> ModificaUtente(Utenti utente);
        Task<bool> RimuoviUtente(Utenti utente);
        Task<bool> Autentica(string username, string password);
        Task<string> GeneraToken(string username);
    }
}
