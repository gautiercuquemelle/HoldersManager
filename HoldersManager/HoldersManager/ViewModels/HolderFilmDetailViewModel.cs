using HoldersManager.Models;
using HoldersManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace HoldersManager.ViewModels
{
    [QueryProperty(nameof(HolderFilmId), nameof(HolderFilmId))]
    public class HolderFilmDetailViewModel : BaseViewModel
    {
        public Command ExposeCommand { get; set; }
        public Command DevelopmentCommand { get; set; }
        public Command UnloadCommand { get; set; }

        private string _holderFilmId;
        public string HolderFilmId
        {
            get { return _holderFilmId; }
            set
            {
                _holderFilmId = value;
                LoadHolderFilmDetails(value);
            }
        }

        private HolderFilm _holderFilm;
        public HolderFilm HolderFilm
        {
            get => _holderFilm;
            set { SetProperty(ref _holderFilm, value); 
                OnPropertyChanged("HolderFilmDetails"); }
        }

        private Film _film;
        public Film Film
        {
            get => _film;
            set
            {
                SetProperty(ref _film, value);
                OnPropertyChanged("Film");
            }
        }

        private FilmExposure _filmExposure;
        public FilmExposure FilmExposure
        {
            get => _filmExposure;
            set
            {
                SetProperty(ref _filmExposure, value);
                OnPropertyChanged("FilmExposure");
            }
        }

        public HolderFilmDetailViewModel()
        {

        }

        private void LoadHolderFilmDetails(string holderFilmId)
        {
            using (var dbcontext = new HoldersManagerContext())
            {
                HolderFilm = dbcontext.HolderFilms.FirstOrDefault(p=>p.Id == int.Parse(holderFilmId));
                Film = dbcontext.Films.FirstOrDefault(p => p.Id == HolderFilm.FilmId);
                FilmExposure = dbcontext.FilmExposures.FirstOrDefault(p=>p.FilmId == HolderFilm.FilmId);
            }
        }

        public void RefreshPage()
        {
            LoadHolderFilmDetails(HolderFilmId);
        }
    }
}
