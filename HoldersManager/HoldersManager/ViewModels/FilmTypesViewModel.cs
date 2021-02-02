using HoldersManager.Models;
using HoldersManager.Services;
using HoldersManager.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace HoldersManager.ViewModels
{
    public class FilmTypesViewModel : BaseViewModel
    {
        public ObservableCollection<FilmType> FilmTypes { get; set; }
        public Command LoadItemsCommand { get; }
        public Command AddFilmTypeCommand { get; }

        public FilmTypesViewModel()
        {
            Title = "Film Types";
            FilmTypes = new ObservableCollection<FilmType>();
            LoadItemsCommand = new Command(ExecuteLoadFilmTypesCommand);

            AddFilmTypeCommand = new Command(OnAddFilmType);

            ExecuteLoadFilmTypesCommand();
        }

        private void ExecuteLoadFilmTypesCommand()
        {
            IsBusy = true;

            try
            {
                FilmTypes.Clear();

                using (var dbContext = new HoldersManagerContext())
                {
                    var filmTypes = dbContext.FilmTypes.ToList();
                    foreach (var item in filmTypes)
                    {
                        FilmTypes.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedFilmType = null;
        }

        private FilmType _selectedFilmType;
        public FilmType SelectedFilmType
        {
            get => _selectedFilmType;
            set
            {
                SetProperty(ref _selectedFilmType, value);
                OnFilmTypeSelected(value);
            }
        }

        private async void OnAddFilmType(object obj)
        {
            //await Shell.Current.GoToAsync(nameof(NewHolderTypePage));
            await Shell.Current.GoToAsync($"{nameof(FilmTypeEditPage)}?{nameof(FilmTypeEditViewModel.FilmTypeId)}=0");
        }

        async void OnFilmTypeSelected(FilmType filmType)
        {
            if (filmType == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(FilmTypeEditPage)}?{nameof(FilmTypeEditViewModel.FilmTypeId)}={filmType.Id}");

            SelectedFilmType = null;
        }
    }
}
