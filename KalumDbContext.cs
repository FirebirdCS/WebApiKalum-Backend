using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;
using WebApiKalum_Backend.Entities;

namespace WebApiKalum
{
    public class KalumDbContext : DbContext
    {
        public DbSet<CarreraTecnica> CarreraTecnica { get; set; }
        public DbSet<Aspirante> Aspirante { get; set; }
        public DbSet<Jornada> Jornada { get; set; }
        public DbSet<ExamenAdmision> ExamenAdmision { get; set; }
        public DbSet<Inscripcion> Inscripcion { get; set; }
        public DbSet<Alumno> Alumno { get; set; }
        public DbSet<Cargo> Cargo { get; set; }
        public DbSet<CuentaPorCobrar> CuentaPorCobrar { get; set; }
        public DbSet<InversionCarreraTecnica> InversionCarreraTecnica { get; set; }
        public DbSet<ResultadoExamenAdmision> ResultadoExamenAdmision { get; set; }
        public DbSet<InscripcionPago> InscripcionPago { get; set; }
        public KalumDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarreraTecnica>().ToTable("CarreraTecnica").HasKey(ct => new { ct.CarreraId });
            modelBuilder.Entity<Jornada>().ToTable("Jornada").HasKey(j => new { j.JornadaId });
            modelBuilder.Entity<ExamenAdmision>().ToTable("ExamenAdmision").HasKey(ex => new { ex.ExamenId });
            modelBuilder.Entity<Aspirante>().ToTable("Aspirante").HasKey(a => new { a.NoExpediente });
            modelBuilder.Entity<Inscripcion>().ToTable("Inscripcion").HasKey(ins => new { ins.InscripcionId });
            modelBuilder.Entity<Alumno>().ToTable("Alumno").HasKey(al => new { al.Carne });
            modelBuilder.Entity<Cargo>().ToTable("Cargo").HasKey(c => new { c.CargoId });
            modelBuilder.Entity<CuentaPorCobrar>().ToTable("CuentaPorCobrar").HasKey(cpc => new { cpc.CuentaId, cpc.Anio, cpc.Carne });
            modelBuilder.Entity<InversionCarreraTecnica>().ToTable("InversionCarreraTecnica").HasKey(ict => new { ict.InversionId });
            modelBuilder.Entity<ResultadoExamenAdmision>().ToTable("ResultadoExamenAdmision").HasKey(rea => new { rea.NoExpediente, rea.Anio });
            modelBuilder.Entity<InscripcionPago>().ToTable("InscripcionPago").HasKey(ip => new { ip.BoletaPago, ip.Anio, ip.NoExpediente });
            modelBuilder.Entity<Aspirante>()
                .HasOne<CarreraTecnica>(a => a.CarreraTecnica)
                .WithMany(ct => ct.Aspirantes)
                .HasForeignKey(a => a.CarreraId);
            modelBuilder.Entity<Aspirante>()
                .HasOne<Jornada>(a => a.Jornada)
                .WithMany(j => j.Aspirantes)
                .HasForeignKey(a => a.JornadaId);
            modelBuilder.Entity<Aspirante>()
                .HasOne<ExamenAdmision>(a => a.ExamenAdmision)
                .WithMany(ex => ex.Aspirantes)
                .HasForeignKey(a => a.ExamenId);
            modelBuilder.Entity<Inscripcion>()
                .HasOne<CarreraTecnica>(ins => ins.CarreraTecnica)
                .WithMany(ct => ct.Inscripciones)
                .HasForeignKey(ins => ins.CarreraId);
            modelBuilder.Entity<Inscripcion>()
                .HasOne<Jornada>(ins => ins.Jornada)
                .WithMany(j => j.Inscripciones)
                .HasForeignKey(ins => ins.JornadaId);
            modelBuilder.Entity<Inscripcion>()
                .HasOne<Alumno>(ins => ins.Alumno)
                .WithMany(al => al.Inscripciones)
                .HasForeignKey(ins => ins.Carne);
            modelBuilder.Entity<CuentaPorCobrar>()
                .HasOne<Cargo>(cpc => cpc.Cargo)
                .WithMany(c => c.CuentasPorCobrar)
                .HasForeignKey(cpc => cpc.CargoId);
            modelBuilder.Entity<CuentaPorCobrar>()
                .HasOne<Alumno>(cpc => cpc.Alumno)
                .WithMany(c => c.CuentasPorCobrar)
                .HasForeignKey(cpc => cpc.Carne);
            modelBuilder.Entity<InversionCarreraTecnica>()
                .HasOne<CarreraTecnica>(ict => ict.CarreraTecnica)
                .WithMany(ict => ict.InversionesCarrerasTecnicas)
                .HasForeignKey(ict => ict.CarreraId);
            modelBuilder.Entity<ResultadoExamenAdmision>()
                .HasOne<Aspirante>(rea => rea.Aspirante)
                .WithMany(rea => rea.ResultadosExamenAdmisiones)
                .HasForeignKey(rea => rea.NoExpediente);
            modelBuilder.Entity<InscripcionPago>()
                .HasOne<Aspirante>(ip => ip.Aspirante)
                .WithMany(ip => ip.InscripcionesPago)
                .HasForeignKey(ip => ip.NoExpediente);
        }
    }
}