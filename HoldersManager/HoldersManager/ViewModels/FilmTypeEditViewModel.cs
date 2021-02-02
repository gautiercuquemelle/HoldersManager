using HoldersManager.Models;
using HoldersManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace HoldersManager.ViewModels
{
    [QueryProperty(nameof(FilmTypeId), nameof(FilmTypeId))]
    public class FilmTypeEditViewModel : BaseViewModel
    {
        public Command CancelCommand { get; }
        public Command SaveCommand { get; }
        public Command DeleteCommand { get; }

        public FilmTypeEditViewModel()
        {
            CancelCommand = new Command(() => Shell.Current.SendBackButtonPressed());
            SaveCommand = new Command(OnSaveFilmType);
            DeleteCommand = new Command(OnDeleteFilmType);
        }

        public string FilmTypeId
        {
            set => LoadFilmTypeDetails(value);
        }

        private FilmType _filmType;
        public FilmType FilmType
        {
            get => _filmType;
            set => SetProperty(ref _filmType, value);
        }

        private void OnSaveFilmType()
        {
            using (var dbcontext = new HoldersManagerContext())
            {
                if (FilmType.Id == 0)
                {
                    // Add new holder type
                    FilmType.CreationDate = DateTime.Now;
                    dbcontext.FilmTypes.Add(FilmType);
                }
                else
                {
                    // Update existing holder type
                    dbcontext.Update(FilmType);
                }

                dbcontext.SaveChangesAsync().Wait();
            }

            Shell.Current.SendBackButtonPressed();
        }

        private void OnDeleteFilmType()
        {
            if (FilmType.Id == 0)
            {
                Shell.Current.SendBackButtonPressed();
                return;
            }

            using (var dbcontext = new HoldersManagerContext())
            {
                if (dbcontext.Films.Where(p => p.FilmTypeId == FilmType.Id).Any())
                {
                    Application.Current.MainPage.DisplayAlert("Error", "Can't delete a film type used on existing film", "Ok");
                    return;
                }

                dbcontext.FilmTypes.Remove(FilmType);
                dbcontext.SaveChangesAsync().Wait();

                Shell.Current.SendBackButtonPressed();
            }
        }

        private void LoadFilmTypeDetails(string filmTypeId)
        {
            using (var dbcontext = new HoldersManagerContext())
            {
                FilmType = dbcontext.FilmTypes.FirstOrDefault(p => p.Id == int.Parse(filmTypeId));

                if (FilmType == null)
                {
                    FilmType = new FilmType { }; // Creation mode
                }
            }
        }
    }
}
