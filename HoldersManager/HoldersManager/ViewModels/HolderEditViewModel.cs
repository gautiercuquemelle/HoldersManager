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
            set => SetProperty(ref _holderTypes, value);
        }

        private void OnSaveHolder()
        {
            using (var dbcontext = new HoldersManagerContext())
            {
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
                    Application.Current.MainPage.DisplayAlert("Error", "Can't delete a holder with loaded film", "Ok");
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

                
            }
        }

    }
}
