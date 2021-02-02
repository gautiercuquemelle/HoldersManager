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
    public partial class CameraEditPage : ContentPage
    {
        public CameraEditPage()
        {
            InitializeComponent();
            BindingContext = new CameraEditViewModel();            
        }        
    }
}
