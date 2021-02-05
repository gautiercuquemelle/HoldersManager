using HoldersManager.Models;
using HoldersManager.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using System.Diagnostics;
using HoldersManager.Views;

namespace HoldersManager.ViewModels
{
    public class HoldersViewModel : BaseViewModel
    {
        private HolderDetails _selectedItem;

        public ObservableCollection<HolderDetails> Holders { get; set; }
        public Command LoadItemsCommand { get; }
        public Command AddHolderCommand { get; }

        public HolderDetails SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnHolderSelected(value);
            }
        }

        public HoldersViewModel()
        {
            Title = "Holders";
            Holders = new ObservableCollection<HolderDetails>();
            LoadItemsCommand = new Command(ExecuteLoadHoldersCommand);
            AddHolderCommand = new Command(OnAddHolder);

            ExecuteLoadHoldersCommand();
        }

        private void ExecuteLoadHoldersCommand()
        {
            IsBusy = true;

            try
            {
                Holders.Clear();

                using (var dbContext = new HoldersManagerContext())
                {
                    var holderTypes = dbContext.HolderTypes.ToList();
                    var holders = dbContext.Holders.ToList();
                    foreach (var holder in holders)
                    {
                        // Get hold type details
                        var holderType = holderTypes.FirstOrDefault(p => p.Id == holder.HolderTypeId);

                        // Get laoding informations
                        Holders.Add(new HolderDetails
                        {
                            Id = holder.Id,
                            HolderName = holder.HolderName,
                            HolderTypeName = holderType.Name,
                            Comments = holder.Comments,
                            NumberOfFrames = holder.NumberOfFrames,
                            CreationDate = holder.CreationDate,
                            DiscardAfterDevelopment = holder.DiscardAfterDevelopment,
                            NumberOfLoadedFrames = GetNumberOfLoadedFrames(dbContext, holder.Id),
                            NumberOfExposedFrames = GetNumberOfExposedFrames(dbContext, holder.Id)
                        });
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

        private int GetNumberOfLoadedFrames(HoldersManagerContext dbContext, int holderId)
        {
            return dbContext.HolderFilms.Count(p => p.HolderId == holderId);
        }

        private int GetNumberOfExposedFrames(HoldersManagerContext dbContext, int holderId)
        {
            return (from hf in dbContext.HolderFilms.Where(p => p.HolderId == holderId)
                    join f in dbContext.Films on hf.FilmId equals f.Id
                    join e in dbContext.FilmExposures on f.Id equals e.FilmId
                    select e.Id).Count();
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }
        
        private async void OnAddHolder(object obj)
        {
            await Shell.Current.GoToAsync($"{nameof(HolderEditPage)}?{nameof(HolderEditViewModel.HolderId)}=0");
        }

        async void OnHolderSelected(HolderDetails holder)
        {
            if (holder == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(HolderDetailPage)}?{nameof(HolderDetailViewModel.HolderId)}={holder.Id}");

            SelectedItem = null;
        }
    }
}
