using HoldersManager.Models;
using HoldersManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace HoldersManager.ViewModels
{
    [QueryProperty(nameof(CameraId), nameof(CameraId))]
    public class CameraEditViewModel : BaseViewModel
    {
        public Command CancelCommand { get; }
        public Command SaveCommand { get; }
        public Command DeleteCommand { get; }

        public CameraEditViewModel()
        {
            CancelCommand = new Command(() => Shell.Current.SendBackButtonPressed());
            SaveCommand = new Command(OnSaveCamera);
            DeleteCommand = new Command(OnDeleteCamera);
        }

        public string CameraId
        {
            set => LoadCameraDetails(value);
        }

        private Camera _camera;
        public Camera Camera
        {
            get => _camera;
            set => SetProperty(ref _camera, value);
        }

        private void OnSaveCamera()
        {
            using (var dbcontext = new HoldersManagerContext())
            {
                if (Camera.Id == 0)
                {
                    // Add new holder type
                    Camera.CreationDate = DateTime.Now;
                    dbcontext.Cameras.Add(Camera);
                }
                else
                {
                    // Update existing holder type
                    dbcontext.Update(Camera);
                }

                dbcontext.SaveChangesAsync().Wait();
            }

            Shell.Current.SendBackButtonPressed();
        }

        private void OnDeleteCamera()
        {
            if (Camera.Id == 0)
            {
                Shell.Current.SendBackButtonPressed();
                return;
            }

            using (var dbcontext = new HoldersManagerContext())
            {
                if (dbcontext.FilmExposures.Where(p => p.CameraId == Camera.Id).Any())
                {
                    Application.Current.MainPage.DisplayAlert("Error", "Can't delete a camera used on existing film exposure", "Ok");
                    return;
                }

                dbcontext.Cameras.Remove(Camera);
                dbcontext.SaveChangesAsync().Wait();

                Shell.Current.SendBackButtonPressed();
            }
        }

        private void LoadCameraDetails(string cameraId)
        {
            using (var dbcontext = new HoldersManagerContext())
            {
                Camera = dbcontext.Cameras.FirstOrDefault(p => p.Id == int.Parse(cameraId));

                if (Camera == null)
                {
                    Camera = new Camera { }; // Creation mode
                }
            }
        }
    }
}
