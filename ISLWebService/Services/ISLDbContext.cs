using ISLWebService.Models;
using Microsoft.EntityFrameworkCore;

namespace ISLWebService.Services
{
    public class ISLDbContext: DbContext
    {
        protected readonly IConfiguration config;

        public ISLDbContext (DbContextOptions<ISLDbContext> options, IConfiguration config): base(options)
        {
            this.config = config;
        }

        public virtual DbSet<Bancali> Bancali => Set<Bancali>();
        public virtual DbSet<Posti> Posti => Set<Posti>();
        public virtual DbSet<Scaffali> Scaffali => Set<Scaffali>();
        public virtual DbSet<Aree> Aree => Set<Aree>();
        public virtual DbSet<Magazzini> Magazzini => Set<Magazzini>();
        public virtual DbSet<Utenti> Utenti => Set<Utenti>();
        public virtual DbSet<Ruoli> Ruoli => Set<Ruoli>();
        public virtual DbSet<Compiti> Task => Set<Compiti>();
        public virtual DbSet<BancaliTask> BancaliTask => Set<BancaliTask>();
        public virtual DbSet<UtentiTask> UtentiTasks => Set<UtentiTask>();
        public virtual DbSet<RuoliTask> RuoliTasks => Set<RuoliTask>();
        public virtual DbSet<UtentiRuoli> UtentiRuoli => Set<UtentiRuoli>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connString = config.GetConnectionString("ISLConnString");
            optionsBuilder.UseMySQL(connString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            /* CHIAVI */
            modelBuilder.Entity<Bancali>()
                .HasKey(b => new { b.NumeroSeriale });

            modelBuilder.Entity<Compiti>()
                .HasKey(c => new { c.Id });

            modelBuilder.Entity<Utenti>()
                .HasKey(u => new { u.Username });

            modelBuilder.Entity<Ruoli>()
                .HasKey(r => new { r.Id });

            modelBuilder.Entity<Magazzini>()
                .HasKey(m => new { m.CodiceMagazzino });

            modelBuilder.Entity<Aree>()
                .HasKey(a => new { a.CodiceArea, a.CodiceMagazzino });

            modelBuilder.Entity<Scaffali>()
                .HasKey(s => new {s.CodiceScaffale, s.CodiceArea, s.CodiceMagazzino });

            modelBuilder.Entity<Posti>()
                .HasKey(p => new { p.CodicePosto, p.CodiceScaffale, p.CodiceArea, p.CodiceMagazzino });

            modelBuilder.Entity<UtentiRuoli>()
                .HasKey(ur => new { ur.Username, ur.IdRuolo });

            modelBuilder.Entity<UtentiTask>()
                .HasKey(ut => new { ut.Username, ut.IdTask});

            modelBuilder.Entity<RuoliTask>()
                .HasKey(rt => new { rt.IdRuolo, rt.IdTask });

            modelBuilder.Entity<BancaliTask>()
                .HasKey(bt => new { bt.NumeroSerialeBancale, bt.IdTask });

            modelBuilder.Entity<TipologieBancali>()
                .HasKey(tb => new { tb.CodiceTipologia });

            /* LEGAMI */

            //1:N Magazzino:Aree
            modelBuilder.Entity<Aree>()
                .HasOne(a => a.Magazzino)
                .WithMany(m => m.Aree)
                .HasForeignKey(a => new { a.CodiceMagazzino });

            //1:N Area:Scaffali
            modelBuilder.Entity<Scaffali>()
                .HasOne(s => s.Area)
                .WithMany(a => a.Scaffali)
                .HasForeignKey(s => new { s.CodiceArea, s.CodiceMagazzino });

            //1:N Scaffale:Posti
            modelBuilder.Entity<Posti>()
                .HasOne(p => p.Scaffale)
                .WithMany(s => s.Posti)
                .HasForeignKey(p => new {p.CodiceScaffale,p.CodiceArea,p.CodiceMagazzino});

            //1:1 Bancale:Posto
            modelBuilder.Entity<Bancali>()
                .HasOne(b => b.Posto)
                .WithOne(p => p.Bancale)
                .HasForeignKey<Bancali>(b => new {b.CodicePosto,b.CodiceScaffale,b.CodiceArea,b.CodiceMagazzino});

            //1:N Tipologia:Bancale
            modelBuilder.Entity<Bancali>()
                .HasOne(b => b.TipologiaBancale)
                .WithMany(tb => tb.Bancali)
                .HasForeignKey(b => new { b.TipologiaCarico });

            //1:N Posto:Task
            modelBuilder.Entity<Compiti>()
                .HasOne(c => c.PostoProvenienza)
                .WithMany(p => p.TaskProvenienza)
                .HasForeignKey(c => new { c.CodicePostoProvenienza, c.CodiceScaffaleProvenienza, c.CodiceAreaProvenienza, c.CodiceMagazzinoProvenienza });

            modelBuilder.Entity<Compiti>()
                .HasOne(c => c.PostoDestinazione)
                .WithMany(p => p.TaskDestinazione)
                .HasForeignKey(c => new { c.CodicePostoDestinazione, c.CodiceScaffaleDestinazione, c.CodiceAreaDestinazione, c.CodiceMagazzinoDestinazione });

            //1:N Magazzino:Utenti
            modelBuilder.Entity<Utenti>()
                .HasOne(u => u.Magazzino)
                .WithMany(m => m.Utenti)
                .HasForeignKey(u => u.CodiceMagazzino);

            //1:N Utente:UtentiRuoli
            modelBuilder.Entity<UtentiRuoli>()
                .HasOne(ur => ur.Utente)
                .WithMany(u => u.UtentiRuoli)
                .HasForeignKey(ur => ur.Username);

            //1:N Ruolo:UtentiRuoli
            modelBuilder.Entity<UtentiRuoli>()
                .HasOne(ur => ur.Ruolo)
                .WithMany(r => r.UtentiRuoli)
                .HasForeignKey(ur => ur.IdRuolo);

            //1:N Utente:UtentiTask
            modelBuilder.Entity<UtentiTask>()
                .HasOne(ut => ut.Utente)
                .WithMany(u => u.UtentiTask)
                .HasForeignKey(ut => ut.Username);

            //1:N Task:UtentiTask
            modelBuilder.Entity<UtentiTask>()
                .HasOne(ut => ut.Task)
                .WithMany(t => t.UtentiTask)
                .HasForeignKey(ut => ut.IdTask);

            //1:N Ruolo:UtentiTask
            modelBuilder.Entity<RuoliTask>()
                .HasOne(rt => rt.Ruolo)
                .WithMany(r => r.RuoliTask)
                .HasForeignKey(rt => rt.IdRuolo);

            //1:N Task:RuoliTask
            modelBuilder.Entity<RuoliTask>()
                .HasOne(rt => rt.Task)
                .WithMany(t => t.RuoliTask)
                .HasForeignKey(rt => rt.IdTask);

            //1:N Task:BancaliTask
            modelBuilder.Entity<BancaliTask>()
                .HasOne(bt => bt.Task)
                .WithMany(t => t.BancaliTask)
                .HasForeignKey(bt => bt.IdTask);

            //1:N Bancale:BancaliTask
            modelBuilder.Entity<BancaliTask>()
                .HasOne(bt => bt.Bancale)
                .WithMany(b => b.BancaliTask)
                .HasForeignKey(bt => bt.NumeroSerialeBancale);

        }

    }
}
