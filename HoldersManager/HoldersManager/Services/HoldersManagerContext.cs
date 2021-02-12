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
        const int SCHEMA_VERSION_NUMBER = 2;

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
        public DbSet<SchemaVersion> SchemaVersions { get; set; }

        public HoldersManagerContext()
        {
            SQLitePCL.Batteries_V2.Init();
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "holdersmanager.db3");

            optionsBuilder
                .UseSqlite($"Filename={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HolderFilm>()
                .Navigation<Holder>(p => p.Holder);

            modelBuilder.Entity<HolderFilm>()
                .Navigation<Film>(p => p.Film);

            modelBuilder.Entity<FilmExposure>()
                .Navigation<ExposureUnit>(p => p.ExposureUnit).AutoInclude(true);
        }

        async public Task<int> InitializeEmptyDbAsync()
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
                Holders.Add(new Holder { CreationDate=DateTime.Now, HolderTypeId=HolderTypes.FirstOrDefault().Id, HolderName="Demo holder", NumberOfFrames=2, DiscardAfterDevelopment=false, Comments="Généré automatiquement" });
            }

            if (await SchemaVersions.CountAsync() == 0)
                SchemaVersions.Add(new SchemaVersion { VersionNumber = SCHEMA_VERSION_NUMBER });

            cptChanges += await SaveChangesAsync();

            return cptChanges;
        }

        async public Task EnsureSchemaIsUpToDate()
        {
            int currentVersion = 0;
            try
            {
                currentVersion = SchemaVersions.FirstOrDefault().VersionNumber;
            }
            catch
            {
                // No Table Version -> Version 0
            }

            if(currentVersion == 0)
            {
                currentVersion = await UpdateSchemaToVersion_1();                
            }
            if(currentVersion == 1)
            {
                currentVersion = await UpdateSchemaToVersion_2();
            }

            
        }

        async private Task<int> UpdateSchemaToVersion_1()
        {
            var rawScript = "CREATE TABLE SchemaVersions (VersionNumber int PRIMARY KEY NOT NULL);";

            await Database.ExecuteSqlRawAsync(rawScript);
            SchemaVersions.Add(new SchemaVersion { VersionNumber = 1 });
            await SaveChangesAsync();

            return 1;
        }

        async private Task<int> UpdateSchemaToVersion_2()
        {
            await Database.ExecuteSqlRawAsync("ALTER TABLE Cameras ADD COLUMN [Order] int null;");
            await Database.ExecuteSqlRawAsync("ALTER TABLE Developers ADD COLUMN [Order] int null;");
            await Database.ExecuteSqlRawAsync("ALTER TABLE ExposureUnits ADD COLUMN [Order] int null;");
            await Database.ExecuteSqlRawAsync("ALTER TABLE FilmTypes ADD COLUMN [Order] int null;");
            await Database.ExecuteSqlRawAsync("ALTER TABLE HolderTypes ADD COLUMN [Order] int null;");

            var schemaVersion = SchemaVersions.FirstOrDefault();
            schemaVersion.VersionNumber = 2 ;
            SchemaVersions.Update(schemaVersion);
            await SaveChangesAsync();

            return 2;
        }
    }
}
