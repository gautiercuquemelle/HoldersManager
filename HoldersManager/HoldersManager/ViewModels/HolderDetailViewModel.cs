using HoldersManager.Models;
using HoldersManager.Services;
using HoldersManager.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace HoldersManager.ViewModels
{
    [QueryProperty(nameof(HolderId), nameof(HolderId))]
    public class HolderDetailViewModel : BaseViewModel
    {
        public Command EditCommand { get; }

        public Command LoadCommand { get; }

        public Command DevelopmentCommand { get; set; }

        private string _holderId;
        public string HolderId
        {
            get { return _holderId; }
            set
            {
                _holderId = value;
                LoadHolderDetails(value);
            }
        }

        private Holder _holder;
        public Holder Holder
        {
            get => _holder;
            set 
            {
                SetProperty(ref _holder, value); 
                OnPropertyChanged(()=>HolderType);
            }
        }

        private HolderType _holderType;
        public HolderType HolderType
        {
            get => _holderType;
            set => SetProperty(ref _holderType, value);
        }

        private HolderFilmDetails _selectedHolderFilm;
        public HolderFilmDetails SelectedHolderFilm
        {
            get => _selectedHolderFilm;
            set
            {
                SetProperty(ref _selectedHolderFilm, value);
                OnHolderFilmSelected(value);
            }
        }

        private ObservableCollection<HolderFilmDetails> _holderFilms;
        public ObservableCollection<HolderFilmDetails> HolderFilms
        {
            get => _holderFilms;
            set
            { 
                SetProperty(ref _holderFilms, value);
                OnPropertyChanged(() => IsUnloaded);
            }
        }

        public bool IsUnloaded
        {
            get
            {
                return _holderFilms == null || _holderFilms.Count() == 0;
            }
        }

        public bool IsExposed
        {
            get
            {
                return _holderFilms != null && _holderFilms.Any(p => p.ExposureDateTime.HasValue);
            }
        }

        public HolderDetailViewModel()
        {
            EditCommand = new Command(OnEditHolder);
            LoadCommand = new Command(OnLoadHolder);
            DevelopmentCommand = new Command(OnDevelopment);
        }

        private async void OnEditHolder()
        {
            await Shell.Current.GoToAsync($"{nameof(HolderEditPage)}?{nameof(HolderEditViewModel.HolderId)}={Holder.Id}");
        }

        private async void OnLoadHolder()
        {
            using (var dbcontext = new HoldersManagerContext())
            {
                if (dbcontext.HolderFilms.Any(p => p.HolderId == Holder.Id))
                {
                    DisplayAlert("Error", "Can't load a partially loaded holder", "OK");
                    return;
                }
            }

            await Shell.Current.GoToAsync($"{nameof(LoadHolderPage)}?{nameof(LoadHolderViewModel.HolderId)}={Holder.Id}");
        }

        private async void OnDevelopment()
        {

        }

        private void LoadHolderDetails(string holderId)
        {
            using (var dbcontext = new HoldersManagerContext())
            {
                Holder = dbcontext.Holders.FirstOrDefault(p => p.Id == int.Parse(holderId));

                if (Holder != null)
                {
                    HolderType = dbcontext.HolderTypes.FirstOrDefault(p => p.Id == Holder.HolderTypeId);

                    HolderFilms = new ObservableCollection<HolderFilmDetails>(
                        from hf in dbcontext.HolderFilms.Where(p => p.HolderId == Holder.Id)
                        join h in dbcontext.Holders on hf.HolderId equals h.Id
                        join f in dbcontext.Films on hf.FilmId equals f.Id
                        join ft in dbcontext.FilmTypes on f.FilmTypeId equals ft.Id
                        join FE in dbcontext.FilmExposures on f.Id equals FE.FilmId into fedefault
                        from fe in fedefault.DefaultIfEmpty()
                        select new HolderFilmDetails
                        {
                            Id = hf.Id,
                            HolderId = hf.HolderId,
                            HolderName = h.HolderName,
                            FilmId = hf.FilmId,
                            FilmName = ft.Name,
                            ISO = ft.ISO,
                            ExposureDateTime = fe.ExposureDateTime,
                            Comments = f.Comments
                        });
                }
                else
                {
                    DisplayAlert("Error", "Can't find selected holder", "Back");
                    Shell.Current.SendBackButtonPressed();
                }
            }
        }

        private async void OnHolderFilmSelected(HolderFilmDetails holderFilm)
        {
            if (holderFilm == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(HolderFilmDetailPage)}?{nameof(HolderFilmDetailViewModel.HolderFilmId)}={holderFilm.Id}");

            SelectedHolderFilm = null;
        }

        public void RefreshPage()
        {
            LoadHolderDetails(HolderId);
        }
    }
}
