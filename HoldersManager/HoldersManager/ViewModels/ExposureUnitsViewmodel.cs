using HoldersManager.Models;
using System;
using System.Collections.Generic;
using System.Text;
using HoldersManager.ViewModels;
using HoldersManager.Views;

namespace HoldersManager.ViewModels
{
    public class ExposureUnitsViewmodel : ConfigListBaseViewModel<ExposureUnit>
    {
        public ExposureUnitsViewmodel()
        {
            Title = "Exposure units";
        }

        protected override string GetNavigationUriToEdit(string id)
        {
            return $"{nameof(ExposureUnitEditPage)}?{nameof(ExposureUnitEditViewModel.ItemId)}={id}";
        }
    }
}
