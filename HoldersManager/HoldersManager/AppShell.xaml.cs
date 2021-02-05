using HoldersManager.ViewModels;
using HoldersManager.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace HoldersManager
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(HolderDetailPage), typeof(HolderDetailPage));
            Routing.RegisterRoute(nameof(HolderEditPage), typeof(HolderEditPage));
            Routing.RegisterRoute(nameof(HolderFilmDetailPage), typeof(HolderFilmDetailPage));
            Routing.RegisterRoute(nameof(LoadHolderPage), typeof(LoadHolderPage));
            Routing.RegisterRoute(nameof(HolderTypesPage), typeof(HolderTypesPage));
            Routing.RegisterRoute(nameof(HolderTypeEditPage), typeof(HolderTypeEditPage));
            Routing.RegisterRoute(nameof(FilmTypesPage), typeof(FilmTypesPage));
            Routing.RegisterRoute(nameof(FilmTypeEditPage), typeof(FilmTypeEditPage));
            Routing.RegisterRoute(nameof(CamerasPage), typeof(CamerasPage));
            Routing.RegisterRoute(nameof(CameraEditPage), typeof(CameraEditPage));
            Routing.RegisterRoute(nameof(ExposureUnitsPage), typeof(ExposureUnitsPage));
            Routing.RegisterRoute(nameof(ExposureUnitEditPage), typeof(ExposureUnitEditPage));
            Routing.RegisterRoute(nameof(DevelopersPage), typeof(DevelopersPage));
            Routing.RegisterRoute(nameof(DeveloperEditPage), typeof(DeveloperEditPage));
        }

    }
}
