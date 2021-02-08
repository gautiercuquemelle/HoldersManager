using HoldersManager.Models;
using HoldersManager.Services;
using HoldersManager.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace HoldersManager.ViewModels
{
    [QueryProperty(nameof(HolderFilmId), nameof(HolderFilmId))]    
    [QueryProperty(nameof(HolderId), nameof(HolderId))]
    [QueryProperty(nameof(HolderFilmNumber), nameof(HolderFilmNumber))]
    public class HolderFilmDetailViewModel : BaseViewModel
    {
        public Command AddExposureCommand { get; set; }
        public Command DevelopmentCommand { get; set; }
        public Command UnloadCommand { get; set; }
        public Command LoadCommand { get; set; }

        public string HolderFilmNumber { get; set; } = "0";
        public string HolderId { get; set; } = "0";

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
            set 
            {
                SetProperty(ref _holderFilm, value);

                OnPropertyChanged(() => IsLoaded);
                OnPropertyChanged(() => IsNotLoaded);
            }
        }

        private Film _film;
        public Film Film
        {
            get => _film;
            set
            {
                SetProperty(ref _film, value);
            }
        }

        private FilmType _filmType;
        public FilmType FilmType
        {
            get => _filmType;
            set
            {
                SetProperty(ref _filmType, value);
            }
        }

        private List<FilmExposure> _filmExposures;
        public List<FilmExposure> FilmExposures
        {
            get => _filmExposures;
            set
            {
                SetProperty(ref _filmExposures, value);
                OnPropertyChanged(() => FilmExposures);
                OnPropertyChanged(() => HasNoExposure);
            }
        }

        private FilmExposure _selectedFilmExposure;
        public FilmExposure SelectedFilmExposure
        {
            get => _selectedFilmExposure;
            set
            {
                SetProperty(ref _selectedFilmExposure, value);
                OnSelectExposure();
            }
        }

        public bool IsLoaded
        {
            get => HolderFilm != null;
        }

        public bool IsNotLoaded
        {
            get => !IsLoaded;
        }

        public bool HasNoExposure
        {
            get
            {
                return _filmExposures == null || _filmExposures.Count() == 0;
            }
        }

        public HolderFilmDetailViewModel()
        {
            AddExposureCommand = new Command(OnAddExposure);
            DevelopmentCommand = new Command(OnDevelopment);
            UnloadCommand= new Command(OnUnload);
            LoadCommand = new Command(OnLoad);
        }

        private void LoadHolderFilmDetails(string holderFilmId)
        {
            using (var dbcontext = new HoldersManagerContext())
            {
                HolderFilm = dbcontext.HolderFilms.FirstOrDefault(p => p.Id == int.Parse(holderFilmId));

                if(HolderFilm == null)
                    HolderFilm = dbcontext.HolderFilms.FirstOrDefault(p => p.Number == int.Parse(HolderFilmNumber) && p.HolderId == int.Parse(HolderId));
                
                if (HolderFilm != null)
                {
                    Film = dbcontext.Films.FirstOrDefault(p => p.Id == HolderFilm.FilmId);
                    FilmType = dbcontext.FilmTypes.FirstOrDefault(p => p.Id == Film.FilmTypeId);
                    FilmExposures = dbcontext.FilmExposures.Where(p => p.FilmId == HolderFilm.FilmId).ToList();
                    FilmExposures.ForEach(p =>
                    {
                        p.Camera = dbcontext.Cameras.FirstOrDefault(fe => fe.Id == p.CameraId);
                    });
                }
                else
                {
                    //HolderFilm = new HolderFilm { Number=int.Parse(HolderFilmNumber)}
                    FilmExposures = new List<FilmExposure>(); 
                }
            }
        }

        private async void OnLoad()
        {
            //using (var dbcontext = new HoldersManagerContext())
            //{
            //    if (dbcontext.HolderFilms.Any(p => p.HolderId == Holder.Id))
            //    {
            //        DisplayAlert("Error", "Can't load a partially loaded holder", "OK");
            //        return;
            //    }
            //}
            if(HolderFilm!=null)
                await Shell.Current.GoToAsync($"{nameof(LoadHolderPage)}?{nameof(LoadHolderViewModel.HolderId)}={HolderFilm.HolderId}&{nameof(LoadHolderViewModel.HolderFilmNumber)}={HolderFilm.Number}");
            else
                await Shell.Current.GoToAsync($"{nameof(LoadHolderPage)}?{nameof(LoadHolderViewModel.HolderId)}={HolderId}&{nameof(LoadHolderViewModel.HolderFilmNumber)}={HolderFilmNumber}");
        }

        private async void OnAddExposure()
        {
            await Shell.Current.GoToAsync($"{nameof(ExposurePage)}?{nameof(ExposureViewModel.FilmId)}={Film.Id}&{nameof(ExposureViewModel.ExposureId)}=0");            
        }

        private async void OnSelectExposure()
        {
            if(SelectedFilmExposure == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(ExposurePage)}?{nameof(ExposureViewModel.FilmId)}={Film.Id}&{nameof(ExposureViewModel.ExposureId)}={SelectedFilmExposure.Id}");

            SelectedFilmExposure = null;
        }

        private async void  OnDevelopment()
        {

        }

        private async void OnUnload()
        {

        }

        public void RefreshPage()
        {
            LoadHolderFilmDetails(HolderFilmId);
        }
    }
}
