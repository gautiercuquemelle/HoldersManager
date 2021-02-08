using HoldersManager.Services;
using HoldersManager.Views;
using System;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HoldersManager
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            using (var dbcontext = new HoldersManagerContext())
            {
                //dbcontext.HolderFilms.RemoveRange(dbcontext.HolderFilms);
                //dbcontext.Holders.RemoveRange(dbcontext.Holders);
                //dbcontext.HolderTypes.RemoveRange(dbcontext.HolderTypes);
                //dbcontext.FilmTypes.RemoveRange(dbcontext.FilmTypes);
                //dbcontext.FilmDevelopments.RemoveRange(dbcontext.FilmDevelopments);
                //dbcontext.Developers.RemoveRange(dbcontext.Developers);
                //dbcontext.FilmExposures.RemoveRange(dbcontext.FilmExposures);
                //dbcontext.ExposureUnits.RemoveRange(dbcontext.ExposureUnits);
                //dbcontext.Cameras.RemoveRange(dbcontext.Cameras);
                //dbcontext.Films.RemoveRange(dbcontext.Films);

                //dbcontext.SaveChangesAsync().Wait();

                var initAsync = dbcontext.InitializeEmptyDbAsync();
                initAsync.Wait();

                Debug.WriteLine($"Elements config créés : {initAsync.Result}");
            }
                        
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {           
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
