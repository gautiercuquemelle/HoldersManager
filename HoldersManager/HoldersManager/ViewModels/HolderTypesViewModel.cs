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
    public class HolderTypesViewModel : BaseViewModel
    {
        private HolderType _selectedHolderType;

        public ObservableCollection<HolderType> HolderTypes { get; set; }
        public Command LoadItemsCommand { get; }
        public Command AddHolderTypeCommand { get; }

        public HolderTypesViewModel()
        {
            Title = "Holder Types";
            HolderTypes = new ObservableCollection<HolderType>();
            LoadItemsCommand = new Command(ExecuteLoadHolderTypesCommand);

            AddHolderTypeCommand = new Command(OnAddHolderType);

            ExecuteLoadHolderTypesCommand();
        }

        private void ExecuteLoadHolderTypesCommand()
        {
            IsBusy = true;

            try
            {
                HolderTypes.Clear();

                using (var dbContext = new HoldersManagerContext())
                {
                    var holderTypes = dbContext.HolderTypes.ToList();
                    foreach (var item in holderTypes)
                    {                        
                        HolderTypes.Add(item);
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
            SelectedHolderType = null;
        }

        public HolderType SelectedHolderType
        {
            get => _selectedHolderType;
            set
            {
                SetProperty(ref _selectedHolderType, value);
                OnHolderTypeSelected(value);
            }
        }

        private async void OnAddHolderType(object obj)
        {
            //await Shell.Current.GoToAsync(nameof(NewHolderTypePage));
            await Shell.Current.GoToAsync($"{nameof(HolderTypeEditPage)}?{nameof(HolderTypeEditViewModel.HolderTypeId)}=0");
        }

        async void OnHolderTypeSelected(HolderType holderType)
        {
            if (holderType == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(HolderTypeEditPage)}?{nameof(HolderTypeEditViewModel.HolderTypeId)}={holderType.Id}");

            SelectedHolderType = null;
        }
    }
}
