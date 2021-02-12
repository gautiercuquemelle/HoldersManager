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
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mzk4MTU1QDMxMzgyZTM0MmUzMEhJbE1tSzRCd0laSVBFdGtzSk00cVJqdFhDN3h1TXBkNGUzYlIySkN2U1E9");
            InitializeComponent();

            using (var dbcontext = new HoldersManagerContext())
            {
                if (dbcontext.Database.EnsureCreated())
                    dbcontext.InitializeEmptyDbAsync().Wait();

                dbcontext.EnsureSchemaIsUpToDate().Wait();
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
