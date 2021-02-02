using HoldersManager.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HoldersManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HoldersPage : ContentPage
    {
        HoldersViewModel _viewModel;

        public HoldersPage()
        {
            InitializeComponent();

            _viewModel = new HoldersViewModel();

            //_viewModel.Holders = new ObservableCollection<Models.Holder>();
            //_viewModel.Holders.Add(new Models.Holder { Comments = "Fake holder 1", CreationDate = DateTime.Now, DiscardAfterDevelopment = false, FrameNumber = 2, HolderName = "01", HolderType = new Models.HolderType { CreationDate = DateTime.Now, Id = 0, Name = "Chassis" }, HolderTypeId = 0, Id = 0 });
            //_viewModel.Holders.Add(new Models.Holder { Comments = "Fake holder 2", CreationDate = DateTime.Now, DiscardAfterDevelopment = true, FrameNumber = 24, HolderName = "HP500 24 poses", HolderType = new Models.HolderType { CreationDate = DateTime.Now, Id = 1, Name = "Pellicule" }, HolderTypeId = 1, Id = 1 });

            this.BindingContext = _viewModel;
        }

        //async void Handle_HolderTapped(object sender, ItemTappedEventArgs e)
        //{
        //    if (e.Item == null)
        //        return;
            
        //    //await DisplayAlert("Item Tapped", "An item was tapped.", "OK");
        //    _viewModel.HolderTapped.Execute(e.Item);
           

        //    //Deselect Item
        //    ((ListView)sender).SelectedItem = null;
        //}
    }
}
