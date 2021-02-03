using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HoldersManager.ViewModels;

namespace HoldersManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DevelopersPage : ContentPage
    {
        DevelopersViewModel _viewModel;
        public DevelopersPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new DevelopersViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.LoadItemsCommand.Execute(null);
        }
    }
}
