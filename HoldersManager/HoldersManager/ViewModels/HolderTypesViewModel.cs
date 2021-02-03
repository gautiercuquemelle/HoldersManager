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
    public class HolderTypesViewModel : ConfigListBaseViewModel<HolderType>
    {
        public HolderTypesViewModel()
        {
            Title = "Holder Types";
        }

        protected override string GetNavigationUriToEdit(string id)
        {
            return $"{nameof(HolderTypeEditPage)}?{nameof(HolderTypeEditViewModel.ItemId)}={id}";
        }
    }
}
