using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HoldersManager.Models;
using Microsoft.EntityFrameworkCore;
using Xamarin.Essentials;
using System.Linq;
using System.Threading.Tasks;

namespace HoldersManager.Services
{
    public class HoldersManagerContext : DbContext
    {
        public DbSet<Camera> Cameras { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<ExposureUnit> ExposureUnits { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<FilmDevelopment> FilmDevelopments { get; set; }
        public DbSet<FilmExposure> FilmExposures { get; set; }
        public DbSet<FilmType> FilmTypes { get; set; }
        public DbSet<Holder> Holders { get; set; }
        public DbSet<HolderFilm> HolderFilms { get; set; }
        public DbSet<HolderType> HolderTypes { get; set; }

        public HoldersManagerContext()
        {
            SQLitePCL.Batteries_V2.Init();
            this.Database.EnsureCreated();            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "holdersmanager.db3");

            optionsBuilder
                .UseSqlite($"Filename={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<HolderType>()
            //    .HasKey(b => b.Id);

            //modelBuilder.Entity<Holder>()
            //    .HasKey(b => b.Id);
        }

        async public Task<int> InitializeEmptyDb()
        {
            if(await HolderTypes.CountAsync() == 0)
            {
                HolderTypes.Add(new HolderType { CreationDate = DateTime.Now, Name = "Chassis 4x5" });
                HolderTypes.Add(new HolderType { CreationDate = DateTime.Now, Name = "Pellicule 35mm" });
                HolderTypes.Add(new HolderType { CreationDate = DateTime.Now, Name = "Pellicule 120mm" });
            }

            if(await FilmTypes.CountAsync()==0)
            {
                FilmTypes.Add(new FilmType { CreationDate = DateTime.Now, ISO = 400, Name = "HP5+" });
                FilmTypes.Add(new FilmType { CreationDate = DateTime.Now, ISO = 5, Name = "Papier photo N&B" });
            }

            if(await Cameras.CountAsync()==0)
            {
                Cameras.Add(new Camera { CreationDate = DateTime.Now, Name = "Chambre Shen Hao 4x5" });
                Cameras.Add(new Camera { CreationDate = DateTime.Now, Name = "Canon FTB-QL" });
            }

            if (await ExposureUnits.CountAsync() == 0)
            {
                ExposureUnits.Add(new ExposureUnit { CreationDate = DateTime.Now, Name = "Milisecondes" });
                ExposureUnits.Add(new ExposureUnit { CreationDate = DateTime.Now, Name = "Secondes" });
                ExposureUnits.Add(new ExposureUnit { CreationDate = DateTime.Now, Name = "Minutes" });                
            }

            if (await Developers.CountAsync() == 0)
            {
                Developers.Add(new Developer { CreationDate=DateTime.Now, Name="Caffenol" });
            }

            int cptChanges = await SaveChangesAsync();

            if (await Holders.CountAsync() == 0)
            {
                Holders.Add(new Holder { CreationDate=DateTime.Now, HolderTypeId=HolderTypes.FirstOrDefault().Id, HolderName="Demo holder", FrameNumber=2, DiscardAfterDevelopment=false, Comments="Généré automatiquement" });
            }

            cptChanges += await SaveChangesAsync();

            return cptChanges;
        }
    }
}
