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
        private Holder _selectedItem;

        public ObservableCollection<Holder> Holders { get; set; }
        public Command LoadItemsCommand { get; }
        public Command AddHolderCommand { get; }        
        public Command<Holder> HolderTapped { get; }

        public HoldersViewModel()
        {
            Title = "Holders";
            Holders = new ObservableCollection<Holder>();
            LoadItemsCommand = new Command(ExecuteLoadHoldersCommand);
            HolderTapped = new Command<Holder>(OnHolderSelected);

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
                    foreach (var item in holders)
                    {
                        item.HolderType = holderTypes.FirstOrDefault(p => p.Id == item.HolderTypeId);
                        Holders.Add(item);
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

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public Holder SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnHolderSelected(value);
            }
        }

        private async void OnAddHolder(object obj)
        {
            //await Shell.Current.GoToAsync(nameof(NewHolderPage));
        }

        async void OnHolderSelected(Holder holder)
        {
            if (holder == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            var state = $"{nameof(HolderDetailPage)}?{nameof(HolderDetailViewModel.HolderId)}={holder.Id}";
            await Shell.Current.GoToAsync(state);

            SelectedItem = null;
        }
    }
}
