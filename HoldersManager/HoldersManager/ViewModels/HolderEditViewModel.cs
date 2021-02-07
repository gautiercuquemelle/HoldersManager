using HoldersManager.Models;
using HoldersManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace HoldersManager.ViewModels
{
    [QueryProperty(nameof(HolderId), nameof(HolderId))]
    public class HolderEditViewModel : BaseViewModel
    {
        public Command CancelCommand { get; }
        public Command SaveCommand { get; }
        public Command DeleteCommand { get; }

        public HolderEditViewModel()
        {
            CancelCommand = new Command(() => Shell.Current.SendBackButtonPressed());
            SaveCommand = new Command(OnSaveHolder);
            DeleteCommand = new Command(OnDeleteHolder);
        }


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
            set => SetProperty(ref _holder, value);
        }

        private List<HolderType> _holderTypes;
        public List<HolderType> HolderTypes
        {
            get => _holderTypes;
            set =>SetProperty(ref _holderTypes, value);
        }

        private HolderType _selectedHolderType;
        public HolderType SelectedHolderType
        {
            get => _selectedHolderType;
            set 
            {
                SetProperty(ref _selectedHolderType, value);
                Holder.HolderTypeId = value != null ? value.Id : 0;
            }
        }

        public bool IsNotLoaded { get; set; } = true;

        private void OnSaveHolder()
        {
            if(SelectedHolderType == null)
            {
                DisplayAlert("Error", "You must select a holder type", "Ok");
                return;
            }            

            using (var dbcontext = new HoldersManagerContext())
            {
                Holder.HolderTypeId = SelectedHolderType.Id;

                if (Holder.Id == 0)
                {
                    // Add new holder type
                    Holder.CreationDate = DateTime.Now;
                    dbcontext.Holders.Add(Holder);
                }
                else
                {
                    // Update existing holder type
                    dbcontext.Update(Holder);
                }

                dbcontext.SaveChangesAsync().Wait();
            }

            Shell.Current.SendBackButtonPressed();
        }

        private void OnDeleteHolder()
        {
            if (Holder.Id == 0)
            {
                Shell.Current.SendBackButtonPressed();
                return;
            }

            using (var dbcontext = new HoldersManagerContext())
            {
                if (dbcontext.HolderFilms.Where(p => p.HolderId == Holder.Id).Any())
                {
                    DisplayAlert("Error", "Can't delete a holder with loaded film", "Ok");
                    return;
                }

                dbcontext.Holders.Remove(Holder);
                dbcontext.SaveChangesAsync().Wait();

                Shell.Current.SendBackButtonPressed();
            }
        }

        private void LoadHolderDetails(string holderId)
        {
            using (var dbcontext = new HoldersManagerContext())
            {
                HolderTypes = dbcontext.HolderTypes.ToList();

                Holder = dbcontext.Holders.FirstOrDefault(p => p.Id == int.Parse(holderId));

                if (Holder == null)
                {
                    Holder = new Holder { }; // Creation mode
                }
                else
                {
                    SelectedHolderType = dbcontext.HolderTypes.FirstOrDefault(p => p.Id == Holder.HolderTypeId);

                    IsNotLoaded = !dbcontext.HolderFilms.Any(p => p.HolderId == Holder.Id);
                }                
            }

            OnPropertyChanged(() => IsNotLoaded);
        }

    }
}
