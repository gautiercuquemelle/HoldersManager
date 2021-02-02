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
    public class CamerasViewModel : BaseViewModel
    {
        public ObservableCollection<Camera> Cameras { get; set; }
        public Command LoadItemsCommand { get; }
        public Command AddCameraCommand { get; }

        public CamerasViewModel()
        {
            Title = "Cameras";
            Cameras = new ObservableCollection<Camera>();
            LoadItemsCommand = new Command(ExecuteLoadCamerasCommand);

            AddCameraCommand = new Command(OnAddCamera);

            ExecuteLoadCamerasCommand();
        }

        private void ExecuteLoadCamerasCommand()
        {
            IsBusy = true;

            try
            {
                Cameras.Clear();

                using (var dbContext = new HoldersManagerContext())
                {
                    var cameras = dbContext.Cameras.ToList();
                    foreach (var item in cameras)
                    {
                        Cameras.Add(item);
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
            SelectedCamera = null;
        }

        private Camera _selectedCamera;
        public Camera SelectedCamera
        {
            get => _selectedCamera;
            set
            {
                SetProperty(ref _selectedCamera, value);
                OnCameraSelected(value);
            }
        }

        private async void OnAddCamera(object obj)
        {
            //await Shell.Current.GoToAsync(nameof(NewHolderTypePage));
            await Shell.Current.GoToAsync($"{nameof(CameraEditPage)}?{nameof(CameraEditViewModel.CameraId)}=0");
        }

        async void OnCameraSelected(Camera camera)
        {
            if (camera == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(CameraEditPage)}?{nameof(CameraEditViewModel.CameraId)}={camera.Id}");

            SelectedCamera = null;
        }
    }
}
