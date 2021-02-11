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
