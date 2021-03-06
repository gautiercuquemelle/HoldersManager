﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using HoldersManager.Services;
using System.Linq;
using HoldersManager.Models;

namespace HoldersManager.ViewModels
{
    [QueryProperty(nameof(FilmId), nameof(FilmId))]
    [QueryProperty(nameof(ExposureId), nameof(ExposureId))]
    public class ExposureViewModel : BaseViewModel
    {
        public Command SaveCommand { get; set; }
        public Command CancelCommand { get; set; }
        public Command DeleteCommand { get; set; }
        public Command LocateCommand { get; set; }

        private string _filmId;
        public string FilmId
        {
            get { return _filmId; }
            set
            {
                SetProperty(ref _filmId, value);
            }
        }

        private string _exposureId;
        public string ExposureId
        {
            get { return _exposureId; }
            set
            {
                SetProperty(ref _exposureId, value);
                LoadExposureDetails(value);
            }
        }

        private FilmExposure _filmExposure;
        public FilmExposure FilmExposure
        {
            get { return _filmExposure; }
            set
            {
                SetProperty(ref _filmExposure, value);
                OnPropertyChanged(() => _filmExposure);
            }
        }

        //public string Location
        //{
        //    get => FilmExposure?.Location;
        //}

        public DateTime? ExposureDate
        {
            get => FilmExposure?.ExposureDateTime.Date;
            set
            {
                if (FilmExposure != null)
                {
                    FilmExposure.ExposureDateTime = (value.HasValue?value.Value.Date:DateTime.Now.Date).AddTicks(ExposureTime?.Ticks ?? 0); 
                }

                OnPropertyChanged(() => ExposureDate);
            }
        }

        public TimeSpan? ExposureTime {
            get => FilmExposure?.ExposureDateTime.TimeOfDay;
            set
            {
                if (FilmExposure != null)
                {
                    FilmExposure.ExposureDateTime = new DateTime(FilmExposure.ExposureDateTime.Date.Ticks).AddTicks(value.Value.Ticks);
                }

                OnPropertyChanged(() => ExposureTime);
            }
        }

        private List<Camera> _cameras;
        public List<Camera> Cameras
        {
            get => _cameras;
            set
            {
                SetProperty(ref _cameras, value);
            }
        }

        private Camera _selectedCamera;
        public Camera SelectedCamera
        {
            get => _selectedCamera;
            set
            {
                SetProperty(ref _selectedCamera, value);                
            }
        }

        private List<ExposureUnit> _exposureUnits;
        public List<ExposureUnit> ExposureUnits
        {
            get => _exposureUnits;
            set
            {
                SetProperty(ref _exposureUnits, value);
            }
        }

        private ExposureUnit _selectedExposureUnit;
        public ExposureUnit SelectedExposureUnit
        {
            get => _selectedExposureUnit;
            set
            {
                SetProperty(ref _selectedExposureUnit, value);
            }
        }

        public ExposureViewModel()
        {
            SaveCommand = new Command(OnSave);
            CancelCommand = new Command(() => Shell.Current.SendBackButtonPressed());
            DeleteCommand = new Command(OnDelete);
            LocateCommand = new Command(OnLocate);
        }

        private void LoadExposureDetails(string filmExposureId)
        {
            using(var dbContext = new HoldersManagerContext())
            {
                FilmExposure = dbContext.FilmExposures.FirstOrDefault(p => p.Id == int.Parse(filmExposureId));
                Cameras = dbContext.Cameras.ToList();
                ExposureUnits = dbContext.ExposureUnits.ToList();

                if (FilmExposure == null)
                {
                    FilmExposure = new FilmExposure
                    {
                        FilmId = int.Parse(FilmId),
                        ExposureDateTime = DateTime.Now
                    };
                    if (Cameras.Count == 1)
                    {
                        SelectedCamera = Cameras.FirstOrDefault();
                    }
                    else if (Cameras.Count > 1)
                    {
                        // Try to see if a camera is defined on another exposure for the same holder
                        var req = from f in dbContext.Films // Starting from the current film
                                  join hf in dbContext.HolderFilms on f.Id equals hf.FilmId // Get the HolderFilm object
                                  join ahf in dbContext.HolderFilms on hf.HolderId equals ahf.HolderId // Then all the HolderFilms related to the same Holder
                                  join af in dbContext.Films on ahf.FilmId equals af.Id // Then all the films related to this holders
                                  join fe in dbContext.FilmExposures on af.Id equals fe.FilmId // And the FilmExposures (if any)
                                  join c in dbContext.Cameras on fe.CameraId equals c.Id // And finally the cameras used for this exposures
                                  where f.Id == int.Parse(FilmId)
                                  select c;

                        SelectedCamera = req.FirstOrDefault();
                    }


                    SelectedExposureUnit = ExposureUnits.FirstOrDefault();
                }
                else
                {
                    SelectedCamera = Cameras.FirstOrDefault(p => p.Id == FilmExposure.CameraId);
                    SelectedExposureUnit = ExposureUnits.FirstOrDefault(p => p.Id == FilmExposure.ExposureUnitId);                    
                }

                OnPropertyChanged(() => ExposureDate);
                OnPropertyChanged(() => ExposureTime);
            }            
        }

        private async void OnSave()
        {
            if (SelectedCamera == null)
            {
                DisplayAlert("Error", "You must select a camera", "Ok");
                return;
            }

            if (SelectedExposureUnit == null)
            {
                DisplayAlert("Error", "You must select an exposure unit", "Ok");
                return;
            }

            FilmExposure.CameraId = SelectedCamera.Id;
            FilmExposure.ExposureUnitId = SelectedExposureUnit.Id;
            //FilmExposure.ExposureDateTime = ExposureDateTime;

            using (var dbcontext = new HoldersManagerContext())
            {
                if (FilmExposure.Id == 0)
                {
                    // Add new holder type
                    FilmExposure.CreationDate = DateTime.Now;
                    dbcontext.FilmExposures.Add(FilmExposure);
                }
                else
                {
                    // Update existing holder type
                    dbcontext.Update(FilmExposure);
                }

                await dbcontext.SaveChangesAsync();
            }

            Shell.Current.SendBackButtonPressed();
        }

        private async void OnDelete()
        {
            if (FilmExposure.Id == 0) // Not created yet
            {
                Shell.Current.SendBackButtonPressed();
                return;
            }

            if (await DisplayConfirmationAlert("Confirm", "Delete this exposure ?", "Yes", "No"))
            {
                using (var dbcontext = new HoldersManagerContext())
                {
                    dbcontext.FilmExposures.Remove(FilmExposure);
                    await dbcontext.SaveChangesAsync();
                }

                Shell.Current.SendBackButtonPressed();
            }
        }
        private async void OnLocate()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {                    
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    
                    FilmExposure.Latitude = location.Latitude;
                    FilmExposure.Longitude = location.Longitude;
                    OnPropertyChanged(() => FilmExposure);                    
                }
            }
            catch (Exception e)
            {
                DisplayAlert("Error", "Cant get GPS coordinates : " + e.Message, "Cancel");
            }
        }
    }
}
