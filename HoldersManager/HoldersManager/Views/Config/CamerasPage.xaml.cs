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
    public partial class CamerasPage : ContentPage
    {
        CamerasViewModel _viewModel;

        public CamerasPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new CamerasViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.LoadItemsCommand.Execute(null);
        }
    }
}
