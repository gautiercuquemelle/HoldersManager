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
    public partial class FilmTypesPage : ContentPage
    {
        FilmTypesViewModel _viewModel;

        public FilmTypesPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new FilmTypesViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.LoadItemsCommand.Execute(null);
        }
    }
}
