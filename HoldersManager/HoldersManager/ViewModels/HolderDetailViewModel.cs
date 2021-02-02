using HoldersManager.Models;
using HoldersManager.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace HoldersManager.ViewModels
{
    [QueryProperty(nameof(HolderId), nameof(HolderId))]
    public class HolderDetailViewModel : BaseViewModel
    {
        private string _holderId;
        public string HolderId 
        {
            get { return _holderId; }
            set
            {
                _holderId = value;
                LoadHolderDetails(value);
            }
        }
        
        private Holder _holder;
        public Holder Holder
        {
            get => _holder;
            set => SetProperty(ref _holder, value);
        }

        public ObservableCollection<HolderFilm> HolderFilms { get; set; }

        public HolderDetailViewModel()
        {
            HolderFilms = new ObservableCollection<HolderFilm>();
        }

        public void LoadHolderDetails(string holderId)
        {
            using (var dbcontext = new HoldersManagerContext())
            {
                Holder = dbcontext.Holders.FirstOrDefault(p => p.Id == int.Parse(holderId));
            }
        }
    }
}
