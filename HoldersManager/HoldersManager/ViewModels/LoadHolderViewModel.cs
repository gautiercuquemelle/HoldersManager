using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Linq;
using HoldersManager.Models;
using HoldersManager.Services;

namespace HoldersManager.ViewModels
{
    [QueryProperty(nameof(HolderId), nameof(HolderId))]
    [QueryProperty(nameof(HolderFilmNumber), nameof(HolderFilmNumber))]
    public class LoadHolderViewModel : BaseViewModel
    {
        public Command LoadCommand { get; set; }
        public Command CancelCommand { get; set; }

        public string HolderId { get; set; }
        public string HolderFilmNumber { get; set; }

        private List<FilmType> _filmTypes;
        public List<FilmType> FilmTypes
        {
            get => _filmTypes;
            set => SetProperty(ref _filmTypes, value);
        }

        private FilmType _selectedFilmType;
        public FilmType SelectedFilmType
        {
            get => _selectedFilmType;
            set
            {
                SetProperty(ref _selectedFilmType, value);
            }
        }

        private string _comments;
        public string Comments
        {
            get => _comments;
            set
            {
                SetProperty(ref _comments, value);
            }
        }

        public LoadHolderViewModel()
        {
            LoadCommand = new Command(OnLoadHolder);
            CancelCommand = new Command(() => Shell.Current.SendBackButtonPressed());

            using (var dbcontext = new HoldersManagerContext())
            {
                FilmTypes = dbcontext.FilmTypes.ToList();
            }
        }

        private async void OnLoadHolder()
        {
            if (SelectedFilmType == null)
            {
                DisplayAlert("Error", "You must select a film type", "Ok");
                return;
            }

            using (var dbcontext = new HoldersManagerContext())
            {
                var holder = dbcontext.Holders.FirstOrDefault(p => p.Id == int.Parse(HolderId));

                if (!string.IsNullOrWhiteSpace(HolderFilmNumber))
                {
                    var film = new Film { CreationDate = DateTime.Now, FilmTypeId = SelectedFilmType.Id, Comments = Comments };

                    var holderFilm = new HolderFilm { CreationDate = DateTime.Now, Film = film, HolderId = holder.Id, Number = int.Parse(HolderFilmNumber) };

                    dbcontext.HolderFilms.Add(holderFilm);

                    await dbcontext.SaveChangesAsync();
                }
                else
                {
                    // Loop based on the number of frames that the holder can contains
                    for (int i = 1; i <= holder.NumberOfFrames; i++)
                    {
                        // Create the film
                        var film = new Film { CreationDate = DateTime.Now, FilmTypeId = SelectedFilmType.Id, Comments = Comments };

                        var holderFilm = new HolderFilm { CreationDate = DateTime.Now, Film = film, Holder = holder, Number = i };

                        dbcontext.HolderFilms.Add(holderFilm);

                        await dbcontext.SaveChangesAsync();
                    }
                }
            }

            Shell.Current.SendBackButtonPressed();
        }
    }
}
