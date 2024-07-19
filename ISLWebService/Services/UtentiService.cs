using ISLWebService.Abstractions;
using ISLWebService.Helpers;
using ISLWebService.Models;
using ISLWebService.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ISLWebService.Services
{
    public class UtentiService: IUtentiService
    {
        private readonly ISLDbContext dbContext;
        private readonly AppSettings appSettings;
        
        public UtentiService( ISLDbContext dbContext, IOptions<AppSettings> appSettings)
        {
            this.dbContext = dbContext;
            this.appSettings = appSettings.Value;
        }

        public async Task<bool> Autentica(string username, string password)
        {
            bool flag = false;

            PasswordHasher hasher = new PasswordHasher();

            Utenti utente = await CercaUtente(username);

            if (utente != null)
            {
                string encPwd = utente.Password;
                flag = hasher.Verify(encPwd, password);
            }

            return flag;
        }

        public async Task<string> GeneraToken(string username)
        {
            Utenti utente = await CercaUtente(username);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            IEnumerable<UtentiRuoli> ruoliUtente = utente.UtentiRuoli;

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, utente.Username));

            foreach(var ur in ruoliUtente)
                claims.Add(new Claim(ClaimTypes.Role, ur.Ruolo.Descrizione));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                //Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }

        public async Task<Utenti> CercaUtente(string username)
        {
            
            return await dbContext.Utenti
                .Where(u => u.Username == username)
                .Include(u => u.UtentiRuoli)
                    .ThenInclude(ur => ur.Ruolo)
                .OrderBy(u => u.Username)
                .FirstOrDefaultAsync();

            /*
            var queryStr = @"SELECT utenti.*, ruoli.descrizione AS ruolo FROM utenti
                JOIN utenti_ruoli ON utenti.username = utenti_ruoli.username
                JOIN ruoli ON utenti_ruoli.id_ruolo = ruoli.id_ruolo
                WHERE utenti.username=@username";
            var usernameParam = new MySqlParameter("@username", username);

            return await dbContext.Utenti.FromSqlRaw(queryStr, usernameParam).FirstOrDefaultAsync();
            */

        }

        public async Task<Utenti> CercaUtenteNoRuoli(string username)
        {
            return await dbContext.Utenti
                .Where(u => u.Username == username)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Utenti>> CercaUtentiFiltro(string filtro)
        {
            return await dbContext.Utenti
                .Where(u => u.Username.Contains(filtro) || u.Cognome.Contains(filtro) || u.Nome.Contains(filtro))
                .Include(u => u.UtentiRuoli)
                    .ThenInclude(ur => ur.Ruolo)
                .OrderBy(u => u.Cognome)
                .ToListAsync();
        } 

        public async Task<IEnumerable<Utenti>> CercaUtentiRuolo(string ruolo)
        {
            return await dbContext.UtentiRuoli
                .Where(ur => ur.Ruolo.Descrizione == ruolo)
                .Select(ur => ur.Utente)
                .ToListAsync();
        }

        public async Task<IEnumerable<Utenti>> ElencoUtenti()
        {
            return await dbContext.Utenti
                .Include(u => u.UtentiRuoli)
                    .ThenInclude(ur => ur.Ruolo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Ruoli>> ElencoRuoli()
        {
            return await dbContext.Ruoli
                .OrderBy(r => r.Id)
                .ToListAsync();
        }

        public async Task<bool> InserisciUtente(Utenti utente)
        {
            dbContext.Add(utente);
            return await Salva();
        }

        public async Task<bool> ModificaUtente(Utenti utente)
        {
            dbContext.Update(utente);
            return await Salva();
        }

        public async Task<bool> RimuoviUtente(Utenti utente)
        {
            dbContext.Remove(utente);
            return await Salva();
        }

        private async Task<bool> Salva()
        {
            var saved = await dbContext.SaveChangesAsync();
            return saved >= 0 ? true : false;
        }

    }
}
