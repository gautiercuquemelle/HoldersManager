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
    public class CameraEditViewModel : ConfigEditBaseViewModel<Camera>
    {        
        protected override bool HasDbConstraintErrors(ref string errorMessage)
        {
            using (var dbcontext = new HoldersManagerContext())
            {
                if (dbcontext.FilmExposures.Where(p => p.CameraId == Item.Id).Any())
                {
                    errorMessage += "Can't delete a camera used on existing film exposure\r\n";                    
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
