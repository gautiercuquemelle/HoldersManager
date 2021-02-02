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
            
            this.BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.LoadItemsCommand.Execute(null);

        }
    }
}
