using HoldersManager.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using HoldersManager.Services;

namespace HoldersManager.ViewModels
{
    public class ConfigViewModel : BaseViewModel
    {
        public Command HolderTypesCommand { get; }
        public Command FilmTypesCommand { get; }
        public Command CamerasCommand { get; }
        public Command ExposureUnitsCommand { get; }
        public Command DevelopersCommand { get; }
        public Command RecreateBddCommand { get; }


        public ConfigViewModel()
        {
            Title = "Config";
            HolderTypesCommand = new Command(async () => await Shell.Current.GoToAsync($"{nameof(HolderTypesPage)}"));
            FilmTypesCommand = new Command(async () => await Shell.Current.GoToAsync($"{nameof(FilmTypesPage)}"));
            CamerasCommand = new Command(async () => await Shell.Current.GoToAsync($"{nameof(CamerasPage)}"));
            ExposureUnitsCommand = new Command(async () => await Shell.Current.GoToAsync($"{nameof(ExposureUnitsPage)}"));
            DevelopersCommand = new Command(async () => await Shell.Current.GoToAsync($"{nameof(DevelopersPage)}"));
            RecreateBddCommand = new Command(OnRecreateBdd);
        }

        private async void OnRecreateBdd()
        {
            if (await DisplayConfirmationAlert("Confirm", "Recreate BDD (will return to default BDD content", "Ok", "Cancel"))
            {
                using (var dbContext = new HoldersManagerContext())
                {
                    dbContext.Database.EnsureDeleted();
                    dbContext.Database.EnsureCreated();
                    await dbContext.InitializeEmptyDbAsync();
                }
            }

        }
    }
}
