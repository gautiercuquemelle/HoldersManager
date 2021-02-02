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
    public partial class HolderDetailPage : ContentPage
    {
        HolderDetailViewModel _viewModel;
        public HolderDetailPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new HolderDetailViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.RefreshView();

        }
    }
}
