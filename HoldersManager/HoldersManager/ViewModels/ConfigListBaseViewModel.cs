using HoldersManager.Models;
using HoldersManager.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace HoldersManager.ViewModels
{
    public abstract class ConfigListBaseViewModel<T> : BaseViewModel where T : BaseConfigModel
    {
        public ObservableCollection<T> Items { get; set; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }

        public ConfigListBaseViewModel()
        {
            Title = "Film Types";
            Items = new ObservableCollection<T>();
            LoadItemsCommand = new Command(ExecuteLoadItemsCommand);

            AddItemCommand = new Command(OnAddItem);

            ExecuteLoadItemsCommand();

            Items.CollectionChanged += Items_CollectionChanged;
        }

        private void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            using (var dbContext = new HoldersManagerContext())
            {
                for (int i=0; i< Items.Count; i++)
                {
                    Items[i].Order = i;
                    dbContext.Set<T>().Update(Items[i]);
                }

                dbContext.SaveChangesAsync().Wait();
            }
        }

        protected void ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();

                using (var dbContext = new HoldersManagerContext())
                {
                    var items = dbContext.Set<T>().OrderBy(p => p.Order).ToList();
                    foreach (var item in items)
                    {
                        Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        private T _selectedItem;
        public T SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(GetNavigationUriToEdit("0"));
        }

        async void OnItemSelected(T item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync(GetNavigationUriToEdit(item.Id.ToString()));

            SelectedItem = null;
        }

        protected abstract string GetNavigationUriToEdit(string id);
    }
}
