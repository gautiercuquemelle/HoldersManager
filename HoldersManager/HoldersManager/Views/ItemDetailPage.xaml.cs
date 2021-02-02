using HoldersManager.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace HoldersManager.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}