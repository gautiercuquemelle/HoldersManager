using HoldersManager.Models;
using HoldersManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace HoldersManager.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class DeveloperEditViewModel : ConfigEditBaseViewModel<Developer>
    {
        protected override bool HasDbConstraintErrors(ref string errorMessage)
        {
            using (var dbcontext = new HoldersManagerContext())
            {
                if (dbcontext.FilmDevelopments.Where(p => p.DeveloperId == Item.Id).Any())
                {
                    errorMessage += "Can't delete a developer used on existing film development\r\n";
                }
            }

            return !string.IsNullOrWhiteSpace(errorMessage);
        }

        protected override bool HasValidationErrors(ref string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(Item.Name))
                errorMessage = "You must specify a name";

            return !string.IsNullOrWhiteSpace(errorMessage);
        }
    }
}
