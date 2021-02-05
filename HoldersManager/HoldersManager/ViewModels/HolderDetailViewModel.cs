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
            set { SetProperty(ref _holder, value); OnPropertyChanged("HolderType"); }
        }

        private HolderType _holderType;
        public HolderType HolderType
        {
            get => _holderType;
            set => SetProperty(ref _holderType, value);
        }

        private ObservableCollection<HolderFilm> _holderFilms;
        public ObservableCollection<HolderFilm> HolderFilms 
        {
            get => _holderFilms;
            set => SetProperty(ref _holderFilms, value);
        }

        public HolderDetailViewModel()
        {
            EditCommand = new Command(OnEditHolder);
            LoadCommand = new Command(OnLoadHolder);

            //HolderFilms = new ObservableCollection<HolderFilm>(); // TODO : Load from DB

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
                    await Application.Current.MainPage.DisplayAlert("Error", "Can't load a partially loaded holder", "OK");
                    return;
                }
            }

            await Shell.Current.GoToAsync($"{nameof(LoadHolderPage)}?{nameof(LoadHolderViewModel.HolderId)}={Holder.Id}");
        }

        private void LoadHolderDetails(string holderId)
        {
            using (var dbcontext = new HoldersManagerContext())
            {
                Holder = dbcontext.Holders.FirstOrDefault(p => p.Id == int.Parse(holderId));

                if (Holder != null)
                {
                    HolderType = dbcontext.HolderTypes.FirstOrDefault(p => p.Id == Holder.HolderTypeId);

                    HolderFilms = new ObservableCollection<HolderFilm>(dbcontext.HolderFilms.Where(p => p.HolderId == Holder.Id));
                }
                else
                {
                    Application.Current.MainPage.DisplayAlert("Error", "Can't find selected holder", "Back");
                    Shell.Current.SendBackButtonPressed();
                }
            }
        }

        public void RefreshView()
        {
            LoadHolderDetails(HolderId);
        }
    }
}
