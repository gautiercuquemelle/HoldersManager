using HoldersManager.Models;
using HoldersManager.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoldersManager.ViewModels
{
    public class DevelopersViewModel : ConfigListBaseViewModel<Developer>
    {
        public DevelopersViewModel()
        {
            Title = "Developers";
        }

        protected override string GetNavigationUriToEdit(string id)
        {
            return $"{nameof(DeveloperEditPage)}?{nameof(DeveloperEditViewModel.ItemId)}={id}";
        }
    }
}
