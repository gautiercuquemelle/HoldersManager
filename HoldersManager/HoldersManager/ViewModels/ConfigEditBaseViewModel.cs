using HoldersManager.Models;
using HoldersManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace HoldersManager.ViewModels
{
    public abstract class ConfigEditBaseViewModel<T> : BaseViewModel where T : BaseConfigModel, new()
    {
        public Command CancelCommand { get; }
        public Command SaveCommand { get; }
        public Command DeleteCommand { get; }

        public string ItemId
        {
            set => LoadItemDetails(value);
        }

        private T _item;
        public T Item
        {
            get => _item;
            set => SetProperty(ref _item, value);
        }

        public ConfigEditBaseViewModel()
        {
            CancelCommand = new Command(() => Shell.Current.SendBackButtonPressed());
            SaveCommand = new Command(OnSaveItem);
            DeleteCommand = new Command(OnDeleteItem);
        }

        private void LoadItemDetails(string itemId)
        {
            using (var dbcontext = new HoldersManagerContext())
            {
                Item = dbcontext.Set<T>().FirstOrDefault(p => p.Id == int.Parse(itemId));

                if (Item == null)
                {
                    Item = new T { }; // Creation mode
                }
            }
        }

        protected abstract bool HasValidationErrors(ref string errorMessage);

        private void OnSaveItem()
        {
            string errorMessage = null;
            if (HasValidationErrors(ref errorMessage))
            {
                Application.Current.MainPage.DisplayAlert("Error", errorMessage, "Ok");
                return;
            }

            using (var dbcontext = new HoldersManagerContext())
            {
                if (Item.Id == 0)
                {
                    // Add new holder type
                    Item.CreationDate = DateTime.Now;
                    dbcontext.Set<T>().Add(Item);
                }
                else
                {
                    // Update existing holder type
                    dbcontext.Update(Item);
                }

                dbcontext.SaveChangesAsync().Wait();
            }

            Shell.Current.SendBackButtonPressed();
        }

        protected abstract bool HasDbConstraintErrors(ref string errorMessage);

        private void OnDeleteItem()
        {
            if (Item.Id == 0)
            {
                Shell.Current.SendBackButtonPressed();
                return;
            }

            using (var dbcontext = new HoldersManagerContext())
            {
                string errorMessage = null;
                if (HasDbConstraintErrors(ref errorMessage))
                {
                    Application.Current.MainPage.DisplayAlert("Error", errorMessage, "Ok");
                    return;
                }

                dbcontext.Set<T>().Remove(Item);
                dbcontext.SaveChangesAsync().Wait();

                Shell.Current.SendBackButtonPressed();
            }
        }
    }
}