using HoldersManager.Models;
using HoldersManager.Services;
using HoldersManager.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace HoldersManager.ViewModels
{
    public class FilmTypesViewModel : ConfigListBaseViewModel<FilmType>
    {        
        public FilmTypesViewModel()
        {
            Title = "Film Types";
        }

        protected override string GetNavigationUriToEdit(string id)
        {
            return $"{nameof(FilmTypeEditPage)}?{nameof(FilmTypeEditViewModel.ItemId)}={id}";
        }
    }
}
