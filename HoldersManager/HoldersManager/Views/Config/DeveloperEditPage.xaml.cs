using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HoldersManager.ViewModels;

namespace HoldersManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeveloperEditPage : ContentPage
    {
        public DeveloperEditPage()
        {
            InitializeComponent();
            BindingContext = new DeveloperEditViewModel();
        }
    }
}