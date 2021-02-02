using HoldersManager.Models;
using HoldersManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace HoldersManager.ViewModels
{
    [QueryProperty(nameof(HolderTypeId), nameof(HolderTypeId))]
    public class HolderTypeEditViewModel : BaseViewModel
    {
        public Command CancelCommand { get; }
        public Command SaveCommand { get; }
        public Command DeleteCommand { get; }

        public HolderTypeEditViewModel()
        {
            CancelCommand = new Command(() => Shell.Current.SendBackButtonPressed());
            SaveCommand = new Command(OnSaveHolderType);
            DeleteCommand = new Command(OnDeleteHolderType);
        }


        private string _holderTypeId;
        public string HolderTypeId
        {
            get { return _holderTypeId; }
            set
            {
                _holderTypeId = value;
                LoadHolderTypeDetails(value);
            }
        }

        private HolderType _holderType;
        public HolderType HolderType
        {
            get => _holderType;
            set => SetProperty(ref _holderType, value);
        }

        private void OnSaveHolderType()
        {
            using (var dbcontext = new HoldersManagerContext())
            {
                if(HolderType.Id == -1)
                {
                    // Add new holder type
                    HolderType.CreationDate = DateTime.Now;
                    dbcontext.HolderTypes.Add(HolderType);                    
                }
                else
                {
                    // Update existing holder type
                    dbcontext.Update(HolderType);
                }

                dbcontext.SaveChangesAsync().Wait();
            }

            Shell.Current.SendBackButtonPressed();
        }

        private void OnDeleteHolderType()
        {
            if (HolderType.Id == 0) 
            {
                Shell.Current.SendBackButtonPressed();
                return;
            }

            using (var dbcontext = new HoldersManagerContext())
            {
                if(dbcontext.Holders.Where(p=>p.HolderTypeId == HolderType.Id).Any())
                {
                    Application.Current.MainPage.DisplayAlert("Error", "Can't delete a holder type used on existing holder", "Ok");
                    return;
                }

                dbcontext.HolderTypes.Remove(HolderType);
                dbcontext.SaveChangesAsync().Wait();

                Shell.Current.SendBackButtonPressed();
            } 
        }

        private void LoadHolderTypeDetails(string holderTypeId)
        {
            using (var dbcontext = new HoldersManagerContext())
            {
                HolderType = dbcontext.HolderTypes.FirstOrDefault(p => p.Id == int.Parse(holderTypeId));

                if(HolderType == null)
                {
                    HolderType = new HolderType {  }; // Creation mode
                }
            }
        }
    }
}
